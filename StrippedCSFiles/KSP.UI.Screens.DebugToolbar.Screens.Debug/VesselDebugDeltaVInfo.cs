using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class VesselDebugDeltaVInfo
{
	public DeltaVStageInfo stageInfo;

	private GameObject stageItem;

	private ScreenDeltaVPartsListInfo partsListInfo;

	private GameObject partsList;

	private DictionaryValueList<VesselDebugDeltaVEngineInfo, ScreenDeltaVEngineInfo> engineItems;

	private ScreenDeltaVStageInfo screenStageInfo;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselDebugDeltaVInfo(int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Create(GameObject stageItemPrefab, RectTransform contentParent, GameObject engineItemPrefab, GameObject propellantItemPrefab, GameObject partsListPrefab, DeltaVStageInfo stageInfo, CalcType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Update(DeltaVStageInfo stageInfo, CalcType type, string calctypeDesc, bool showEngines, bool showParts, bool showAllStages)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetActive(bool active, bool partsActive, bool enginesActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int SetSiblingsUIIndex(int index)
	{
		throw null;
	}
}
