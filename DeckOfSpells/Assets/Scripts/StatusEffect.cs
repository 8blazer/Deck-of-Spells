using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
		if (!statusList.ContainsKey(s) || statusList[s] < 0)
		{
			GameObject effect = Instantiate(effectPrefab, new Vector3(1000, 0, 0), Quaternion.identity);
			effect.GetComponent<StatusEffect>().SetStatus(s);
			effectObjects.Add(effect);
			statusList.Add(s, effectTime);
		}
		else
		{
			statusList[s] = effectTime;
		}
		SpawnStatus();
	}

	public void UpdateStatus()
	{
		List<Status> updateList = new List<Status>();
		foreach (KeyValuePair<Status, int> entry in statusList)
		{
			if (entry.Value > 0)
			{
				updateList.Add(entry.Key);
			}
		}

		for (int i = 0; i < updateList.Count; i++)
		{
			statusList[updateList[i]]--;
			if (updateList[i] == 0)
			{
				DeleteStatus(updateList[i]);
				i--;
			}
		}
	}

	public void DeleteStatus(Status s)
	{
		for (int i = 0; i < effectObjects.Count; i++)
		{
			if (effectObjects[i].GetComponent<StatusEffect>().GetStatus() == s)
			{
				Destroy(effectObjects[i]);
				effectObjects.RemoveAt(i);
				i--;
			}
		}
		statusList.Remove(s);
		SpawnStatus();
	}

	private void SpawnStatus()
	{
		for (int i = 0; i < effectObjects.Count; i++)
		{
			effectObjects[i].transform.position = this.transform.position + new Vector3(i * 100, 100, 0);
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
