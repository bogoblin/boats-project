using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour {

	public GameObject Sail, Rudder, InnerBoat;

	public float mass, HullSurfaceArea;

	private SailBehavior sailBehavior;
	private RudderBehavior rudderBehavior;
	private float DensityOfWater = 10;

	private Vector3 force = Vector3.zero;
	private Vector3 velocity = Vector3.zero;
	private Vector3 torque = Vector3.zero;
	private Vector3 angularVelocity = Vector3.zero;
	private Vector3 prevPosition, globalVelocity;

	public Vector3 GetLocalVelocity() {
		return velocity;
	}
	public Vector3 GetGlobalVelocity() {
		return velocity;
	}
	public float GetHeadingAngle() {
		return this.transform.eulerAngles.y * Mathf.Deg2Rad;
	}

	public float BouyancyAcceleration() {
		if (this.transform.position.y > 0) {
			return 0;
		}
		else {
			return (1-this.transform.position.y) * 9.81f;
		}
	}

	// Use this for initialization
	void Start () {
		sailBehavior = Sail.GetComponent<SailBehavior>();
		rudderBehavior = Rudder.GetComponent<RudderBehavior>();
		prevPosition = this.transform.position;
		globalVelocity = Vector3.zero;
	}

	void AddForce(Vector3 forceToAdd) {
		force += forceToAdd;
	}
	void AddTorque(Vector3 torqueToAdd) {
		torque += torqueToAdd;
	}
	float HeelAngle() {
		return InnerBoat.transform.eulerAngles.z;
	}
	void DoPhysics() {
		prevPosition = this.transform.position;
		force += Vector3.up * -9.81f;
		velocity += force * Time.deltaTime;
		velocity = Vector3.Lerp(velocity, velocity*0.9f, Time.deltaTime);
		angularVelocity = new Vector3(
			0,
			Mathf.Lerp(angularVelocity.y, 0, Time.deltaTime),
			Mathf.Lerp(angularVelocity.z, 0, Time.deltaTime)
		);
		angularVelocity += torque * Time.deltaTime;
		this.transform.Translate(velocity*Time.deltaTime, Space.Self);
		this.transform.Rotate(0, angularVelocity.y*Time.deltaTime, 0, Space.Self);
		InnerBoat.transform.Rotate(0, 0, angularVelocity.z*Time.deltaTime, Space.Self);
		globalVelocity = (this.transform.position - prevPosition)/Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		force = Vector3.zero;
		torque = Vector3.zero;
		//Debug.Log(Input.GetAxis("BalanceRight")-Input.GetAxis("BalanceLeft"));
		
		// Simulate the vertical force from the water on the boat
		AddForce(Vector3.up * BouyancyAcceleration());
		// Simulate the torque given by the water on the boat
		if (HeelAngle() > 180) {
			AddTorque( 10 * Vector3.forward * Mathf.Pow(360-HeelAngle(), 2));
		} else {
			AddTorque(-10 * Vector3.forward * Mathf.Pow(HeelAngle(), 2));
		}

		// Simulate the torque from the rudder
		AddTorque(-Vector3.up * rudderBehavior.GetAngularAcceleration());

		//Simulate the torque from the sail
		Vector3 SailLift = sailBehavior.GetLift();
		if (Vector3.Dot(SailLift, -this.transform.right) > 0) {
			AddTorque(-Vector3.Dot(SailLift, -this.transform.right)*this.transform.forward*0.1f);
		} else {
			AddTorque(Vector3.Dot(SailLift, -this.transform.right)*this.transform.forward*0.1f);
		}

		// Simulate the forward motion from the sail
		float ForwardLift = Vector3.Dot(this.transform.forward, SailLift);
		ForwardLift = Mathf.Clamp(ForwardLift, 0, 10000);
		//Debug.Log(ForwardLift);
		float BackwardDrag = 0.5f * DensityOfWater * HullSurfaceArea * Time.deltaTime * Mathf.Pow(GetLocalVelocity().magnitude, 2);
		if (BackwardDrag > ForwardLift) {
			BackwardDrag = ForwardLift;
		}
		
		AddForce((ForwardLift - BackwardDrag) * Vector3.forward);
		DoPhysics();
		return;
	}
}
