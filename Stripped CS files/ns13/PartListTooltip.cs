using System;
using System.Collections.Generic;
using System.Text;
using ns11;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns13;

public class PartListTooltip : Tooltip
{
	public delegate string PartStatsDelegate(Part p, bool showUpgradesAvail);

	public GameObject costPanel;

	public Callback<PartListTooltip> onPurchase;

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

	public List<PartListTooltipWidget> extInfoModules = new List<PartListTooltipWidget>();

	public List<PartListTooltipWidget> extInfoRscs = new List<PartListTooltipWidget>();

	public List<PartListTooltipWidget> extInfoVariants = new List<PartListTooltipWidget>();

	public GameObject panelExtended;

	public RectTransform extInfoListContainer;

	public RectTransform extInfoListSpacer;

	public RectTransform extInfoListSpacerVariants;

	public AvailablePart partInfo;

	public PartUpgradeHandler.Upgrade upgrade;

	public Part partRef;

	public PartModule.PartUpgradeState upgradeState;

	public ConfigNode upgradeNode;

	public bool showCostInExtendedInfo;

	public bool hasCreatedExtendedInfo;

	public bool hasExtendedInfo;

	public CurrencyModifierQuery currencyQuery;

	public float entryCost;

	public float partCost;

	public float upgradeCost;

	public string entryCostText;

	public bool requiresEntryPurchase;

	public PartIcon _partIcon;

	public float iconSpin = 60f;

	public Material[] materials;

	public static PartStatsDelegate GetPartStats = _GetPartStats;

	public static string cacheAutoLOC_456241;

	public static string cacheAutoLOC_7003267;

	public static string cacheAutoLOC_7003268;

	public static string cacheAutoLOC_456346;

	public static string cacheAutoLOC_456368;

	public static string cacheAutoLOC_456391;

	public static string cacheAutoLOC_7003246;

	public static string cacheAutoLOC_6005097;

	public bool HasExtendedInfo => hasExtendedInfo;

	public bool isGrey { get; set; }

	public PartIcon partIcon
	{
		get
		{
			if (_partIcon == null)
			{
				_partIcon = new PartIcon(null);
			}
			return _partIcon;
		}
		set
		{
			_partIcon = value;
		}
	}

	public GameObject icon
	{
		get
		{
			if (_partIcon == null)
			{
				_partIcon = new PartIcon(null);
			}
			return _partIcon.Icon;
		}
	}

	public Transform iconTransform { get; set; }

	public void Update()
	{
		if (!(icon == null) && !isGrey)
		{
			iconTransform.localRotation *= Quaternion.AngleAxis(iconSpin * Time.deltaTime, Vector3.up);
			PartListTooltipController.SetupScreenSpaceMask((RectTransform)UIMasterController.Instance.tooltipCanvas.transform);
			PartListTooltipController.SetScreenSpaceMaskMaterials(materials);
		}
	}

	public void OnDestroy()
	{
		if (icon != null)
		{
			EditorPartIcon.CleanUpMaterials(icon);
			UnityEngine.Object.Destroy(icon);
		}
		for (int i = 0; i < extInfoModules.Count; i++)
		{
			UnityEngine.Object.Destroy(extInfoModules[i].gameObject);
		}
		for (int j = 0; j < extInfoRscs.Count; j++)
		{
			UnityEngine.Object.Destroy(extInfoRscs[j].gameObject);
		}
		for (int k = 0; k < extInfoVariants.Count; k++)
		{
			UnityEngine.Object.Destroy(extInfoVariants[k].gameObject);
		}
	}

	public void SetupGrayout(string grayoutMessage)
	{
		isGrey = true;
		textGreyoutMessage.enabled = true;
		textGreyoutMessage.text = Localizer.Format(grayoutMessage);
	}

	public void Setup(AvailablePart availablePart, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		HidePreviousTooltipWidgets();
		this.onPurchase = onPurchase;
		partInfo = availablePart;
		partRef = partInfo.partPrefab;
		upgradeState = PartModule.PartUpgradeState.NONE;
		upgradeNode = null;
		if (PartUpgradeManager.Handler.UgpradesAllowed())
		{
			SetupUpgradeInfo(availablePart);
		}
		textName.text = partInfo.title;
		textInfoBasic.text = "<color=" + XKCDColors.HexFormat.LightCyan + ">" + GetPartStats(partRef, showUpgradesAvail: true) + "</color>\n\n<color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + ((upgradeState == PartModule.PartUpgradeState.AVAILABLE) ? GetUpgradedPrimaryInfo(partInfo, 6) : GetPrimaryInfo(partInfo, 6)) + "</color>";
		textManufacturer.text = partInfo.manufacturer;
		textDescription.text = partInfo.description;
		if (PartUpgradeManager.Handler.UgpradesAllowed())
		{
			if (upgradeState == PartModule.PartUpgradeState.AVAILABLE)
			{
				textDescription.text += Localizer.Format(PartModule.UpgradesAvailableString);
			}
			else if (upgradeState == PartModule.PartUpgradeState.LOCKED)
			{
				textDescription.text += Localizer.Format(PartModule.UpgradesLockedString);
			}
		}
		partCost = partInfo.cost + partInfo.partPrefab.GetModuleCosts(partInfo.cost);
		partCost = Mathf.Clamp(partCost, partRef.partInfo.minimumCost, Mathf.Abs(partCost));
		if (upgradeCost != 0f)
		{
			textCost.text = Localizer.Format("#autoLOC_6002278", "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>", partCost.ToString("N2"), "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>", upgradeCost.ToString("N2"));
		}
		else
		{
			textCost.text = Localizer.Format("#autoLOC_456128", "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>", partCost.ToString("N2"));
		}
		imagePart.texture = texture;
		SetupScaleGauge(partInfo.iconScale);
		requiresEntryPurchase = !ResearchAndDevelopment.PartModelPurchased(partInfo) && ResearchAndDevelopment.PartTechAvailable(partInfo);
		if (requiresEntryPurchase)
		{
			currencyQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDPartPurchase, -partInfo.entryCost, 0f, 0f);
			entryCostText = currencyQuery.GetCostLine();
			bool flag = currencyQuery.CanAfford();
			string text = Localizer.Format("#autoLOC_7003265", entryCostText);
			buttonPurchase.gameObject.SetActive(flag);
			buttonPurchaseCaption.text = text;
			buttonPurchaseRed.gameObject.SetActive(!flag);
			buttonPurchaseCaptionRed.text = text;
			buttonPurchase.onClick.AddListener(onPurchaseButton);
			buttonPurchaseRed.onClick.AddListener(onBtnPurchaseRed);
			buttonPurchaseContainer.SetActive(value: true);
			costPanel.SetActive(value: false);
		}
		else
		{
			if (Funding.Instance == null && HighLogic.LoadedScene == GameScenes.SPACECENTER)
			{
				costPanel.SetActive(value: false);
			}
			else if (!costPanel.activeSelf)
			{
				costPanel.SetActive(value: true);
			}
			entryCost = partInfo.entryCost;
			entryCostText = entryCost.ToString("N0");
			buttonPurchaseContainer.SetActive(value: false);
		}
		hasExtendedInfo = partInfo.resourceInfos.Count > 0 || partInfo.moduleInfos.Count > 0;
		showCostInExtendedInfo = requiresEntryPurchase;
		hasCreatedExtendedInfo = false;
		panelExtended.SetActive(value: false);
	}

	public void UpdateVariantText(string varName)
	{
		ModulePartVariants modulePartVariants = partRef.FindModuleImplementing<ModulePartVariants>();
		if (!(modulePartVariants == null))
		{
			modulePartVariants.SetVariant(varName);
			float num = partRef.partInfo.partPrefab.mass + partRef.GetModuleMass(partRef.partInfo.partPrefab.mass);
			num = Mathf.Clamp(num, partRef.partInfo.MinimumMass, Mathf.Abs(num));
			textInfoBasic.text = "<color=" + XKCDColors.HexFormat.LightCyan + ">" + _GenerateStatsText(partRef, showUpgradesAvail: true, num) + "</color>\n\n<color=" + XKCDColors.HexFormat.KSPBadassGreen + ">" + ((upgradeState == PartModule.PartUpgradeState.AVAILABLE) ? GetUpgradedPrimaryInfo(partInfo, 6) : GetPrimaryInfo(partInfo, 6)) + "</color>";
			partCost = partInfo.cost + partInfo.partPrefab.GetModuleCosts(partInfo.cost);
			partCost = Mathf.Clamp(partCost, partRef.partInfo.minimumCost, Mathf.Abs(partCost));
			if (upgradeCost != 0f)
			{
				textCost.text = Localizer.Format("#autoLOC_6002278", "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>", partCost.ToString("N2"), "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>", upgradeCost.ToString("N2"));
			}
			else
			{
				textCost.text = Localizer.Format("#autoLOC_456128", "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>", partCost.ToString("N2"));
			}
			textInfoBasic.ForceMeshUpdate();
			textCost.ForceMeshUpdate();
		}
	}

	public void UpdateCargoPartModuleInfo()
	{
		ModuleCargoPart moduleCargoPart = partRef.FindModuleImplementing<ModuleCargoPart>();
		if (moduleCargoPart == null)
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < partInfo.moduleInfos.Count)
			{
				if (partInfo.moduleInfos[num].moduleName == moduleCargoPart.GUIName)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		partInfo.moduleInfos[num].info = moduleCargoPart.GetInfo();
		for (int i = 0; i < extInfoModules.Count; i++)
		{
			if (partInfo.moduleInfos[num].moduleDisplayName == extInfoModules[i].textName.text)
			{
				extInfoModules[i].textInfo.text = partInfo.moduleInfos[num].info;
			}
		}
	}

	public void SetupUpgradeInfo(AvailablePart availablePart)
	{
		for (int i = 0; i < availablePart.partConfig.nodes.Count; i++)
		{
			if (availablePart.partConfig.nodes[i].HasValue("name") && availablePart.partConfig.nodes[i].GetValue("name") == "PartStatsUpgradeModule")
			{
				upgradeNode = availablePart.partConfig.nodes[i];
				break;
			}
		}
		upgradeState = PartModule.UpgradesAvailable(availablePart.partPrefab, upgradeNode);
		if (upgradeState != PartModule.PartUpgradeState.AVAILABLE)
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < availablePart.partPrefab.Modules.Count)
			{
				if (availablePart.partPrefab.Modules[num].HasUpgrades())
				{
					availablePart.partPrefab.Modules[num].ApplyUpgrades(PartModule.StartState.Editor);
				}
				if (availablePart.partPrefab.Modules[num].moduleName == "PartStatsUpgradeModule")
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		availablePart.partPrefab.Modules[num].OnLoad(upgradeNode);
		for (int j = 0; j < availablePart.partPrefab.Modules[num].upgradesApplied.Count; j++)
		{
			PartUpgradeHandler.Upgrade upgrade = PartUpgradeManager.Handler.GetUpgrade(availablePart.partPrefab.Modules[num].upgradesApplied[j]);
			if (upgrade == null)
			{
				continue;
			}
			if (upgrade.cost != 0f)
			{
				if (upgrade.cumulativeCost)
				{
					upgradeCost += upgrade.cost;
				}
				else
				{
					upgradeCost = upgrade.cost;
				}
			}
			PartListTooltipWidget newTooltipWidget = GetNewTooltipWidget(extInfoModuleWidgetPrefab);
			newTooltipWidget.Setup(availablePart.partPrefab.Modules[num].GetModuleDisplayName(), upgrade.description);
			newTooltipWidget.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		}
	}

	public void Setup(AvailablePart availablePart, PartUpgradeHandler.Upgrade up, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		HidePreviousTooltipWidgets();
		this.onPurchase = onPurchase;
		partInfo = availablePart;
		partRef = partInfo.partPrefab;
		upgrade = up;
		textName.text = upgrade.title;
		textInfoBasic.text = (string.IsNullOrEmpty(upgrade.basicInfo) ? " " : upgrade.basicInfo);
		textManufacturer.text = (string.IsNullOrEmpty(upgrade.manufacturer) ? " " : upgrade.manufacturer);
		textDescription.text = upgrade.description;
		if (upgrade.cost != 0f)
		{
			textCost.text = Localizer.Format("#autoLOC_6002272", "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>", upgrade.cumulativeCost ? "+" : "", upgrade.cost.ToString("N2"));
		}
		else
		{
			textCost.text = " ";
		}
		imagePart.texture = texture;
		SetupScaleGauge(partInfo.iconScale);
		requiresEntryPurchase = !HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch && PartUpgradeManager.Handler.IsAvailableToUnlock(upgrade.name);
		if (requiresEntryPurchase)
		{
			currencyQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDPartPurchase, (upgrade.entryCost > 0f) ? (0f - upgrade.entryCost) : ((float)(-partInfo.entryCost)), 0f, 0f);
			entryCostText = currencyQuery.GetCostLine();
			bool flag = currencyQuery.CanAfford();
			string text = Localizer.Format("#autoLOC_7003265", entryCostText);
			buttonPurchase.gameObject.SetActive(flag);
			buttonPurchaseCaption.text = text;
			buttonPurchaseRed.gameObject.SetActive(!flag);
			buttonPurchaseCaptionRed.text = text;
			buttonPurchase.onClick.AddListener(onPurchaseButton);
			buttonPurchaseRed.onClick.AddListener(onBtnPurchaseRed);
			buttonPurchaseContainer.SetActive(value: true);
			costPanel.SetActive(value: false);
		}
		else
		{
			entryCost = upgrade.entryCost;
			entryCostText = entryCost.ToString("N0");
			buttonPurchaseContainer.SetActive(value: false);
		}
		hasExtendedInfo = upgrade.IsUsed();
		hasCreatedExtendedInfo = false;
		showCostInExtendedInfo = requiresEntryPurchase && upgrade.cost != 0f;
		panelExtended.SetActive(value: false);
	}

	public void SetupScaleGauge(float iconScale)
	{
		if (partInfo.iconScale < 1f)
		{
			textScale.text = cacheAutoLOC_456241;
			sliderScaleTape.value = iconScale;
		}
		else
		{
			textScale.text = Localizer.Format("#autoLOC_7003266", (1f / partInfo.iconScale * 100f).ToString("0.0"));
			sliderScaleTape.value = 1f;
		}
	}

	public void CreateExtendedInfo(bool showPartCost)
	{
		AvailablePart.ModuleInfo moduleInfo = null;
		if (showPartCost)
		{
			PartListTooltipWidget newTooltipWidget = GetNewTooltipWidget(extInfoRscWidgePrefab);
			newTooltipWidget.Setup(cacheAutoLOC_7003267, textCost.text);
			newTooltipWidget.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		}
		if (upgradeState == PartModule.PartUpgradeState.AVAILABLE)
		{
			int i = 0;
			for (int count = partInfo.partPrefab.Modules.Count; i < count; i++)
			{
				PartModule partModule = partInfo.partPrefab.Modules[i];
				if (partModule.moduleName != "PartStatsUpgradeModule")
				{
					string info = partModule.GetInfo();
					if (!string.IsNullOrEmpty(info))
					{
						PartListTooltipWidget newTooltipWidget2 = GetNewTooltipWidget(extInfoModuleWidgetPrefab);
						newTooltipWidget2.Setup(partModule.GetModuleDisplayName(), info);
						newTooltipWidget2.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
					}
				}
			}
		}
		else
		{
			int j = 0;
			for (int count2 = partInfo.moduleInfos.Count; j < count2; j++)
			{
				if (partInfo.moduleInfos[j].moduleName != ModulePartVariants.GetTitle())
				{
					AvailablePart.ModuleInfo moduleInfo2 = partInfo.moduleInfos[j];
					PartListTooltipWidget newTooltipWidget3 = GetNewTooltipWidget(extInfoModuleWidgetPrefab);
					newTooltipWidget3.Setup(moduleInfo2.moduleDisplayName, moduleInfo2.info);
					newTooltipWidget3.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
				}
				else
				{
					moduleInfo = partInfo.moduleInfos[j];
				}
			}
		}
		if (extInfoListContainer.childCount > 2 && partInfo.resourceInfos.Count > 0)
		{
			extInfoListSpacer.gameObject.SetActive(value: true);
			extInfoListSpacer.SetSiblingIndex(extInfoListContainer.childCount - 1);
		}
		else
		{
			extInfoListSpacer.gameObject.SetActive(value: false);
		}
		int k = 0;
		for (int count3 = partInfo.resourceInfos.Count; k < count3; k++)
		{
			AvailablePart.ResourceInfo resourceInfo = partInfo.resourceInfos[k];
			PartListTooltipWidget newTooltipWidget4 = GetNewTooltipWidget(extInfoRscWidgePrefab);
			newTooltipWidget4.Setup(resourceInfo.displayName.LocalizeRemoveGender(), resourceInfo.info);
			newTooltipWidget4.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		}
		if (extInfoListContainer.childCount > 2 && moduleInfo != null)
		{
			extInfoListSpacerVariants.gameObject.SetActive(value: true);
			extInfoListSpacerVariants.SetSiblingIndex(extInfoListContainer.childCount - 1);
		}
		else
		{
			extInfoListSpacerVariants.gameObject.SetActive(value: false);
		}
		if (moduleInfo != null)
		{
			PartListTooltipWidget newTooltipWidget5 = GetNewTooltipWidget(extInfoVariantsWidgePrefab);
			newTooltipWidget5.Setup(moduleInfo.moduleDisplayName, moduleInfo.info);
			newTooltipWidget5.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		}
	}

	public void HidePreviousTooltipWidgets()
	{
		for (int i = 0; i < extInfoModules.Count; i++)
		{
			if (extInfoModules[i].gameObject.activeSelf)
			{
				extInfoModules[i].gameObject.SetActive(value: false);
			}
		}
		for (int j = 0; j < extInfoRscs.Count; j++)
		{
			if (extInfoRscs[j].gameObject.activeSelf)
			{
				extInfoRscs[j].gameObject.SetActive(value: false);
			}
		}
		for (int k = 0; k < extInfoVariants.Count; k++)
		{
			if (extInfoVariants[k].gameObject.activeSelf)
			{
				extInfoVariants[k].gameObject.SetActive(value: false);
			}
		}
	}

	public PartListTooltipWidget GetNewTooltipWidget(PartListTooltipWidget prefab)
	{
		List<PartListTooltipWidget> list = null;
		if (prefab == extInfoModuleWidgetPrefab)
		{
			list = extInfoModules;
		}
		else if (prefab == extInfoRscWidgePrefab)
		{
			list = extInfoRscs;
		}
		else
		{
			if (!(prefab == extInfoVariantsWidgePrefab))
			{
				return null;
			}
			list = extInfoVariants;
		}
		int num = 0;
		while (true)
		{
			if (num < list.Count)
			{
				if (!list[num].gameObject.activeSelf)
				{
					break;
				}
				num++;
				continue;
			}
			PartListTooltipWidget partListTooltipWidget = UnityEngine.Object.Instantiate(prefab);
			list.Add(partListTooltipWidget);
			return partListTooltipWidget;
		}
		list[num].gameObject.SetActive(value: true);
		return list[num];
	}

	public void CreateExtendedUpgradeInfo(bool showPartCost)
	{
		if (showPartCost)
		{
			PartListTooltipWidget newTooltipWidget = GetNewTooltipWidget(extInfoRscWidgePrefab);
			newTooltipWidget.Setup(cacheAutoLOC_7003268, textCost.text);
			newTooltipWidget.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		}
		List<string[]> usedByStrings = upgrade.GetUsedByStrings();
		int i = 0;
		for (int count = usedByStrings.Count; i < count; i++)
		{
			PartListTooltipWidget newTooltipWidget2 = GetNewTooltipWidget(extInfoModuleWidgetPrefab);
			newTooltipWidget2.Setup(usedByStrings[i][0], usedByStrings[i][1]);
			newTooltipWidget2.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		}
	}

	public void DestroyExtendedInfo()
	{
		int childCount = extInfoListContainer.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = extInfoListContainer.transform.GetChild(i);
			if (!(child == extInfoListSpacer))
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	public void DisplayExtendedInfo(bool display, string rmbHintText)
	{
		if (HasExtendedInfo)
		{
			bool active;
			if ((active = display && extInfoListContainer.childCount > 1) && !hasCreatedExtendedInfo)
			{
				CreateExtendedInfo(requiresEntryPurchase);
				hasCreatedExtendedInfo = true;
			}
			panelExtended.SetActive(active);
		}
		textRMBHint.text = rmbHintText;
	}

	public static string _GenerateStatsText(Part p, bool showUpgradesAvail, float mass)
	{
		string text = ((!p.applyKerbalMassModification || p.CrewCapacity <= 0) ? "" : (" + " + cacheAutoLOC_6005097));
		string text2 = Localizer.Format("#autoLOC_6005098", (mass + p.GetResourceMass()).ToString("0.0###"), text, p.crashTolerance.ToString("0.0###"), p.gTolerance.ToString("N0"), p.maxPressure.ToString("N0"), p.maxTemp.ToString("F0"), p.skinMaxTemp.ToString("F0"));
		if (p.CrewCapacity > 0)
		{
			text2 = text2 + cacheAutoLOC_456346 + p.CrewCapacity;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		string techID = string.Empty;
		int num = p.Modules.Count - 1;
		while (num >= 0 && (!flag || !flag2))
		{
			if (p.Modules[num] is IToggleCrossfeed toggleCrossfeed)
			{
				flag |= toggleCrossfeed.CrossfeedToggleableEditor();
				flag2 |= toggleCrossfeed.CrossfeedToggleableFlight();
				if (flag3 && toggleCrossfeed.CrossfeedRequiresTech() && !toggleCrossfeed.CrossfeedHasTech())
				{
					techID = toggleCrossfeed.CrossfeedTech();
				}
				else
				{
					flag3 = false;
				}
			}
			num--;
		}
		if (!flag && !flag2)
		{
			if (!p.fuelCrossFeed)
			{
				text2 += cacheAutoLOC_456368;
			}
		}
		else
		{
			int num2 = 0;
			if (flag && flag2)
			{
				num2 = 0;
			}
			else if (flag)
			{
				num2 = 1;
			}
			else if (flag2)
			{
				num2 = 2;
			}
			text2 += Localizer.Format("#autoLOC_456374", num2);
			if (flag3)
			{
				text2 = text2 + " " + Localizer.Format("#autoLOC_456381", Localizer.Format(ResearchAndDevelopment.GetTechnologyTitle(techID)));
			}
			text2 = text2 + " " + Localizer.Format("#autoLOC_456384", Convert.ToInt32(p.fuelCrossFeed));
		}
		if (ResearchAndDevelopment.IsExperimentalPart(p.partInfo.partPrefab.partInfo))
		{
			text2 += cacheAutoLOC_456391;
		}
		if (showUpgradesAvail && PartUpgradeManager.Handler.CanHaveUpgrades())
		{
			switch (PartModule.UpgradesAvailable(p))
			{
			case PartModule.PartUpgradeState.AVAILABLE:
				text2 += Localizer.Format(PartModule.UpgradesAvailableString);
				break;
			case PartModule.PartUpgradeState.LOCKED:
				text2 += Localizer.Format(PartModule.UpgradesLockedString);
				break;
			}
		}
		return text2;
	}

	public static string _GetPartStats(Part p, bool showUpgradesAvail)
	{
		return _GenerateStatsText(p, showUpgradesAvail, Mathf.Clamp(p.mass, p.partInfo.MinimumMass, Mathf.Abs(p.mass)));
	}

	public static string GetUpgradedPrimaryInfo(AvailablePart aP, int maxLines)
	{
		List<string> list = new List<string>();
		if (aP.partPrefab.GetType().IsSubclassOf(typeof(Part)))
		{
			if (aP.partPrefab is IModuleInfo)
			{
				IModuleInfo moduleInfo = aP.partPrefab as IModuleInfo;
				list.Add(moduleInfo.GetPrimaryField() + "\n");
			}
			else
			{
				list.Add(aP.partPrefab.drawStats().Trim());
			}
		}
		int i = 0;
		for (int count = aP.partPrefab.Modules.Count; i < count; i++)
		{
			PartModule partModule = aP.partPrefab.Modules[i];
			if (partModule is IModuleInfo)
			{
				IModuleInfo moduleInfo2 = partModule as IModuleInfo;
				list.Add(moduleInfo2.GetPrimaryField());
			}
		}
		for (int j = 0; j < aP.resourceInfos.Count; j++)
		{
			AvailablePart.ResourceInfo resourceInfo = aP.resourceInfos[j];
			if (!string.IsNullOrEmpty(resourceInfo.primaryInfo))
			{
				list.Add(resourceInfo.primaryInfo + "\n");
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		int k = 0;
		for (int count2 = list.Count; k < count2 && k < maxLines; k++)
		{
			stringBuilder.Append(list[k]);
		}
		return stringBuilder.ToString().Trim();
	}

	public static string GetPrimaryInfo(AvailablePart aP, int maxLines)
	{
		List<string> list = new List<string>();
		int count = aP.moduleInfos.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart.ModuleInfo moduleInfo = aP.moduleInfos[i];
			if (!string.IsNullOrEmpty(moduleInfo.primaryInfo))
			{
				list.Add(moduleInfo.primaryInfo + "\n");
			}
		}
		count = aP.resourceInfos.Count;
		for (int j = 0; j < count; j++)
		{
			AvailablePart.ResourceInfo resourceInfo = aP.resourceInfos[j];
			if (!string.IsNullOrEmpty(resourceInfo.primaryInfo))
			{
				list.Add(resourceInfo.primaryInfo + "\n");
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		int k = 0;
		for (int count2 = list.Count; k < count2 && k < maxLines; k++)
		{
			stringBuilder.Append(list[k]);
		}
		return stringBuilder.ToString().Trim();
	}

	public void onPurchaseButton()
	{
		onPurchase(this);
	}

	public void onBtnPurchaseRed()
	{
		ScreenMessages.PostScreenMessage(cacheAutoLOC_7003246, 3f, ScreenMessageStyle.UPPER_CENTER);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_456241 = Localizer.Format("#autoLOC_456241");
		cacheAutoLOC_7003267 = Localizer.Format("#autoLOC_7003267");
		cacheAutoLOC_7003268 = Localizer.Format("#autoLOC_7003268");
		cacheAutoLOC_456346 = "\n<b>" + Localizer.Format("#autoLOC_456346") + "</b> ";
		cacheAutoLOC_456368 = Localizer.Format("#autoLOC_456368");
		cacheAutoLOC_456391 = Localizer.Format("#autoLOC_456391");
		cacheAutoLOC_7003246 = Localizer.Format("#autoLOC_7003246");
		cacheAutoLOC_6005097 = Localizer.Format("#autoLOC_6005097");
	}
}
