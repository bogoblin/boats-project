using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlStyleDropdown : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Control Style");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
