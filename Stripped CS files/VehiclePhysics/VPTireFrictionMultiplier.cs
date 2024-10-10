using UnityEngine;

namespace VehiclePhysics;

public class VPTireFrictionMultiplier : VehicleBehaviour
{
	[Range(0f, 2f)]
	public float frictionMultiplier = 1f;

	public float m_lastValue;

	public override void OnEnableVehicle()
	{
		m_lastValue = 1f;
	}

	public override void FixedUpdateVehicle()
	{
		if (frictionMultiplier != m_lastValue)
		{
			SetFrictionMultiplierInAllWheels(base.vehicle, frictionMultiplier);
			m_lastValue = frictionMultiplier;
		}
	}

	public override void OnDisableVehicle()
	{
		SetFrictionMultiplierInAllWheels(base.vehicle, 1f);
	}

	public static void SetFrictionMultiplierInAllWheels(VehicleBase vehicle, float multiplier)
	{
		int axleCount = vehicle.GetAxleCount();
		for (int i = 0; i < axleCount; i++)
		{
			int wheelsInAxle = vehicle.GetWheelsInAxle(i);
			for (int j = 0; j < wheelsInAxle; j++)
			{
				int wheelIndex = vehicle.GetWheelIndex(i, (VehicleBase.WheelPos)j);
				vehicle.SetWheelTireFrictionMultiplier(wheelIndex, multiplier);
			}
		}
	}
}
