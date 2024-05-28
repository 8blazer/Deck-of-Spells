using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		SaveData save = SaveSystem.LoadData(false);
		if (save != null && PlayerPrefs.GetInt("UseTemp") != 0)
		{
			PlayerPrefs.SetInt("UseTemp", 1);
			GetComponent<Button>().interactable = true;
		}
	}
}
