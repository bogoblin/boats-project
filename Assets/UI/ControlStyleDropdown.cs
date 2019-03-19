using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlStyleDropdown : Dropdown {
	// This behavior ensures that the control style dropdown menu always displays the correct tooltip

	public GameObject[] Tooltips;
	private string[] ControlStyles = {"Direct", "Indirect", "Automatic"};

	void Start () {
		// Initialise the dropdown value to be the saved control style
		string ControlStyle = PlayerPrefs.GetString("Control Style", ControlStyles[0]);
		for (int i=0; i<ControlStyles.Length; i++) {
			if (ControlStyles[i] == ControlStyle) {
				OnValueChanged(i);
			}
		}
	}
	
	public void OnValueChanged (int newValue) {
		this.value = newValue;
		Debug.Log("adsfasdf");
		for (int i=0; i<Tooltips.Length; i++) {
			// Show the tooltip if it is the correct one to show, otherwise hide it
			bool show = (newValue == i);
			Tooltips[i].SetActive(show);
		}
		PlayerPrefs.SetString("Control Style", ControlStyles[newValue]);
	}
}
