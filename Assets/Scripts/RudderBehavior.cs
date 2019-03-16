using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RudderBehavior : MonoBehaviour {
	
	private float _localRudderAngle;

	public float LocalRudderAngle {
		set {
			_localRudderAngle = value;
			transform.localRotation = Quaternion.AngleAxis(value, Vector3.up);
		}
		get {
			return _localRudderAngle;
		}
	}

	public GameObject Ship;
	private BoatBehavior boatBehavior;

	public float GetAngularAcceleration() {
		// How much the boat should turn, in radians per second per second
		return -Mathf.Sqrt(boatBehavior.GetLocalVelocity().z + 1) * LocalRudderAngle;
	}

	// Use this for initialization
	void Start () {
		boatBehavior = Ship.GetComponent<BoatBehavior>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		LocalRudderAngle = boatBehavior.controller.GetRudder()*30;
	}
}
