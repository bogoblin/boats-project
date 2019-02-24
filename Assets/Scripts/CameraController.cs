using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class CameraController : MonoBehaviour {

	public GameObject cameraTarget;
	private GameObject camera;
	public float speed = 1;
	public enum CameraMode {
		Checkpoint,
		Behind
	};
	public CameraMode cameraMode = CameraMode.Checkpoint;
	private Vector3 originalPosition;

	void Start () {
		// Get the camera game object
		camera = transform.GetChild(0).gameObject;
		originalPosition = camera.transform.localPosition;
	}
	
	void Update () {
		if (XCI.GetButtonDown(XboxButton.Y)      ) ToggleCameraMode();
		if (XCI.GetButton(XboxButton.RightBumper)) ZoomIn ();
		if (XCI.GetButton(XboxButton.LeftBumper )) ZoomOut();

		camera.transform.localPosition = Vector3.Lerp(
			originalPosition*1.5f,
			originalPosition*0.5f,
			ZoomLevel
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
