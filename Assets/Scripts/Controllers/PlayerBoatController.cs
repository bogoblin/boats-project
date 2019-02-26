using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerBoatController : MonoBehaviour, IBoatController {
	public string ControlStyle {
		get { return PlayerPrefs.GetString("Control Style", "Direct"); }
	}
	public float GetRudder() {
		return XCI.GetAxis(XboxAxis.LeftStickX);
	}
	public float GetSailPull() {
		switch(ControlStyle) {
		  case "Indirect":
			return XCI.GetAxis(XboxAxis.RightTrigger);

		  case "Automatic":
			return GetComponent<BoatSail>().IdealPull();

		  default: return 0;
		}
	}
	public float GetSailTurn() {
		return XCI.GetAxis(XboxAxis.RightStickX);
	}
	public bool UsePull() {
		return !ControlStyle.Equals("Direct");
	}
}
