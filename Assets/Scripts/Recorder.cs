using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		rudder = boat.rudderBehavior;
		StartRecording();
	}

	public void StartRecording() {
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

		if (frame == 400) StopRecording();
	}

	public void StopRecording() {
		recording = false;
		Debug.Log("stopped recording on frame "+frame.ToString());
		Destroy(gameObject);
		GameObject playbackBoat = Instantiate(Manager.Instance.PlayerBoatPrefab, position[0], rotation[0]);
		Playback playback = playbackBoat.AddComponent(typeof(Playback)) as Playback;
		playback.StartPlayback(position, rotation, localSailAngle, localRudderAngle);
		// dump output to file or whatever
	}
}
