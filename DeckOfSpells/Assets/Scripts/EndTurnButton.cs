using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private Card selectedCard;
    [SerializeField] private GameObject deckManager;
	[SerializeField] private GameObject turnManager;


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
            turnManager.GetComponent<TurnManager>().SetPlayerCard(CardName.None, 0);
        }
        deckManager.GetComponent<DeckManager>().DeleteCards();
	}

    public void setCard(Card card)
    {
        selectedCard = card;
        GetComponent<Button>().interactable = true;
    }
}
