using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Show the current point of sail on the text object this is attached to.
/// </summary>
public class PointOfSailText : MonoBehaviour {

	Text text;
	BoatSail sail;

	void Start () {
		text = GetComponent<Text>();
		sail = GetComponentInParent<BoatSail>();
	}

	void Update () {
		text.text = sail.PointOfSailing();
	}
}
