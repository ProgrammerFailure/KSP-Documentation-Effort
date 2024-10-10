using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Advanced Damper", 21)]
public class VPAdvancedDamper : VehicleBehaviour
{
	public int axle;

	public float slowBumpRate = 1500f;

	public float fastBumpRate = 1000f;

	[Range(0f, 1f)]
	public float bumpSpeedSplit = 0.127f;

	public float slowReboundRate = 2800f;

	public float fastReboundRate = 2000f;

	[Range(0f, 1f)]
	public float reboundSpeedSplit = 0.127f;

	public int[] m_wheels;

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

	public void OnValidate()
	{
		if (slowBumpRate < 0f)
		{
			slowBumpRate = 0f;
		}
		if (fastBumpRate < 0f)
		{
			fastBumpRate = 0f;
		}
		if (bumpSpeedSplit < 0f)
		{
			bumpSpeedSplit = 0f;
		}
		if (slowReboundRate < 0f)
		{
			slowReboundRate = 0f;
		}
		if (fastReboundRate < 0f)
		{
			fastReboundRate = 0f;
		}
		if (reboundSpeedSplit < 0f)
		{
			reboundSpeedSplit = 0f;
		}
	}

	public override int GetUpdateOrder()
	{
		return -100;
	}

	public override void UpdateVehicleSuspension()
	{
		for (int i = 0; i < m_wheels.Length; i++)
		{
			VehicleBase.WheelState wheelState = base.vehicle.wheelState[m_wheels[i]];
			wheelState.wheelCol.runtimeDamperRate = ComputeDamperRate(wheelState.contactSpeed);
		}
	}

	public float ComputeDamperRate(float speed)
	{
		if (speed >= 0f)
		{
			return ComputeDamperRate(speed, slowBumpRate, fastBumpRate, bumpSpeedSplit);
		}
		return ComputeDamperRate(0f - speed, slowReboundRate, fastReboundRate, reboundSpeedSplit);
	}

	public static float ComputeDamperRate(float speed, float slowRate, float fastRate, float speedSplit)
	{
		float num = 0f;
		if (speed < speedSplit)
		{
			return slowRate;
		}
		return fastRate + speedSplit / speed * (slowRate - fastRate);
	}

	public float ComputeDamperForce(float speed)
	{
		if (speed >= 0f)
		{
			return ComputeDamperForce(speed, slowBumpRate, fastBumpRate, bumpSpeedSplit);
		}
		return ComputeDamperForce(0f - speed, slowReboundRate, fastReboundRate, reboundSpeedSplit);
	}

	public static float ComputeDamperForce(float speed, float slowRate, float fastRate, float speedSplit)
	{
		float num = 0f;
		if (speed < speedSplit)
		{
			return slowRate * speed;
		}
		return speedSplit * (slowRate - fastRate) + fastRate * speed;
	}
}
