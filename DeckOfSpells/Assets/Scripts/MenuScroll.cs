using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScroll : MonoBehaviour
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    private int currentY = 0;

    public bool leftSide = true;
    public bool selected = false;
    private CardName card;

    private Camera menuCamera;

	private void Start()
	{
        menuCamera = GameObject.Find("MenuCamera").GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
    {
        if (menuCamera.enabled)
        {
			if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentY < maxY)
			{
				if (leftSide && Input.mousePosition.x < Screen.width / 2)
				{
					transform.position += new Vector3(0, 1f, 0);
					currentY++;
				}
				else if (!leftSide && Input.mousePosition.x > Screen.width / 2)
				{
					transform.position += new Vector3(0, 1f, 0);
					currentY++;
				}
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentY > minY)
			{
				if (leftSide && Input.mousePosition.x < Screen.width / 2)
				{
					transform.position += new Vector3(0, -1f, 0);
					currentY--;
				}
				else if (!leftSide && Input.mousePosition.x > Screen.width / 2)
				{
					transform.position += new Vector3(0, -1f, 0);
					currentY--;
				}
			}
		}
	}

    public void SetCard(CardName newCard)
    {
        card = newCard;
    }

    public CardName GetCardName() 
    { 
        return card;
    }
}
