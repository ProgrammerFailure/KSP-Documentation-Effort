using System.Runtime.CompilerServices;
using EdyCommonTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Progressive Suspension", 23)]
public class VPProgressiveSuspension : VehicleBehaviour
{
	public enum Wheel
	{
		Left,
		Right,
		Both
	}

	public int axle;

	public Wheel wheel;

	[Range(0f, 0.999f)]
	[Tooltip("Minimum compression ratio for the suspension offsets to have effect. Maximum offsets are applied at 100% compression.")]
	public float minCompression;

	[Tooltip("Maximum spring amount to be applied at 100% compression.")]
	[FormerlySerializedAs("springRateOffsetAtMaxDepth")]
	public float maxSpringRateOffset;

	[Tooltip("Also apply an offset to the suspension damper.")]
	public bool adjustDamper;

	[FormerlySerializedAs("damperRateOffsetAtMaxDepth")]
	[Tooltip("Maximum damper amount to be applied at 100% compression.")]
	public float maxDamperRateOffset;

	[Range(0.001f, 0.999f)]
	[Tooltip("0.5 = lineal, >0.5 = fast increment first, <0.5 = slow increment first")]
	public float linearityFactor;

	private int[] m_wheels;

	private BiasedRatio m_springRateBias;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPProgressiveSuspension()
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
}
