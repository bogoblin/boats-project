using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTime : MonoBehaviour {

	Text text;

	void Start() {
		text = GetComponent<Text>();
	}

	public void SetTime(float timeInSeconds) {
		int minutes = Mathf.FloorToInt(timeInSeconds/60);
		int seconds = Mathf.FloorToInt(timeInSeconds) - minutes*60;
		int centiseconds = Mathf.FloorToInt((timeInSeconds - minutes*60 - seconds)*100);
		text.text = minutes.ToString() + ":" + seconds.ToString("D2") + "." + centiseconds.ToString("D2");
	}
}
