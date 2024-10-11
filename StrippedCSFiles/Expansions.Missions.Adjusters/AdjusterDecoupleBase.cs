using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterDecoupleBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDecoupleBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterDecoupleBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsBlockingDecouple()
	{
		throw null;
	}
}
