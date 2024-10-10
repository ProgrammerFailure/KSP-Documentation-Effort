using System;
using ModuleWheels;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterWheelBrakesBase : AdjusterPartModuleBase
{
	public AdjusterWheelBrakesBase()
	{
	}

	public AdjusterWheelBrakesBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleWheelBrakes);
	}

	public virtual float ApplyTorqueAdjustment(float torque)
	{
		return torque;
	}
}
