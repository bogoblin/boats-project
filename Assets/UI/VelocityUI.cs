using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityUI : MonoBehaviour {

	public GameObject Ship, Sail;
	private BoatBehavior boatBehavior;
	private SailBehavior sailBehavior;
	public GameObject VelocityArrow, WindArrow, ApparentWindArrow;
	private RectTransform VelocityArrowRT, WindArrowRT, ApparentWindArrowRT;

	// Use this for initialization
	void Start () {
		boatBehavior = Ship.GetComponent<BoatBehavior>();
		sailBehavior = Sail.GetComponent<SailBehavior>();
		VelocityArrowRT = VelocityArrow.GetComponent<RectTransform>();
		WindArrowRT = WindArrow.GetComponent<RectTransform>();
		ApparentWindArrowRT = ApparentWindArrow.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		VelocityArrowRT.localScale = Vector3.one * boatBehavior.GetGlobalVelocity().magnitude;
		VelocityArrowRT.eulerAngles = new Vector3(0, 0, -Ship.transform.eulerAngles.y);
		Vector3 Wind = sailBehavior.GetWind();
		WindArrowRT.localScale = Vector3.one * Wind.magnitude;
		WindArrowRT.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Wind.x, Wind.z)*Mathf.Rad2Deg);
	}
}
