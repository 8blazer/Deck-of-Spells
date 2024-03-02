using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Minions : MonoBehaviour
{
    private int health;
    private int damage;
    [SerializeField] private RuntimeAnimatorController spikeyAnimator;
    [SerializeField] private RuntimeAnimatorController treeAnimator;
    [SerializeField] private RuntimeAnimatorController wallAnimator;

    public void CreateMinion(Minion minionType, int hp, int dmg)
    {
        health = hp;
        damage = dmg;
        if (minionType == Minion.Tree)
        {
            GetComponent<Animator>().runtimeAnimatorController = treeAnimator;
        }
        else if (minionType == Minion.Wall)
        {
            GetComponent<Animator>().runtimeAnimatorController = wallAnimator;
        }
        else
        {
			GetComponent<Animator>().runtimeAnimatorController = spikeyAnimator;
		}
	}
	public int TakeDamage(int dmg, bool poison)
    {
        if (poison)
        {
            health -= dmg;
            return dmg;
        }
		if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frightened) && dmg > 0)
		{
			dmg = (int)(dmg * 1.5f);
		}
		health -= dmg;
		if (health < 1)
		{
            GetComponent<StatusEffect>().UpdateStatus();
			GetComponent<StatusEffect>().UpdateStatus();
			GetComponent<StatusEffect>().UpdateStatus();
			GetComponent<StatusEffect>().UpdateStatus();
			return health * -1;
		}
        return 0;
	}

    public int GetDamage()
    {
		if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Asleep))
		{
			if (Random.Range(0, 4) > 0)
			{
				return 0;
			}
			GetComponent<StatusEffect>().DeleteStatus(Status.Asleep);
		}

        if (GetComponent<StatusEffect>().GetStatusList().ContainsKey(Status.Frozen))
        {
            if (Random.Range(0, 3) == 0)
            {
                return 0;
            }
        }
		return damage;
    }

    public int GetHealth() { return health; }

}

public enum Minion
{
    Tree,
    Spikey,
    Wall
}
