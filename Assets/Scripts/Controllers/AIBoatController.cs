using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoatController : MonoBehaviour, IBoatController {
	public GameObject target;
	private Weather weather;
	void Start () {
		weather = Weather.Instance;
		sail = GetComponent<BoatSail>();
	}
	///<summary>
	/// The direction that the boat should point at ideally, if the wind was behind it.
	///</summary>
	private Vector3 idealDirection; 

	///<summary>
	/// The direction that the boat should be in.
	///</summary>
	private Vector3 targetDirection;

	///<summary>
	/// The boat will sail no closer to the wind than this
	///</summary>
	public float tackAngle = 45;

	float Errf (float x) { return 2*(1 / (Mathf.Exp(-x) + 1)) - 1; }

	void Update () {
		target = GetComponent<BoatBehavior>().target;
		if (target == null) {
			target = this.gameObject;
		}
		// What we're trying to do here is choose a sensible direction to head in.
		// Niavely, this is the direction of the target, but sailing directly into the wind is bad.
		idealDirection = 
			Vector3.Normalize(target.transform.position - transform.position);

		// If this heading angle is low, the boat is sailing with the wind.
		float headingAngleToWind = Vector3.SignedAngle(idealDirection, weather.GetWindVector(), Vector3.up);

		// We can't sail directly into the wind.
		// If the direction is rotated so that it lands inside the tacking zone,
		// we can sail there. that is what this block does.
		if 		(headingAngleToWind >   180 - tackAngle)  {
			targetDirection = Quaternion.AngleAxis( tackAngle, Vector3.up) * idealDirection;
		}
		else if (headingAngleToWind < -(180 - tackAngle)) {
			targetDirection = Quaternion.AngleAxis(-tackAngle, Vector3.up) * idealDirection;
		}
		else {
			// Sailing directly towards the target is fine here.
			targetDirection = idealDirection;
		}
	}
	public float GetRudder () {
		float angleBetween = Mathf.Deg2Rad * 
			Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
		return Errf(angleBetween);
	}
	private BoatSail sail;
	public float GetSailPull () { return sail.IdealPull(); }
	public float GetSailTurn () { return 0;                }
	public  bool     UsePull () { return true;             }
}