using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnotsDisplay : MonoBehaviour {

	public GameObject Boat;
	private BoatBehavior boatBehavior;

	// Use this for initialization
	void Start () {
		boatBehavior = Boat.GetComponent<BoatBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Text>().text = (boatBehavior.GetLocalVelocity().magnitude * 1.944f).ToString("F1") + " knots";
	}
}
