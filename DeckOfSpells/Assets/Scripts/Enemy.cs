using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    private int health = 105;

	List<CardName> deck = new List<CardName>();
	List<CardName> discardDeck = new List<CardName>();
	List<CardName> selectedCards = new List<CardName>();
    [SerializeField] private GameObject turnManager;
	[SerializeField] private TMP_Text cardSelectedText;
	private int cardPriority = 0;

	private CardColor comboColor = CardColor.None;
	public int comboNumber;
	private bool comboDelay = false;
	public int comboDelayTimer = 0;
	public CardColor comboDelayColor = CardColor.None;
	private bool comboBroken = false;

	// Start is called before the first frame update
	void Start()
    {
        healthText.text = "105";

		//deck.Add(CardName.Fireball);
		//deck.Add(CardName.Fireball);
		//deck.Add(CardName.Lightning);
		//deck.Add(CardName.Fireball);
		//deck.Add(CardName.Fireball);
		//deck.Add(CardName.Landslide);
		//deck.Add(CardName.ComboBreaker);
		//deck.Add(CardName.Freeze);
		//deck.Add(CardName.Freeze);
		//deck.Add(CardName.Freeze);
		deck.Add(CardName.Reflect);
		deck.Add(CardName.Reflect);
		deck.Add(CardName.Reflect);


		SelectCard();
	}
    public void SelectCard()
    {
        while (selectedCards.Count > 0)
        {
            deck.Add(selectedCards[0]);
            selectedCards.RemoveAt(0);
        }
        
		for (int i = 0; i < 3; i++)
		{
			if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frozen))
			{
				if (Random.Range(0, 3) == 0)
				{
					continue;
				}
			}
			int cardValue = Random.Range(0, deck.Count);
			if (comboDelay && !CheckCardValidity(deck[cardValue]))
			{
				continue;
			}
			selectedCards.Add(deck[cardValue]);
			deck.RemoveAt(cardValue);
		}

		if (selectedCards.Count > 0)
		{
			CardName selectedCard = selectedCards[0];
			cardSelectedText.text = selectedCard.ToString();
			switch (selectedCard)
			{
				case CardName.Fireball:
				case CardName.Lightning:
				case CardName.Landslide:
					cardPriority = 100 + comboNumber;
					UpdateCombo(CardColor.Red);
					break;
				default:
					cardPriority = 50 + comboNumber;
					UpdateCombo(CardColor.Green);
					break;
			}
			if (comboBroken)
			{
				comboBroken = false;
				UpdateCombo(CardColor.None);
			}
			turnManager.GetComponent<TurnManager>().SetEnemyCard(selectedCard, cardPriority);
		}
		else
		{
			UpdateCombo(CardColor.None);
			cardSelectedText.text = "None";
			turnManager.GetComponent<TurnManager>().SetEnemyCard(CardName.None, 0);
		}
    }

    public void TakeDamage(int damage)
    {
		if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frightened) && damage > 0)
		{
			damage = (int)(damage * 1.5f);
		}
        health -= damage;
		healthText.text = health.ToString();
		if (health < 1)
        {
            Destroy(gameObject);
        }
    }

	public void SetBroken()
	{
		comboBroken = true;
	}

    public int GetHealth()
    {
        return health;
    }

	public void UpdateCombo(CardColor color)
	{
		if (color == comboColor)
		{
			if (comboNumber < 3)
			{
				comboNumber++;
			}
			if (comboNumber == 3)
			{
				comboDelay = true;
				comboDelayColor = color;
			}
		}
		else
		{
			comboColor = color;
			comboNumber = 1;
		}

		if (comboDelay)
		{
			comboDelayTimer++;
			if (comboDelayTimer > 2)
			{
				comboDelay = false;
				comboDelayTimer = 0;
				comboDelayColor = CardColor.None;
			}
		}
	}

	private bool CheckCardValidity(CardName cardToCheck)
	{
		if (comboDelayColor == CardColor.Red)
		{
			if (cardToCheck == CardName.Lightning || cardToCheck == CardName.Landslide || cardToCheck == CardName.Fireball)
			{
				return false;
			}
		}
		else if (comboDelayColor == CardColor.Blue)
		{
			if (cardToCheck == CardName.Spikey || cardToCheck == CardName.Tree || cardToCheck == CardName.Wall)
			{
				return false;
			}
		}
		else if (comboDelayColor == CardColor.Yellow)
		{
			if (cardToCheck == CardName.Freeze || cardToCheck == CardName.Frighten || cardToCheck == CardName.Poison || cardToCheck == CardName.Lullaby || cardToCheck == CardName.ComboBreaker)
			{
				return false;
			}
		}
		else if (cardToCheck == CardName.ComboBooster || cardToCheck == CardName.Cure || cardToCheck == CardName.Boost || cardToCheck == CardName.Revivify || cardToCheck == CardName.Reflect)
		{
			return false;
		}

		return true;
	}

	public CardColor GetComboColor()
	{
		return comboColor;
	}
}
