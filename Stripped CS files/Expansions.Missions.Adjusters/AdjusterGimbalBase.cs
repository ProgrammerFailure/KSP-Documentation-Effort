using System;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterGimbalBase : AdjusterPartModuleBase
{
	public AdjusterGimbalBase()
	{
	}

	public AdjusterGimbalBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleGimbal);
	}

	public virtual Vector3 ApplyControlAdjustment(Vector3 control)
	{
		return control;
	}
}
