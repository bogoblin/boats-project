using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

	//public GameObject[] Checkpoints;
	public int NumberOfCheckpoints;

	private int CurrentCheckpoint = 0;
	private float time = 0;
	private bool Started = false;
	private bool Finished = false;
	private BoxCollider Trigger;

	// Use this for initialization
	void Start () {
		Trigger = this.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Started && !Finished) {
			time += Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (CurrentCheckpoint == 0 && !Started) {
			CurrentCheckpoint = 1;
			Started = true;
			Debug.Log("Started!");
		}
		if (CurrentCheckpoint == NumberOfCheckpoints+1) {
			Finished = true;
			Debug.Log("Finished in "+time.ToString()+" seconds!");
		}
	}

	public void TouchCheckpoint(int CheckpointIndex) {
		if (Started && CurrentCheckpoint == CheckpointIndex) {
			CurrentCheckpoint += 1;
			Debug.Log("Hit Checkpoint "+(CheckpointIndex).ToString()+"!  "+time.ToString()+"s");
		}
	}
}
