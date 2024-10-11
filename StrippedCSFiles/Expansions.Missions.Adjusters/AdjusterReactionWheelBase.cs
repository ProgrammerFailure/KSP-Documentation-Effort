using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterReactionWheelBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterReactionWheelBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterReactionWheelBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Vector3 ApplyTorqueAdjustment(Vector3 torque)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsReactionWheelBroken()
	{
		throw null;
	}
}
