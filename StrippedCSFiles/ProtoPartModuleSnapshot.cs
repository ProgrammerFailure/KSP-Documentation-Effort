using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;

public class ProtoPartModuleSnapshot
{
	public ConfigNode moduleValues;

	public string moduleName;

	public PartModule moduleRef;

	private bool hasSaved;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartModuleSnapshot(PartModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartModuleSnapshot(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartModule Load(Part hostPart, ref int moduleIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnsureModuleValuesIsInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartModuleAdjuster(AdjusterPartModuleBase newAdjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartModuleAdjusterList(List<AdjusterPartModuleBase> moduleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartModuleAdjuster(AdjusterPartModuleBase removeAdjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartModuleAdjuster(Guid removeAdjusterID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<T> GetListOfActiveAdjusters<T>() where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ProtoPartModuleRepair()
	{
		throw null;
	}
}
