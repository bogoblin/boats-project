using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedPanel : MonoBehaviour {
	public GameObject placeText;

	public void Show() {
		transform.GetChild(0).gameObject.SetActive(true);
		Time.timeScale = 0;
	}
	void Start () {
		transform.GetChild(0).gameObject.SetActive(false);
	}
}
