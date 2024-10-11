using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class VPTireFrictionMultiplier : VehicleBehaviour
{
	[Range(0f, 2f)]
	public float frictionMultiplier;

	private float m_lastValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPTireFrictionMultiplier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetFrictionMultiplierInAllWheels(VehicleBase vehicle, float multiplier)
	{
		throw null;
	}
}
