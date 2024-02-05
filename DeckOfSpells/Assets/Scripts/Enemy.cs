using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    private int health = 105;

	List<CardName> deck = new List<CardName>();
	List<CardName> discardDeck = new List<CardName>();
	List<CardName> selectedCards = new List<CardName>();
    [SerializeField] private GameObject turnManager;

	// Start is called before the first frame update
	void Start()
    {
        healthText.text = "105";

		deck.Add(CardName.Fireball);
		deck.Add(CardName.Fireball);
		deck.Add(CardName.Lightning);
		deck.Add(CardName.Fireball);
		deck.Add(CardName.Fireball);
		deck.Add(CardName.ComboBreaker);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCard()
    {
        while (selectedCards.Count > 0)
        {
            deck.Add(selectedCards[0]);
            selectedCards.RemoveAt(0);
        }
        
        while (deck.Count < 3)
        {
            int i = Random.Range(0, deck.Count);
			selectedCards.Add(deck[i]);
            deck.RemoveAt(i);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
		healthText.text = health.ToString();
		if (health < 1)
        {
            Destroy(gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }
}
