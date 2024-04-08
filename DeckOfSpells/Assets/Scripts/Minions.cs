using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minions : MonoBehaviour
{
    private Minion minion;
    private int health;
    private int damage;
    private float timer;
    [SerializeField] private RuntimeAnimatorController spikeyAnimator;
    [SerializeField] private RuntimeAnimatorController treeAnimator;
    [SerializeField] private RuntimeAnimatorController wallAnimator;

    public void CreateMinion(Minion minionType, int hp, int dmg)
    {
        health = hp;
        damage = dmg;
        minion = minionType;
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

	private void Update()
	{
        if (GetComponent<Animator>().GetBool("IsHurt") || GetComponent<Animator>().GetBool("IsDead"))
        {
            timer += Time.deltaTime;
            if (timer > .3f && !GetComponent<Animator>().IsInTransition(0) && GetComponent<Animator>().GetBool("IsHurt"))
            {
				GetComponent<Animator>().SetBool("IsHurt", false);
                timer = 0;
			}
            else if (timer > .3f && GetComponent<Animator>().GetBool("IsDead") && !GetComponent<Animator>().IsInTransition(0))
            {
                Destroy(gameObject);
            }
        }
	}
	public int TakeDamage(int dmg, bool poison)
    {
        GetComponent<Animator>().SetBool("IsHurt", true);

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
            GetComponent<Animator>().SetBool("IsDead", true);
            GetComponent<StatusEffect>().UpdateStatus();
			GetComponent<StatusEffect>().UpdateStatus();
			GetComponent<StatusEffect>().UpdateStatus();
			GetComponent<StatusEffect>().UpdateStatus();
            if (minion != Minion.Wall)
            {
				return health * -1;
			}
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

    public void ChangeDamage(int damageChange)
    {
        damage += damageChange;
    }

}

public enum Minion
{
    Tree,
    Spikey,
    Wall
}
