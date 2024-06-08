using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    List<CardName> deck = new List<CardName>();
	List<CardName> backupDeck = new List<CardName>();
	List<CardName> discardDeck = new List<CardName>();
	public List<GameObject> selectedCards = new List<GameObject>();
	public GameObject cardPrefab;
	public GameObject canvas;

	private CardColor comboColor = CardColor.None;
	public int comboNumber;
	private bool comboDelay = false;
	public int comboDelayTimer = 0;
	public CardColor comboDelayColor = CardColor.None;
	[SerializeField] private Button endTurnButton;
	[SerializeField] private GameObject comboCounter;
	[SerializeField] private List<Sprite> comboCounterImages = new List<Sprite>();

	private bool comboBroken = false;


    // Start is called before the first frame update
    void Start()
    {
		/*
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
		*/
		//ChooseCards();
	}

	public void SetDeck(Dictionary<CardName, int> map)
	{
		List<CardName> cards = new List<CardName>();
		foreach (CardName card in map.Keys)
		{
			for (int i = 0; i < map[card]; i++)
			{
				cards.Add(card);
			}
		}

		deck = cards;
		foreach (CardName card in deck)
		{
			backupDeck.Add(card);
		}
		ChooseCards();
	}

	public void ResetDeck()
	{
		deck.Clear();
		foreach (CardName card in backupDeck)
		{
			deck.Add(card);
		}
		comboCounter.GetComponent<Image>().sprite = comboCounterImages[0];
		endTurnButton.GetComponent<Button>().interactable = true;
		ChooseCards();
	}

    public void ChooseCards()
	{
		if (comboBroken)
		{
			comboBroken = false;
			UpdateCombo(CardColor.None);
		}

		while (selectedCards.Count > 0)
		{
			discardDeck.Add(selectedCards[0].GetComponent<Card>().cardName);
			Destroy(selectedCards[0]);
			selectedCards.RemoveAt(0);
		}

		for (int i = 0; i < 3; i++)
		{
			if (deck.Count == 0)
			{
				if (discardDeck.Count > 0)
				{
					while (discardDeck.Count > 0)
					{
						deck.Add(discardDeck[0]);
						discardDeck.RemoveAt(0);
					}
				}
				else
				{
					i = 4;
					break;
				}
			}
			GameObject card = Instantiate(cardPrefab, new Vector3((i * 200) + 150, 160, 0), Quaternion.identity);
			selectedCards.Add(card);
			int cardValue = UnityEngine.Random.Range(0, deck.Count - 1);
			card.GetComponent<Card>().cardName = deck[cardValue];
			deck.RemoveAt(cardValue);
			card.transform.SetParent(canvas.transform);
		}

		StartCoroutine(CheckCardValidity());

	}

	private IEnumerator CheckCardValidity()
	{
		yield return new WaitForSeconds(0.1f);
		foreach (GameObject c in selectedCards)
		{
			if (c.GetComponent<Button>().interactable)
			{
				endTurnButton.GetComponent<Button>().interactable = false;
			}
		}
	}

	public void DeleteCards()
	{
		while (selectedCards.Count > 0)
		{
			discardDeck.Add(selectedCards[0].GetComponent<Card>().cardName);
			Destroy(selectedCards[0]);
			selectedCards.RemoveAt(0);
		}
	}

	public void SetBroken()
	{
		comboBroken = true;
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

		switch (comboColor)
		{
			case CardColor.Red:
				{
					comboCounter.GetComponent<Image>().sprite = comboCounterImages[comboNumber];
					break;
				}
			case CardColor.Blue:
				{
					comboCounter.GetComponent<Image>().sprite = comboCounterImages[comboNumber + 3];
					break;
				}
			case CardColor.Green:
				{
					comboCounter.GetComponent<Image>().sprite = comboCounterImages[comboNumber + 6];
					break;
				}
			case CardColor.Yellow:
				{
					comboCounter.GetComponent<Image>().sprite = comboCounterImages[comboNumber + 9];
					break;
				}
		}
	}

	public void GameEnd()
	{
		while (selectedCards.Count > 0)
		{
			Destroy(selectedCards[0]);
			selectedCards.RemoveAt(0);
		}
		endTurnButton.GetComponent<Button>().interactable = false;
	}

	public CardColor GetComboColor()
	{
		return comboColor;
	}

}
