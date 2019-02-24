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
	private float initialYPosition;

	void Start () {
		initialYPosition = transform.position.y;
	}
	
	void Update () {
		if (Input.GetButtonDown("Change Camera")) {
			switch(cameraMode) {
				case CameraMode.Checkpoint: cameraMode = CameraMode.Behind; break;
				case CameraMode.Behind: cameraMode = CameraMode.Checkpoint; break;
			}
		}
		// Keep camera y position the same
		Vector3 correctPosition = new Vector3(
			transform.position.x,
			initialYPosition,
			transform.position.z
		);
		transform.SetPositionAndRotation(
			correctPosition,
			transform.rotation
		);

		float angleBetween;
		switch (cameraMode) {
			case CameraMode.Checkpoint:
			angleBetween = Vector3.SignedAngle(transform.forward, cameraTarget.transform.forward, Vector3.down);
			transform.Rotate(0, -angleBetween * Time.deltaTime * speed, 0, Space.World);
			transform.eulerAngles = new Vector3(
				0, transform.eulerAngles.y, 0
			);
			break;
			case CameraMode.Behind:
			transform.localEulerAngles = Vector3.zero;
			break;
			default:
			break;
		}
		
	}
}
