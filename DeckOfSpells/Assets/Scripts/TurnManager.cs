using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    private CardName playerCard;
    private CardName enemyCard;

    private int playerPriority;
    private int enemyPriority;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private DeckManager deckManager;

	[SerializeField] private TMP_Text outcomeText;

	[SerializeField] private GameObject minionPrefab;

	[SerializeField] private RuntimeAnimatorController fireballAnimation;
	[SerializeField] private RuntimeAnimatorController lightningAnimation;
	[SerializeField] private RuntimeAnimatorController landslideAnimation;
	[SerializeField] private RuntimeAnimatorController freezeAnimation;
	[SerializeField] private RuntimeAnimatorController frightenAnimation;
	[SerializeField] private RuntimeAnimatorController poisonAnimation;
	[SerializeField] private RuntimeAnimatorController lullabyAnimation;
	[SerializeField] private RuntimeAnimatorController cureAnimation;
	[SerializeField] private RuntimeAnimatorController boostAnimation;
	[SerializeField] private RuntimeAnimatorController reflectAnimation;
	[SerializeField] private GameObject cardEffectPrefab;



	void Start()
    {
        deckManager = GameObject.FindWithTag("GameController").GetComponent<DeckManager>();
    }

    public void SetPlayerCard(CardName cardName, int priority)
    {
        playerCard = cardName;
        playerPriority = priority;
        StartCoroutine(PlayTurn());
    }

	public void SetEnemyCard(CardName cardName, int priority)
	{
		enemyCard = cardName;
        enemyPriority = priority;
	}
	IEnumerator PlayTurn()
    {
		if (playerCard != CardName.None && enemyCard != CardName.None)
        {
			if (playerPriority >= enemyPriority)
			{
				PlayerTurn();
				player.GetComponent<StatusEffect>().UpdateStatus();
				yield return new WaitForSeconds(.5f);
				if (enemy.GetComponent<Enemy>().GetHealth() > 0)
				{
					EnemyTurn();
					enemy.GetComponent<StatusEffect>().UpdateStatus();
				}
			}
			else
			{
				EnemyTurn();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				yield return new WaitForSeconds(.5f);
				if (player.GetComponent<Player>().GetHealth() > 0)
				{
					PlayerTurn();
					player.GetComponent<StatusEffect>().UpdateStatus();
				}
			}
		}
        else if (playerCard == CardName.None)
        {
            EnemyTurn();
			enemy.GetComponent<StatusEffect>().UpdateStatus();
			yield return new WaitForSeconds(.5f);
			player.GetComponent<StatusEffect>().UpdateStatus();
		}
		else
		{
			PlayerTurn();
			enemy.GetComponent<StatusEffect>().UpdateStatus();
			yield return new WaitForSeconds(.5f);
			player.GetComponent<StatusEffect>().UpdateStatus();
		}

		//Check to see if the game has ended
		if (player.GetComponent<Player>().GetHealth() < 1 && enemy.GetComponent<Enemy>().GetHealth() < 1)
		{
			if (playerCard == CardName.Reflect)
			{
				enemy.GetComponent<Enemy>().EndBattle();
				outcomeText.text = "Player Wins!";
				deckManager.GameEnd();
			}
			else if (enemyCard == CardName.Reflect)
			{
				outcomeText.text = "Enemy Wins!";
				deckManager.GameEnd();
			}
		}
		else if (player.GetComponent<Player>().GetHealth() < 1)
		{
			outcomeText.text = "Enemy Wins!";
			deckManager.GameEnd();
		}
		else if (enemy.GetComponent<Enemy>().GetHealth() < 1)
		{
			enemy.GetComponent<Enemy>().EndBattle();
			outcomeText.text = "Player Wins!";
			deckManager.GameEnd();
		}
		else
		{
			enemy.GetComponent<Enemy>().SelectCard();
			deckManager.ChooseCards();
		}
		playerCard = CardName.None;
    }

    private void PlayerTurn()
    {
		if (player.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
		{
			player.GetComponent<Player>().TakeDamage(2, true);
		}

        if (player.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Asleep))
        {
            if (Random.Range(0, 4) > 0)
            {
                return;
            }
            player.GetComponent<StatusEffect>().DeleteStatus(Status.Asleep);
        }
        
        switch (playerCard)
        {
            case CardName.Lightning:
				int[] intArray = null;
				if (deckManager.comboNumber == 1)
                {
                    intArray = new int[3] {1, 2, 3};
                }
                else if (deckManager.comboNumber == 2)
                {
					intArray = new int[3] {2, 3, 4};
				}
                else
                {
					intArray = new int[3] {3, 4, 5};
				}
				if (enemyCard == CardName.Reflect)
				{
					if (enemy.GetComponent<Enemy>().comboNumber == 1)
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else if (enemy.GetComponent<Enemy>().comboNumber == 2)
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray) / 2, false);
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else
					{
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					GameObject effect1 = Instantiate(cardEffectPrefab, new Vector3(7.2f, -1, 0), Quaternion.identity);
					effect1.GetComponent<Animator>().runtimeAnimatorController = reflectAnimation;
				}
				else
				{
					enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
				}
				GameObject effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = lightningAnimation;
				break;
            case CardName.Fireball:
				if (deckManager.comboNumber == 1)
				{
					intArray = new int[3] {1, 1, 2};
				}
				else if (deckManager.comboNumber == 2)
				{
					intArray = new int[3] {2, 3, 3};
				}
				else
				{
					intArray = new int[3] {5, 6, 7};
				}
				if (enemyCard == CardName.Reflect)
				{
					if (enemy.GetComponent<Enemy>().comboNumber == 1)
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else if (enemy.GetComponent<Enemy>().comboNumber == 2)
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray) / 2, false);
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else
					{
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					effect = Instantiate(cardEffectPrefab, new Vector3(7.2f, -1, 0), Quaternion.identity);
					effect.GetComponent<Animator>().runtimeAnimatorController = reflectAnimation;
				}
				else
				{
					enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<SpriteRenderer>().flipX = true;
				effect.GetComponent<Animator>().runtimeAnimatorController = fireballAnimation;
				break;
			case CardName.Landslide:
				if (enemyCard == CardName.Reflect)
				{
					if (enemy.GetComponent<Enemy>().comboNumber == 1)
					{
						enemy.GetComponent<Enemy>().TakeDamage(3, false);
						player.GetComponent<Player>().TakeDamage(3, false);
					}
					else if (enemy.GetComponent<Enemy>().comboNumber == 2)
					{
						enemy.GetComponent<Enemy>().TakeDamage(1, false);
						player.GetComponent<Player>().TakeDamage(3, false);
					}
					else
					{
						player.GetComponent<Player>().TakeDamage(3, false);
					}
					effect = Instantiate(cardEffectPrefab, new Vector3(7.2f, -1, 0), Quaternion.identity);
					effect.GetComponent<Animator>().runtimeAnimatorController = reflectAnimation;
				}
				else
				{
					enemy.GetComponent<Enemy>().TakeDamage(3, false);
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = landslideAnimation;
				break;
			case CardName.Freeze:
				enemy.GetComponent<StatusEffect>().AddStatus(Status.Frozen, deckManager.comboNumber);
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = freezeAnimation;
				break;
			case CardName.Lullaby:
				if (deckManager.comboNumber == 1 && Random.Range(0, 4) < 2)
				{
					enemy.GetComponent<StatusEffect>().AddStatus(Status.Asleep, 2);
				}
				else if (deckManager.comboNumber == 2 && Random.Range(0, 4) < 3)
				{
					enemy.GetComponent<StatusEffect>().AddStatus(Status.Asleep, 2);
				}
				else if (deckManager.comboNumber == 3)
				{
					enemy.GetComponent<StatusEffect>().AddStatus(Status.Asleep, 2);
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = lullabyAnimation;
				break;
			case CardName.Frighten:
				enemy.GetComponent<StatusEffect>().AddStatus(Status.Frightened, deckManager.comboNumber + 1);
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = frightenAnimation;
				break;
			case CardName.Poison:
				enemy.GetComponent<StatusEffect>().AddStatus(Status.Poisoned, deckManager.comboNumber);
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = poisonAnimation;
				break;
			case CardName.ComboBreaker:
				if (deckManager.comboNumber == 1)
				{
					enemy.GetComponent<Enemy>().comboNumber = 0;
				}
				else if (deckManager.comboNumber == 2)
				{
					enemy.GetComponent<Enemy>().comboNumber = 0;
					enemy.GetComponent<Enemy>().SetBroken();
				}
				else
				{
					enemy.GetComponent<Enemy>().comboNumber = 0;
					enemy.GetComponent<Enemy>().comboDelayColor = enemy.GetComponent<Enemy>().GetComboColor();
					enemy.GetComponent<Enemy>().comboDelayTimer = 1;
				}
				break;
			case CardName.Revivify:
				player.GetComponent<Player>().TakeDamage((deckManager.comboNumber + 1) * -1, false);
				break;
			case CardName.Cure:
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				if (deckManager.comboNumber > 1)
				{
					player.GetComponent<Player>().CureMinions();
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = cureAnimation;
				break;
			case CardName.Tree:
				GameObject minion = Instantiate(minionPrefab, player.transform.position + new Vector3(1.8f, 0, 0), Quaternion.identity);
				if (deckManager.comboNumber == 1)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Tree, 2, 0);
				}
				else if (deckManager.comboNumber == 2)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Tree, 3, 1);
				}
				else
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Tree, 5, 1);
				}
				player.GetComponent<Player>().AddMinion(minion);
				break;
			case CardName.Spikey:
				minion = Instantiate(minionPrefab, player.transform.position + new Vector3(1.8f, 0, 0), Quaternion.identity);
				if (deckManager.comboNumber == 1)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Spikey, 1, 1);
				}
				else if (deckManager.comboNumber == 2)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Spikey, 2, 2);
				}
				else
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Spikey, 3, 3);
				}
				player.GetComponent<Player>().AddMinion(minion);
				break;
			case CardName.Wall:
				minion = Instantiate(minionPrefab, player.transform.position + new Vector3(1.8f, 0, 0), Quaternion.identity);
				if (deckManager.comboNumber == 1)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Wall, 3, 0);
				}
				else if (deckManager.comboNumber == 2)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Wall, 5, 0);
				}
				else
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Wall, 7, 0);
				}
				player.GetComponent<Player>().AddMinion(minion);
				break;
			case CardName.Boost:
				player.GetComponent<Player>().ChangeMinionDamage(deckManager.comboNumber);
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = boostAnimation;
				break;
		}
		enemy.GetComponent<Enemy>().TakeDamage(player.GetComponent<Player>().MinionTurn(), false);
	}

    private void EnemyTurn()
    {
		if (enemy.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
		{
			enemy.GetComponent<Enemy>().TakeDamage(2, true);
		}
		if (enemy.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Asleep))
		{
			if (Random.Range(0, 4) > 0)
			{
				return;
			}
			enemy.GetComponent<StatusEffect>().DeleteStatus(Status.Asleep);
		}

		switch (enemyCard)
		{
			case CardName.Lightning:
				int[] intArray = null;
				if (enemy.GetComponent<Enemy>().comboNumber == 1)
				{
					intArray = new int[3] { 1, 2, 3 };
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 2)
				{
					intArray = new int[3] { 2, 3, 4 };
				}
				else
				{
					intArray = new int[3] { 3, 4, 5 };
				}
				if (playerCard == CardName.Reflect)
				{
					if (deckManager.comboNumber == 1)
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else if (deckManager.comboNumber == 2)
					{
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray) / 2, false);
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
					}
					GameObject effect1 = Instantiate(cardEffectPrefab, new Vector3(-7f, -1, 0), Quaternion.identity);
					effect1.GetComponent<Animator>().runtimeAnimatorController = reflectAnimation;
				}
				else
				{
					player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
				}
				GameObject effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = lightningAnimation;
				break;
			case CardName.Fireball:
				if (enemy.GetComponent<Enemy>().comboNumber == 1)
				{
					intArray = new int[3] { 1, 1, 2 };
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 2)
				{
					intArray = new int[3] { 2, 3, 3 };
				}
				else
				{
					intArray = new int[3] { 5, 6, 7 };
				}
				if (playerCard == CardName.Reflect)
				{
					if (deckManager.comboNumber == 1)
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else if (deckManager.comboNumber == 2)
					{
						player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray) / 2, false);
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
					}
					else
					{
						enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray), false);
					}
					effect = Instantiate(cardEffectPrefab, new Vector3(-7f, -1, 0), Quaternion.identity);
					effect.GetComponent<Animator>().runtimeAnimatorController = reflectAnimation;
				}
				else
				{
					player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray), false);
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = fireballAnimation;
				break;
			case CardName.Landslide:
				if (playerCard == CardName.Reflect)
				{
					if (deckManager.comboNumber == 1)
					{
						enemy.GetComponent<Enemy>().TakeDamage(3, false);
						player.GetComponent<Player>().TakeDamage(3, false);
					}
					else if (deckManager.comboNumber == 2)
					{
						player.GetComponent<Player>().TakeDamage(1, false);
						enemy.GetComponent<Enemy>().TakeDamage(3, false);
					}
					else
					{
						enemy.GetComponent<Enemy>().TakeDamage(3, false);
					}
					effect = Instantiate(cardEffectPrefab, new Vector3(-7f, -1, 0), Quaternion.identity);
					effect.GetComponent<Animator>().runtimeAnimatorController = reflectAnimation;
				}
				else
				{
					player.GetComponent<Player>().TakeDamage(3, false);
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = landslideAnimation;
				break;
			case CardName.Freeze:
				player.GetComponent<StatusEffect>().AddStatus(Status.Frozen, enemy.GetComponent<Enemy>().comboNumber);
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = freezeAnimation;
				break;
			case CardName.Lullaby:
				if (enemy.GetComponent<Enemy>().comboNumber == 1 && Random.Range(0, 4) < 2)
				{
					player.GetComponent<StatusEffect>().AddStatus(Status.Asleep, 2);
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 2 && Random.Range(0, 4) < 3)
				{
					player.GetComponent<StatusEffect>().AddStatus(Status.Asleep, 2);
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 3)
				{
					player.GetComponent<StatusEffect>().AddStatus(Status.Asleep, 2);
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = lullabyAnimation;
				break;
			case CardName.Frighten:
				player.GetComponent<StatusEffect>().AddStatus(Status.Frightened, enemy.GetComponent<Enemy>().comboNumber + 1);
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = frightenAnimation;
				break;
			case CardName.Poison:
				player.GetComponent<StatusEffect>().AddStatus(Status.Poisoned, enemy.GetComponent<Enemy>().comboNumber);
				effect = Instantiate(cardEffectPrefab, new Vector3(-7.5f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = poisonAnimation;
				break;
			case CardName.ComboBreaker:
				if (enemy.GetComponent<Enemy>().comboNumber == 11)
				{
					deckManager.comboNumber = 0;
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 2)
				{
					deckManager.comboNumber = 0;
					deckManager.SetBroken();
				}
				else
				{
					deckManager.comboNumber = 0;
					enemy.GetComponent<Enemy>().comboDelayColor = deckManager.GetComboColor();
					deckManager.comboDelayTimer = 1;
				}
				break;
			case CardName.Revivify:
				enemy.GetComponent<Enemy>().TakeDamage((enemy.GetComponent<Enemy>().comboNumber + 1) * -1, false);
				break;
			case CardName.Cure:
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				if (enemy.GetComponent<Enemy>().comboNumber > 1)
				{
					enemy.GetComponent<Enemy>().CureMinions();
				}
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = cureAnimation;
				break;
			case CardName.Tree:
				GameObject minion = Instantiate(minionPrefab, enemy.transform.position - new Vector3(2.5f, .3f, 0), Quaternion.identity);
				minion.GetComponent<SpriteRenderer>().flipX = true;
				if (enemy.GetComponent<Enemy>().comboNumber == 1)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Tree, 2, 0);
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 2)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Tree, 3, 1);
				}
				else
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Tree, 5, 1);
				}
				enemy.GetComponent<Enemy>().AddMinion(minion);
				break;
			case CardName.Spikey:
				minion = Instantiate(minionPrefab, enemy.transform.position - new Vector3(2.5f, .3f, 0), Quaternion.identity);
				minion.GetComponent<SpriteRenderer>().flipX = true;
				if (enemy.GetComponent<Enemy>().comboNumber == 1)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Spikey, 1, 1);
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 2)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Spikey, 2, 2);
				}
				else
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Spikey, 3, 3);
				}
				enemy.GetComponent<Enemy>().AddMinion(minion);
				break;
			case CardName.Wall:
				minion = Instantiate(minionPrefab, enemy.transform.position - new Vector3(2.5f, .3f, 0), Quaternion.identity);
				minion.GetComponent<SpriteRenderer>().flipX = true;
				if (enemy.GetComponent<Enemy>().comboNumber == 1)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Wall, 3, 0);
				}
				else if (enemy.GetComponent<Enemy>().comboNumber == 2)
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Wall, 5, 0);
				}
				else
				{
					minion.GetComponent<Minions>().CreateMinion(Minion.Wall, 7, 0);
				}
				enemy.GetComponent<Enemy>().AddMinion(minion);
				break;
			case CardName.Boost:
				enemy.GetComponent<Enemy>().ChangeMinionDamage(enemy.GetComponent<Enemy>().comboNumber);
				effect = Instantiate(cardEffectPrefab, new Vector3(7.7f, -1, 0), Quaternion.identity);
				effect.GetComponent<Animator>().runtimeAnimatorController = boostAnimation;
				break;
		}

		player.GetComponent<Player>().TakeDamage(enemy.GetComponent<Enemy>().MinionTurn(), false);
	}

    private int DamageRandomizer(int[] intArray)
    {
        return intArray[Random.Range(0, intArray.Length)];
    }
}
