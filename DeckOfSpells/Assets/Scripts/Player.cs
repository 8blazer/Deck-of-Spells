using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    private int health = 1000;
    [SerializeField] private TMP_Text healthText;
	[SerializeField] private GameObject deckManager;
	public void TakeDamage(int damage)
	{
		if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frightened) && damage > 0)
		{
			damage = (int)(damage * 1.5f);
		}
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
