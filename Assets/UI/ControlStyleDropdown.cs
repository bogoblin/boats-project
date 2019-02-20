using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlStyleDropdown : MonoBehaviour {
	// This behavior ensures that the control style dropdown menu always displays the correct tooltip

	public GameObject[] Tooltips;

	void Start () {
		// Initialise the dropdown value to be the saved control style
		this.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Control Style");
	}
	
	void Update () {
		for (int i=0; i<Tooltips.Length; i++) {
			// Show the tooltip if it is the correct one to show, otherwise hide it
			bool show = (this.GetComponent<Dropdown>().value == i);
			Tooltips[i].SetActive(show);
		}
	}
}
