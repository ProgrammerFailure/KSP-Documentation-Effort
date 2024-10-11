using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KSCFacilityContextMenu : AnchoredDialog
{
	public enum DismissAction
	{
		None,
		Enter,
		Repair,
		Upgrade,
		Demolish,
		Downgrade
	}

	protected string facilityName;

	protected string description;

	protected float facilityDamage;

	protected string facilityDamageLevel;

	protected string damageColor;

	protected int level;

	protected float maxLevel;

	protected string levelText;

	protected float upgradeCost;

	protected float downgradeCost;

	protected float repairCost;

	protected string repairCostColor;

	private string upgradeString;

	private string downgradeString;

	private Color btnTextBaseColor;

	protected SpaceCenterBuilding host;

	protected Callback<DismissAction> OnMenuDismiss;

	[SerializeField]
	protected Button RepairButton;

	[SerializeField]
	protected TextMeshProUGUI RepairButtonText;

	[SerializeField]
	protected Button EnterButton;

	[SerializeField]
	protected TextMeshProUGUI EnterButtonText;

	[SerializeField]
	public TextMeshProUGUI descriptionText;

	[SerializeField]
	public TextMeshProUGUI statusText;

	[SerializeField]
	private Button UpgradeButton;

	[SerializeField]
	private TextMeshProUGUI UpgradeButtonText;

	[SerializeField]
	private Button DemolishButton;

	[SerializeField]
	private Button DowngradeButton;

	[SerializeField]
	private TextMeshProUGUI DowngradeButtonText;

	[SerializeField]
	public TextMeshProUGUI levelFieldText;

	[SerializeField]
	public TextMeshProUGUI levelStatsText;

	[SerializeField]
	private Color canAffordColor;

	[SerializeField]
	private Color cannotAffordColor;

	private bool showDowngradeControls;

	private bool hasFacility;

	private bool isUpgradeable;

	private bool willRefresh;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSCFacilityContextMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KSCFacilityContextMenu Create(SpaceCenterBuilding host, Callback<DismissAction> onMenuDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnKSCStructureEvent(DestructibleBuilding dB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnFacilityValuesModified()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string getFundsTextColorTag(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetWindowTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CreateWindowContent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRepairButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterBtnInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpgradeButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpgradeButtonHoverIn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpgradeButtonHoverOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDemolishButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDowngradeButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnClickOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss(DismissAction dma)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PreviewNextLevel(bool previewState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void StartThis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroyThis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}
}
