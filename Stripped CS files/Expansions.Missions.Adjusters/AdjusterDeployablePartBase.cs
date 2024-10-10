using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDeployablePartBase : AdjusterPartModuleBase
{
	public AdjusterDeployablePartBase()
	{
	}

	public AdjusterDeployablePartBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleDeployablePart);
	}

	public virtual bool IsDeployablePartStuck()
	{
		return false;
	}
}
