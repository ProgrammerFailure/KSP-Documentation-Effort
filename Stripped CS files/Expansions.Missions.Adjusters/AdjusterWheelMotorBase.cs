using System;
using ModuleWheels;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterWheelMotorBase : AdjusterPartModuleBase
{
	public AdjusterWheelMotorBase()
	{
	}

	public AdjusterWheelMotorBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleWheelMotor);
	}

	public virtual float ApplyTorqueAdjustment(float torque)
	{
		return torque;
	}
}
