using System;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterKerbNetAccessBase : AdjusterPartModuleBase
{
	public AdjusterKerbNetAccessBase()
	{
	}

	public AdjusterKerbNetAccessBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleKerbNetAccess);
	}
}
