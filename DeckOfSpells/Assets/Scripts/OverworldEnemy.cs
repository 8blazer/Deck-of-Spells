using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    [SerializeField] List<CardName> enemyDeck;
	List<CardName> playerDeck;
    private bool sceneLoading = false;

    // Update is called once per frame
    void Update()
    {
		if (sceneLoading)
		{
			if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("ConnerScene"))
			{
				GameObject enemy = GameObject.Find("Enemy");
				enemy.GetComponent<Enemy>().SetDeck(enemyDeck);
				GameObject deckManager = GameObject.Find("DeckManager");
				deckManager.GetComponent<DeckManager>().SetDeck(playerDeck);
				sceneLoading = false;
				Destroy(this.gameObject);
			}
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player")
        {
			collision.gameObject.GetComponent<PlayerMovement>().SaveData(false);
			playerDeck = collision.gameObject.GetComponent<PlayerMovement>().GetCardsSelected();
			DontDestroyOnLoad(this.gameObject);
			GetComponent<SpriteRenderer>().enabled = false;
			sceneLoading = true;
			SceneManager.LoadScene("ConnerScene");
		}
	}
}
