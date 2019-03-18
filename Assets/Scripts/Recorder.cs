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

	string recordingString = "";

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
		recordingString = "";
		recording = true;
		frame = 0;
		position = new List<Vector3>();
		rotation = new List<Quaternion>();
		localSailAngle = new List<float>();
		localRudderAngle = new List<float>();
		wind = Weather.Instance.GetWindVector();
		water = Weather.Instance.GetWaterVector();

		recordingString += Weather.Instance.WindSpeed.ToString()+",";
		recordingString += Weather.Instance.WindAngle.ToString()+",";
		recordingString += Weather.Instance.WaterSpeed.ToString()+",";
		recordingString += Weather.Instance.WaterAngle.ToString()+",";

		string coursename = "thecourse";
		recordingString += coursename+",";
	}

	void FixedUpdate() {
		if (!recording) return;

		position.Add(boat.transform.position);
		rotation.Add(boat.transform.rotation);
		localSailAngle.Add(sail.localSailAngle);
		localRudderAngle.Add(rudder.LocalRudderAngle);

		recordingString += position[frame].x.ToString()+",";
		recordingString += position[frame].y.ToString()+",";
		recordingString += position[frame].z.ToString()+",";

		recordingString += rotation[frame].x.ToString()+",";
		recordingString += rotation[frame].y.ToString()+",";
		recordingString += rotation[frame].z.ToString()+",";
		recordingString += rotation[frame].w.ToString()+",";

		recordingString += localSailAngle[frame].ToString()+",";
		recordingString += localRudderAngle[frame].ToString()+",";
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
		GetComponentInChildren<SaveReplayDialog>().ShowDialog(this);
	}

	public void PlayRecording() {
		Destroy(gameObject);
		GameObject playbackBoat = Manager.InstantiatePlayerBoat(position[0], rotation[0]);
		Playback playback = playbackBoat.AddComponent(typeof(Playback)) as Playback;
		playback.StartPlayback(position, rotation, localSailAngle, localRudderAngle);
	}

	public void Save(string recordingName) {
		recordingString = recordingName+","+frame.ToString()+","+recordingString;

		// Save this recording to playerprefs (IndexedDB in web)
		PlayerPrefs.SetString("recording-"+recordingName, recordingString);

		// Update the list of recordings by putting this recording at the end with a comma (separator)
		PlayerPrefs.SetString("recordings", PlayerPrefs.GetString("recordings", "")+recordingName+",");
		Debug.Log(recordingName + " replay saved");
	}

	public bool IsRecording() {
		return recording;
	}
}
