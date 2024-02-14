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
        if (playerCard != CardName.None)
        {
			if (playerPriority >= enemyPriority)
			{
				Debug.Log("Player first");
				PlayerTurn();
				player.GetComponent<StatusEffect>().UpdateStatus();
				if (enemy.GetComponent<Enemy>().GetHealth() > 0)
				{
					EnemyTurn();
					enemy.GetComponent<StatusEffect>().UpdateStatus();
				}
			}
			else
			{
				Debug.Log("Enemy first");
				EnemyTurn();
				enemy.GetComponent<StatusEffect>().UpdateStatus();
				if (player.GetComponent<Player>().GetHealth() > 0)
				{
					PlayerTurn();
					player.GetComponent<StatusEffect>().UpdateStatus();
				}
			}
		}
        else
        {
            EnemyTurn();
			enemy.GetComponent<StatusEffect>().UpdateStatus();
		}
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
        if (player.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frozen))
        {
            if (Random.Range(0, 3) == 0)
            {
                return;
            }
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
		}
    }

    private void EnemyTurn()
    {
		if (enemy.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frozen))
		{
			if (Random.Range(0, 3) == 0)
			{
				return;
			}
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
		}
	}

    private int DamageRandomizer(int[] intArray)
    {
        return intArray[Random.Range(0, intArray.Length)];
    }
}
