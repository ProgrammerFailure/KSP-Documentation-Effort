using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class NodeListTooltipController : PinnableTooltipController
{
	public NodeListTooltip tooltipPrefab;

	public MEBasicNode basicNode;

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (NodeListTooltipMasterController.Instance.currentTooltip == null || eventData.button != PointerEventData.InputButton.Right || !(UIMasterController.Instance.CurrentTooltip is IPinnableTooltipController pinnableTooltipController) || !((NodeListTooltipController)pinnableTooltipController == this))
		{
			return;
		}
		if (!pinned)
		{
			if (!NodeListTooltipMasterController.Instance.displayExtendedInfo)
			{
				NodeListTooltipMasterController.Instance.displayExtendedInfo = true;
				NodeListTooltipMasterController.Instance.currentTooltip.DisplayExtendedInfo(display: true, GetTooltipHintText(NodeListTooltipMasterController.Instance.currentTooltip));
			}
			base.OnPointerClick(eventData);
		}
		else if (NodeListTooltipMasterController.Instance.displayExtendedInfo)
		{
			NodeListTooltipMasterController.Instance.displayExtendedInfo = false;
			NodeListTooltipMasterController.Instance.currentTooltip.DisplayExtendedInfo(display: false, GetTooltipHintText(NodeListTooltipMasterController.Instance.currentTooltip));
			base.OnPointerClick(eventData);
		}
		Canvas.ForceUpdateCanvases();
		UIMasterController.RepositionTooltip(NodeListTooltipMasterController.Instance.currentTooltip.RectTransform, Vector2.one);
	}

	public override void OnTooltipSpawned(Tooltip tooltip)
	{
		NodeListTooltipMasterController.Instance.currentTooltip = tooltip as NodeListTooltip;
		MEGUINodeIcon component = GetComponent<MEGUINodeIcon>();
		if (component != null)
		{
			CreateTooltip(tooltip as NodeListTooltip, component);
		}
	}

	public override void OnTooltipDespawned(Tooltip instance)
	{
		NodeListTooltipMasterController.Instance.currentTooltip = null;
	}

	public void CreateTooltip(NodeListTooltip tooltip, MEGUINodeIcon nodeIcon)
	{
		if (nodeIcon != null && nodeIcon.basicNode != null)
		{
			basicNode = nodeIcon.basicNode;
			tooltip.Setup(basicNode, nodeIcon.nodeImage);
		}
		tooltip.DisplayExtendedInfo(NodeListTooltipMasterController.Instance.displayExtendedInfo, GetTooltipHintText(tooltip));
		Canvas.ForceUpdateCanvases();
		UIMasterController.RepositionTooltip((RectTransform)tooltip.transform, Vector2.one);
	}

	public string GetTooltipHintText(NodeListTooltip tooltip)
	{
		if (tooltip.HasExtendedInfo)
		{
			if (!NodeListTooltipMasterController.Instance.displayExtendedInfo)
			{
				return Localizer.Format("#autoLOC_456638");
			}
			if (!pinned)
			{
				return Localizer.Format("#autoLOC_456642");
			}
			return "[RMB]: Less Info";
		}
		return "<color=orange>" + Localizer.Format("#autoLOC_456651") + "</color>";
	}
}
