using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScroll : MonoBehaviour
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y < maxY)
        {
            transform.position += new Vector3(0, 1f, 0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y > minY)
        {
			transform.position += new Vector3(0, -1f, 0);
		}
    }
}
