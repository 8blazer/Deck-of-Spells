using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int health = 1000;
    private Dictionary<Status, int> statusList = new Dictionary<Status, int>();

    public int GetHealth()
    {
        return health;
    }

    public Dictionary<Status, int> GetStatus()
    {
        return statusList;
    }

    public void SetStatus(Status s, int effectTime)
    {
        statusList.Add(s, effectTime);
    }
}
