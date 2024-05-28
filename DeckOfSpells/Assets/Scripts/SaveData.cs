using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SaveData
{
    public int health;
    public int maxHealth;
    public int coins;
    public bool leftTowerDefeated;
    public bool rightTowerDefeated;
    public bool topTowerDefeated;
    public bool bottomTowerDefeated;
    public Dictionary<CardName, int> cardsUnlocked;
    public Dictionary<CardName, int> selectedCards;
    public float[] position = new float[2];

	public int tempHealth;
	public int tempMaxHealth;
	public int tempCoins;
	public bool tempLeftTowerDefeated;
	public bool tempRightTowerDefeated;
	public bool tempTopTowerDefeated;
	public bool tempBottomTowerDefeated;
	public Dictionary<CardName, int> tempCardsUnlocked;
	public Dictionary<CardName, int> tempSelectedCards;
	public float[] tempPosition = new float[2];

	public SaveData(GameObject objectToSave, bool tempSave)
    {
        if (objectToSave.GetComponent<PlayerMovement>() != null)
        {
			if (tempSave)
			{
				PlayerPrefs.SetInt("UseTemp", 2);
				tempPosition = new float[2];
				tempPosition[0] = objectToSave.transform.position.x;
				tempPosition[1] = objectToSave.transform.position.y;
				tempCardsUnlocked = objectToSave.GetComponent<PlayerMovement>().GetCardsUnlocked();
				tempSelectedCards = objectToSave.GetComponent<PlayerMovement>().GetCardsSelected();
			}
			else
			{
				PlayerPrefs.SetInt("UseTemp", 1);
				position = new float[2];
				tempPosition = new float[2];
				position[0] = objectToSave.transform.position.x;
				position[1] = objectToSave.transform.position.y;
				tempPosition[0] = objectToSave.transform.position.x;
				tempPosition[1] = objectToSave.transform.position.y;
				cardsUnlocked = objectToSave.GetComponent<PlayerMovement>().GetCardsUnlocked();
				selectedCards = objectToSave.GetComponent<PlayerMovement>().GetCardsSelected();
				tempCardsUnlocked = cardsUnlocked;
				tempSelectedCards = selectedCards;
			}
		}
    }
}
