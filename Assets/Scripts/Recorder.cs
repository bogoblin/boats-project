using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Recorder : MonoBehaviour {

	List<Vector3> position;
	List<Quaternion> rotation;
	List<float> localSailAngle;
	List<float> localRudderAngle;
	Vector3 wind;
	Vector3 water;
	bool recording = false;
	int frame = 0;

	BoatBehavior boat;
	BoatSail sail;
	RudderBehavior rudder;

	void Start() {
		boat = GetComponent<BoatBehavior>();
		sail = GetComponent<BoatSail>();
		rudder = boat.Rudder.GetComponent<RudderBehavior>();
	}

	public void StartRecording() {
		Debug.Log("Recording started.");
		recording = true;
		frame = 0;
		position = new List<Vector3>();
		rotation = new List<Quaternion>();
		localSailAngle = new List<float>();
		localRudderAngle = new List<float>();
		wind = Weather.Instance.GetWindVector();
		water = Weather.Instance.GetWaterVector();
	}

	void FixedUpdate() {
		if (!recording) return;

		position.Add(boat.transform.position);
		rotation.Add(boat.transform.rotation);
		localSailAngle.Add(sail.localSailAngle);
		localRudderAngle.Add(rudder.LocalRudderAngle);
		frame++;
	}

	void Update() {
		if (recording) {
			if (XCI.GetButtonDown(XboxButton.DPadUp)) {
				StopRecording();
			}
		} else {
			if (XCI.GetButtonDown(XboxButton.DPadUp)) {
				StartRecording();
			}
			if (frame > 0 && XCI.GetButtonDown(XboxButton.DPadDown)) {
				PlayRecording();
			}
		}
	}

	public void StopRecording() {
		recording = false;
		Debug.Log("stopped recording on frame "+frame.ToString());
		// dump output to file or whatever
	}

	public void PlayRecording() {
		Destroy(gameObject);
		GameObject playbackBoat = Instantiate(Manager.Instance.PlayerBoatPrefab, position[0], rotation[0]);
		Playback playback = playbackBoat.AddComponent(typeof(Playback)) as Playback;
		playback.StartPlayback(position, rotation, localSailAngle, localRudderAngle);
	}
}
