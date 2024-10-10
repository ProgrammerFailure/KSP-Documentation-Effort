using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterSASBase : AdjusterPartModuleBase
{
	public AdjusterSASBase()
	{
	}

	public AdjusterSASBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleSAS);
	}

	public virtual int ApplySASServiceLevelAdjustment(int sasServiceLevel)
	{
		return sasServiceLevel;
	}
}
