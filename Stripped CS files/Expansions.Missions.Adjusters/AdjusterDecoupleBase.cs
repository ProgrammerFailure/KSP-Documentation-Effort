using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDecoupleBase : AdjusterPartModuleBase
{
	public AdjusterDecoupleBase()
	{
	}

	public AdjusterDecoupleBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleDecouple);
	}

	public virtual bool IsBlockingDecouple()
	{
		return false;
	}
}
