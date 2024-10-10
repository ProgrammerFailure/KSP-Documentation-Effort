using System.Collections.Generic;
using ns13;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

[RequireComponent(typeof(UIList))]
public class RDPartList : MonoBehaviour
{
	public GameObject partListItem;

	public Dictionary<AvailablePart, Texture2D> partIcons = new Dictionary<AvailablePart, Texture2D>();

	public RectTransform partTransformMask;

	public ScrollRect partListScrollRect;

	public UIList scrollList;

	public RDNode selected_node;

	public List<RDPartListItem> partListItems = new List<RDPartListItem>();

	public List<RDPartListItem> listItems => partListItems;

	public void Awake()
	{
		scrollList = GetComponent<UIList>();
		partListScrollRect = partTransformMask.GetComponent<ScrollRect>();
		partListScrollRect.onValueChanged.AddListener(OnPartListUpdate);
	}

	public void OnDestroy()
	{
		partListScrollRect.onValueChanged.RemoveListener(OnPartListUpdate);
	}

	public void Refresh()
	{
		SetupParts(selected_node);
		if (RDTechTreeSearchBar.Instance != null)
		{
			RDTechTreeSearchBar.Instance.SelectPartIcons();
		}
	}

	public void SetupParts(RDNode node)
	{
		selected_node = node;
		scrollList.Clear(destroyElements: true);
		partListItems.Clear();
		if (node == null)
		{
			return;
		}
		List<AvailablePart> list = new List<AvailablePart>();
		int i = 0;
		for (int count = node.tech.partsAssigned.Count; i < count; i++)
		{
			AvailablePart item = node.tech.partsAssigned[i];
			if (!node.tech.partsPurchased.Contains(item))
			{
				list.Add(item);
			}
		}
		List<AvailablePart> purchased = new List<AvailablePart>(node.tech.partsPurchased);
		AddParts(purchased, list);
		List<PartUpgradeHandler.Upgrade> list2 = new List<PartUpgradeHandler.Upgrade>();
		List<PartUpgradeHandler.Upgrade> list3 = new List<PartUpgradeHandler.Upgrade>();
		List<PartUpgradeHandler.Upgrade> upgradesForTech = PartUpgradeManager.Handler.GetUpgradesForTech(node.tech.techID);
		int j = 0;
		for (int count2 = upgradesForTech.Count; j < count2; j++)
		{
			PartUpgradeHandler.Upgrade upgrade = upgradesForTech[j];
			if (PartUpgradeManager.Handler.IsUnlocked(upgrade.name))
			{
				list2.Add(upgrade);
			}
			else
			{
				list3.Add(upgrade);
			}
		}
		AddUpgrades(list2, list3);
		OnPartListUpdate(Vector2.zero);
	}

	public void AddUpgrades(List<PartUpgradeHandler.Upgrade> purchased, List<PartUpgradeHandler.Upgrade> available)
	{
		int i = 0;
		for (int count = purchased.Count; i < count; i++)
		{
			AddUpgradeListItem(purchased[i], purchased: true);
		}
		int j = 0;
		for (int count2 = available.Count; j < count2; j++)
		{
			AddUpgradeListItem(available[j], purchased: false);
		}
	}

	public void AddUpgradeListItem(PartUpgradeHandler.Upgrade upgrade, bool purchased)
	{
		RDPartListItem componentInChildren = Object.Instantiate(partListItem).GetComponentInChildren<RDPartListItem>();
		string text = "";
		AvailablePart partInfoByName = PartLoader.getPartInfoByName(upgrade.partIcon);
		if (purchased)
		{
			text = Localizer.Format("#autoLOC_470834");
			SetPart(componentInChildren, unlocked: true, text, partInfoByName, upgrade);
		}
		else
		{
			if (Funding.Instance != null)
			{
				float upgradeCost = PartUpgradeManager.Handler.GetUpgradeCost(upgrade.name);
				CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDPartPurchase, 0f - upgradeCost, 0f, 0f);
				text = ((selected_node.tech.state != RDTech.State.Available) ? ("<color=" + XKCDColors.HexFormat.LightBlueGrey + ">" + currencyModifierQuery.GetCostLine(displayInverted: true, useCurrencyColors: false, useInsufficientCurrencyColors: true, includePercentage: true) + "</color>") : currencyModifierQuery.GetCostLine(displayInverted: true, useCurrencyColors: false, useInsufficientCurrencyColors: true, includePercentage: true));
			}
			else
			{
				text = "";
			}
			SetPart(componentInChildren, unlocked: false, text, partInfoByName, upgrade);
		}
		scrollList.AddItem(componentInChildren.GetComponentInParent<UIListItem>());
		partListItems.Add(componentInChildren);
	}

	public void AddParts(List<AvailablePart> purchased, List<AvailablePart> assigned)
	{
		int i = 0;
		for (int count = purchased.Count; i < count; i++)
		{
			if (!purchased[i].TechHidden)
			{
				AddPartListItem(purchased[i], purchased: true);
			}
		}
		int j = 0;
		for (int count2 = assigned.Count; j < count2; j++)
		{
			if (!assigned[j].TechHidden)
			{
				AddPartListItem(assigned[j], purchased: false);
			}
		}
	}

	public void AddPartListItem(AvailablePart part, bool purchased)
	{
		RDPartListItem componentInChildren = Object.Instantiate(partListItem).GetComponentInChildren<RDPartListItem>();
		string text = "";
		if (purchased)
		{
			text = Localizer.Format("#autoLOC_470883");
			SetPart(componentInChildren, unlocked: true, text, part, null);
		}
		else
		{
			if (Funding.Instance != null)
			{
				CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDPartPurchase, -part.entryCost, 0f, 0f);
				text = ((selected_node.tech.state != RDTech.State.Available) ? ("<color=" + XKCDColors.HexFormat.LightBlueGrey + ">" + currencyModifierQuery.GetCostLine(displayInverted: true, useCurrencyColors: false, useInsufficientCurrencyColors: true, includePercentage: true) + "</color>") : currencyModifierQuery.GetCostLine(displayInverted: true, useCurrencyColors: false, useInsufficientCurrencyColors: true, includePercentage: true));
			}
			else
			{
				text = "";
			}
			SetPart(componentInChildren, unlocked: false, text, part, null);
		}
		scrollList.AddItem(componentInChildren.GetComponentInParent<UIListItem>());
		partListItems.Add(componentInChildren);
	}

	public void SetPart(RDPartListItem item, bool unlocked, string label, AvailablePart part, PartUpgradeHandler.Upgrade upgrade)
	{
		item.Setup(label, unlocked ? "unlocked" : "purchaseable", part, upgrade);
	}

	public void OnPartListUpdate(Vector2 vector)
	{
		PartListTooltipController.SetupScreenSpaceMask(partTransformMask);
		int count = partListItems.Count;
		while (count-- > 0)
		{
			PartListTooltipController.SetScreenSpaceMaskMaterials(partListItems[count].materials);
		}
	}
}
