using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    private Status status;
    private Player player;

	private Dictionary<Status, int> statusList = new Dictionary<Status, int>();
	private List<GameObject> effectObjects = new List<GameObject>();
	[SerializeField] private GameObject effectPrefab;

	void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

	public Dictionary<Status, int> GetStatusList()
	{
		return statusList;
	}

	public Status GetStatus()
	{
		return status;
	}

	public void SetStatus(Status s)
	{
		status = s;
	}

	public void AddStatus(Status s, int effectTime)
	{
		if (!statusList.ContainsKey(s) || statusList[s] < 1)
		{
			GameObject effect = Instantiate(effectPrefab, new Vector3(1000, 0, 0), Quaternion.identity);
			effect.GetComponent<StatusEffect>().SetStatus(s);
			effectObjects.Add(effect);
			SpawnStatus();
		}
		statusList.Add(s, effectTime);
	}

	public void UpdateStatus()
	{
		foreach (KeyValuePair<Status, int> entry in statusList)
		{
			if (entry.Value > 0)
			{
				statusList[entry.Key]--;
				if (entry.Value == 1)
				{
					DeleteStatus(entry.Key);
				}
			}
		}
	}

	public void DeleteStatus(Status s)
	{
		foreach (GameObject effect in effectObjects)
		{
			if (effect.GetComponent<StatusEffect>().GetStatus() == s)
			{
				effectObjects.Remove(effect);
				Destroy(effect);
			}
		}
		SpawnStatus();
	}

	private void SpawnStatus()
	{
		for (int i = 0; i < effectObjects.Count; i++)
		{
			effectObjects[i].transform.position = this.transform.position + 
				new Vector3(i * 100, 0, 0);
		}
	}

}

public enum Status
{
	None,
	Frozen,
	Asleep,
	Poisoned,
	Frightened
}
