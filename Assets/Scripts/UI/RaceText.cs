using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceText : MonoBehaviour {

	Text text;

	void Start () {
		text = GetComponent<Text>();
	}

	public void SetText(string textToSet, Color color) {
		text.color = color;
		text.text = textToSet;
		text.CrossFadeAlpha(1, 0, false);
		text.CrossFadeAlpha(0, 4.0f, false);
	}
}
