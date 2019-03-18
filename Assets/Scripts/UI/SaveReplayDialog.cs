using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SaveReplayDialog : MonoBehaviour {

	Recorder callbackRecorder;
	string replayName = "";

	float timeScaleWhenOpened = 1;

	static Regex validReplayName = new Regex(@"^(\w|\s)$");

	void Start () {
		CloseDialog();
	}

	public void ShowDialog(Recorder recorder) {
		// All the elements of the dialog box are children of a panel, the first and only child of this object
		transform.GetChild(0).gameObject.SetActive(true);
		callbackRecorder = recorder;

		// Keep track of the timescale, just in case it isn't just 1
		timeScaleWhenOpened = Time.timeScale;
		// Pause the game when this dialog box is open
		Time.timeScale = 0;
	}

	void CloseDialog() {
		transform.GetChild(0).gameObject.SetActive(false);

		replayName = "";
		callbackRecorder = null;

		// Set the timescale back to what it was when this was opened
		Time.timeScale = timeScaleWhenOpened;
	}

	/// <summary>
	/// This method should be invoked when the save button is pushed.
	/// </summary>
	public void Save() {
		if (callbackRecorder == null) return;
		// TODO: check that name doesn't have any commas in it
		callbackRecorder.Save(replayName);
		ResetRecorder();
		CloseDialog();
	}

	/// <summary>
	/// This method should be invoked when the cancel button is pushed.
	/// </summary>
	public void Cancel() {
		ResetRecorder();
		CloseDialog();
	}

	/// <summary>
	/// This method should be invoked when the input box is changed.
	/// </summary>
	public void SetReplayName(string text) {
		replayName = text;
	}

	/// <summary>
	/// This discards the old recorder and creates a new one on the same boat.
	/// This resets the recorder, deleting the recorded data and any state.
	/// </summary>
	void ResetRecorder() {
		callbackRecorder.gameObject.AddComponent(typeof(Recorder));
		Destroy(callbackRecorder);
	}
}
