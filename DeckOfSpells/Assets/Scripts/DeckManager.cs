using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckManager : MonoBehaviour
{
    List<CardName> deck = new List<CardName>();
	List<CardName> discardDeck = new List<CardName>();
	public List<GameObject> selectedCards = new List<GameObject>();
	public GameObject cardPrefab;
	public GameObject canvas;

	public TMP_Text deckText;
	public TMP_Text discardText;


    // Start is called before the first frame update
    void Start()
    {
		deck.Add(CardName.Fireball);
        deck.Add(CardName.Lightning);
		deck.Add(CardName.Tree);
		deck.Add(CardName.Spikey);
		deck.Add(CardName.Wall);
		deck.Add(CardName.Revivify);
		deck.Add(CardName.Boost);
		deck.Add(CardName.Cure);
		deck.Add(CardName.ComboBooster);
		deck.Add(CardName.Frighten);
		deck.Add(CardName.Freeze);
		deck.Add(CardName.ComboBreaker);

		ChooseCards();
	}

    public void ChooseCards()
	{
		while (selectedCards.Count > 0)
		{
			discardDeck.Add(selectedCards[0].GetComponent<Card>().cardName);
			Destroy(selectedCards[0]);
			selectedCards.RemoveAt(0);
		}

		for (int i = 0; i < 3; i++)
		{
			GameObject card = Instantiate(cardPrefab, new Vector3((i * 200) + 150, 160, 0), Quaternion.identity);
			selectedCards.Add(card);
			int cardValue = UnityEngine.Random.Range(0, deck.Count - 1);
			card.GetComponent<Card>().cardName = deck[cardValue];
			deck.RemoveAt(cardValue);
			card.transform.SetParent(canvas.transform);
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
				}
			}
		}

		/*
		for (int i = 0; i < selectedCards.Count; i++)
		{
			discardDeck.Add(selectedCards[i].GetComponent<Card>().cardName);
		}
		*/

		deckText.text = "Cards left in deck: " + deck.Count;
		discardText.text = "Cards in discard: " + discardDeck.Count;

	}


}
