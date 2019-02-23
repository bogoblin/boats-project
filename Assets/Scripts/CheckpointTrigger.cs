using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {

	public int CheckpointIndex;
	public GameObject CheckpointManagerObject;
	private CheckpointManager checkpointManager;

	// Use this for initialization
	void Start () {
		checkpointManager = CheckpointManagerObject.GetComponent<CheckpointManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
		checkpointManager.TouchCheckpoint(CheckpointIndex);
	}
}
