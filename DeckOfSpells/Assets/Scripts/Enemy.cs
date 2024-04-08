using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    private int health = 20;
	private int startHealth;

	List<CardName> deck = new List<CardName>();
	List<CardName> selectedCards = new List<CardName>();
    [SerializeField] private GameObject turnManager;
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject deckManager;
	private int cardPriority = 0;

	private CardColor comboColor = CardColor.None;
	public int comboNumber;
	private bool comboDelay = false;
	public int comboDelayTimer = 0;
	public CardColor comboDelayColor = CardColor.None;
	private bool comboBroken = false;

	private List<GameObject> minions = new List<GameObject>();

	[SerializeField] private float redFavor = 1;
	[SerializeField] private float greenFavor = 1;
	[SerializeField] private float blueFavor = 1;
	[SerializeField] private float yellowFavor = 1;

	// Start is called before the first frame update
	void Start()
    {
        healthText.text = "20";
		startHealth = health;

		deck.Add(CardName.Fireball);
		deck.Add(CardName.Fireball);
		deck.Add(CardName.Lightning);
		deck.Add(CardName.Lightning);
		deck.Add(CardName.Landslide);
		deck.Add(CardName.Landslide);
		deck.Add(CardName.Spikey);
		deck.Add(CardName.Spikey);
		deck.Add(CardName.Tree);
		deck.Add(CardName.Tree);
		deck.Add(CardName.Wall);
		deck.Add(CardName.Wall);
		deck.Add(CardName.Boost);
		deck.Add(CardName.Boost);
		deck.Add(CardName.Cure);
		deck.Add(CardName.Cure);
		deck.Add(CardName.ComboBooster);
		deck.Add(CardName.ComboBooster);
		deck.Add(CardName.Reflect);
		deck.Add(CardName.Reflect);
		deck.Add(CardName.Freeze);
		deck.Add(CardName.Freeze);
		deck.Add(CardName.Frighten);
		deck.Add(CardName.Frighten);
		deck.Add(CardName.Poison);
		deck.Add(CardName.Poison);
		deck.Add(CardName.Lullaby);
		deck.Add(CardName.Lullaby);
		deck.Add(CardName.Revivify);
		deck.Add(CardName.Revivify);

		//deck.Add(CardName.Reflect);


		SelectCard();
	}

	private void Update()
	{
		//Debug.Log(player.GetComponent<Animator>().GetInteger("Color"));
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
			CardName selectedCard = SelectCardAI();
			switch (selectedCard)
			{
				case CardName.Fireball:
				case CardName.Lightning:
				case CardName.Landslide:
					cardPriority = 100 + comboNumber;
					UpdateCombo(CardColor.Red);
					break;
				case CardName.Wall:
				case CardName.Spikey:
				case CardName.Tree:
					cardPriority = 75 + comboNumber;
					UpdateCombo(CardColor.Blue);
					break;
				case CardName.Poison:
				case CardName.Lullaby:
				case CardName.Freeze:
				case CardName.Frighten:
					cardPriority = 25 + comboNumber;
					UpdateCombo(CardColor.Yellow);
					break;
				case CardName.ComboBreaker:
					cardPriority += 1000;
					UpdateCombo(CardColor.Yellow);
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
			turnManager.GetComponent<TurnManager>().SetEnemyCard(CardName.None, 0);
		}
    }

	private CardName SelectCardAI()
	{
		float[] probabilities = new float[3];

		for (int i = 0; i < selectedCards.Count; i++)
		{
			switch (selectedCards[i])
			{
				case CardName.Fireball:
				case CardName.Lightning:
				case CardName.Landslide:
					probabilities[i] = redFavor;
					if (comboColor == CardColor.Red)
					{
						probabilities[i] *= comboNumber;
					}
					if (player.GetComponent<Player>().GetHealth() < 6)
					{
						probabilities[i] += 10;
					}
					break;
				case CardName.Spikey:
				case CardName.Tree:
				case CardName.Wall:
					probabilities[i] = blueFavor;
					if (comboColor == CardColor.Blue)
					{
						probabilities[i] *= comboNumber;
					}
					probabilities[i] *= (health / startHealth);
					break;
				case CardName.Revivify:
				case CardName.Boost:
				case CardName.ComboBooster:
				case CardName.Cure:
				case CardName.Reflect:
					probabilities[i] = greenFavor;
					if (comboColor == CardColor.Green)
					{
						probabilities[i] *= comboNumber;
					}
					if (selectedCards[i] == CardName.Boost)
					{
						probabilities[i] *= minions.Count;
					}
					else if (selectedCards[i] == CardName.Cure && GetComponent<StatusEffect>().GetStatusList().Count == 0)
					{
						probabilities[i] = 0;
					}
					else if (selectedCards[i] == CardName.ComboBooster && comboNumber >= 2)
					{
						probabilities[i] = 0;
					}
					break;
				case CardName.Frighten:
				case CardName.Lullaby:
				case CardName.Poison:
				case CardName.Freeze:
					probabilities[i] = yellowFavor;
					if (comboColor == CardColor.Yellow)
					{
						probabilities[i] *= comboNumber;
					}
					break;
			}
		}

		if (selectedCards.Count == 0)
		{
			return CardName.None;
		}
		else if (selectedCards.Count == 1)
		{
			return selectedCards[0];
		}
		else if (selectedCards.Count == 2)
		{
			if (probabilities[0] > probabilities[1])
			{
				return selectedCards[0];
			}
			return selectedCards[1];
		}

		float rnd = Random.Range(0, redFavor + blueFavor + greenFavor + yellowFavor);
        if (rnd > probabilities[1] + probabilities[2])
        {
			return selectedCards[0];
        }
		else if (probabilities[1] > probabilities[2])
		{
			return selectedCards[1];
		}
		else
		{
			return selectedCards[2];
		}
	}

    public void TakeDamage(int damage, bool poison)
    {
		if (poison)
		{
			for (int i = 0; i < minions.Count; i++)
			{
				minions[i].GetComponent<Minions>().TakeDamage(damage, true);
				if (minions[i].GetComponent<Minions>().GetHealth() <= 0)
				{
					Destroy(minions[i]);
					minions.RemoveAt(i);
				}
			}
			GetComponent<Animator>().SetTrigger("Hurt");
			health -= damage;
		}
		else
		{
			for (int i = 0; i < minions.Count; i++)
			{
				if (damage > 0)
				{
					damage = minions[i].GetComponent<Minions>().TakeDamage(damage, false);
				}
				if (minions[i].GetComponent<Minions>().GetHealth() <= 0)
				{
					//Destroy(minions[i]);
					minions.RemoveAt(i);
				}
			}
			if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frightened))
			{
				damage = (int)(damage * 1.5f);
			}
			if (damage > 0)
			{
				GetComponent<Animator>().SetTrigger("Hurt");
				health -= damage;
			}
		}
		healthText.text = health.ToString();
		if (health < 1)
		{
			GetComponent<Animator>().SetBool("Defeated", true);
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
	public void ChangeMinionDamage(int damageChange)
	{
		foreach (GameObject minion in minions)
		{
			minion.GetComponent<Minions>().ChangeDamage(damageChange);
		}
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

	public int MinionTurn()
	{
		int damage = 0;

		foreach (GameObject minion in minions)
		{
			damage += minion.GetComponent<Minions>().GetDamage();
		}

		if (damage > 0)
		{
			player.GetComponent<Animator>().SetTrigger("Hurt");
		}

		return damage;
	}

	public void AddMinion(GameObject minion)
	{
		foreach (GameObject min in minions)
		{
			min.transform.position -= new Vector3(2, 0, 0);
		}
		minions.Add(minion);
	}

	public void CureMinions()
	{
		foreach (GameObject minion in minions)
		{
			minion.GetComponent<StatusEffect>().UpdateStatus();
			minion.GetComponent<StatusEffect>().UpdateStatus();
			minion.GetComponent<StatusEffect>().UpdateStatus();
			minion.GetComponent<StatusEffect>().UpdateStatus();
		}
	}
}
