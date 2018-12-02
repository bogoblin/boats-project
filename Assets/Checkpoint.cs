using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public GameObject buoy;
	public Checkpoint nextCheckpoint;
	public BoatBehavior boat;
	private BoxCollider triggerBox;
	public bool startFinish = false;
	private bool isCurrentCheckpoint = false;

	// Use this for initialization
	void Start () {
		// Start with the trigger not showing
		this.GetComponent<MeshRenderer>().enabled = false;
		triggerBox = this.GetComponent<BoxCollider>();
		if (startFinish) {
			MakeNextCheckpoint();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void MakeNextCheckpoint() {
		Debug.Log("awsoem!");
		this.GetComponent<MeshRenderer>().enabled = true;
		isCurrentCheckpoint = true;
		boat.cameraPointTo = buoy;
	}

	public void OnTriggerEnter(Collider other) {
		nextCheckpoint.MakeNextCheckpoint();
		boat.SetNextCheckpoint(nextCheckpoint);
		this.GetComponent<MeshRenderer>().enabled = false;
	}
}
