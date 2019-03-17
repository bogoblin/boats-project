using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SailHintMarker : MonoBehaviour {

	BoatSail sail;
	Vector3 originalPosition;
	Image image;
	RectTransform rect;

	// Use this for initialization
	void Start () {
		sail = GetComponentInParent<BoatSail>();
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		float factor = (sail.IdealAngle() - sail.localSailAngle)/Mathf.PI;
		if (Mathf.Abs(factor) > 0.9) factor = 0;
		float hue = Mathf.Lerp(120, 0, Mathf.Abs(factor)*10)/360;
		Color color = Color.HSVToRGB(hue, 1, 1);
		image.color = color;
		rect.localPosition = new Vector3(Mathf.Clamp(factor*1000, -256, 256), 0, 0);
	}
}