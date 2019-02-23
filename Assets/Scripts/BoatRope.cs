using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRope : MonoBehaviour {

	private LineRenderer line;
	public GameObject end;
	public int segments = 2;
	public float totalRopeLength = 2;

	float Cosh(float x) {
		return (Mathf.Exp(x) + Mathf.Exp(-x)) / 2;
	}

	// Use this for initialization
	void Start () {
		line = this.GetComponent<LineRenderer>();
		line.positionCount = 2;//;= segments;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 ropeStart = this.transform.position;
		Vector3 ropeEnd = end.transform.position;
		// float sailPull = 1 - Input.GetAxis("Pull");
		// float ropeLength = sailPull * totalRopeLength;
		// float distance = Vector3.Distance(ropeStart, ropeEnd);
		// float power;
		// if (ropeLength > distance) {
		// 	power = ropeLength / distance;
		// 	Debug.Log(power);
		// } else {
		// 	power = 1;
		// }
		// for (int i = 0; i < segments; i++) {
		// 	float fraction = (float)i/(float)(segments-1);
		// 	Vector3 segPosition = Vector3.Lerp(ropeStart, ropeEnd, fraction);
		// 	segPosition = new Vector3(
		// 		Mathf.Lerp(ropeStart.x, ropeEnd.x, fraction),
		// 		Mathf.Lerp(ropeStart.y, ropeEnd.y, Mathf.Pow(fraction, power)),
		// 		Mathf.Lerp(ropeStart.z, ropeEnd.z, fraction)
		// 	);

		// 	line.SetPosition(i, segPosition);
		// }
		line.SetPosition(0, ropeEnd);
		line.SetPosition(1, ropeStart);
	}
}
