using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterAlternatorBase : AdjusterPartModuleBase
{
	public AdjusterAlternatorBase()
	{
	}

	public AdjusterAlternatorBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleAlternator);
	}

	public virtual float ApplyOutputAdjustment(float output)
	{
		return output;
	}
}
