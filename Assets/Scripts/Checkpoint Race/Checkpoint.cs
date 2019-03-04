using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	private CheckpointRace race;
	private int index;

	public void SetRace(CheckpointRace cr) { race = cr; }
	public void SetIndex(int i) { index = i; }

	private void OnTriggerEnter(Collider other) {
		BoatBehavior boat = other.GetComponentInParent<BoatBehavior>();
		if (boat != null) {
			race.HitCheckpoint(boat, index);
		}
	}

	void Start() {
		Hide();
	}

	public void Show() {
		GetComponent<MeshRenderer>().enabled = true;
	}
	public void Hide() {
		GetComponent<MeshRenderer>().enabled = false;
	}
	public GameObject target {
		get { return gameObject; }
	}
}
