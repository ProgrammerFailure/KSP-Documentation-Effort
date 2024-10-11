using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Aerodynamic Surface", 40)]
public class VPAeroSurface : VehicleBehaviour
{
	public float dragCoefficient;

	public float downforceCoefficient;

	public bool showDebugLabel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPAeroSurface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}
}
