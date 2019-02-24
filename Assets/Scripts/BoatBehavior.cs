using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour {

	public GameObject Sail, Rudder, InnerBoat, cameraPointTo, cameraTarget;
	public IBoatController controller {
		get {
			return GetComponent<IBoatController>();
		}
	}

	public float mass, HullSurfaceArea, boatArea;

	[HideInInspector] public SailBehavior sailBehavior;
	private RudderBehavior rudderBehavior;
	private const float DensityOfWater = 10;
	private const float DensityOfAir = 1.225f;
	private const float Gravity = 9.81f;

	private Vector3 force = Vector3.zero;
	private Vector3 velocity = Vector3.zero;
	private Vector3 torque = Vector3.zero;
	private Vector3 angularVelocity = Vector3.zero;
	private Weather weather;

	public string controlStyle {
		get {
			return PlayerPrefs.GetString("Control Style", "Indirect");
		}
		set {
			PlayerPrefs.SetString("Control Style", controlStyle);
		}
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
		float topArea = width * length;

		if (depth < 0) 
		{ 	// Boat is not underwater at all
			// We add air resistance here that the boat gets from going upwards
			float airResistance = -topArea * DensityOfAir * Mathf.Pow(velocity.y, 2) / 2;
			return airResistance;
		}
		else 
		{ 	// Boat is underwater
			float displacedVolume = length * submergedPrismArea();
			float buoyancyForce = DensityOfWater * displacedVolume * Gravity * 5;
			float waterResistance = CoefficientOfDragInWater * topArea * DensityOfWater * Mathf.Pow(velocity.y, 2) / 2;
			return buoyancyForce + waterResistance;
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
	public float BuoyancyMoment()
	// The moment that buoyancy creates clockwise on the z axis
	{
		float buoyancyCentre = width * Mathf.Sin(HeelAngle());
		float distance = -buoyancyCentre * Mathf.Cos(HeelAngle());
		return mass * Gravity * distance;
	}
	public float GravityMoment()
	// The moment that gravity creates counterclockwise on the z axis
	{
		float distance = 0.3f * Mathf.Sin(HeelAngle());
		return mass * Gravity * distance;
	}
	public float WindForceMoment(float thwartshipWind)
	// The moment that wind force on the sail creates counterclockwise on the z axis
	{
		float distance = 0.1f * Mathf.Cos(HeelAngle());
		return distance * (thwartshipWind 
		- 0.5f * Mathf.Sign(angularVelocity.z) * Mathf.Pow(angularVelocity.z, 2) * sailBehavior.Area * DensityOfAir); // wind resistance on the sail
	}
	public float LateralResistanceMoment(float thwartshipWind)
	{
		float distance = -0.05f * Mathf.Cos(HeelAngle());
		return distance * thwartshipWind;
	}

	void Start () {
		weather = Weather.Instance;
		sailBehavior = Sail.GetComponent<SailBehavior>();
		rudderBehavior = Rudder.GetComponent<RudderBehavior>();
	}

	void AddForce(Vector3 forceToAdd) {
		force += forceToAdd;
	}
	void AddTorque(Vector3 torqueToAdd) {
		torque += torqueToAdd;
	}
	float HeelAngle() {
		// Return heel angle in radians
		return InnerBoat.transform.eulerAngles.z * Mathf.Deg2Rad;
	}
	void DoPhysics() {
		// Integrate velocity
		velocity += (force / mass) * Time.deltaTime;
		this.transform.Translate(velocity*Time.deltaTime, Space.Self);
		
		// Integrate angular velocity
		angularVelocity += torque * Time.deltaTime;
		this.transform.Rotate(0, angularVelocity.y*Time.deltaTime, 0, Space.Self);
		InnerBoat.transform.Rotate(0, 0, angularVelocity.z*Time.deltaTime, Space.Self);

		// Reset force and torque
		force = Vector3.zero;
		torque = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		controlStyle = PlayerPrefs.GetString("Control Style", "Indirect");

		cameraTarget.transform.LookAt(cameraPointTo.transform, Vector3.up);
		
		//Debug.Log(Input.GetAxis("BalanceRight")-Input.GetAxis("BalanceLeft"));
		/*
		// Simulate the vertical force from the water
		AddForce(Vector3.up * BouyancyForce());
		// Simulate the vertical force from gravity
		AddForce(Vector3.down * mass * Gravity);
		*/
		// Simulate the torque given by the water on the boat
		AddTorque(BuoyancyMoment() * Vector3.forward);
		AddTorque(GravityMoment() * Vector3.forward);

		// Simulate the torque from the rudder
		AddTorque(-Vector3.up * rudderBehavior.GetAngularAcceleration());
		AddTorque(Vector3.forward * rudderBehavior.GetAngularAcceleration() * 2);

		//Simulate the torque from the sail
		Vector3 SailLift = sailBehavior.GetLift();
		float thwartshipWind = Vector3.Dot(SailLift, -this.transform.right);
		AddTorque(WindForceMoment(thwartshipWind) * Vector3.forward);
		AddTorque(LateralResistanceMoment(thwartshipWind) * Vector3.forward);

		// Simulate the forward motion from the sail
		float ForwardLift = Vector3.Dot(this.transform.forward, SailLift);
		if (ForwardLift > 0) {
			AddForce(ForwardLift * Vector3.forward);
		}

		// Simulate drag on the boat going forward
		float BackwardDrag = WaterResistanceForce();
		AddForce(BackwardDrag * Vector3.back);

		// The boat should move with the current
		this.transform.Translate(weather.GetWaterVector()*Time.deltaTime, Space.World);

		// Dampen the rotation of the boat
		AddTorque(
			-DensityOfWater * angularVelocity.y * Vector3.up * 0.1f
		);

		DoPhysics();

		// Wrap position of boat
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
