using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRope : MonoBehaviour {

	private LineRenderer line;
	public GameObject end, boat;
	[HideInInspector] public BoatBehavior boatBehavior;
	public int segments = 2;
	private float totalRopeLength;
	Vector3 ropeStart, ropeEnd;

	float Cosh(float x) {
		return (Mathf.Exp(x) + Mathf.Exp(-x)) / 2;
	}

	void Start () {
		Vector3 ropeStart = this.transform.position;
		Vector3 ropeEnd = end.transform.position;
		
		boatBehavior = boat.GetComponent<BoatBehavior>();
		line = this.GetComponent<LineRenderer>();
		line.positionCount = segments + 1; // Include last point
		totalRopeLength = Vector3.Distance(ropeStart, ropeEnd);
	}
	void Update () {
		Vector3 ropeStart = this.transform.position;
		Vector3 ropeEnd = end.transform.position;
		float sailPull = boatBehavior.sailBehavior.SailPull;

		float ropeLength = (1-sailPull*0.6f) * totalRopeLength;
		float distance = Vector3.Distance(ropeStart, ropeEnd);
		float tightness = (distance / ropeLength) * 10;



		for (int i = 0; i <= segments; i++) {
			float fraction = (float)i/(float)(segments);
			float x = fraction;
			float a = tightness;

			// Modelling the rope using a catenary.
			// this expression returns a number between 0 and 1 for 0 <= x <= 1
			// The curve is tighter if a is higher
			float yFraction = (Cosh(x/a)-1) / (Cosh(1/a)-1);
			Vector3 segPosition = new Vector3(
				Mathf.Lerp(ropeStart.x, ropeEnd.x, fraction),
				Mathf.Lerp(ropeStart.y, ropeEnd.y, yFraction),
				Mathf.Lerp(ropeStart.z, ropeEnd.z, fraction)
			);

			line.SetPosition(i, segPosition);
		}
	}
}
