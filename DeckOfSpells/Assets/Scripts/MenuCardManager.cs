using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCardManager : MonoBehaviour
{
	public GameObject menuCardPrefab;
	[SerializeField] private GameObject selectedCardBackgroundPrefab;
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

	public List<GameObject> cards = new List<GameObject>();
	public Dictionary<CardName, int> unlockedCards;
	public Dictionary<CardName, int> selectedCards;

	void Start()
	{
		Invoke("CreateMenu", 0.2f);
	}
	public void CreateMenu()
	{
		PlayerMovement player = GameObject.Find("Player").GetComponent<PlayerMovement>();
		unlockedCards = player.GetCardsUnlocked();
		selectedCards = player.GetCardsSelected();

		RenderUnlockedCards();
		RenderSelectedCards();
	}

	public void ClearCards()
	{
		foreach (GameObject card in cards)
		{
			Destroy(card);
		}
		cards.Clear();
	}

	public void RenderSelectedCards()
	{
		int x = -630;
		int y = 250;
		foreach (CardName card in selectedCards.Keys)
		{
			for (int i = 0; i < selectedCards[card]; i++)
			{
				GameObject newCard = Instantiate(menuCardPrefab, transform.position, Quaternion.identity);
				cards.Add(newCard);
				newCard.GetComponent<Button>().enabled = false;
				newCard.transform.SetParent(transform.parent, false);
				newCard.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
				newCard.GetComponent<MenuScroll>().leftSide = true;
				x += 230;
				if (x > -100)
				{
					x = -630;
					y -= 300;
				}
				SetCardData(newCard, card);
			}
		}
	}

	private void RenderUnlockedCards()
	{
		int x = 170;
		int y = 250;

		foreach (CardName card in unlockedCards.Keys)
		{
			int selectedNumber = 0;
			if (selectedCards.ContainsKey(card))
			{
				selectedNumber = selectedCards[card];
				for (int i = 0; i < selectedCards[card]; i++)
				{
					GameObject newCard = Instantiate(menuCardPrefab, transform.position, Quaternion.identity);
					newCard.transform.SetParent(transform.parent, false);
					newCard.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
					newCard.GetComponent<MenuScroll>().leftSide = false;
					x += 230;
					if (x > 700)
					{
						x = 170;
						y -= 300;
					}

					SetCardData(newCard, card);
					newCard.GetComponent<MenuScroll>().selected = true;
				}
			}
			for (int i = 0; i < unlockedCards[card] - selectedNumber; i++)
			{
				GameObject newCard = Instantiate(menuCardPrefab, transform.position, Quaternion.identity);
				newCard.transform.SetParent(transform.parent, false);
				newCard.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
				newCard.GetComponent<MenuScroll>().leftSide = false;
				x += 230;
				if (x > 700)
				{
					x = 170;
					y -= 300;
				}

				SetCardData(newCard, card);
				newCard.GetComponent<MenuScroll>().selected = false;
			}
		}
	}

	private void SetCardData(GameObject newCard, CardName name)
	{
		switch (name)
		{
			case CardName.Fireball:
				newCard.GetComponent<Image>().sprite = fireballCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Fireball);
				break;
			case CardName.Lightning:
				newCard.GetComponent<Image>().sprite = lightningCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Lightning);
				break;
			case CardName.Landslide:
				newCard.GetComponent<Image>().sprite = landslideCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Landslide);
				break;
			case CardName.Tree:
				newCard.GetComponent<Image>().sprite = treeCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Tree);
				break;
			case CardName.Spikey:
				newCard.GetComponent<Image>().sprite = spikeyCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Spikey);
				break;
			case CardName.Wall:
				newCard.GetComponent<Image>().sprite = wallCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Wall);
				break;
			case CardName.Revivify:
				newCard.GetComponent<Image>().sprite = revivifyCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Revivify);
				break;
			case CardName.Cure:
				newCard.GetComponent<Image>().sprite = cureCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Cure);
				break;
			case CardName.ComboBooster:
				newCard.GetComponent<Image>().sprite = comboBoosterCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.ComboBooster);
				break;
			case CardName.Boost:
				newCard.GetComponent<Image>().sprite = boostCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Boost);
				break;
			case CardName.Reflect:
				newCard.GetComponent<Image>().sprite = reflectCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Reflect);
				break;
			case CardName.Freeze:
				newCard.GetComponent<Image>().sprite = freezeCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Freeze);
				break;
			case CardName.Frighten:
				newCard.GetComponent<Image>().sprite = frightenCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Frighten);
				break;
			case CardName.Poison:
				newCard.GetComponent<Image>().sprite = poisonCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Poison);
				break;
			case CardName.Lullaby:
				newCard.GetComponent<Image>().sprite = lullabyCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.Lullaby);
				break;
			case CardName.ComboBreaker:
				newCard.GetComponent<Image>().sprite = comboBreakerCardSprite;
				newCard.GetComponent<MenuScroll>().SetCard(CardName.ComboBreaker);
				break;
		}
	}

}
