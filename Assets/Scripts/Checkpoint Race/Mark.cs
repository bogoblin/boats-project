using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		BoatBehavior boatBehavior = collision.gameObject.GetComponentInParent<BoatBehavior>();
		if (boatBehavior) {
			CheckpointInfo checkpointInfo = GetComponentInParent<CheckpointRace>().checkpointInfos[boatBehavior];
			checkpointInfo.MarkHit();
		}
	}
}
