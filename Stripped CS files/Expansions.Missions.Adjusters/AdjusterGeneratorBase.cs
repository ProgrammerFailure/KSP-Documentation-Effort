using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterGeneratorBase : AdjusterPartModuleBase
{
	public AdjusterGeneratorBase()
	{
	}

	public AdjusterGeneratorBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleGenerator);
	}

	public virtual float ApplyEfficiencyAdjustment(float efficiency)
	{
		return efficiency;
	}
}
