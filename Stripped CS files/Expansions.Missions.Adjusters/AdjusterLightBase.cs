using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterLightBase : AdjusterPartModuleBase
{
	public AdjusterLightBase()
	{
	}

	public AdjusterLightBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleLight);
	}

	public virtual float ApplyIntensityAdjustment(float intensity)
	{
		return intensity;
	}
}
