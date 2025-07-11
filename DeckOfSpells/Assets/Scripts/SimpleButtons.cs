using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
		PlayerPrefs.DeleteAll();
		string path = Application.persistentDataPath;
		DirectoryInfo directory = new DirectoryInfo(path);
		directory.Delete(true);
		Directory.CreateDirectory(path);
		SceneManager.LoadScene("Overworld");
    }

    public void LoadGame()
    {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt("UseTemp", 1);
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TitleScreen"))
		{
			DontDestroyOnLoad(this.gameObject);
			GetComponent<Canvas>().enabled = false;
			sceneLoading = true;
		}
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

	public void ResetFight()
	{
		GameObject turnManager = GameObject.Find("TurnManager");
		turnManager.GetComponent<TurnManager>().ResetFight();
	}

}
