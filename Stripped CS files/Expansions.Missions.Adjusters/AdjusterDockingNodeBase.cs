using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDockingNodeBase : AdjusterPartModuleBase
{
	public AdjusterDockingNodeBase()
	{
	}

	public AdjusterDockingNodeBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleDockingNode);
	}

	public virtual bool IsBlockingUndock()
	{
		return false;
	}
}
