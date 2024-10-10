using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

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

	public string facilityName = "KSC Facility";

	public string description = "This is a facility at the Kerbal Space Center. If you hit it hard enough, it might collapse in a nice, satisfying explosion.";

	public float facilityDamage;

	public string facilityDamageLevel = "None";

	public string damageColor;

	public int level;

	public float maxLevel;

	public string levelText = "";

	public float upgradeCost;

	public float downgradeCost;

	public float repairCost;

	public string repairCostColor;

	public string upgradeString;

	public string downgradeString;

	public Color btnTextBaseColor = new Color(0.8352942f, 0.9607844f, 0.9960785f, 1f);

	public SpaceCenterBuilding host;

	public Callback<DismissAction> OnMenuDismiss = delegate
	{
	};

	[SerializeField]
	public Button RepairButton;

	[SerializeField]
	public TextMeshProUGUI RepairButtonText;

	[SerializeField]
	public Button EnterButton;

	[SerializeField]
	public TextMeshProUGUI EnterButtonText;

	[SerializeField]
	public TextMeshProUGUI descriptionText;

	[SerializeField]
	public TextMeshProUGUI statusText;

	[SerializeField]
	public Button UpgradeButton;

	[SerializeField]
	public TextMeshProUGUI UpgradeButtonText;

	[SerializeField]
	public Button DemolishButton;

	[SerializeField]
	public Button DowngradeButton;

	[SerializeField]
	public TextMeshProUGUI DowngradeButtonText;

	[SerializeField]
	public TextMeshProUGUI levelFieldText;

	[SerializeField]
	public TextMeshProUGUI levelStatsText;

	[SerializeField]
	public Color canAffordColor = Color.white;

	[SerializeField]
	public Color cannotAffordColor = Color.white;

	public bool showDowngradeControls;

	public bool hasFacility;

	public bool isUpgradeable;

	public bool willRefresh;

	public static KSCFacilityContextMenu Create(SpaceCenterBuilding host, Callback<DismissAction> onMenuDismiss)
	{
		KSCFacilityContextMenu component = Object.Instantiate(AssetBase.GetPrefab("FacilityContextMenu")).GetComponent<KSCFacilityContextMenu>();
		component.hasFacility = host != null && host.Facility != null;
		component.isUpgradeable = ScenarioUpgradeableFacilities.Instance != null && component.hasFacility && (object)host.Facility != null;
		component.showDowngradeControls = Input.GetKey(KeyCode.LeftControl);
		component.name = host.facilityName + " Context Menu";
		component.anchor = host.transform;
		component.host = host;
		component.facilityName = host.buildingInfoName;
		component.description = host.buildingDescription;
		component.OnMenuDismiss = onMenuDismiss;
		component.clampedToScreen = true;
		component.transform.SetParent(UIMasterController.Instance.mainCanvas.transform, worldPositionStays: false);
		UIMasterController.ClampToScreen((RectTransform)component.transform, Vector2.one * 50f);
		GameEvents.OnKSCStructureCollapsing.Add(component.OnKSCStructureEvent);
		GameEvents.OnKSCStructureCollapsed.Add(component.OnKSCStructureEvent);
		GameEvents.OnKSCStructureRepairing.Add(component.OnKSCStructureEvent);
		GameEvents.OnKSCStructureRepaired.Add(component.OnKSCStructureEvent);
		GameEvents.onFacilityContextMenuSpawn.Fire(component);
		return component;
	}

	public void OnKSCStructureEvent(DestructibleBuilding dB)
	{
		if (willRefresh)
		{
			return;
		}
		int num = host.destructibles.Length;
		do
		{
			if (num-- <= 0)
			{
				return;
			}
		}
		while (!(host.destructibles[num] == dB));
		willRefresh = true;
		OnFacilityValuesModified();
	}

	public void OnFacilityValuesModified()
	{
		willRefresh = false;
		facilityDamage = host.GetStructureDamage();
		facilityDamageLevel = SpaceCenterBuilding.GetStructureDamageLevel(facilityDamage);
		damageColor = XKCDColors.ColorTranslator.ToHex(Color.Lerp(XKCDColors.ColorTranslator.FromHtml("#B4D455"), XKCDColors.Orange, Mathf.Clamp(facilityDamage, 0f, 100f) / 100f));
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.StructureRepair, 0f - host.GetRepairsCost(), 0f, 0f);
		repairCost = 0f - currencyModifierQuery.GetTotal(Currency.Funds);
		repairCostColor = ((!(Funding.Instance != null)) ? XKCDColors.HexFormat.LightGrey : (((double)repairCost < Funding.Instance.Funds) ? "#B4D455" : XKCDColors.HexFormat.Orange));
		if (hasFacility)
		{
			upgradeCost = host.Facility.GetUpgradeCost();
			downgradeCost = host.Facility.GetDowngradeCost();
			level = host.Facility.FacilityLevel;
			maxLevel = host.Facility.MaxLevel;
			levelText = host.Facility.GetLevelText();
		}
		else
		{
			upgradeCost = 0f;
			downgradeCost = 0f;
			level = 0;
			maxLevel = 0f;
			levelText = string.Empty;
		}
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			CurrencyModifierQuery currencyModifierQuery2 = CurrencyModifierQuery.RunQuery(TransactionReasons.StructureConstruction, 0f - upgradeCost, 0f, 0f);
			upgradeCost = 0f - currencyModifierQuery2.GetTotal(Currency.Funds);
			CurrencyModifierQuery currencyModifierQuery3 = CurrencyModifierQuery.RunQuery(TransactionReasons.StructureConstruction, 0f - downgradeCost, 0f, 0f);
			downgradeCost = 0f - currencyModifierQuery3.GetTotal(Currency.Funds);
			levelFieldText.text = Localizer.Format("#autoLOC_475323", level + 1);
			upgradeString = currencyModifierQuery2.GetCostLine(displayInverted: true, useCurrencyColors: true, useInsufficientCurrencyColors: true, includePercentage: false, "\n");
			downgradeString = currencyModifierQuery3.GetCostLine(displayInverted: true, useCurrencyColors: true, useInsufficientCurrencyColors: true, includePercentage: false, "\n");
			if ((float)level != maxLevel)
			{
				UpgradeButton.interactable = true;
				UpgradeButtonText.text = Localizer.Format("#autoLOC_475331", upgradeString);
			}
			else
			{
				UpgradeButton.interactable = false;
				UpgradeButtonText.text = Localizer.Format("#autoLOC_475336");
			}
		}
		else
		{
			levelFieldText.text = Localizer.Format("#autoLOC_475323", level + 1);
			UpgradeButton.gameObject.SetActive(value: false);
			UpgradeButton.interactable = false;
			DowngradeButton.gameObject.SetActive(value: false);
			DowngradeButton.interactable = false;
		}
		levelStatsText.text = levelText;
		if (host.Operational)
		{
			windowTitleField.color = XKCDColors.ElectricLime;
			if (facilityDamage > 0f)
			{
				statusText.text = Localizer.Format("#autoLOC_475347") + " <b><color=" + damageColor + ">" + Localizer.Format("#autoLOC_257237") + " (" + facilityDamageLevel + " " + Localizer.Format("#autoLOC_6002247") + ")</color></b>";
			}
			else
			{
				statusText.text = Localizer.Format("#autoLOC_475351");
			}
			EnterButtonText.color = XKCDColors.ColorTranslator.FromHtml("#B4D455");
			EnterButton.interactable = true;
			if (showDowngradeControls)
			{
				DowngradeButton.gameObject.SetActive(value: false);
				DemolishButton.gameObject.SetActive(value: true);
				if (host.destructibles.Length != 0)
				{
					DemolishButton.interactable = true;
				}
				else
				{
					DemolishButton.interactable = false;
				}
			}
			else
			{
				DowngradeButton.gameObject.SetActive(value: false);
				DemolishButton.gameObject.SetActive(value: false);
			}
		}
		else
		{
			windowTitleField.color = XKCDColors.Orange;
			statusText.text = Localizer.Format("#autoLOC_475380", XKCDColors.HexFormat.Orange);
			EnterButtonText.color = Color.clear;
			EnterButton.interactable = false;
			if (level > 0 && showDowngradeControls && HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
			{
				DemolishButton.gameObject.SetActive(value: false);
				DowngradeButton.gameObject.SetActive(isUpgradeable);
				DowngradeButtonText.text = Localizer.Format("#autoLOC_6002251") + " " + Mathf.Max(0, level) + "\n" + downgradeString;
				if (!Funding.CanAfford(downgradeCost))
				{
					DowngradeButton.interactable = false;
				}
				else
				{
					DowngradeButton.interactable = true;
				}
			}
			else if (showDowngradeControls)
			{
				DowngradeButton.gameObject.SetActive(value: false);
				DemolishButton.gameObject.SetActive(value: true);
				DemolishButton.interactable = false;
			}
			else
			{
				DowngradeButton.gameObject.SetActive(value: false);
				DemolishButton.gameObject.SetActive(value: false);
			}
		}
		if (repairCost == 0f)
		{
			if (facilityDamage > 0f)
			{
				RepairButton.interactable = false;
				RepairButtonText.color = btnTextBaseColor.smethod_0(0.6f);
				RepairButtonText.text = "<color=" + XKCDColors.HexFormat.LightGrey + ">" + Localizer.Format("#autoLOC_6002248") + "</color>\n";
			}
			else
			{
				RepairButton.interactable = false;
				RepairButtonText.color = btnTextBaseColor.smethod_0(0.3f);
				RepairButtonText.text = Localizer.Format("#autoLOC_475425");
			}
		}
		else
		{
			RepairButton.interactable = true;
			RepairButtonText.color = btnTextBaseColor.smethod_0(1f);
			if (Funding.Instance != null)
			{
				RepairButtonText.text = Localizer.Format("#autoLOC_475433", repairCostColor, currencyModifierQuery.GetCostLine(displayInverted: true, useCurrencyColors: true, useInsufficientCurrencyColors: true, includePercentage: false, "\n"));
			}
			else
			{
				RepairButtonText.text = Localizer.Format("#autoLOC_6002249");
			}
		}
	}

	public string getFundsTextColorTag(float amount)
	{
		return "<color=" + XKCDColors.ColorTranslator.ToHex(Funding.CanAfford(amount) ? canAffordColor : cannotAffordColor) + ">";
	}

	public override string GetWindowTitle()
	{
		return facilityName;
	}

	public override void CreateWindowContent()
	{
		descriptionText.text = description;
		RepairButton.onClick.AddListener(OnRepairButtonInput);
		EnterButton.onClick.AddListener(OnEnterBtnInput);
		UpgradeButton.onClick.AddListener(OnUpgradeButtonInput);
		UIOnHover uIOnHover = UpgradeButton.gameObject.AddComponent<UIOnHover>();
		uIOnHover.onEnter.AddListener(OnUpgradeButtonHoverIn);
		uIOnHover.onExit.AddListener(OnUpgradeButtonHoverOut);
		DemolishButton.onClick.AddListener(OnDemolishButtonInput);
		DowngradeButton.onClick.AddListener(OnDowngradeButtonInput);
		OnFacilityValuesModified();
	}

	public void OnRepairButtonInput()
	{
		Dismiss(DismissAction.Repair);
	}

	public void OnEnterBtnInput()
	{
		Dismiss(DismissAction.Enter);
	}

	public void OnUpgradeButtonInput()
	{
		Dismiss(DismissAction.Upgrade);
	}

	public void OnUpgradeButtonHoverIn()
	{
		PreviewNextLevel(previewState: true);
	}

	public void OnUpgradeButtonHoverOut()
	{
		PreviewNextLevel(previewState: false);
	}

	public void OnDemolishButtonInput()
	{
		Dismiss(DismissAction.Demolish);
	}

	public void OnDowngradeButtonInput()
	{
		Dismiss(DismissAction.Downgrade);
	}

	public override void OnClickOut()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.UI_DIALOGS))
		{
			Dismiss(DismissAction.None);
		}
	}

	public void Dismiss(DismissAction dma)
	{
		GameEvents.onFacilityContextMenuDespawn.Fire(this);
		OnMenuDismiss(dma);
		GameEvents.OnKSCStructureCollapsing.Remove(OnKSCStructureEvent);
		GameEvents.OnKSCStructureCollapsed.Remove(OnKSCStructureEvent);
		GameEvents.OnKSCStructureRepairing.Remove(OnKSCStructureEvent);
		GameEvents.OnKSCStructureRepaired.Remove(OnKSCStructureEvent);
		Terminate();
	}

	public void PreviewNextLevel(bool previewState)
	{
		if (!hasFacility)
		{
			levelStatsText.text = string.Empty;
		}
		else if (previewState)
		{
			levelStatsText.text = "<color=" + XKCDColors.HexFormat.ElectricLime + ">" + host.Facility.GetNextLevelText() + "</color>";
		}
		else
		{
			levelStatsText.text = host.Facility.GetLevelText();
		}
	}

	public override void StartThis()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
	}

	public override void OnDestroyThis()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
		Terminate();
	}
}
