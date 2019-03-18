using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPadPlayPause : MonoBehaviour {

	public Sprite play, pause;
	Image thisImage;

	void Start() {
		thisImage = GetComponent<Image>();
	}
	void Update () {
		Playback playback = GetComponentInParent<Playback>();
		if (playback == null) {
			thisImage.enabled = false;
		}
		else if (playback.IsPlaying()) {
			thisImage.sprite = pause;
			thisImage.enabled = true;
		}
		else {
			thisImage.sprite = play;
			thisImage.enabled = true;
		}
	}
}
