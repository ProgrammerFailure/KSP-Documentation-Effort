using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterCargoBayBase : AdjusterPartModuleBase
{
	public AdjusterCargoBayBase()
	{
	}

	public AdjusterCargoBayBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleCargoBay);
	}

	public virtual bool IsCargoBayStuck()
	{
		return false;
	}
}
