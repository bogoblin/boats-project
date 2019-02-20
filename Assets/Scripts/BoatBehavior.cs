using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour {

	public GameObject Sail, Rudder, InnerBoat, cameraPointTo, cameraTarget;

	public float mass, HullSurfaceArea, boatArea;

	private SailBehavior sailBehavior;
	private RudderBehavior rudderBehavior;
	private const float DensityOfWater = 10;
	private const float Gravity = 9.81f;

	private Vector3 force = Vector3.zero;
	private Vector3 velocity = Vector3.zero,
					prevVelocity = Vector3.zero;
	private Vector3 torque = Vector3.zero;
	private Vector3 angularVelocity = Vector3.zero;
	private Weather weather;
	public GameObject weatherObject;

	[Range(0, 1)]
	private int controlStyle = 0;
	// Control Styles:
	// 0 - Direct
	// 1 - Indirect

	public void SetControlStyle(int newControlStyle) {
		if (newControlStyle > 1) return;
		controlStyle = newControlStyle;
		PlayerPrefs.SetInt("Control Style", controlStyle);
	}
	public int GetControlStyle() {
		return controlStyle;
	}

	public Vector3 GetLocalVelocity() {
		return velocity;
	}
	public Vector3 GetGlobalVelocity() {
		return Quaternion.Euler(this.transform.eulerAngles) * GetLocalVelocity();// + weather.GetWaterVector()*Time.deltaTime;
	}
	public float GetHeadingAngle() {
		return this.transform.eulerAngles.y * Mathf.Deg2Rad;
	}

	// Dimensions for prism model
	const float width = 1.4f; const float height = 0.4f; const float length = 4.2f;
	public float getDepth()
	{
		/* 
		To simplify the calculation of the buoyant forces on the boat,
		the boat is modelled as a triangular prism, with an isoceles triangle
		as the cross-section.
		*/
		float waterLevel = 0;
		float bottomOfBoatYCoordinate = this.transform.position.y - height;
		// Calculate how far the bottom of the hull is underwater
		float depth = waterLevel - bottomOfBoatYCoordinate;
		return depth;
	}
	public float submergedPrismArea() {
		float depth = getDepth();
		// Calculate the area of the front of the submerged prism
		float area = (width * Mathf.Pow(depth, 2)) / (2 * height);
		return area;
	}
	public float BouyancyForce() 
	{
		float depth = getDepth();

		if (depth < 0) 
		{ 	// Boat is not underwater at all
			return 0;
		}
		else 
		{ 	// Boat is underwater
			float displacedVolume = length * submergedPrismArea();
			return DensityOfWater * displacedVolume * Gravity;
		}
	}
	private const float CoefficientOfDragInWater = 8.0f;
	public float WaterResistanceForce()
	{
		float area = submergedPrismArea();
		float forwardVelocity = Vector3.Dot(velocity, Vector3.forward);
		if (forwardVelocity <= 0) return 0;

		float resistance = DensityOfWater * velocity.sqrMagnitude * CoefficientOfDragInWater * area / 2;
		return resistance;
	}

	// Use this for initialization
	void Start () {
		weather = weatherObject.GetComponent<Weather>();
		sailBehavior = Sail.GetComponent<SailBehavior>();
		sailBehavior.SetWeather(weather);
		rudderBehavior = Rudder.GetComponent<RudderBehavior>();
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
		velocity += (force / mass) * Time.deltaTime;
		//velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime*0.1f);
		//velocity -= Vector3.up * velocity.y * Time.deltaTime;
		prevVelocity = velocity;
		angularVelocity = new Vector3(
			0,
			Mathf.Lerp(angularVelocity.y, 0, Time.deltaTime),
			Mathf.Lerp(angularVelocity.z, 0, Time.deltaTime)
		);
		angularVelocity += torque * Time.deltaTime;
		this.transform.Translate(velocity*Time.deltaTime, Space.Self);
		this.transform.Translate(weather.GetWaterVector()*Time.deltaTime, Space.World);
		this.transform.Rotate(0, angularVelocity.y*Time.deltaTime, 0, Space.Self);
		InnerBoat.transform.Rotate(0, 0, angularVelocity.z*Time.deltaTime, Space.Self);
	}
	
	// Update is called once per frame
	void Update () {
		controlStyle = PlayerPrefs.GetInt("Control Style", 0);

		cameraTarget.transform.LookAt(cameraPointTo.transform, Vector3.up);
		force = Vector3.zero;
		torque = Vector3.zero;
		//Debug.Log(Input.GetAxis("BalanceRight")-Input.GetAxis("BalanceLeft"));
		
		// Simulate the vertical force from the water
		AddForce(Vector3.up * BouyancyForce());
		// Simulate the vertical force from gravity
		AddForce(Vector3.down * mass * Gravity);

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
		if (ForwardLift > 0) {
			AddForce(ForwardLift * Vector3.forward);
		}

		float BackwardDrag = WaterResistanceForce();
		AddForce(BackwardDrag * Vector3.back);

		DoPhysics();
		if (transform.position.x > 256) {
			transform.position -= Vector3.right*512;
		} else if (transform.position.x < -256) {
			transform.position += Vector3.right*512;
		}
		if (transform.position.z > 256) {
			transform.position -= Vector3.forward*512;
		} else if (transform.position.z < -256) {
			transform.position += Vector3.forward*512;
		}
		return;
	}
}
