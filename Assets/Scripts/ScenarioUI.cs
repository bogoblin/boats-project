using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class ScenarioUI : MonoBehaviour {
	private bool showing = false;
	public Button dummy;
	void Update () {
		showing = XCI.GetButton(XboxButton.Back);
		GetComponentInParent<Canvas>().enabled = showing;
		if (!showing) {
			dummy.Select();
		}
	}
}
