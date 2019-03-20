using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadReplayDialog : MonoBehaviour {

	Dropdown dropdown;
	float timeScaleWhenOpened;
	bool showing = false;
	void Start () {
		CloseDialog();
	}
	
	public void ShowDialog() {
		if (showing) return;
		
		transform.GetChild(0).gameObject.SetActive(true);
		dropdown = GetComponentInChildren<Dropdown>();
		dropdown.ClearOptions();
		dropdown.AddOptions(Manager.listOfRecordings);

		// Keep track of the timescale, just in case it isn't just 1
		timeScaleWhenOpened = Time.timeScale;
		// Pause the game when this dialog box is open
		Time.timeScale = 0;
		showing = true;
	}

	void CloseDialog() {
		transform.GetChild(0).gameObject.SetActive(false);

		// Set the timescale back to what it was when this was opened
		Time.timeScale = timeScaleWhenOpened;
		showing = false;
	}

	public void Load() {
		// load the chosen replay
		Manager.ShowReplay(dropdown.value);
		CloseDialog();
	}

	public void Cancel() {
		CloseDialog();
	}
}
