using System;
using CommNet;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterProbeControlPointBase : AdjusterPartModuleBase
{
	public AdjusterProbeControlPointBase()
	{
	}

	public AdjusterProbeControlPointBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleProbeControlPoint);
	}

	public virtual bool IsProbeControlPointBroken()
	{
		return false;
	}
}
