using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Editor;

public class PartListTooltip : Tooltip
{
	public delegate string PartStatsDelegate(Part p, bool showUpgradesAvail);

	public GameObject costPanel;

	private Callback<PartListTooltip> onPurchase;

	public TextMeshProUGUI textName;

	public RawImage imagePart;

	public Slider sliderScaleTape;

	public TextMeshProUGUI textScale;

	public TextMeshProUGUI textInfoBasic;

	public TextMeshProUGUI textGreyoutMessage;

	public TextMeshProUGUI textManufacturer;

	public TextMeshProUGUI textDescription;

	public TextMeshProUGUI textCost;

	public TextMeshProUGUI textRMBHint;

	public GameObject buttonPurchaseContainer;

	public Button buttonPurchase;

	public Button buttonPurchaseRed;

	public Button buttonToggleVariant;

	public TextMeshProUGUI buttonPurchaseCaption;

	public TextMeshProUGUI buttonPurchaseCaptionRed;

	public PartListTooltipWidget extInfoModuleWidgetPrefab;

	public PartListTooltipWidget extInfoRscWidgePrefab;

	public PartListTooltipWidget extInfoVariantsWidgePrefab;

	private List<PartListTooltipWidget> extInfoModules;

	private List<PartListTooltipWidget> extInfoRscs;

	private List<PartListTooltipWidget> extInfoVariants;

	public GameObject panelExtended;

	public RectTransform extInfoListContainer;

	public RectTransform extInfoListSpacer;

	public RectTransform extInfoListSpacerVariants;

	private AvailablePart partInfo;

	private PartUpgradeHandler.Upgrade upgrade;

	private Part partRef;

	private PartModule.PartUpgradeState upgradeState;

	private ConfigNode upgradeNode;

	private bool showCostInExtendedInfo;

	private bool hasCreatedExtendedInfo;

	private bool hasExtendedInfo;

	private CurrencyModifierQuery currencyQuery;

	private float entryCost;

	private float partCost;

	private float upgradeCost;

	private string entryCostText;

	private bool requiresEntryPurchase;

	private PartIcon _partIcon;

	public float iconSpin;

	public Material[] materials;

	public static PartStatsDelegate GetPartStats;

	private static string cacheAutoLOC_456241;

	private static string cacheAutoLOC_7003267;

	private static string cacheAutoLOC_7003268;

	private static string cacheAutoLOC_456346;

	private static string cacheAutoLOC_456368;

	private static string cacheAutoLOC_456391;

	private static string cacheAutoLOC_7003246;

	private static string cacheAutoLOC_6005097;

	public bool HasExtendedInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isGrey
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public PartIcon partIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public GameObject icon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Transform iconTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartListTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartListTooltip()
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
	public void SetupGrayout(string grayoutMessage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(AvailablePart availablePart, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateVariantText(string varName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCargoPartModuleInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupUpgradeInfo(AvailablePart availablePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(AvailablePart availablePart, PartUpgradeHandler.Upgrade up, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetupScaleGauge(float iconScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateExtendedInfo(bool showPartCost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HidePreviousTooltipWidgets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartListTooltipWidget GetNewTooltipWidget(PartListTooltipWidget prefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateExtendedUpgradeInfo(bool showPartCost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyExtendedInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisplayExtendedInfo(bool display, string rmbHintText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string _GenerateStatsText(Part p, bool showUpgradesAvail, float mass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string _GetPartStats(Part p, bool showUpgradesAvail)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetUpgradedPrimaryInfo(AvailablePart aP, int maxLines)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPrimaryInfo(AvailablePart aP, int maxLines)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPurchaseButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnPurchaseRed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
