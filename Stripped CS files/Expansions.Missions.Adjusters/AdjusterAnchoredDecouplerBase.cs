using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterAnchoredDecouplerBase : AdjusterPartModuleBase
{
	public AdjusterAnchoredDecouplerBase()
	{
	}

	public AdjusterAnchoredDecouplerBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleAnchoredDecoupler);
	}

	public virtual bool IsBlockingDecouple()
	{
		return false;
	}
}
