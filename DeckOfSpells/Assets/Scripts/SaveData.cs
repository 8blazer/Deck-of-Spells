using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
    public List<CardName> cardsUnlocked;
    public List<CardName> selectedCards;
    public float[] position;

    public SaveData(GameObject objectToSave)
    {
        if (objectToSave.GetComponent<PlayerMovement>() != null)
        {
			position = new float[2];
			position[0] = objectToSave.transform.position.x;
			position[1] = objectToSave.transform.position.y;
            cardsUnlocked = objectToSave.GetComponent<PlayerMovement>().GetCardsUnlocked();
            selectedCards = objectToSave.GetComponent<PlayerMovement>().GetCardsSelected();
		}


    }


}
