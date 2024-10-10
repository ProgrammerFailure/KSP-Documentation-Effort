using System;
using CompoundParts;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterFuelLineBase : AdjusterPartModuleBase
{
	public AdjusterFuelLineBase()
	{
	}

	public AdjusterFuelLineBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(CModuleFuelLine);
	}
}
