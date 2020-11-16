using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    private Transform bar;
    private TextMesh text;

	private void Awake () {
        bar = transform.Find("Bar");
        text = GetComponent<TextMesh>();
	}

    public void SetSize(float sizeNormalized) {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetColor(Color color) {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }

    public void SetText(string name)
    {
        text.text = name;
    }
}
