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
	private int cardPriority = 0;

	private CardColor comboColor = CardColor.None;
	public int comboNumber;
	private bool comboDelay = false;
	public int comboDelayTimer = 0;
	public CardColor comboDelayColor = CardColor.None;

	// Start is called before the first frame update
	void Start()
    {
        healthText.text = "105";

		deck.Add(CardName.Fireball);
		deck.Add(CardName.Fireball);
		deck.Add(CardName.Lightning);
		deck.Add(CardName.Fireball);
		deck.Add(CardName.Fireball);
        deck.Add(CardName.Landslide);
		deck.Add(CardName.ComboBreaker);

        SelectCard();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCard()
    {
        while (selectedCards.Count > 0)
        {
            deck.Add(selectedCards[0]);
            selectedCards.RemoveAt(0);
        }
        
        while (deck.Count < 3)
        {
            int i = Random.Range(0, deck.Count);
			selectedCards.Add(deck[i]);
            deck.RemoveAt(i);
        }

		CardName selectedCard = deck[0];
		switch (selectedCard)
		{
			case CardName.Fireball: case CardName.Lightning: case CardName.Landslide:
				cardPriority = 100 + comboNumber;
				UpdateCombo(CardColor.Red);
				break;
			default:
				cardPriority = 50 + comboNumber;
				UpdateCombo(CardColor.Green);
				break;
		}
		turnManager.GetComponent<TurnManager>().SetEnemyCard(deck[0], cardPriority);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
		healthText.text = health.ToString();
		if (health < 1)
        {
            Destroy(gameObject);
        }
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

	public CardColor GetComboColor()
	{
		return comboColor;
	}
}
