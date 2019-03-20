using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Playback : MonoBehaviour {

	List<Vector3> position;
	List<Quaternion> rotation;
	List<float> localSailAngle;
	List<float> localRudderAngle;
	bool playing = false;
	int frame = 0;

	BoatBehavior boat;
	BoatSail sail;
	RudderBehavior rudder;

	void Awake() {
		// Having a recorder and a playback on the same boat will cause problems, so delete the recorder if there is one.
		Destroy(GetComponent<Recorder>());
	}

	public void LoadReplay(string replayName) {
		replayName = "recording-"+replayName;
		string[] raw = PlayerPrefs.GetString(replayName, "").Split(',');
		int numOfFrames = int.Parse(raw[1]);
		List<Vector3> positionL = new List<Vector3>();
		List<Quaternion> rotationL = new List<Quaternion>();
		List<float> localSailAngleL = new List<float>();
		List<float> localRudderAngleL = new List<float>();

		Weather.Instance.WindSpeed = float.Parse(raw[2]);
		Weather.Instance.WindAngle = float.Parse(raw[3]);
		Weather.Instance.WaterSpeed = float.Parse(raw[4]);
		Weather.Instance.WaterAngle = float.Parse(raw[5]);
		string coursename = raw[6];
		int c = 7; // the cursor
		for (int f=0; f<numOfFrames; f++) {
			positionL.Add(new Vector3(
				float.Parse(raw[c]), 
				float.Parse(raw[c+1]), 
				float.Parse(raw[c+2])
			));
			rotationL.Add(new Quaternion(
				float.Parse(raw[c+3]),
				float.Parse(raw[c+4]),
				float.Parse(raw[c+5]),
				float.Parse(raw[c+6])
			));
			localSailAngleL.Add(float.Parse(raw[c+7]));
			localRudderAngleL.Add(float.Parse(raw[c+8]));
			c += 9;
		}
		StartPlayback(positionL, rotationL, localSailAngleL, localRudderAngleL);
	}

	public void StartPlayback(List<Vector3> positionL, List<Quaternion> rotationL, 
			List<float> localSailAngleL, List<float> localRudderAngleL) 
	{
		frame = 0;
		boat = GetComponent<BoatBehavior>();
		sail = GetComponent<BoatSail>();
		rudder = boat.Rudder.GetComponent<RudderBehavior>();
		position = positionL;
		rotation = rotationL;
		localSailAngle = localSailAngleL;
		localRudderAngle = localRudderAngleL;
		ResumePlayback();
	}

	void FixedUpdate() {
		ApplyRecordedAttributes();

		if (!playing) return;

		frame++;
	}

	void Update() {
		if (playing) {
			if (XCI.GetButtonDown(XboxButton.DPadUp)) {
				PausePlayback();
			}
		} else {
			if (XCI.GetButtonDown(XboxButton.DPadUp)) {
				ResumePlayback();
			}
		}
		if (XCI.GetButtonDown(XboxButton.DPadLeft)) {
			GetComponentInChildren<LoadReplayDialog>().ShowDialog();
		}
		int framesToStep = Mathf.RoundToInt(XCI.GetAxis(XboxAxis.LeftStickX)*2.5f);
		Step(framesToStep);
	}

	void ApplyRecordedAttributes() {
		if (frame >= position.Count) {
			frame = position.Count - 1;
			StopPlayback();
		}
		else if (frame < 0) {
			frame = 0;
		}
		boat.transform.position = position[frame];
		boat.transform.rotation = rotation[frame];
		sail.localSailAngle = localSailAngle[frame];
		rudder.LocalRudderAngle = localRudderAngle[frame];
	}

	public void StopPlayback() {
		playing = false;
		Time.timeScale = 0;
	}

	public void PausePlayback() {
		playing = false;
		Time.timeScale = 0;
	}
	public void ResumePlayback() {
		Time.timeScale = 1;
		playing = true;
		if (frame >= position.Count - 1) {
			frame = 0;
		}
	}
	public void Step(int framesToStep) {
		frame += framesToStep - Mathf.RoundToInt(Time.timeScale); // Subtracting timescale means that ffwd = rwnd
		ApplyRecordedAttributes();
	}
	public void Seek(int frameToSeek) {
		frame = frameToSeek;
		ApplyRecordedAttributes();
	}
	public bool IsPlaying() {
		return playing;
	}
}
