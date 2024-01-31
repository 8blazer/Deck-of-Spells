using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private Card selectedCard;
    [SerializeField] private GameObject deckManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCard()
    {
        if (selectedCard != null)
        {
			selectedCard.GetComponent<Card>().PlayCard();
		}
        else
        {
            deckManager.GetComponent<DeckManager>().UpdateCombo(CardColor.None);
            deckManager.GetComponent<DeckManager>().ChooseCards();
        }
    }

    public void setCard(Card card)
    {
        selectedCard = card;
        GetComponent<Button>().interactable = true;
    }
}
