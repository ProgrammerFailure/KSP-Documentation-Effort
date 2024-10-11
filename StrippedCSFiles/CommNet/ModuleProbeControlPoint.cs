using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;

namespace CommNet;

public class ModuleProbeControlPoint : PartModule
{
	[KSPField]
	public int minimumCrew;

	[KSPField]
	public bool multiHop;

	private List<AdjusterProbeControlPointBase> adjusterCache;

	private static string cacheAutoLOC_7000026;

	private static string cacheAutoLOC_118899;

	private static string cacheAutoLOC_6004043;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleProbeControlPoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanControlUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool IsProbeControlPointBroken(List<AdjusterProbeControlPointBase> adjusterList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
