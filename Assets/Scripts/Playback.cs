using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		Destroy(GetComponent<Recorder>());
	}

	public void StartPlayback(List<Vector3> positionL, List<Quaternion> rotationL, 
			List<float> localSailAngleL, List<float> localRudderAngleL) 
	{
		playing = true;
		frame = 0;
		boat = GetComponent<BoatBehavior>();
		sail = GetComponent<BoatSail>();
		rudder = boat.Rudder.GetComponent<RudderBehavior>();
		position = positionL;
		rotation = rotationL;
		localSailAngle = localSailAngleL;
		localRudderAngle = localRudderAngleL;

	}

	void FixedUpdate() {
		if (!playing) return;

		ApplyRecordedAttributes();
		frame++;
		if (frame == position.Count) {
			StopPlayback();
		}
	}

	void ApplyRecordedAttributes() {
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
		Time.timeScale = 0;
	}
	public void ResumePlayback() {
		Time.timeScale = 1;
	}
	public void Step(int framesToStep) {
		frame += framesToStep;
		ApplyRecordedAttributes();
	}
	public void Seek(int frameToSeek) {
		frame = frameToSeek;
		ApplyRecordedAttributes();
	}
}
