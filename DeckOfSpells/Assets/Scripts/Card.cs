using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class Card : MonoBehaviour
{
    
    private CardColor cardColor;
    public CardName cardName;
    //[SerializeField] private TMP_Text nameText;
    private GameObject deckManager;
    [SerializeField] private GameObject endTurnButton;
    private GameObject turnManager;
    private GameObject player;
    private int cardPriority = 0;
    [SerializeField] private Sprite fireballCardSprite;
	[SerializeField] private Sprite lightningCardSprite;
	[SerializeField] private Sprite landslideCardSprite;
	[SerializeField] private Sprite treeCardSprite;
	[SerializeField] private Sprite wallCardSprite;
	[SerializeField] private Sprite spikeyCardSprite;
	[SerializeField] private Sprite revivifyCardSprite;
	[SerializeField] private Sprite cureCardSprite;
	[SerializeField] private Sprite boostCardSprite;
	[SerializeField] private Sprite comboBoosterCardSprite;
	[SerializeField] private Sprite reflectCardSprite;
	[SerializeField] private Sprite freezeCardSprite;
	[SerializeField] private Sprite frightenCardSprite;
	[SerializeField] private Sprite poisonCardSprite;
	[SerializeField] private Sprite lullabyCardSprite;
	[SerializeField] private Sprite comboBreakerCardSprite;
	[SerializeField] private GameObject frozenCardEffect;

	// Start is called before the first frame update
	void Start()
    {
        deckManager = GameObject.FindWithTag("GameController");
        endTurnButton = GameObject.Find("EndTurnButton");
        turnManager = GameObject.Find("TurnManager");
        player = GameObject.FindWithTag("Player");
        //nameText.text = cardName.ToString();
        switch (cardName)
        {
            case CardName.Fireball: case CardName.Lightning: case CardName.Landslide:
                cardColor = CardColor.Red;
                //GetComponent<Image>().color = new Color(255, 0, 0);
                cardPriority = 100;
                if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Red)
                {
                    cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
                }
                if (cardName == CardName.Fireball)
                {
                    GetComponent<Image>().sprite = fireballCardSprite;
                }
                else if (cardName == CardName.Lightning)
                {
                    GetComponent<Image>().sprite = lightningCardSprite;
                }
                else
                {
                    GetComponent<Image>().sprite = landslideCardSprite;
                }
                break;
            case CardName.Tree: case CardName.Spikey: case CardName.Wall:
                cardColor = CardColor.Blue;
                //GetComponent<Image>().color = new Color(0, 0, 255);
				cardPriority = 75;
				if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Blue)
				{
					cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
				}
				if (cardName == CardName.Tree)
				{
					GetComponent<Image>().sprite = treeCardSprite;
				}
				else if (cardName == CardName.Spikey)
				{
					GetComponent<Image>().sprite = spikeyCardSprite;
				}
				else
				{
					GetComponent<Image>().sprite = wallCardSprite;
				}
				break;
            case CardName.Revivify: case CardName.Cure: case CardName.Boost: case CardName.ComboBooster: case CardName.Reflect:
                cardColor = CardColor.Green;
                //GetComponent<Image>().color = new Color(0, 255, 0);
				cardPriority = 50;
				if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Green)
				{
					cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
				}
				if (cardName == CardName.Revivify)
				{
					GetComponent<Image>().sprite = revivifyCardSprite;
				}
				else if (cardName == CardName.Cure)
				{
					GetComponent<Image>().sprite = cureCardSprite;
				}
				else if (cardName == CardName.Boost)
				{
					GetComponent<Image>().sprite = boostCardSprite;
				}
				else if (cardName == CardName.ComboBooster)
				{
					GetComponent<Image>().sprite = comboBoosterCardSprite;
				}
				else
				{
					GetComponent<Image>().sprite = reflectCardSprite;
				}
				break;
            default:
                cardColor = CardColor.Yellow;
                //GetComponent<Image>().color = new Color(255, 255, 0);
				cardPriority = 25;
                //nameText.color = new Color(0, 0, 0);
				if (deckManager.GetComponent<DeckManager>().GetComboColor() == CardColor.Yellow)
				{
					cardPriority += deckManager.GetComponent<DeckManager>().comboNumber;
				}
				if (cardName == CardName.Frighten)
				{
					GetComponent<Image>().sprite = frightenCardSprite;
				}
				else if (cardName == CardName.Freeze)
				{
					GetComponent<Image>().sprite = freezeCardSprite;
				}
				else if (cardName == CardName.Lullaby)
				{
					GetComponent<Image>().sprite = lullabyCardSprite;
				}
				else if (cardName == CardName.Poison)
				{
					GetComponent<Image>().sprite = poisonCardSprite;
				}
				else
				{
					GetComponent<Image>().sprite = comboBreakerCardSprite;
					cardPriority += 1000;
				}
				break;
        }
        if (cardColor == deckManager.GetComponent<DeckManager>().comboDelayColor)
        {
            GetComponent<Button>().interactable = false;
        }

        if (player.GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frozen) && UnityEngine.Random.Range(0, 3) == 0)
        {
			GetComponent<Button>().interactable = false;
			//GameObject freezeCard = Instantiate(frozenCardEffect, transform.position, Quaternion.identity);
			Vector3 canvasPosition = Camera.main.WorldToScreenPoint(GetComponent<RectTransform>().TransformPoint(transform.position));
			canvasPosition = new Vector3(canvasPosition.x / GetComponent<RectTransform>().sizeDelta.x, canvasPosition.y / GetComponent<RectTransform>().sizeDelta.y, 0);

			GameObject freezeCard = Instantiate(frozenCardEffect, transform.position, Quaternion.identity);
			freezeCard.transform.SetParent(transform, true);
			freezeCard.transform.position += new Vector3(0, -5, 0);
		}

        if (cardName == CardName.ComboBreaker)
        {
            cardPriority = 999;
        }

    }

    public void SelectCard()
    {
        endTurnButton.GetComponent<EndTurnButton>().setCard(this);

		switch (cardColor)    //Animator colors are: None = 0; Red = 1; Blue = 2; Green = 3; Yellow = 4;
		{
			case CardColor.Red:
				if (!player.GetComponent<Animator>().GetBool("Red"))
				{
					if (player.GetComponent<Animator>().GetInteger("Color") != 0)
					{
						player.GetComponent<Animator>().SetTrigger("ChangeColor");
					}
					player.GetComponent<Animator>().SetBool("Red", true);
					player.GetComponent<Animator>().SetBool("Blue", false);
					player.GetComponent<Animator>().SetBool("Green", false);
					player.GetComponent<Animator>().SetBool("Yellow", false);
					player.GetComponent<Animator>().SetInteger("Color", 1);
				}
				break;
			case CardColor.Blue:
				if (!player.GetComponent<Animator>().GetBool("Blue"))
				{
					if (player.GetComponent<Animator>().GetInteger("Color") != 0)
					{
						player.GetComponent<Animator>().SetTrigger("ChangeColor");
					}
					player.GetComponent<Animator>().SetBool("Blue", true);
					player.GetComponent<Animator>().SetBool("Red", false);
					player.GetComponent<Animator>().SetBool("Green", false);
					player.GetComponent<Animator>().SetBool("Yellow", false);
					player.GetComponent<Animator>().SetInteger("Color", 2);
				}
				break;
			case CardColor.Green:
				if (!player.GetComponent<Animator>().GetBool("Green"))
				{
					if (player.GetComponent<Animator>().GetInteger("Color") != 0)
					{
						player.GetComponent<Animator>().SetTrigger("ChangeColor");
					}
					player.GetComponent<Animator>().SetBool("Green", true);
					player.GetComponent<Animator>().SetBool("Blue", false);
					player.GetComponent<Animator>().SetBool("Red", false);
					player.GetComponent<Animator>().SetBool("Yellow", false);
					player.GetComponent<Animator>().SetInteger("Color", 3);
				}
				break;
			case CardColor.Yellow:
				if (!player.GetComponent<Animator>().GetBool("Yellow"))
				{
					if (player.GetComponent<Animator>().GetInteger("Color") != 0)
					{
						player.GetComponent<Animator>().SetTrigger("ChangeColor");
					}
					player.GetComponent<Animator>().SetBool("Yellow", true);
					player.GetComponent<Animator>().SetBool("Blue", false);
					player.GetComponent<Animator>().SetBool("Green", false);
					player.GetComponent<Animator>().SetBool("Red", false);
					player.GetComponent<Animator>().SetInteger("Color", 4);
				}
				break;
		}
		
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
