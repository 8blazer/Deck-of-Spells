using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    private Status status;
    private Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void SetStatus()
    {
		
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
