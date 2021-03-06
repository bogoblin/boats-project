﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToWeatherDirection : MonoBehaviour {
	private Weather weather;
	public enum WhichElement {
		wind,
		water
	};
	public WhichElement element;

	// Use this for initialization
	void Start () {
		weather = Weather.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		float angle = element==WhichElement.wind?weather.GetWindAngle():weather.GetWaterAngle();
		this.transform.SetPositionAndRotation(
			this.transform.position,
			Quaternion.AngleAxis(angle, Vector3.forward)
		);
	}
}
