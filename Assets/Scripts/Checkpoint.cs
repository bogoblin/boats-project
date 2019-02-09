using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour {

	public GameObject buoy;
	public Checkpoint nextCheckpoint;
	public BoatBehavior boat;
	public Text timeText;
	private float currentTime = 0, currentLapTime = 0, bestTime = -1;
	private int lapNumber = 0;
	private BoxCollider triggerBox;
	public bool startFinish = false;
	private bool isCurrentCheckpoint = false;

	// Use this for initialization
	void Start () {
		// Start with the trigger not showing
		this.GetComponent<MeshRenderer>().enabled = false;
		triggerBox = this.GetComponent<BoxCollider>();
		if (startFinish) {
			MakeNextCheckpoint(0, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		currentLapTime += Time.deltaTime;
	}

	public void MakeNextCheckpoint(float lapTime, int lapN) {
		currentLapTime = lapTime;
		lapNumber = lapN;
		this.GetComponent<MeshRenderer>().enabled = true;
		isCurrentCheckpoint = true;
		boat.cameraPointTo = buoy;
	}

	public void OnTriggerEnter(Collider other) {
		if (isCurrentCheckpoint) {
			isCurrentCheckpoint = false;
			bool isFirstCheckpoint = false;
			this.GetComponent<MeshRenderer>().enabled = false;
			timeText.text = "";
			if (startFinish && lapNumber == 0) {
				lapNumber = 1;
				isFirstCheckpoint = true;
			} else if (startFinish) {
				timeText.text += "Lap Complete!\n";
			}
			if (bestTime == -1 && !isFirstCheckpoint) { // If no best time is set
				timeText.text += currentLapTime.ToString("F2") + "s";
				bestTime = currentLapTime;
				timeText.color = Color.black;
			} else if (bestTime > 0 && bestTime > currentLapTime) { // If we beat the best time
				timeText.text += currentLapTime.ToString("F2") + "s (" + (currentLapTime - bestTime).ToString("F2") + "s)";
				bestTime = currentLapTime;
				timeText.color = Color.green;
				if (startFinish) {
					timeText.text += "\nNew Lap Record!";
				}
			} else if (bestTime > 0 && bestTime <= currentLapTime){ // If we didn't beat the best time
				timeText.text += currentLapTime.ToString("F2") + "s (+" + (currentLapTime - bestTime).ToString("F2") + "s)";
				timeText.color = Color.red;
			}
			if (isFirstCheckpoint) {
				timeText.color = Color.yellow;
				timeText.text = "Lap Started";
			}
			if (startFinish) {
				currentLapTime = 0;
			}
			timeText.CrossFadeAlpha(1, 0, false);
			timeText.CrossFadeAlpha(0, 4, false);
			nextCheckpoint.MakeNextCheckpoint(currentLapTime, lapNumber);
		}
	}
}
