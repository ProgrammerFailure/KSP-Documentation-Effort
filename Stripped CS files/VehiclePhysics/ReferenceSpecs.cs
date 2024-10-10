using System;

namespace VehiclePhysics;

[Serializable]
public class ReferenceSpecs
{
	public float maxSpeed = 50f;

	public float maxRpm = 7000f;

	public float maxTorque = 200f;

	public float maxPower = 100f;

	public int numGears = 5;

	public float maxSuspensionDistance = 0.25f;

	public float maxSpringRate = 25000f;

	public float maxAccelerationG = 3f;
}
