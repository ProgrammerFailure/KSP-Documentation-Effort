using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterEnginesBase : AdjusterPartModuleBase
{
	public AdjusterEnginesBase()
	{
	}

	public AdjusterEnginesBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleEngines);
	}

	public virtual float ApplyThrottleAdjustment(float throttle)
	{
		return throttle;
	}

	public virtual bool IsEngineDead()
	{
		return false;
	}
}
