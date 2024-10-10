using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterGPSBase : AdjusterPartModuleBase
{
	public AdjusterGPSBase()
	{
	}

	public AdjusterGPSBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleGPS);
	}
}
