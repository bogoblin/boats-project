using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindActOnCloth : MonoBehaviour {

	public GameObject Sail;
	private Cloth ClothComponent;
	private SailBehavior sailBehavior;

	// Use this for initialization
	void Start () {
		ClothComponent = this.GetComponent<Cloth>();
		sailBehavior = Sail.GetComponent<SailBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
		ClothComponent.externalAcceleration = sailBehavior.GetWind();
	}
}
