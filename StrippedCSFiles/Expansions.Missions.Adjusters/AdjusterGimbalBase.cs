using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterGimbalBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterGimbalBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterGimbalBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Vector3 ApplyControlAdjustment(Vector3 control)
	{
		throw null;
	}
}
