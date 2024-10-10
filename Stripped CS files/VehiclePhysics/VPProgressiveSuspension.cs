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

	public Wheel wheel = Wheel.Both;

	[Range(0f, 0.999f)]
	[Tooltip("Minimum compression ratio for the suspension offsets to have effect. Maximum offsets are applied at 100% compression.")]
	public float minCompression;

	[Tooltip("Maximum spring amount to be applied at 100% compression.")]
	[FormerlySerializedAs("springRateOffsetAtMaxDepth")]
	public float maxSpringRateOffset = 5000f;

	[Tooltip("Also apply an offset to the suspension damper.")]
	public bool adjustDamper;

	[FormerlySerializedAs("damperRateOffsetAtMaxDepth")]
	[Tooltip("Maximum damper amount to be applied at 100% compression.")]
	public float maxDamperRateOffset = 500f;

	[Range(0.001f, 0.999f)]
	[Tooltip("0.5 = lineal, >0.5 = fast increment first, <0.5 = slow increment first")]
	public float linearityFactor = 0.5f;

	public int[] m_wheels;

	public BiasedRatio m_springRateBias = new BiasedRatio();

	public override void OnEnableVehicle()
	{
		int wheelsInAxle = base.vehicle.GetWheelsInAxle(axle);
		if (wheelsInAxle == 0)
		{
			DebugLogWarning("Invalid axle! Component disabled");
			base.enabled = false;
			return;
		}
		m_wheels = new int[wheelsInAxle];
		for (int i = 0; i < wheelsInAxle; i++)
		{
			m_wheels[i] = base.vehicle.GetWheelIndex(axle, (VehicleBase.WheelPos)i);
		}
	}

	public override void FixedUpdateVehicle()
	{
		for (int i = 0; i < m_wheels.Length; i++)
		{
			VehicleBase.WheelState wheelState = base.vehicle.wheelState[m_wheels[i]];
			float t = Mathf.InverseLerp(minCompression, 1f, Mathf.Clamp01(wheelState.suspensionCompression));
			wheelState.wheelCol.runtimeSpringRate += m_springRateBias.BiasedLerp(0f, maxSpringRateOffset, t, linearityFactor);
			if (adjustDamper)
			{
				wheelState.wheelCol.runtimeDamperRate += m_springRateBias.BiasedLerp(0f, maxDamperRateOffset, t, linearityFactor);
			}
		}
	}
}
