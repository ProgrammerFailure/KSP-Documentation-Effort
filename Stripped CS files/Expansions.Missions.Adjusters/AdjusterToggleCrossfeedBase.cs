using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterToggleCrossfeedBase : AdjusterPartModuleBase
{
	public AdjusterToggleCrossfeedBase()
	{
	}

	public AdjusterToggleCrossfeedBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleToggleCrossfeed);
	}
}
