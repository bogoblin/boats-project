﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PauseMenu : MonoBehaviour {

	float timeScaleWhenPaused = 1;

	void Hide() {
		showing = false;
		transform.GetChild(0).gameObject.SetActive(false);
		Time.timeScale = timeScaleWhenPaused;
	}
	void Show() {
		showing = true;
		transform.GetChild(0).gameObject.SetActive(true);
		timeScaleWhenPaused = Time.timeScale;
		Time.timeScale = 0;
	}
	void Toggle() {
		if (showing) Hide();
		else Show();
	}

	bool showing = false;

	void Start () {
		Hide();
	}
	void Update () {
		if (XCI.GetButtonDown(XboxButton.Start)) {
			Toggle();
		}
	}

	public void Continue() {
		Hide();
	}
	public void ExitToMenu() {
		Manager.MainMenu();
	}
	public GameObject recordingUi, sailGuide;
	public void SetShowRecordingControls(bool show) {
		recordingUi.SetActive(show);
	}
	public void SetShowSailGuide(bool show) {
		sailGuide.SetActive(show);
	}
}
