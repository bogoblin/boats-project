using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointRace : MonoBehaviour {

	public bool lapped = false;
	public int numberOfLaps = 3;
	int playerPlace;
	
	[HideInInspector]
	public Dictionary<BoatBehavior, CheckpointInfo> checkpointInfos;
	private int checkpointCount;
	
	[HideInInspector]
	public Checkpoint[] checkpoints;
	private BoatBehavior[] boats;

	// Use this for initialization
	void Start () {
		playerPlace = 1; // What position the player is in - we update when another boat finishes.
		checkpoints = GetComponentsInChildren<Checkpoint>();
		checkpointInfos = new Dictionary<BoatBehavior, CheckpointInfo>();
		for (int i=0; i<checkpoints.Length; i++) {
			checkpoints[i].SetRace(this);
			checkpoints[i].SetIndex(i);
		}
		checkpoints[0].Show();

		int numberOfAI = Manager.numberOfAi;
		
		boats = new BoatBehavior[numberOfAI+1];
		boats[0] = Manager.InstantiatePlayerBoat(Vector3.zero, Quaternion.AngleAxis(90, Vector3.up)).GetComponent<BoatBehavior>();
		Debug.Log(boats[0].transform.position);

		for (int i=0; i<numberOfAI; i++) {
			boats[i+1] = Manager.InstantiateAiBoat(Vector3.zero + -Vector3.forward*(i+1)*10, Quaternion.AngleAxis(90, Vector3.up)).GetComponent<BoatBehavior>();
		}

		foreach (BoatBehavior boat in boats) {
			boat.target = checkpoints[0].target;
			Debug.Log(boat.target.transform.position);
		}
	}

	public void HitCheckpoint(BoatBehavior boat, int index) {
		try {
			checkpointInfos.Add(boat, new CheckpointInfo(boat, this));
		}
		catch (ArgumentException) {} // If the boat is already in the dictionary then we don't need to add it again

		if (checkpointInfos[boat].Hit(index)) {
			int nextIndex = (index+1)%checkpoints.Length;
			if (boat.isPlayer) {
				checkpoints[index].Hide();
				checkpoints[nextIndex].Show();
			}
			boat.target = checkpoints[nextIndex].target;
		}
	}

	public void Finish(BoatBehavior boat) {
		if (boat == boats[0]) { // is player boat
			boat.gameObject.GetComponentInChildren<FinishedPanel>().Show();
		} else {
			playerPlace++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		try {
			checkpointInfos[boats[0]].Update();
		}
		catch (KeyNotFoundException) {}
	}
}
