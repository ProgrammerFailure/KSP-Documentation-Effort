using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Rolling Friction", 41)]
public class VPRollingFriction : VehicleBehaviour
{
	public enum Model
	{
		Constant,
		Linear,
		Quadratic
	}

	public Model model;

	public float coefficient;

	public bool showDebugLabel;

	public float[] frictionFactors;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPRollingFriction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyRollingFriction(VehicleBase.WheelState ws, float factor)
	{
		throw null;
	}
}
