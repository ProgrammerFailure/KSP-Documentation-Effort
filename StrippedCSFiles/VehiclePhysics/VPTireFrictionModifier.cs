using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Tire Friction Modifier", 42)]
public class VPTireFrictionModifier : VehicleBehaviour
{
	public enum Wheel
	{
		Left,
		Right,
		Both
	}

	public int axle;

	public Wheel wheel;

	public TireFriction tireFriction;

	private TireFriction m_originalLeftFriction;

	private TireFriction m_originalRightFriction;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPTireFrictionModifier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableVehicle()
	{
		throw null;
	}
}
