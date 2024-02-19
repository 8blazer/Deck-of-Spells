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



	void Start()
    {
        deckManager = GameObject.FindWithTag("GameController").GetComponent<DeckManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerCard(CardName cardName, int priority)
    {
        playerCard = cardName;
        playerPriority = priority;
        PlayTurn();
    }

	public void SetEnemyCard(CardName cardName, int priority)
	{
		enemyCard = cardName;
        enemyPriority = priority;
	}
	public void PlayTurn()
    {
        if (playerCard != CardName.None && enemyCard != CardName.None)
        {
			if (playerPriority >= enemyPriority)
			{
				Debug.Log("Player first");
				PlayerTurn();
				if (player.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
				{
					player.GetComponent<Player>().TakeDamage(3);
				}
				player.GetComponent<StatusEffect>().UpdateStatus();
				if (enemy.GetComponent<Enemy>().GetHealth() > 0)
				{
					EnemyTurn();
					if (enemy.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
					{
						enemy.GetComponent<Enemy>().TakeDamage(3);
					}
					enemy.GetComponent<StatusEffect>().UpdateStatus();
				}
			}
			else
			{
				Debug.Log("Enemy first");
				EnemyTurn();
				if (enemy.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
				{
					enemy.GetComponent<Enemy>().TakeDamage(3);
				}
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				if (player.GetComponent<Player>().GetHealth() > 0)
				{
					PlayerTurn();
					if (player.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
					{
						player.GetComponent<Player>().TakeDamage(3);
					}
					player.GetComponent<StatusEffect>().UpdateStatus();
				}
			}
		}
        else if (playerCard != CardName.None)
        {
            EnemyTurn();
			if (enemy.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
			{
				enemy.GetComponent<Enemy>().TakeDamage(3);
			}
			enemy.GetComponent<StatusEffect>().UpdateStatus();
			player.GetComponent<StatusEffect>().UpdateStatus();
		}
		else
		{
			PlayerTurn();
			if (player.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Poisoned))
			{
				player.GetComponent<Player>().TakeDamage(3);
			}
			enemy.GetComponent<StatusEffect>().UpdateStatus();
			player.GetComponent<StatusEffect>().UpdateStatus();
		}

		//Check to see if the game has ended
		if (player.GetComponent<Player>().GetHealth() < 1)
		{
			outcomeText.text = "Enemy Wins!";
			deckManager.GameEnd();
		}
		else if (enemy.GetComponent<Enemy>().GetHealth() < 1)
		{
			outcomeText.text = "Player Wins!";
			deckManager.GameEnd();
		}
		else
		{
			enemy.GetComponent<Enemy>().SelectCard();
			deckManager.ChooseCards();
		}
    }

    private void PlayerTurn()
    {
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
				enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray));
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
				enemy.GetComponent<Enemy>().TakeDamage(DamageRandomizer(intArray));
				break;
			case CardName.Landslide:
                enemy.GetComponent<Enemy>().TakeDamage(3);
				break;
			case CardName.Freeze:
				enemy.GetComponent<StatusEffect>().AddStatus(Status.Frozen, deckManager.comboNumber);
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
				break;
			case CardName.Frighten:
				enemy.GetComponent<StatusEffect>().AddStatus(Status.Frightened, deckManager.comboNumber + 1);
				break;
			case CardName.Poison:
				enemy.GetComponent<StatusEffect>().AddStatus(Status.Poisoned, deckManager.comboNumber);
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
					enemy.GetComponent<Enemy>().comboDelayColor = enemy.GetComponent <Enemy>().GetComboColor();
					enemy.GetComponent<Enemy>().comboDelayTimer = 1;
				}
				break;
			case CardName.Revivify:
				player.GetComponent<Player>().TakeDamage((deckManager.comboNumber + 1) * -1);
				break;
			case CardName.Cure:
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				player.GetComponent<StatusEffect>().UpdateStatus();
				break;
		}
    }

    private void EnemyTurn()
    {
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
				player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray));
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
				player.GetComponent<Player>().TakeDamage(DamageRandomizer(intArray));
				break;
			case CardName.Landslide:
				player.GetComponent<Player>().TakeDamage(3);
				break;
			case CardName.Freeze:
				player.GetComponent<StatusEffect>().AddStatus(Status.Frozen, enemy.GetComponent<Enemy>().comboNumber);
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
				break;
			case CardName.Frighten:
				player.GetComponent<StatusEffect>().AddStatus(Status.Frightened, enemy.GetComponent<Enemy>().comboNumber + 1);
				break;
			case CardName.Poison:
				player.GetComponent<StatusEffect>().AddStatus(Status.Poisoned, enemy.GetComponent<Enemy>().comboNumber);
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
				enemy.GetComponent<Enemy>().TakeDamage((enemy.GetComponent<Enemy>().comboNumber + 1) * -1);
				break;
			case CardName.Cure:
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				break;
		}
	}

    private int DamageRandomizer(int[] intArray)
    {
        return intArray[Random.Range(0, intArray.Length)];
    }
}
