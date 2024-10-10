using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterControlSurfaceBase : AdjusterPartModuleBase
{
	public AdjusterControlSurfaceBase()
	{
	}

	public AdjusterControlSurfaceBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleControlSurface);
	}

	public virtual float ApplyActuatorSpeedAdjustment(float actuatorSpeed)
	{
		return actuatorSpeed;
	}
}
