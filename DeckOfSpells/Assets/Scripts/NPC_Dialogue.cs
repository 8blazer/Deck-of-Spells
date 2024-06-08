using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{

    private bool playerNear = false;
    private bool textScroll = false;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text text;
    [SerializeField] private List<string> textList = new List<string>();
    private int boxNumber = 0;
	private float timer = 0;
	[SerializeField] private float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear && Input.GetKeyUp(KeyCode.E))
        {
            if (canvas.enabled)
            {
				if (boxNumber == textList.Count - 1 && text.text == textList[boxNumber])
				{
					canvas.enabled = false;
					text.text = "";
					boxNumber = 0;
					return;
				}
				if (textScroll)
				{
					text.text = textList[boxNumber];
					//boxNumber++;
				}
				else if (text.text == textList[boxNumber])
				{
					text.text = "";
					boxNumber++;
					textScroll = true;
				}
			}
			else
			{
				canvas.enabled = true;
				textScroll = true;
			}
        }
		else if (canvas.enabled && text.text != textList[boxNumber])
		{
			timer += Time.deltaTime;
			if (timer > scrollSpeed)
			{
				text.text += textList[boxNumber][text.text.Length];
				timer = 0;
			}
			//StartCoroutine(MoveText());
		}
		if (text.text == textList[boxNumber])
		{
			textScroll = false;
		}
    }


	private void OnTriggerEnter2D(Collider2D collision)
	{
		playerNear = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		playerNear = false;
	}
}
