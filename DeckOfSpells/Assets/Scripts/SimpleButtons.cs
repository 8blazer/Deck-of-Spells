using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleButtons : MonoBehaviour
{
    bool sceneLoading = false;

	private void Update()
	{
		if (sceneLoading)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Overworld"))
            {
				GameObject player = GameObject.Find("Player");
				player.GetComponent<PlayerMovement>().LoadData();
				sceneLoading = false;
				Destroy(this.gameObject);
			}
        }
	}

	public void NewGame()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void LoadGame()
    {
		DontDestroyOnLoad(this.gameObject);
        GetComponent<Canvas>().enabled = false;
		sceneLoading = true;
		SceneManager.LoadScene("Overworld");
    }

	public void MenuCard()
	{
		GameObject player = GameObject.Find("Player");
		if (GetComponent<MenuScroll>().selected)
		{
			player.GetComponent<PlayerMovement>().RemoveSelectedCard(GetComponent<MenuScroll>().GetCardName());
			GameObject.Find("MenuCardManager").GetComponent<MenuCardManager>().ClearCards();
			GameObject.Find("MenuCardManager").GetComponent<MenuCardManager>().RenderSelectedCards();
			GetComponent<MenuScroll>().selected = false;
		}
		else
		{
			player.GetComponent<PlayerMovement>().AddSelectedCard(GetComponent<MenuScroll>().GetCardName());
			GameObject.Find("MenuCardManager").GetComponent<MenuCardManager>().ClearCards();
			GameObject.Find("MenuCardManager").GetComponent<MenuCardManager>().RenderSelectedCards();
			GetComponent<MenuScroll>().selected = true;
		}
	}

}
