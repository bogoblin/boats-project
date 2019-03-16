﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RudderBehavior : MonoBehaviour {

	public float LocalRudderAngle = 0;

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
		this.transform.Rotate(Vector3.up, LocalRudderAngle);
		LocalRudderAngle = boatBehavior.controller.GetRudder()*30;
		this.transform.Rotate(Vector3.up, -LocalRudderAngle);
	}
}
