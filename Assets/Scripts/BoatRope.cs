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
		Material[] newMaterials = {
			new Material(Shader.Find("Sprites/Diffuse"))
		};
		line.materials = newMaterials;
	}
	Color GetColor(float tightness) {
		return Color.HSVToRGB(Mathf.Lerp(240f/360f, 1, tightness), 1, 1);
	}
	void FixedUpdate () {
		Vector3 ropeStart = start.transform.position;
		Vector3 ropeEnd = end.transform.position;
		float tightness = sail.GetTightness();

		Color color = GetColor(tightness);
		line.startColor = color;
		line.endColor   = color;

		//float ropeLength = (1-sailPull*0.6f) * totalRopeLength;
		//float distance = Vector3.Distance(ropeStart, ropeEnd);
		//float tightness = (distance / ropeLength) * 10;

		for (int i = 0; i <= segments; i++) {
			float fraction = (float)i/(float)(segments);
			float yFraction;
			// Modelling the rope using a catenary.
			// this expression returns a number between 0 and 1 for 0 <= x <= 1
			// The curve is tighter if a is higher
			float a = 1;
			float x = fraction;
			float catenaryY = (Cosh(x/a)-1) / (Cosh(1/a)-1);
			yFraction = Mathf.Lerp(fraction, catenaryY, 1 - tightness);

			Vector3 segPosition = new Vector3(
				Mathf.Lerp(ropeStart.x, ropeEnd.x, fraction),
				Mathf.Lerp(ropeStart.y, ropeEnd.y, yFraction),
				Mathf.Lerp(ropeStart.z, ropeEnd.z, fraction)
			);

			line.SetPosition(i, segPosition);
		}
	}
}
