using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextReadWeatherSpeed : MonoBehaviour {
	private Weather weather;
	public enum WhichElement {
		wind,
		water
	};
	public WhichElement element;
	private Text text;

	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text>();
		weather = Weather.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = element==WhichElement.wind?
			String.Format("Wind speed: {0:0.0}m/s", weather.GetWindSpeed()) :
			String.Format("Water speed: {0:0.0}m/s", weather.GetWaterSpeed());
	}
}
