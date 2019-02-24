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
	// The direction that the boat should point at ideally, if the wind was behind it.
	private Vector3 idealDirection; 

	// The direction that the boat should be in.
	private Vector3 targetDirection;
	private float pull;

	void Update () {
		idealDirection = 
			Vector3.Normalize(target.transform.position - transform.position);
		float dot = Vector3.Dot(idealDirection, weather.GetWindVector().normalized);
		pull = Mathf.Clamp01(-2*dot);
		targetDirection = idealDirection;
	}
	public float GetRudder () {
		float angleBetween = Mathf.Deg2Rad * 
			Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
		return angleBetween;
	}
	public float GetSailPull () {
		return pull;
	}
	public float GetSailTurn () {
		return 0;
	}
}
