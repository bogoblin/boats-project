using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoatController : MonoBehaviour, IBoatController {
	private BoatBehavior boatBehavior;
	public GameObject target;
	private Weather weather;
	void Start () {
		weather = Weather.Instance;
	}
	void Update () {
		
	}
	public float GetRudder () {
		return 1;
	}
	public float GetSailPull () {
		return 0;
	}
	public float GetSailTurn () {
		return 0;
	}
}
