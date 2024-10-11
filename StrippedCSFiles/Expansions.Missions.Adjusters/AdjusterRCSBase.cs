using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterRCSBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterRCSBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterRCSBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Vector3 ApplyInputRotationAdjustment(Vector3 inputRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Vector3 ApplyInputLinearAdjustment(Vector3 inputLinear)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsRCSBroken()
	{
		throw null;
	}
}
