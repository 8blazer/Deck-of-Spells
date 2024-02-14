using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    
    private CardColor cardColor;
    public CardName cardName;
    [SerializeField] private TMP_Text nameText;
    private GameObject deckManager;
    [SerializeField] private GameObject endTurnButton;
    private GameObject turnManager;
    private int cardPriority = 0;


    // Start is called before the first frame update
    void Start()
    {
        deckManager = GameObject.FindWithTag("GameController");
        endTurnButton = GameObject.Find("EndTurnButton");
        turnManager = GameObject.Find("TurnManager");
        nameText.text = cardName.ToString();
        switch (cardName)
        {
            case CardName.Fireball: case CardName.Lightning: case CardName.Landslide:
                cardColor = CardColor.Red;
                GetComponent<Image>().color = new Color(255, 0, 0);
                cardPriority = 100;
                if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Red)
                {
                    cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
                }
                break;
            case CardName.Tree: case CardName.Spikey: case CardName.Wall:
                cardColor = CardColor.Blue;
                GetComponent<Image>().color = new Color(0, 0, 255);
				cardPriority = 75;
				if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Blue)
				{
					cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
				}
				break;
            case CardName.Revivify: case CardName.Cure: case CardName.Boost: case CardName.ComboBooster: case CardName.Reflect:
                cardColor = CardColor.Green;
                GetComponent<Image>().color = new Color(0, 255, 0);
				cardPriority = 50;
				if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Green)
				{
					cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
				}
				break;
            default:
                cardColor = CardColor.Yellow;
                GetComponent<Image>().color = new Color(255, 255, 0);
				cardPriority = 25;
                nameText.color = new Color(0, 0, 0);
				if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Yellow)
				{
					cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
				}
				break;
        }
        if (cardColor == deckManager.GetComponent<DeckManager>().comboDelayColor)
        {
            GetComponent<Button>().interactable = false;
        }
        if (cardName == CardName.ComboBreaker)
        {
            cardPriority = 999;
        }

    }

    public void SelectCard()
    {
        endTurnButton.GetComponent<EndTurnButton>().setCard(this);
    }
	public void PlayCard()
    {
		deckManager.GetComponent<DeckManager>().UpdateCombo(cardColor);
		deckManager.GetComponent<DeckManager>().selectedCards.Remove(this.gameObject);
		turnManager.GetComponent<TurnManager>().SetPlayerCard(cardName, cardPriority);
		Destroy(this.gameObject);
	}
}

public enum CardColor { Red, Blue, Green, Yellow, None }
public enum CardName { 
    Fireball, 
    Lightning, 
    Landslide,
    Tree, 
    Spikey, 
    Wall, 
    Revivify, 
    Cure, 
    Boost, 
    ComboBooster, 
    Reflect, 
    Frighten,
    Lullaby,
    Freeze,
    Poison,
    ComboBreaker,
    None
}
