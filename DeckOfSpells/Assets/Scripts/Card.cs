using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    
    [SerializeField] private CardColor cardColor;
    public CardName cardName;
    [SerializeField] private TMP_Text nameText;
    private GameObject deckManager;

    // Start is called before the first frame update
    void Start()
    {
        deckManager = GameObject.FindWithTag("GameController");
        nameText.text = cardName.ToString();
        switch (cardName)
        {
            case CardName.Fireball: case CardName.Lightning:
                cardColor = CardColor.Red;
                GetComponent<Image>().color = new Color(255, 0, 0);
                break;
            case CardName.Tree: case CardName.Spikey: case CardName.Wall:
                cardColor = CardColor.Blue;
                GetComponent<Image>().color = new Color(0, 0, 255);
                break;
            case CardName.Revivify: case CardName.Cure: case CardName.Boost: case CardName.ComboBooster: case CardName.Reflect:
                cardColor = CardColor.Green;
                GetComponent<Image>().color = new Color(0, 255, 0);
                break;
            default:
                cardColor = CardColor.Yellow;
                GetComponent<Image>().color = new Color(255, 255, 0);
                break;
        }
    }
    
    public void PlayCard()
    {
        deckManager.GetComponent<DeckManager>().selectedCards.Remove(this.gameObject);
        deckManager.GetComponent<DeckManager>().ChooseCards();
		Destroy(this.gameObject);
	}





}

public enum CardColor { Red, Blue, Green, Yellow };
public enum CardName { 
    Fireball, 
    Lightning, 
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
    ComboBreaker
}
