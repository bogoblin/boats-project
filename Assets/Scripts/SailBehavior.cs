using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailBehavior : MonoBehaviour {
	private enum ControlStyle {
		automatic, move, windmove
	}
	private ControlStyle controlStyle = ControlStyle.windmove;

	private float LocalSailAngle = 0; // The angle of the sail, in degrees
	private float DensityOfAir = 1.225f;

	private Vector3 ApparentWind;

	public GameObject Ship, ApparentWindArrow, SailForceArrow;
	private BoatBehavior boatBehavior;
	public float Area;
	private Weather weather;
	private float SailPull = 0;

	public Vector3 GetWind() {
		return weather.GetWindVector();
	}

	public Vector3 GetVelocity() {
		return boatBehavior.GetGlobalVelocity();
	}

	public Vector3 GetApparentWind() {
		Vector3 AW = GetWind() - GetVelocity();
		AW = new Vector3(AW.x, 0, AW.z);
		return Vector3.Lerp(ApparentWind, AW, 0.2f);
	}

	public float GetSailAngle() {
		return this.transform.eulerAngles.y*Mathf.Deg2Rad;
	}

	public float GetLiftMagnitude() {
		float ApparentWindAngle = Mathf.Atan2(ApparentWind.x, ApparentWind.z);
		return 0.5f * DensityOfAir * ApparentWind.sqrMagnitude * Mathf.Sin(GetSailAngle() - ApparentWindAngle) * Area;
	}

	public void SetWeather(Weather w) {
		weather = w;
	}

	public Vector3 GetLift() {
		// Lift = 1/2 * rho * v * v * a
		ApparentWind = GetApparentWind();
		float ApparentWindAngle = Mathf.Atan2(ApparentWind.x, ApparentWind.z);
		ApparentWindArrow.transform.eulerAngles = new Vector3(0, ApparentWindAngle * Mathf.Rad2Deg, 0);
		float LiftMagnitude = GetLiftMagnitude();
		Vector3 SailNormal = Quaternion.AngleAxis(GetSailAngle() * Mathf.Rad2Deg, Vector3.up) * Vector3.left;
		//Debug.Log(SailNormal);
		float ForceArrowScale = Mathf.Clamp(0.0002f * LiftMagnitude, -1, 1) * 2;
		SailForceArrow.transform.localScale = ForceArrowScale * Vector3.one;
		return SailNormal * LiftMagnitude;
	}

	// Use this for initialization
	void Start () {
		boatBehavior = Ship.GetComponent<BoatBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (controlStyle) {
			case ControlStyle.automatic:
			break;

			case ControlStyle.windmove:
			float SailTorque = GetLiftMagnitude()*0.1f;
			SailPull = Mathf.Lerp(SailPull, Mathf.Clamp(Input.GetAxis("Pull"), 0, 1), Time.deltaTime*10); 
			LocalSailAngle -= SailTorque*Time.deltaTime;
			LocalSailAngle = Mathf.Clamp(LocalSailAngle, -90, 90);
			if (LocalSailAngle > 0) {
				LocalSailAngle = Mathf.Clamp(LocalSailAngle, -90, (1-SailPull)*90);
			} else {
				LocalSailAngle = Mathf.Clamp(LocalSailAngle, -(1-SailPull)*90, 90);
			}
			break;

			case ControlStyle.move:
			LocalSailAngle -= Input.GetAxis("Sail");
			break;

			default:
			break;
		}
		LocalSailAngle = Mathf.Clamp(LocalSailAngle, -90, 90);
		this.transform.localRotation = Quaternion.AngleAxis(LocalSailAngle, Vector3.up);
		
		//Debug.Log(GetLift());
	}
}
