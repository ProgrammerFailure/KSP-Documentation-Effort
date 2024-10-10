using ns11;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns13;

public class InventoryPartListTooltipController : PinnableTooltipController
{
	public InventoryPartListTooltip tooltipPrefab;

	public AvailablePart partInfo;

	public InventoryPartListTooltip tooltipInstance;

	public EditorPartIcon editorPartIcon;

	public bool tooltipVisible;

	public float waitTimer;

	public new InventoryPartListTooltip TooltipPrefabInstance => TooltipPrefabInstance;

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!(editorPartIcon != null) || !editorPartIcon.isEmptySlot)
		{
			if (tooltipInstance == null && UIMasterController.Instance != null)
			{
				UIMasterController.Instance.SpawnTooltip(this);
				tooltipInstance.toolTipController = this;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(editorPartIcon.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
				tooltipVisible = !tooltipVisible;
				tooltipInstance.gameObject.SetActive(tooltipVisible);
				tooltipInstance.RefreshToolTipHeight(localPoint);
				base.OnPointerClick(eventData);
			}
			Canvas.ForceUpdateCanvases();
		}
	}

	public override void OnTooltipPinned()
	{
	}

	public override void OnTooltipUnpinned()
	{
	}

	public override bool OnTooltipAboutToSpawn()
	{
		return true;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if ((!(editorPartIcon != null) || !editorPartIcon.isEmptySlot) && !(UIPartActionControllerInventory.Instance.CurrentCargoPart != null))
		{
			base.OnPointerEnter(eventData);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (!(editorPartIcon != null) || !editorPartIcon.isEmptySlot)
		{
			if (tooltipVisible)
			{
				waitTimer = 0.08f;
			}
			base.OnPointerExit(eventData);
		}
	}

	public override void OnTooltipSpawned(Tooltip tooltip)
	{
		if (editorPartIcon == null)
		{
			editorPartIcon = GetComponent<EditorPartIcon>();
		}
		if (editorPartIcon != null)
		{
			CreateTooltip(tooltip as InventoryPartListTooltip, editorPartIcon);
		}
	}

	public override bool OnTooltipAboutToDespawn()
	{
		tooltipVisible = false;
		return true;
	}

	public override void OnTooltipDespawned(Tooltip instance)
	{
		DestroyTooltip();
		if (HighLogic.LoadedSceneIsEditor)
		{
			PartListTooltipMasterController.Instance.currentTooltip = null;
		}
	}

	public void Update()
	{
		if (waitTimer > 0f)
		{
			waitTimer -= Time.deltaTime;
		}
		else if (waitTimer <= 0f)
		{
			waitTimer = 0f;
			if (!tooltipVisible && pinned && !tooltipInstance.mouseOver)
			{
				UIMasterController.Instance.UnpinTooltip(this);
				pinned = false;
				base.OnPointerExit((PointerEventData)null);
			}
		}
	}

	public void PinToolTip()
	{
		if (!pinned)
		{
			UIMasterController.Instance.PinTooltip(this);
			pinned = true;
		}
	}

	public void CreateTooltip(InventoryPartListTooltip tooltip, EditorPartIcon partIcon)
	{
		tooltipInstance = tooltip;
		tooltipVisible = false;
		tooltipInstance.gameObject.SetActive(value: false);
		tooltipInstance.toolTipController = this;
		if (partIcon != null)
		{
			if (partIcon.isEmptySlot)
			{
				return;
			}
			partInfo = partIcon.partInfo;
			if (partIcon.isPart)
			{
				tooltip.Setup(partInfo, null);
			}
			else
			{
				tooltip.Setup(partInfo, partIcon.upgrade, null);
			}
		}
		Canvas.ForceUpdateCanvases();
		UIMasterController.RepositionTooltip((RectTransform)tooltip.transform, Vector2.one);
	}

	public void DestroyTooltip()
	{
		if (PartListTooltipMasterController.Instance != null && PartListTooltipMasterController.Instance.currentTooltip != null && PartListTooltipMasterController.Instance.currentTooltip.icon != null)
		{
			PartListTooltipMasterController.Instance.currentTooltip.icon.SetActive(value: false);
		}
	}
}
