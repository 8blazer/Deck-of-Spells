using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    private int health = 15;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "15";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
		healthText.text = health.ToString();
		if (health < 1)
        {
            Destroy(gameObject);
        }
    }
}
