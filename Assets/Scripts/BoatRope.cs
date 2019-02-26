using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRope : MonoBehaviour {
	private LineRenderer line;
	public GameObject start, end, rope;
	private BoatSail sail;
	public int segments = 2;
	private float totalRopeLength;
	Vector3 ropeStart, ropeEnd;

	float Cosh(float x) {
		return (Mathf.Exp(x) + Mathf.Exp(-x)) / 2;
	}

	void Start () {
		Vector3 ropeStart = start.transform.position;
		Vector3 ropeEnd = end.transform.position;
		
		sail = GetComponent<BoatSail>();
		line = rope.GetComponent<LineRenderer>();
		line.positionCount = segments + 1; // Include last point
		totalRopeLength = Vector3.Distance(ropeStart, ropeEnd);
	}
	void Update () {
		Vector3 ropeStart = start.transform.position;
		Vector3 ropeEnd = end.transform.position;
		float looseness = 1-sail.GetTightness();

		//float ropeLength = (1-sailPull*0.6f) * totalRopeLength;
		//float distance = Vector3.Distance(ropeStart, ropeEnd);
		//float tightness = (distance / ropeLength) * 10;

		for (int i = 0; i <= segments; i++) {
			float fraction = (float)i/(float)(segments);
			float yFraction;
			if (looseness > 0) {
				// Modelling the rope using a catenary.
				// this expression returns a number between 0 and 1 for 0 <= x <= 1
				// The curve is tighter if a is higher
				float x = fraction;
				float a = 1 - looseness;
				yFraction = (Cosh(x/a)-1) / (Cosh(1/a)-1);
			} else {
				yFraction = fraction;
			}

			Vector3 segPosition = new Vector3(
				Mathf.Lerp(ropeStart.x, ropeEnd.x, fraction),
				Mathf.Lerp(ropeStart.y, ropeEnd.y, yFraction),
				Mathf.Lerp(ropeStart.z, ropeEnd.z, fraction)
			);

			line.SetPosition(i, segPosition);
		}
	}
}
