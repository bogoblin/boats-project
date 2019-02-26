using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSail : MonoBehaviour {

	public GameObject sailObject; // We want this gameobject to match the sail's angle.
	public GameObject forceArrow, apparentWindArrow;
	public float area; // The area of the sail.

	/// <summary>
	/// The angle of the boat, in world space.
	/// </summary>
	private float boatAngle {
		get { return transform.eulerAngles.y * Mathf.Deg2Rad; }
	}

	/// <summary>
	/// The sail angle relative to the boat, in radians.
	/// 0 means that the sail is completely parallel to the boat.
	/// This value should be capped between -pi/2 and pi/2
	/// </summary>
	private float localSailAngle {
		get { return _localSailAngle; }
		set { 
			_localSailAngle = value; 
			sailObject.transform.localEulerAngles = new Vector3(0, localSailAngle*Mathf.Rad2Deg, 0);
		}
	}
	private float _localSailAngle = 0;
	private Vector3 SailNormal(float localAngle) {
		float angle = boatAngle + localAngle;
		return Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.up) * Vector3.right;
	}
	private float sailAngularVelocity = 0;

	/// <summary>
	/// The sail angle in world space, in radians.
	/// This is automatically calculated and is a read only property.
	/// </summary>
	private float globalSailAngle { get { return boatAngle + localSailAngle; } }

	private BoatBehavior boatBehavior;
	private IBoatController controller;
	private Weather weather;

	private Vector3 ApparentWind() {
		return weather.GetWindVector() - boatBehavior.GetGlobalVelocity();
	}
	private float ApparentWindAngle() {
		return Vector3.SignedAngle(ApparentWind(), Vector3.forward, Vector3.up) 
			* Mathf.Deg2Rad;
	}
	public float LiftMagnitude(float localAngle)
	{
		float sailAngle = boatAngle + localAngle;
		return 0.5f * Constants.DensityOfAir 
			* ApparentWind().sqrMagnitude 
			* Mathf.Sin(ApparentWindAngle() - sailAngle) 
			* area;
	}
	public float LiftMagnitude() { return LiftMagnitude(localSailAngle); }
	public Vector3 Lift(float localAngle) { 
		return SailNormal(localAngle) * LiftMagnitude(localAngle);
	}
	public Vector3 Lift() { return Lift(localSailAngle); }

	void Start() 
	{
		boatBehavior = GetComponent<BoatBehavior>();
		controller = boatBehavior.controller;
		weather = Weather.Instance;
	}
	
	private float sailPull = 0;
	public float GetTightness() {
		if (sailPull == 1) return 1;
		return (Mathf.Abs(localSailAngle/(Mathf.PI/2))) / (1-sailPull);
	}
	public float mass = 20;
	void Update() {
		// Update the sail's position based on the controller's inputs.
		if (controller.UsePull()) 
		{
			sailAngularVelocity -= LiftMagnitude()/mass * Time.deltaTime;
			// Dampen the sailAngularVelocity
			sailAngularVelocity *= (1-Time.deltaTime*mass);


			localSailAngle += sailAngularVelocity * Time.deltaTime;

			// The sail pull is how much the sail is being pulled in.
			// At 0, the sail isn't being pulled in at all, so it can go all the way out.
			// At 1, it is being pulled in completely and will be parallel with the boat.
			sailPull = controller.GetSailPull();
			float maxAngleFraction = 1 - sailPull;
			float newLocalSailAngle = Mathf.Clamp(
				localSailAngle, 
				-(Mathf.PI/2)*maxAngleFraction, 
				 (Mathf.PI/2)*maxAngleFraction );

			// Some clamping has occurred, which means the sail should bounce back a bit.
			if (newLocalSailAngle != localSailAngle) {
				sailAngularVelocity = localSailAngle - newLocalSailAngle;
				localSailAngle = newLocalSailAngle;
			}
		}
		else
		{
			sailAngularVelocity = -controller.GetSailTurn();
			localSailAngle += sailAngularVelocity * Time.deltaTime;
			localSailAngle = Mathf.Clamp(
				localSailAngle, 
				-(Mathf.PI/2), 
				 (Mathf.PI/2) );
		}

		// Set the scales and rotations of the force and apparent wind arrows.
		forceArrow.transform.localScale = 0.01f * LiftMagnitude()/weather.GetWindSpeed() * Vector3.one;
		apparentWindArrow.transform.eulerAngles = new Vector3(0, ApparentWindAngle()*Mathf.Rad2Deg, 0);
	}
}
