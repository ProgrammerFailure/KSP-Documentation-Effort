using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterLiftingSurfaceBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterLiftingSurfaceBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterLiftingSurfaceBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Vector3 ApplyLiftForceAdjustment(Vector3 liftForce)
	{
		throw null;
	}
}
