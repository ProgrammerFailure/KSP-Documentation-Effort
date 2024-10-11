using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Advanced Damper", 21)]
public class VPAdvancedDamper : VehicleBehaviour
{
	public int axle;

	public float slowBumpRate;

	public float fastBumpRate;

	[Range(0f, 1f)]
	public float bumpSpeedSplit;

	public float slowReboundRate;

	public float fastReboundRate;

	[Range(0f, 1f)]
	public float reboundSpeedSplit;

	private int[] m_wheels;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPAdvancedDamper()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetUpdateOrder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicleSuspension()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float ComputeDamperRate(float speed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float ComputeDamperRate(float speed, float slowRate, float fastRate, float speedSplit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float ComputeDamperForce(float speed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float ComputeDamperForce(float speed, float slowRate, float fastRate, float speedSplit)
	{
		throw null;
	}
}
