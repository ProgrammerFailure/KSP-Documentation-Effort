using System.Collections;
using System.Collections.Generic;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns13;

public class InventoryPartListTooltip : Tooltip, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler
{
	public TextMeshProUGUI textName;

	public PartListTooltipWidget extInfoModuleWidgetPrefab;

	public PartListTooltipWidget extInfoRscWidgePrefab;

	public List<PartListTooltipWidget> extInfoModules = new List<PartListTooltipWidget>();

	public List<PartListTooltipWidget> extInfoRscs = new List<PartListTooltipWidget>();

	public RectTransform extInfoListContainer;

	public RectTransform extInfoListSpacer;

	public InventoryPartListTooltipController toolTipController;

	public bool mouseOver;

	public StoredPart inventoryStoredPart;

	public AvailablePart partInfo;

	public PartUpgradeHandler.Upgrade upgrade;

	public Part partRef;

	public PartModule.PartUpgradeState upgradeState;

	public RectTransform prefabTransform;

	public WaitForEndOfFrame wfeof;

	public bool isGrey { get; set; }

	public void Update()
	{
		if (!isGrey)
		{
			PartListTooltipController.SetupScreenSpaceMask((RectTransform)UIMasterController.Instance.tooltipCanvas.transform);
		}
	}

	public void OnDestroy()
	{
		for (int i = 0; i < extInfoModules.Count; i++)
		{
			Object.Destroy(extInfoModules[i].gameObject);
		}
		for (int j = 0; j < extInfoRscs.Count; j++)
		{
			Object.Destroy(extInfoRscs[j].gameObject);
		}
	}

	public void Setup(AvailablePart availablePart, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		HidePreviousTooltipWidgets();
		partInfo = availablePart;
		partRef = partInfo.partPrefab;
		textName.text = partInfo.title;
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered != null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered.inventoryPartActionUI.inventoryPartModule.storedParts.TryGetValue(UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered.slotIndex, out inventoryStoredPart);
		}
		prefabTransform = GetComponent<RectTransform>();
		wfeof = new WaitForEndOfFrame();
		CreateInfoWidgets();
	}

	public void Setup(AvailablePart availablePart, PartUpgradeHandler.Upgrade up, Callback<PartListTooltip> onPurchase, RenderTexture texture = null)
	{
		HidePreviousTooltipWidgets();
		partInfo = availablePart;
		partRef = partInfo.partPrefab;
		upgrade = up;
		textName.text = upgrade.title;
		if (UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered != null)
		{
			UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered.inventoryPartActionUI.inventoryPartModule.storedParts.TryGetValue(UIPartActionControllerInventory.Instance.CurrentInventorySlotHovered.slotIndex, out inventoryStoredPart);
		}
		CreateInfoWidgets();
	}

	public void RefreshToolTipHeight(Vector2 localCursor)
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(WaitAndRefreshHeight(localCursor));
		}
	}

	public IEnumerator WaitAndRefreshHeight(Vector2 localCursor)
	{
		yield return wfeof;
		float num = 50f;
		int childCount = extInfoListContainer.childCount;
		while (childCount-- > 0)
		{
			RectTransform component = extInfoListContainer.GetChild(childCount).GetComponent<RectTransform>();
			if (component.gameObject.activeInHierarchy)
			{
				num += component.rect.height * 1.1f;
			}
		}
		num = Mathf.Clamp(num, 50f, 350f);
		prefabTransform.sizeDelta = new Vector2(prefabTransform.rect.width, num);
		localCursor.x = (70f - (35f + localCursor.x)) * GameSettings.UI_SCALE;
		localCursor.y = num * -0.5f;
		UIMasterController.RepositionTooltip(prefabTransform, localCursor);
		UIMasterController.ClampToScreen(prefabTransform, Vector2.one * 5f * GameSettings.UI_SCALE);
	}

	public void CreateInfoWidgets()
	{
		ProtoPartSnapshot protoPartSnapshot = ((inventoryStoredPart == null) ? null : inventoryStoredPart.snapshot);
		if (protoPartSnapshot != null)
		{
			partInfo = protoPartSnapshot.partInfo;
		}
		int i = 0;
		for (int count = partInfo.moduleInfos.Count; i < count; i++)
		{
			AvailablePart.ModuleInfo moduleInfo = partInfo.moduleInfos[i];
			if (!moduleInfo.moduleName.ToLowerInvariant().Contains("variant"))
			{
				PartListTooltipWidget newTooltipWidget = GetNewTooltipWidget(extInfoModuleWidgetPrefab);
				newTooltipWidget.Setup(moduleInfo.moduleDisplayName, moduleInfo.info);
				newTooltipWidget.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
			}
		}
		int j = 0;
		for (int count2 = protoPartSnapshot.resources.Count; j < count2; j++)
		{
			AvailablePart.ResourceInfo currentResourceInfo = protoPartSnapshot.resources[j].GetCurrentResourceInfo();
			PartListTooltipWidget newTooltipWidget2 = GetNewTooltipWidget(extInfoRscWidgePrefab);
			newTooltipWidget2.Setup(currentResourceInfo.displayName.LocalizeRemoveGender(), currentResourceInfo.info);
			newTooltipWidget2.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
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
	}

	public PartListTooltipWidget GetNewTooltipWidget(PartListTooltipWidget prefab)
	{
		List<PartListTooltipWidget> list = null;
		if (prefab == extInfoModuleWidgetPrefab)
		{
			list = extInfoModules;
		}
		else
		{
			if (!(prefab == extInfoRscWidgePrefab))
			{
				return null;
			}
			list = extInfoRscs;
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
			PartListTooltipWidget partListTooltipWidget = Object.Instantiate(prefab);
			list.Add(partListTooltipWidget);
			return partListTooltipWidget;
		}
		list[num].gameObject.SetActive(value: true);
		return list[num];
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		toolTipController.Unpin();
		toolTipController.OnPointerExit(eventData);
		mouseOver = false;
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		mouseOver = true;
	}
}
