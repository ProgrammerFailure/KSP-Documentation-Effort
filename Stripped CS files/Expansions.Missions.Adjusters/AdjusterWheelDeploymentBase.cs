using System;
using ModuleWheels;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterWheelDeploymentBase : AdjusterPartModuleBase
{
	public AdjusterWheelDeploymentBase()
	{
	}

	public AdjusterWheelDeploymentBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleWheelDeployment);
	}
}
