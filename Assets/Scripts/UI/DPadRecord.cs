using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPadRecord : MonoBehaviour {

	public Sprite off, on;
	Image thisImage;

	void Start() {
		thisImage = GetComponent<Image>();
	}
	void Update () {
		Recorder recorder = GetComponentInParent<Recorder>();
		if (recorder == null) {
			thisImage.enabled = false;
		}
		else if (recorder.IsRecording()) {
			thisImage.sprite = on;
			thisImage.enabled = true;
		}
		else {
			thisImage.sprite = off;
			thisImage.enabled = true;
		}
	}
}
