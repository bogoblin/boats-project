using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class CameraController : MonoBehaviour {

	public GameObject cameraTarget;
	public float speed = 1;
	public enum CameraMode {
		Checkpoint,
		Behind
	};
	public CameraMode cameraMode = CameraMode.Checkpoint;
	private Vector3 originalPosition;
	
	void Update () {
		if (XCI.GetButtonDown(XboxButton.Y)) ToggleCameraMode();
		if (XCI.GetButton(XboxButton.RightBumper)) ZoomIn();
		if (XCI.GetButton(XboxButton.LeftBumper)) ZoomOut();

		// camera.transform.localPosition = Vector3.Lerp(
		// 	originalPosition*1.5f,
		// 	originalPosition*0.5f,
		// 	ZoomLevel
		// );
		transform.localScale = Mathf.Lerp(1.5f, 0.5f, ZoomLevel) * Vector3.one;

		float angleBetween;
		switch (cameraMode) {
			case CameraMode.Checkpoint:
			// Find the angle we need to turn to face the target
			angleBetween = Vector3.SignedAngle(transform.forward, cameraTarget.transform.forward, Vector3.down);
			// Rotate a small percentage of that angle
			transform.Rotate(0, -angleBetween * Time.deltaTime * speed, 0, Space.World);
			// Just making sure that the camera only rotates on the y axis.
			transform.eulerAngles = new Vector3(
				0, transform.eulerAngles.y, 0
			);
			break;
			case CameraMode.Behind:
			// Rotate towards the identity rotation
			transform.localRotation = Quaternion.Slerp(
				transform.localRotation,
				Quaternion.identity,
				Time.deltaTime
			);
			break;
			default:
			break;
		}
	}

	private float ZoomLevel = 0.5f;
	public float ZoomSpeed = 0.01f;
	void ZoomIn() {
		ZoomLevel += ZoomSpeed;
		ZoomLevel = Mathf.Clamp01(ZoomLevel);
	}
	void ZoomOut() {
		ZoomLevel -= ZoomSpeed;
		ZoomLevel = Mathf.Clamp01(ZoomLevel);
	}
	void ToggleCameraMode() {
		switch(cameraMode) {
			case CameraMode.Checkpoint: cameraMode = CameraMode.Behind; break;
			case CameraMode.Behind: cameraMode = CameraMode.Checkpoint; break;
		}
	}
}
