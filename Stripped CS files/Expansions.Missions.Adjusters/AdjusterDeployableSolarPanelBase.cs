using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDeployableSolarPanelBase : AdjusterPartModuleBase
{
	public AdjusterDeployableSolarPanelBase()
	{
	}

	public AdjusterDeployableSolarPanelBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleDeployableSolarPanel);
	}

	public virtual float ApplyEfficiencyAdjustment(float efficiency)
	{
		return efficiency;
	}
}
