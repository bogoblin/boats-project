using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSail : MonoBehaviour {

	public GameObject sailObject; // We want this gameobject to match the sail's angle.
	public GameObject forceArrow, apparentWindArrow;
	public float area; // The area of the sail.

	private float boatAngle {
		get { return transform.eulerAngles.y * Mathf.Deg2Rad; }
	}

	// The sail angle relative to the boat, in radians.
	// 0 means that the sail is completely parallel to the boat.
	// This value should be capped between -pi/2 and pi/2
	private float _localSailAngle = 0;
	private float localSailAngle {
		get { return _localSailAngle; }
		set { 
			_localSailAngle = Mathf.Clamp(value, -Mathf.PI/2, Mathf.PI/2); 
			sailObject.transform.localEulerAngles = new Vector3(0, localSailAngle*Mathf.Rad2Deg, 0);
		}
	}
	private Vector3 sailNormal {
		get { 
			return Quaternion.AngleAxis(globalSailAngle * Mathf.Rad2Deg, Vector3.up) * Vector3.right;
		}
	}
	private float sailAngularVelocity = 0;

	// The sail angle in world space, in radians.
	// This is automatically calculated and is a read only property.
	private float globalSailAngle {
		get { return boatAngle + localSailAngle; }
	}

	private BoatBehavior boatBehavior;
	private IBoatController controller {
		get { return boatBehavior.controller; }
	}
	private Weather weather;

	private Vector3 ApparentWind() {
		return weather.GetWindVector() - boatBehavior.GetGlobalVelocity();
	}
	private float ApparentWindAngle() {
		return Vector3.SignedAngle(ApparentWind(), Vector3.forward, Vector3.up) 
			* Mathf.Deg2Rad;
	}
	public float LiftMagnitude(float sailAngle) {
		return 0.5f * Constants.DensityOfAir 
			* ApparentWind().sqrMagnitude 
			* Mathf.Sin(ApparentWindAngle() - sailAngle) 
			* area;
	}
	public float LiftMagnitude() {
		return LiftMagnitude(globalSailAngle);
	}
	public Vector3 Lift() {
		return sailNormal * LiftMagnitude();
	}

	void Start () {
		boatBehavior = GetComponent<BoatBehavior>();
		weather = Weather.Instance;
	}
	
	void Update () {
		if (controller.UsePull()) 
		{
			sailAngularVelocity += LiftMagnitude(globalSailAngle);
			float maxAngleFraction = 1 - controller.GetSailPull();
			localSailAngle += sailAngularVelocity * Time.deltaTime;
			localSailAngle = Mathf.Clamp(
				localSailAngle, 
				-(Mathf.PI/2)*maxAngleFraction, 
				 (Mathf.PI/2)*maxAngleFraction );
		}
		else
		{
			sailAngularVelocity = -controller.GetSailTurn();
			localSailAngle += sailAngularVelocity * Time.deltaTime;
		}

		// Set the scales and rotations of the force and apparent wind arrows.
		forceArrow.transform.localScale = 0.01f * LiftMagnitude()/weather.GetWindSpeed() * Vector3.one;
		apparentWindArrow.transform.eulerAngles = new Vector3(0, ApparentWindAngle()*Mathf.Rad2Deg, 0);
	}
}
