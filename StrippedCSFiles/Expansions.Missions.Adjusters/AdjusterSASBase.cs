using System;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterSASBase : AdjusterPartModuleBase
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterSASBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterSASBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int ApplySASServiceLevelAdjustment(int sasServiceLevel)
	{
		throw null;
	}
}
