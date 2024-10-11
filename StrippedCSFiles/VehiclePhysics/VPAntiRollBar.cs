using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Anti-roll Bar", 20)]
public class VPAntiRollBar : VehicleBehaviour
{
	public enum Mode
	{
		Stiffness,
		SpringRate,
		Legacy
	}

	public int axle;

	public Mode mode;

	[Range(0f, 1f)]
	[Tooltip("Ratio of spring rate than may be transferred among wheels")]
	public float stiffness;

	[Tooltip("Proportion of the stiffness that forces the wheels to move together.\nWARNING: Setting it too high may cause noticeable shakiness!")]
	[Range(0f, 1f)]
	public float rigidity;

	[Tooltip("Spring rate per compression distance that gets transferred among wheels")]
	public float springRate;

	[Tooltip("Spring rate per compression ratio that gets transferred among wheels. This is the previously used mode, now discouraged.")]
	public float antiRollRate;

	private VehicleBase.WheelState m_leftState;

	private VPWheelCollider m_leftCollider;

	private VehicleBase.WheelState m_rightState;

	private VPWheelCollider m_rightCollider;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPAntiRollBar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicleSuspension()
	{
		throw null;
	}
}
