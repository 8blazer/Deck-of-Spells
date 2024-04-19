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

	private List<CardName> selectedCards = new List<CardName>();
	private List<CardName> unlockedCards = new List<CardName>();

	private void Start()
	{
		unlockedCards.Add(CardName.Fireball);
		unlockedCards.Add(CardName.Lightning);
		unlockedCards.Add(CardName.Spikey);
		unlockedCards.Add(CardName.Frighten);
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

	public void SaveData()
	{
		SaveSystem.SaveData(this.gameObject);
	}

	public void LoadData()
	{
		SaveData data = SaveSystem.LoadData();

		health = data.health;
		maxHealth = data.maxHealth;
		transform.position = new Vector3(data.position[0], data.position[1], 0);
		selectedCards = data.selectedCards;
		unlockedCards = data.cardsUnlocked;
	}

	public List<CardName> GetCardsUnlocked()
	{
		return unlockedCards;
	}

	public List<CardName> GetCardsSelected()
	{
		return selectedCards;
	}

	public void AddSelectedCard(CardName card)
	{
		selectedCards.Add(card);
	}

	public void RemoveSelectedCard(CardName card)
	{
		selectedCards.Remove(card);
	}
}
