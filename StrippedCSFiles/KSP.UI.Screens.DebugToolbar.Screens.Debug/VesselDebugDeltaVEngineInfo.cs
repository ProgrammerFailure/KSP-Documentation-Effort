using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class VesselDebugDeltaVEngineInfo
{
	public DeltaVEngineInfo engineInfo;

	private GameObject engineItem;

	private DictionaryValueList<DeltaVPropellantInfo, ScreenDeltaVPropellantInfo> propellantItems;

	private ScreenDeltaVEngineInfo screenEngineInfo;

	private GameObject propellantItemPrefab;

	private RectTransform contentParent;

	private bool setUpComplete;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselDebugDeltaVEngineInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenDeltaVEngineInfo Create(GameObject engineItemPrefab, RectTransform contentParent, GameObject propellantItemPrefab, DeltaVEngineInfo engineInfo, List<DeltaVPropellantInfo> stagePropInfo, CalcType type, int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Update(DeltaVEngineInfo engineInfo, List<DeltaVPropellantInfo> stagePropInfo, CalcType type, string calctypeDesc, int stage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetActive(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal int SetPropellantUISiblings(int index)
	{
		throw null;
	}
}
