using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailBehavior : MonoBehaviour {

	private float LocalSailAngle = 0; // The angle of the sail, in degrees
	private float DensityOfAir = 1.225f;

	public GameObject Ship, ApparentWindArrow, SailForceArrow;
	private BoatBehavior boatBehavior;
	public float Area;
	private Weather weather;
	[HideInInspector] public float SailPull = 0;
	private float sailVelocity = 0;

	public Vector3 GetWind() {
		return weather.GetWindVector();
	}

	public Vector3 GetVelocity() {
		return boatBehavior.GetGlobalVelocity();
	}

	/// <summary>
	/// Returns the wind relative to the boat's movement.
	/// </summary>
	public Vector3 GetApparentWind() {
		return GetWind() - GetVelocity();
	}
	public float GetApparentWindAngle() {
		return Vector3.SignedAngle(GetApparentWind(), Vector3.forward, Vector3.up) 
			* Mathf.Deg2Rad;
	}
	public float GetSailAngle() {
		return this.transform.eulerAngles.y
			* Mathf.Deg2Rad;
	}
	public float GetLiftMagnitude() {
		return 0.5f * DensityOfAir * GetApparentWind().sqrMagnitude 
			* Mathf.Sin(GetApparentWindAngle() - GetSailAngle()) * Area;
	}
	public Vector3 GetLift() {
		// Lift = 1/2 * rho * v * v * a
		Vector3 SailNormal = transform.right;
		Debug.Log(GetLiftMagnitude());

		// TODO: refactor this
		ApparentWindArrow.transform.eulerAngles = new Vector3(0, GetApparentWindAngle() * Mathf.Rad2Deg, 0);
		float ForceArrowScale = Mathf.Clamp(0.0002f * GetLiftMagnitude(), -1, 1) * 2;
		SailForceArrow.transform.localScale = ForceArrowScale * Vector3.one;


		return SailNormal * GetLiftMagnitude();
	}

	void Start () {
		boatBehavior = Ship.GetComponent<BoatBehavior>();
		weather = Weather.Instance;
	}
	
	void Update () {
		Debug.Log(boatBehavior.controlStyle);
		switch (boatBehavior.controlStyle) {
			case "Direct":
			LocalSailAngle -= boatBehavior.controller.GetSailTurn();
			break;
			case "Indirect":
			float SailTorque = GetLiftMagnitude();
			float difference = 0;
			SailPull = Mathf.Lerp(SailPull, Mathf.Clamp(boatBehavior.controller.GetSailPull(), 0, 1), Time.deltaTime*10); 
			Debug.Log(SailPull);
			if (LocalSailAngle > 0) {
				difference = LocalSailAngle - Mathf.Clamp(LocalSailAngle, -90, (1-SailPull)*90);
			} else {
				difference = LocalSailAngle - Mathf.Clamp(LocalSailAngle, -(1-SailPull)*90, 90);
			}
			sailVelocity = - difference - SailTorque;
			LocalSailAngle += sailVelocity*Time.deltaTime;
			LocalSailAngle = Mathf.Clamp(LocalSailAngle, -(1-SailPull)*90, (1-SailPull)*90);
			break;
			default:
			break;
		}
		LocalSailAngle = Mathf.Clamp(LocalSailAngle, -90, 90);
		transform.localRotation = Quaternion.AngleAxis(LocalSailAngle, Vector3.up);
	}
}
