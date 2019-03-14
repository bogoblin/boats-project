using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnotsDisplay : MonoBehaviour {
	private BoatBehavior boatBehavior;

	// Use this for initialization
	void Start () {
		boatBehavior = GetComponentInParent<BoatBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 boatVelocity = boatBehavior.GetLocalVelocity();
		Vector3 velocityOnXZPlane = boatVelocity - Vector3.up * boatVelocity.y;
		this.GetComponent<Text>().text = (velocityOnXZPlane.magnitude * 1.944f).ToString("F1");// + " knots";
	}
}
