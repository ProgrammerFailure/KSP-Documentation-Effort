using UnityEngine;

public class SimpleOrbitController : MonoBehaviour
{
	public Transform satellite;

	public double distance;

	public double period;

	public double orbitalSpeed;

	public Vector3d orbitalPosition;

	public double orbitalAngle;

	public QuaternionD orbitalRot;

	public void Start()
	{
		orbitalPosition = Vector3d.right * distance;
	}

	public void FixedUpdate()
	{
		orbitalSpeed = 360.0 / period * (double)Time.fixedDeltaTime;
		orbitalRot = QuaternionD.AngleAxis(orbitalSpeed, Vector3d.up);
		orbitalPosition = orbitalRot * orbitalPosition;
		satellite.position = base.transform.position + orbitalPosition;
	}
}
