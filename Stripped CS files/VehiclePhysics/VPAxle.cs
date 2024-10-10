using System;

namespace VehiclePhysics;

[Serializable]
public class VPAxle
{
	public VPWheelCollider leftWheel;

	public VPWheelCollider rightWheel;

	public Brakes.BrakeCircuit brakeCircuit;

	public Steering.SteeringMode steeringMode;

	public float steeringRatio = 1f;
}
