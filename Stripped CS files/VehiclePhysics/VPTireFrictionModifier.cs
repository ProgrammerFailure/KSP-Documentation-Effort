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

	public Wheel wheel = Wheel.Both;

	public TireFriction tireFriction = new TireFriction();

	public TireFriction m_originalLeftFriction;

	public TireFriction m_originalRightFriction;

	public override void OnEnableVehicle()
	{
		if (wheel == Wheel.Both || wheel == Wheel.Left)
		{
			int wheelIndex = base.vehicle.GetWheelIndex(axle);
			m_originalLeftFriction = base.vehicle.GetWheelTireFriction(wheelIndex);
			base.vehicle.SetWheelTireFriction(wheelIndex, tireFriction);
		}
		if (wheel == Wheel.Both || wheel == Wheel.Right)
		{
			int wheelIndex2 = base.vehicle.GetWheelIndex(axle, VehicleBase.WheelPos.Right);
			m_originalRightFriction = base.vehicle.GetWheelTireFriction(wheelIndex2);
			base.vehicle.SetWheelTireFriction(wheelIndex2, tireFriction);
		}
	}

	public override void OnDisableVehicle()
	{
		if (wheel == Wheel.Both || wheel == Wheel.Left)
		{
			int wheelIndex = base.vehicle.GetWheelIndex(axle);
			base.vehicle.SetWheelTireFriction(wheelIndex, m_originalLeftFriction);
		}
		if (wheel == Wheel.Both || wheel == Wheel.Right)
		{
			int wheelIndex2 = base.vehicle.GetWheelIndex(axle, VehicleBase.WheelPos.Right);
			base.vehicle.SetWheelTireFriction(wheelIndex2, m_originalRightFriction);
		}
	}
}
