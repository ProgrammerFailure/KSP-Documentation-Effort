using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight;

public class NavBallBurnVector : MonoBehaviour
{
	public Transform vectorProgr;

	public Transform indicationArrow;

	public NavBall navBall;

	public HelixGauge deltaVGauge;

	public HelixGauge deltaVGaugeRed;

	public Transform StageMarkersParent;

	public TextMeshProUGUI ebtText;

	public TextMeshProUGUI TdnText;

	public TextMeshProUGUI readoutText;

	public TextMeshProUGUI sbtText;

	public TextMeshProUGUI bbPercentText;

	public Button btnWarpTo;

	public Button btnAccept;

	public Button btnDismiss;

	public Button btnPercentMinus;

	public Button btnPercentPlus;

	public UIPanelTransitionToggle[] navBallCollapseGroups;

	public double dVremaining;

	public double estimatedBurnTime;

	public double startBurnTime;

	public float accuracy;

	private Vector3 direction;

	private PatchedConicSolver solver;

	[SerializeField]
	private GameObject stageMarkerPrefab;

	[SerializeField]
	private RectTransform stageMarkerMaskRect;

	private DictionaryValueList<int, RotationalGaugeOffsetMarker> stageMarkers;

	[SerializeField]
	private bool enoughDeltaV;

	[SerializeField]
	private int burnBeforePercent;

	[SerializeField]
	private GameObject burnBeforeObject;

	private static double epsilon;

	private bool nodeDeltaVChanged;

	private double nodeDeltaV;

	private bool vesselDeltaVChanged;

	private double vesselTotalDeltaV;

	[SerializeField]
	private List<int> stagesProcessed;

	[SerializeField]
	private Transform EBTextPosBasicMode;

	[SerializeField]
	private Transform EBTextPosExtMode;

	[SerializeField]
	private Transform TdnTextPosBasicMode;

	[SerializeField]
	private Transform TdnTextPosExtMode;

	[SerializeField]
	private double startBurn;

	private static string cacheAutoLOC_258912;

	private static string cacheAutoLOC_460852;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NavBallBurnVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static NavBallBurnVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSwitch(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAdvancedMode(bool mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateNavballBurnVectors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateBurnUIText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDVStageMarkers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double CalculateBurnTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AutoStepNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearStageMarkers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveRemainingStageMarkers(List<int> stagesProcessed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void btnPercentMinusClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void btnPercentPlusClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDismissButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnWarpToButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool navBallIsExpanded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
