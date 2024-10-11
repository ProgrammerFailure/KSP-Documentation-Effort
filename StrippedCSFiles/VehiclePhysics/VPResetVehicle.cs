using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Utility/Reset Vehicle", 1)]
public class VPResetVehicle : VehicleBehaviour
{
	public KeyCode resetVehicleKey;

	public float heightIncrement;

	private bool m_doResetVehicle;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPResetVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DoReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetVehicle(VehicleBase vehicle, float height)
	{
		throw null;
	}
}
