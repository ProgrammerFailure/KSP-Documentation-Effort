using System;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterLiftingSurfaceBase : AdjusterPartModuleBase
{
	public AdjusterLiftingSurfaceBase()
	{
	}

	public AdjusterLiftingSurfaceBase(MENode node)
		: base(node)
	{
	}

	public override Type GetTargetPartModule()
	{
		return typeof(ModuleLiftingSurface);
	}

	public virtual Vector3 ApplyLiftForceAdjustment(Vector3 liftForce)
	{
		return liftForce;
	}
}
