using System;
using System.Runtime.CompilerServices;

namespace VehiclePhysics;

[Serializable]
public class ReferenceSpecs
{
	public float maxSpeed;

	public float maxRpm;

	public float maxTorque;

	public float maxPower;

	public int numGears;

	public float maxSuspensionDistance;

	public float maxSpringRate;

	public float maxAccelerationG;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReferenceSpecs()
	{
		throw null;
	}
}
