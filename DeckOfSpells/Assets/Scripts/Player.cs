using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    private int health = 20;
    [SerializeField] private TMP_Text healthText;
	[SerializeField] private GameObject deckManager;
	private List<GameObject> minions = new List<GameObject>();
	public int minionCount = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	public void TakeDamage(int damage, bool poison)
	{
		if (poison)
		{
			for (int i = 0; i < minionCount; i++)
			{
				minions[i].GetComponent<Minions>().TakeDamage(damage, true);
				if (minions[i].GetComponent<Minions>().GetHealth() <= 0)
				{
					Destroy(minions[i]);
					minions.RemoveAt(i);
					minionCount--;
				}
			}
			GetComponent<Animator>().SetTrigger("Hurt");
			health -= damage;
		}
		else
		{
			for (int i = 0; i < minionCount; i++)
			{
				if (damage > 0)
				{
					damage = minions[i].GetComponent<Minions>().TakeDamage(damage, false);
				}
				if (minions[i].GetComponent<Minions>().GetHealth() <= 0)
				{
					//Destroy(minions[i]);
					minions.RemoveAt(i);
					minionCount--;
				}
			}
			if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frightened))
			{
				damage = (int)(damage * 1.5f);
			}
			if (damage > 0)
			{
				GetComponent<Animator>().SetTrigger("Hurt");
				health -= damage;
			}
		}
		healthText.text = health.ToString();
		if (health < 1)
		{
			GetComponent<Animator>().SetBool("Defeated", true);
		}
	}
	public int GetHealth()
    {
        return health;
    }

	public List<GameObject> GetMinionList()
	{
		return minions;
	}

	public void AddMinion(GameObject minion)
	{
		minionCount++;
		foreach (GameObject min in minions)
		{
			min.transform.position += new Vector3(2, 0, 0);
		}
		minions.Add(minion);
	}

	public void CureMinions()
	{
		foreach (GameObject minion in minions)
		{
			minion.GetComponent<StatusEffect>().UpdateStatus();
			minion.GetComponent<StatusEffect>().UpdateStatus();
			minion.GetComponent<StatusEffect>().UpdateStatus();
			minion.GetComponent<StatusEffect>().UpdateStatus();
		}
	}

	public int MinionTurn()
	{
		int damage = 0;

		foreach (GameObject minion in minions)
		{
			damage += minion.GetComponent<Minions>().GetDamage();
		}

		return damage;
	}

	public void ChangeMinionDamage(int damageChange)
	{
		foreach (GameObject minion in minions)
		{
			minion.GetComponent<Minions>().ChangeDamage(damageChange);
		}
	}

}
