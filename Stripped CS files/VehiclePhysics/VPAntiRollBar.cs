using EdyCommonTools;
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
	public float stiffness = 0.5f;

	[Tooltip("Proportion of the stiffness that forces the wheels to move together.\nWARNING: Setting it too high may cause noticeable shakiness!")]
	[Range(0f, 1f)]
	public float rigidity = 0.5f;

	[Tooltip("Spring rate per compression distance that gets transferred among wheels")]
	public float springRate = 20000f;

	[Tooltip("Spring rate per compression ratio that gets transferred among wheels. This is the previously used mode, now discouraged.")]
	public float antiRollRate = 20000f;

	public VehicleBase.WheelState m_leftState;

	public VPWheelCollider m_leftCollider;

	public VehicleBase.WheelState m_rightState;

	public VPWheelCollider m_rightCollider;

	public override void OnEnableVehicle()
	{
		int wheelIndex = base.vehicle.GetWheelIndex(axle);
		int wheelIndex2 = base.vehicle.GetWheelIndex(axle, VehicleBase.WheelPos.Right);
		if (wheelIndex >= 0 && wheelIndex2 >= 0)
		{
			m_leftState = base.vehicle.wheelState[wheelIndex];
			m_rightState = base.vehicle.wheelState[wheelIndex2];
			m_leftCollider = m_leftState.wheelCol;
			m_rightCollider = m_rightState.wheelCol;
		}
		else
		{
			DebugLogWarning("Invalid axle! Component disabled");
			base.enabled = false;
		}
	}

	public override void UpdateVehicleSuspension()
	{
		switch (mode)
		{
		case Mode.Stiffness:
		{
			float num7 = Mathf.Clamp(m_leftState.contactDepth, 0f, m_leftCollider.runtimeSuspensionTravel);
			float num8 = Mathf.Clamp(m_rightState.contactDepth, 0f, m_rightCollider.runtimeSuspensionTravel);
			float num9 = m_leftCollider.runtimeSpringRate + m_rightCollider.runtimeSpringRate;
			float num10 = m_leftCollider.runtimeDamperRate + m_rightCollider.runtimeDamperRate;
			float num11 = num7 + num8;
			if (num11 > 0f)
			{
				float num12 = Mathf.Lerp(0.5f, num7 / num11, stiffness);
				float num13 = 1f - num12;
				m_leftCollider.runtimeSpringRate = num9 * num12;
				m_leftCollider.runtimeDamperRate = num10 * num12;
				m_rightCollider.runtimeSpringRate = num9 * num13;
				m_rightCollider.runtimeDamperRate = num10 * num13;
				float num14 = MathUtility.FastAbs(num7 - num8) * stiffness * rigidity;
				if (num7 > num8)
				{
					m_rightCollider.runtimeSuspensionTravel -= num14;
				}
				else
				{
					m_leftCollider.runtimeSuspensionTravel -= num14;
				}
			}
			else
			{
				m_leftCollider.runtimeSpringRate = Mathf.Lerp(m_leftCollider.runtimeSpringRate, num9, stiffness);
				m_rightCollider.runtimeSpringRate = Mathf.Lerp(m_rightCollider.runtimeSpringRate, num9, stiffness);
			}
			break;
		}
		case Mode.SpringRate:
		{
			float num3 = Mathf.Clamp(m_leftState.contactDepth, 0f, m_leftCollider.runtimeSuspensionTravel);
			float num4 = Mathf.Clamp(m_rightState.contactDepth, 0f, m_rightCollider.runtimeSuspensionTravel);
			float num5 = num4 - num3;
			if (num3 > num4)
			{
				m_rightCollider.runtimeSuspensionTravel -= Mathf.Abs(num5) * rigidity;
			}
			else
			{
				m_leftCollider.runtimeSuspensionTravel -= Mathf.Abs(num5) * rigidity;
			}
			float num6 = springRate * num5;
			m_leftCollider.runtimeSpringRate -= num6;
			m_rightCollider.runtimeSpringRate += num6;
			break;
		}
		case Mode.Legacy:
		{
			float num = Mathf.Clamp01(m_rightState.suspensionCompression) - Mathf.Clamp01(m_leftState.suspensionCompression);
			float num2 = antiRollRate * num;
			m_leftCollider.runtimeSpringRate -= num2;
			m_rightCollider.runtimeSpringRate += num2;
			break;
		}
		}
	}
}
