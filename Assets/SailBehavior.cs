using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailBehavior : MonoBehaviour {

	private float LocalSailAngle = 0; // The angle of the sail, in degrees
	private float DensityOfAir = 1.225f;

	private Vector3 ApparentWind;

	public float WindMultiplier;

	public GameObject Ship, ApparentWindArrow, SailForceArrow;
	private BoatBehavior boatBehavior;
	public float Area, WindSpeed, WindAngle;

	public Vector3 GetWind() {
		return WindSpeed * new Vector3(Mathf.Sin(WindAngle*Mathf.Deg2Rad), 0, Mathf.Cos(WindAngle*Mathf.Deg2Rad));
	}

	public Vector3 GetVelocity() {
		return boatBehavior.GetGlobalVelocity();
	}

	public Vector3 GetApparentWind() {
		Vector3 AW = GetWind();// - GetVelocity();
		return new Vector3(AW.x, 0, AW.z);
	}

	public float GetSailAngle() {
		return this.transform.eulerAngles.y*Mathf.Deg2Rad;
	}

	public Vector3 GetLift() {
		// Lift = 1/2 * rho * v * v * a
		ApparentWind = GetApparentWind();
		float ApparentWindAngle = Mathf.Atan2(ApparentWind.x, ApparentWind.z);
		ApparentWindArrow.transform.eulerAngles = new Vector3(0, ApparentWindAngle * Mathf.Rad2Deg, 0);
		float LiftMagnitude = WindMultiplier * 0.5f * DensityOfAir * ApparentWind.sqrMagnitude * Mathf.Sin(GetSailAngle() - ApparentWindAngle) * Area;
		Vector3 SailNormal = Quaternion.AngleAxis(GetSailAngle() * Mathf.Rad2Deg, Vector3.up) * Vector3.left;
		//Debug.Log(SailNormal);
		SailForceArrow.transform.localScale = 0.001f * new Vector3(LiftMagnitude, LiftMagnitude, LiftMagnitude);
		return SailNormal * LiftMagnitude;
	}

	// Use this for initialization
	void Start () {
		boatBehavior = Ship.GetComponent<BoatBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(GetLift());
		this.transform.Rotate(Vector3.up, -LocalSailAngle);
		LocalSailAngle -= Input.GetAxis("Sail");
		LocalSailAngle = Mathf.Clamp(LocalSailAngle, -90, 90);
		this.transform.Rotate(Vector3.up, LocalSailAngle);
		//Debug.Log(GetLift());
	}
}
