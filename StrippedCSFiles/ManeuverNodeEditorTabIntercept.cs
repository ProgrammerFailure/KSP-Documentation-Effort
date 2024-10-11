using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;

public class ManeuverNodeEditorTabIntercept : ManeuverNodeEditorTab
{
	[SerializeField]
	private TextMeshProUGUI intercept1Label;

	[SerializeField]
	private TextMeshProUGUI intercept2Label;

	[SerializeField]
	private TooltipController_Text intercept1DistanceTooltip;

	[SerializeField]
	private TooltipController_Text intercept2DistanceTooltip;

	[SerializeField]
	private TooltipController_Text intercept1TimeTooltip;

	[SerializeField]
	private TooltipController_Text intercept2TimeTooltip;

	[SerializeField]
	private TooltipController_Text phaseAngleTooltip;

	private IntersectInformation intersectInfo;

	public TextMeshProUGUI intercept1DistanceLabel;

	public TextMeshProUGUI intercept1TimeLabel;

	public TextMeshProUGUI intercept2DistanceLabel;

	public TextMeshProUGUI intercept2TimeLabel;

	public TextMeshProUGUI phaseAngleLabel;

	private ManeuverNodeEditorTabButton editorTab;

	private TooltipController_Text buttonTooltip;

	private bool patchesAheadLimitOK;

	private bool approachMode;

	private bool approachStrings;

	private Orbit orbitToDisplay;

	private Orbit orbitContainingUT;

	private Vector3d clApprPos;

	private Vector3d clApprTgtPos;

	private double clApprSeparation;

	private static string cacheAutoLOC_7003285;

	private static string cacheAutoLOC_6002332;

	private static string cacheAutoLOC_7001411;

	private static string cacheAutoLOC_6005034_1;

	private static string cacheAutoLOC_6005034_2;

	private static string cacheAutoLOC_6005035_1;

	private static string cacheAutoLOC_6005035_2;

	private static string cacheAutoLOC_6005036_1;

	private static string cacheAutoLOC_6005036_2;

	private static string cacheAutoLOC_6005021_1;

	private static string cacheAutoLOC_6005021_2;

	private static string cacheAutoLOC_6005022_1;

	private static string cacheAutoLOC_6005022_2;

	private static string cacheAutoLOC_6005023_1;

	private static string cacheAutoLOC_6005023_2;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorTabIntercept()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetEditorTab()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetIntersectOrApproach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetInitialValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateUIElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsTabInteractable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IntersectInformation GetInterceptValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}
