using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private Card selectedCard;
    [SerializeField] private GameObject deckManager;
	[SerializeField] private GameObject turnManager;

    public void playCard()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Animator>().SetBool("SpellChosen", false);
		player.GetComponent<Animator>().SetBool("SpellFinished", true);
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
