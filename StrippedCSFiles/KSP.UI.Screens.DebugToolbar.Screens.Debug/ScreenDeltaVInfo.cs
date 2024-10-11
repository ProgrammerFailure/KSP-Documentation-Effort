using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class ScreenDeltaVInfo : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI activeVesselName_text;

	[SerializeField]
	private TextMeshProUGUI vesselStageGroups_text;

	[SerializeField]
	private TextMeshProUGUI vesselTotalDeltaV_text;

	[SerializeField]
	private TextMeshProUGUI vesselCalcType_text;

	[SerializeField]
	private TextMeshProUGUI vesselTotalBurnTime_text;

	[SerializeField]
	private Toggle showVacuum;

	[SerializeField]
	private Toggle showShowAllStages;

	[SerializeField]
	private Toggle showEngines;

	[SerializeField]
	private Toggle showParts;

	[SerializeField]
	private Toggle logVerbose;

	[SerializeField]
	private RectTransform contentParent;

	[SerializeField]
	private GameObject stageItemPrefab;

	[SerializeField]
	private GameObject engineItemPrefab;

	[SerializeField]
	private GameObject propellantItemPrefab;

	[SerializeField]
	private GameObject partsListPrefab;

	[SerializeField]
	private List<VesselDebugDeltaVInfo> screenObjects;

	public bool resetScreenObjects;

	[SerializeField]
	private VesselDeltaV vesselDeltaV;

	private string cached_autoLOC_901095;

	private string cache_autoLOC_8002204;

	private string cache_autoLOC_8002205;

	private string cache_autoLOC_8002206;

	private IComparer<VesselDebugDeltaVInfo> stageSorting;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenDeltaVInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSiblingsUIIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearScreenObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorRestoreState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorLoad(ShipConstruct ship, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorPodPicked(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeVessel(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLogVerboseToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CacheTypeStrings()
	{
		throw null;
	}
}
