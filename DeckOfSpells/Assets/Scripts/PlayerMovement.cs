using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float movementDropoff;
	[SerializeField] private Camera menuCamera;
	private bool canMove = true;
	public int health;
	public int maxHealth;
	public int coins;

	private Dictionary<CardName, int> selectedCards = new Dictionary<CardName, int>();
	private Dictionary<CardName, int> unlockedCards = new Dictionary<CardName, int>();

	private void Start()
	{
		SaveData save = SaveSystem.LoadData(false);
		if (save != null)
		{
			if (PlayerPrefs.GetInt("UseTemp") == 2)
			{
				transform.position = new Vector2(save.tempPosition[0], save.tempPosition[1]);
				selectedCards = save.tempSelectedCards;
				unlockedCards = save.tempCardsUnlocked;
				coins += save.tempCoins;
			}
			else
			{
				transform.position = new Vector2(save.position[0], save.position[1]);
				selectedCards = save.selectedCards;
				unlockedCards = save.cardsUnlocked;
				coins = save.coins;
				health = save.health;
			}
		}

		if (unlockedCards.Count == 0)
		{
			unlockedCards.Add(CardName.Fireball, 3);
			unlockedCards.Add(CardName.Lightning, 1);
			unlockedCards.Add(CardName.Spikey, 2);
			unlockedCards.Add(CardName.Frighten, 1);
		}
		
	}

	// Update is called once per frame
	void Update()
    {
        if (canMove)
        {
			Vector2 velocity = new Vector2(0, 0);
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				velocity += new Vector2(-moveSpeed, 0);
			}
			else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				velocity += new Vector2(moveSpeed, 0);
			}
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			{
				velocity += new Vector2(0, moveSpeed);
			}
			else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			{
				velocity += new Vector2(0, -moveSpeed);
			}
			if (velocity.x == 0 && velocity.y == 0)
			{
				GetComponent<Rigidbody2D>().velocity *= movementDropoff;
			}
			else
			{
				GetComponent<Rigidbody2D>().velocity = velocity;
			}
		}

		else
		{
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}

		GetComponent<Rigidbody2D>().velocity.Normalize();

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (menuCamera.enabled)
			{
				menuCamera.enabled = false;
				canMove = true;
			}
			else
			{
				canMove = false;
				menuCamera.enabled = true;
			}
		}
	}

	public void SaveData(bool tempSave)
	{
		SaveSystem.SaveData(this.gameObject, tempSave);
	}

	public void LoadData()
	{
		SaveData data = SaveSystem.LoadData(false);

		health = data.health;
		maxHealth = data.maxHealth;
		selectedCards = data.selectedCards;
		unlockedCards = data.cardsUnlocked;
		if (data.position != null)
		{
			transform.position = new Vector3(data.position[0], data.position[1], 0);
		}

	}

	public Dictionary<CardName, int> GetCardsUnlocked()
	{
		return unlockedCards;
	}

	public Dictionary<CardName, int> GetCardsSelected()
	{
		return selectedCards;
	}

	public void AddSelectedCard(CardName card)
	{
		if (selectedCards.ContainsKey(card))
		{
			selectedCards[card]++;
		}
		else
		{
			selectedCards.Add(card, 1);
		}
		//selectedCards.Add(card, selectedCards.TryGetValue(card, out 1));
	}

	public void RemoveSelectedCard(CardName card)
	{
		if (selectedCards[card] == 1)
		{
			selectedCards.Remove(card);
		}
		else
		{
			selectedCards[card]--;
		}
	}

	public void AddUnlockedCard(CardName card)
	{
		if (unlockedCards.ContainsKey(card))
		{
			unlockedCards[card]++;
		}
		else
		{
			unlockedCards.Add(card, 1);
		}
	}
}
