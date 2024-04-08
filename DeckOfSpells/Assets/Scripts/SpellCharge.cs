using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCharge : MonoBehaviour
{
    // Start is called before the first frame update

    private Color color;
    [SerializeField] float changeSpeed = .5f;
    Color changeColor = Color.white;
    private bool isChanging = false;

    // Update is called once per frame
    void Update()
    {
        if (!isChanging && changeColor != Color.white)
        {
            SetColor(changeColor);
        }
	}

    public void SetColor(Color spellColor)
    {
        color = GetComponent<SpriteRenderer>().color;
        changeColor = spellColor;
        if (!isChanging)
        {
			StartCoroutine(ChangeEngineColour(spellColor));
		}
    }

	private IEnumerator ChangeEngineColour(Color endColor)
	{
        isChanging = true;
        changeColor = Color.white;
		float tick = 0f;
		while (GetComponent<SpriteRenderer>().color != endColor)
		{
			tick += Time.deltaTime * changeSpeed;
			GetComponent<SpriteRenderer>().color = Color.Lerp(color, endColor, tick);
			yield return null;
		}
        isChanging = false;
	}

}

public enum SpellColor
{
    None,
    Red,
    Blue,
    Green,
    Yellow
}
