using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterScienceExperimentBase : AdjusterPartModuleBase
{
	public AdjusterScienceExperimentBase()
	{
	}

	public AdjusterScienceExperimentBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleScienceExperiment);
	}
}
