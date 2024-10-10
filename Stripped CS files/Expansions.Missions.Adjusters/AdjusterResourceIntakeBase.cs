using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterResourceIntakeBase : AdjusterPartModuleBase
{
	public AdjusterResourceIntakeBase()
	{
	}

	public AdjusterResourceIntakeBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleResourceIntake);
	}
}
