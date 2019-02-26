using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindActOnCloth : MonoBehaviour {

	private Weather weather;
	private Cloth ClothComponent;
	private SailBehavior sailBehavior;

	// Use this for initialization
	void Start () {
		ClothComponent = this.GetComponent<Cloth>();
		weather = Weather.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		ClothComponent.externalAcceleration = weather.GetWindVector();
	}
}
