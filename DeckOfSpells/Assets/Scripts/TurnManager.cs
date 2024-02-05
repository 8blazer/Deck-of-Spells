using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private CardName playerCard;
    private CardName enemyCard;

    private int playerPriority;
    private int enemyPriority;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerCard(CardName cardName, int priority)
    {
        playerCard = cardName;
        playerPriority = priority;
        PlayTurn();
    }

	public void SetEnemyCard(CardName cardName, int priority)
	{
		enemyCard = cardName;
        enemyPriority = priority;
	}
	private void PlayTurn()
    {
        if (playerPriority >= enemyPriority)
        {
            PlayerTurn();
            if (enemy.GetComponent<Enemy>().GetHealth() > 0)
            {
                EnemyTurn();
            }
        }
        else
        {
            EnemyTurn();
            if (player.GetComponent<Player>().GetHealth() > 0)
            {
                PlayerTurn();
            }
        }
    }

    private void PlayerTurn()
    {
        if (player.GetComponent<Player>().GetStatus().ContainsKey(Status.Frozen))
        {
            if (Random.Range(0, 3) == 0)
            {
                return;
            }
        }
        if (player.GetComponent<Player>().GetStatus().ContainsKey(Status.Asleep))
        {
            if (Random.Range(0, 4) > 0)
            {
                return;
            }
        }
        
    }

    private void EnemyTurn()
    {

    }
}
