using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScroll : MonoBehaviour
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    private int currentY = 0;

    [SerializeField] private bool leftSide;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
