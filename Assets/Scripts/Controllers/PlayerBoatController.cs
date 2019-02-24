using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerBoatController : MonoBehaviour, IBoatController {
	public float GetRudder() {
		return XCI.GetAxis(XboxAxis.LeftStickX);
	}
	public float GetSailPull() {
		return XCI.GetAxis(XboxAxis.RightTrigger);
	}
	public float GetSailTurn() {
		return XCI.GetAxis(XboxAxis.RightStickX);
	}
}
