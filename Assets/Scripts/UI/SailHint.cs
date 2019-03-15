using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailHint : MonoBehaviour {

	BoatSail sail;
	Vector3 originalPosition;
	MeshRenderer meshR;

	// Use this for initialization
	void Start () {
		sail = GetComponentInParent<BoatSail>();
		originalPosition = transform.localPosition;
		meshR = GetComponent<MeshRenderer>();
		meshR.material = new Material(Shader.Find("Sprites/Diffuse")); 
	}
	
	// Update is called once per frame
	void Update () {
		float factor = (sail.IdealAngle() - sail.localSailAngle);
		if (factor > 3) factor = 0;
		Color color = Color.Lerp(Color.green, Color.red, factor);
		meshR.material.color = color;
		transform.localPosition = originalPosition + Vector3.right * factor * 0.1f;
	}
}
