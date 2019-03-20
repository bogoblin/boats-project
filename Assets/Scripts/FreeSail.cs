using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSail : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject boat = Manager.InstantiatePlayerBoat(Vector3.zero, Quaternion.identity);
		if (boat) boat.GetComponentInChildren<CameraController>().cameraMode = CameraController.CameraMode.Behind;
	}
}
