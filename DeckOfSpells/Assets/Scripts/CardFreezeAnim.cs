using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFreezeAnim : MonoBehaviour
{
    [SerializeField] private float fps;
    private float timer = 0;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    private int index = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > fps)
        {
            timer = 0;
            index++;
            if (!GetComponent<Image>().sprite.Equals(sprites[sprites.Count - 1]))
            {
                GetComponent<Image>().sprite = sprites[index - 1];
            }
        }
    }
}
