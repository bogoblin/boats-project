using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour {

	public GameObject water;

	public float WindSpeed, // Speed of the wind, in m/s
				 WindAngle; // Angle of the wind, in degrees

	public void SetWindSpeed(float speed) {
		WindSpeed = speed;
	}
	public Vector3 GetWindVector() {
		return WindSpeed * new Vector3(Mathf.Sin(WindAngle*Mathf.Deg2Rad), 0, Mathf.Cos(WindAngle*Mathf.Deg2Rad));
	}
	public float GetWindAngle() {
		return WindAngle;
	}
	public float GetWindSpeed() {
		return WindSpeed;
	}
	public float WaterSpeed, // Speed of the water, in m/s
				 WaterAngle; // Angle of the water, in degrees

	public void SetWaterSpeed(float speed) {
		WaterSpeed = speed;
	}
	
	public Vector3 GetWaterVector() {
		return WaterSpeed * new Vector3(Mathf.Sin(WaterAngle*Mathf.Deg2Rad), 0, Mathf.Cos(WaterAngle*Mathf.Deg2Rad));
	}
	public float GetWaterAngle() {
		return WaterAngle;
	}
	public float GetWaterSpeed() {
		return WaterSpeed;
	}
	public void SetWaterVector(float speed, float angle) {
		WaterSpeed = speed;
		WaterAngle = angle;
		WaterMaterial.SetVector("WaveSpeed", new Vector4(
			-GetWaterVector().x, -GetWaterVector().z, -GetWaterVector().x, -GetWaterVector().z
		)/(Time.deltaTime*2));
	}

	private Material WaterMaterial;

	// Use this for initialization
	void Start () {
		WaterMaterial = water.GetComponent<Renderer>().sharedMaterial;
		SetWaterVector(WaterSpeed, WaterAngle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
