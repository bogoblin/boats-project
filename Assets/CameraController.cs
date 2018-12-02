using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject cameraTarget;
	public float speed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float angleBetween = Vector3.SignedAngle(this.transform.forward, cameraTarget.transform.forward, Vector3.down);
		Debug.Log(angleBetween);
		if (Mathf.Abs(angleBetween) < 0.1f) {
			angleBetween = 0;
		}
		//this.transform.eulerAngles = Vector3.Angle Lerp(this.transform.eulerAngles, cameraTarget.transform.eulerAngles, Time.deltaTime*speed);
		this.transform.Rotate(0, -angleBetween * Time.deltaTime * speed, 0, Space.World);
	}
}
