using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject cameraTarget;
	public float speed = 1;
	public enum CameraMode {
		Checkpoint,
		Behind
	};
	public CameraMode cameraMode = CameraMode.Checkpoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (cameraMode) {
			case CameraMode.Checkpoint:
			float angleBetween = Vector3.SignedAngle(this.transform.forward, cameraTarget.transform.forward, Vector3.down);
			//this.transform.eulerAngles = Vector3.Angle Lerp(this.transform.eulerAngles, cameraTarget.transform.eulerAngles, Time.deltaTime*speed);
			this.transform.Rotate(0, -angleBetween * Time.deltaTime * speed, 0, Space.World);
			this.transform.eulerAngles = new Vector3(
				0, this.transform.eulerAngles.y, 0
			);
			break;
			case CameraMode.Behind:
			this.transform.localEulerAngles = Vector3.zero;
			break;
			default:
			break;
		}
		
	}
}
