using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherSlider : Slider {

	public Text weatherText;
	public enum WhichWeather {
		Wind, Water
	}
	public WhichWeather whichWeather;
	public enum SpeedOrAngle {
		Speed, Angle
	}
	public SpeedOrAngle speedOrAngle;

	// Use this for initialization
	void Start () {
		weatherText = GetComponentInChildren<Text>();
		Debug.Log(gameObject.name);
		if (gameObject.name.Contains("Speed")) {
			speedOrAngle = SpeedOrAngle.Speed;
		} else {
			speedOrAngle = SpeedOrAngle.Angle;
		}

		if (gameObject.name.Contains("Wind")) {
			whichWeather = WhichWeather.Wind;
			if (speedOrAngle == SpeedOrAngle.Speed) value = Weather.Instance.GetWindSpeed();
			else value = Weather.Instance.GetWindAngle();
		} else {
			whichWeather = WhichWeather.Water;
			if (speedOrAngle == SpeedOrAngle.Speed) value = Weather.Instance.GetWaterSpeed();
			else value = Weather.Instance.GetWaterAngle();
		}
		OnValueChanged(value);
	}

	public void OnValueChanged(float newValue) {
		if (whichWeather == WhichWeather.Wind) {
			if (speedOrAngle == SpeedOrAngle.Speed) {
				Weather.Instance.WindSpeed = newValue;
				weatherText.text = (newValue*Constants.MPSToKnots).ToString("F1")+" knots";
			} else {
				Weather.Instance.WindAngle = newValue;
				weatherText.text = (newValue).ToString("F0")+"o";
			}
		} else {
			if (speedOrAngle == SpeedOrAngle.Speed) {
				Weather.Instance.WaterSpeed = newValue;
				weatherText.text = (newValue*Constants.MPSToKnots).ToString("F1")+" knots";
			} else {
				Weather.Instance.WaterAngle = newValue;
				weatherText.text = (newValue).ToString("F0")+"o";
			}
		}
	}
}
