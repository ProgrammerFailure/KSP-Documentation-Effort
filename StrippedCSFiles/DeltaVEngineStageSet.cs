using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class DeltaVEngineStageSet
{
	[Serializable]
	public class DeltaVStageInfoEntry
	{
		public DeltaVStageInfo stageInfo_InstanceOne;

		public DeltaVStageInfo stageInfo_InstanceTwo;

		public VesselDeltaV vesselDeltaV;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public DeltaVStageInfoEntry(VesselDeltaV vesselDeltaV)
		{
			throw null;
		}
	}

	protected class StageSortingInstanceOne : IComparer<DeltaVStageInfoEntry>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StageSortingInstanceOne()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int Compare(DeltaVStageInfoEntry x, DeltaVStageInfoEntry y)
		{
			throw null;
		}
	}

	[SerializeField]
	private List<DeltaVStageInfoEntry> stageInfoEntries;

	[SerializeField]
	internal List<DeltaVEngineInfo> engineInfo_InstanceOne;

	[SerializeField]
	internal List<DeltaVEngineInfo> engineInfo_InstanceTwo;

	public int operatingIndex;

	public int workingIndex;

	internal int flightSceneOperatingIndex;

	public VesselDeltaV vesselDeltaV;

	public List<int> payloadStages;

	protected IComparer<DeltaVStageInfo> stageSorting;

	public virtual List<DeltaVStageInfo> WorkingStageInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual List<DeltaVStageInfo> OperatingStageInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual List<DeltaVEngineInfo> WorkingEngineInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual List<DeltaVEngineInfo> OperatingEngineInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVEngineStageSet(VesselDeltaV vesselDeltaV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AddStageInfo(DeltaVStageInfo stageInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateStageInfo(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateStageInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RemoveStaleStages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetPayload(DeltaVStageInfo stageInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RemoveStagedStages(bool sortStages = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SortStages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void AddEngineWorkingSet(DeltaVEngineInfo engineInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RemoveInvalidEnginesWorkingSet()
	{
		throw null;
	}
}
