using System.Collections.Generic;
using ns13;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class NodeListTooltip : Tooltip
{
	public TextMeshProUGUI textName;

	public RawImage imageNode;

	public TextMeshProUGUI textInfoBasic;

	public TextMeshProUGUI textDescription;

	public TextMeshProUGUI textRMBHint;

	public PartListTooltipWidget extInfoWidgetPrefab;

	public List<PartListTooltipWidget> extInfoWidgets = new List<PartListTooltipWidget>();

	public Sprite extInfoTestModuleSprite;

	public Sprite extInfoActionModuleSprite;

	public GameObject panelExtended;

	public RectTransform extInfoListContainer;

	public RectTransform extInfoListSpacerPrefab;

	public List<RectTransform> extInfoSpacers = new List<RectTransform>();

	public MEBasicNode basicNode;

	public bool hasExtendedInfo;

	public bool hasCreatedExtendedInfo;

	public bool HasExtendedInfo => hasExtendedInfo;

	public void Setup(MEBasicNode basicNode, RawImage image)
	{
		HidePreviousTooltipWidgets();
		this.basicNode = basicNode;
		textName.text = basicNode.displayName;
		textInfoBasic.text = basicNode.categoryShortDescription;
		textDescription.text = basicNode.tooltipDescription;
		if (image != null)
		{
			imageNode.texture = image.texture;
		}
		hasExtendedInfo = basicNode.extInfoActionModules.Count > 0 || basicNode.extInfoTestModules.Count > 0;
		hasCreatedExtendedInfo = false;
		panelExtended.SetActive(value: false);
	}

	public void CreateExtendedInfo()
	{
		int i = 0;
		for (int count = basicNode.extInfoTestModules.Count; i < count; i++)
		{
			CreateExtInfoElement(basicNode.extInfoTestModules[i], testModule: true);
		}
		int j = 0;
		for (int count2 = basicNode.extInfoActionModules.Count; j < count2; j++)
		{
			CreateExtInfoElement(basicNode.extInfoActionModules[j], testModule: false);
		}
	}

	public void CreateExtInfoElement(MEBasicNode.ExtendedInfo mInfo, bool testModule)
	{
		PartListTooltipWidget newTooltipWidget = GetNewTooltipWidget();
		if (testModule)
		{
			newTooltipWidget.widgetColourImage.sprite = extInfoTestModuleSprite;
		}
		else
		{
			newTooltipWidget.widgetColourImage.sprite = extInfoActionModuleSprite;
		}
		newTooltipWidget.Setup(mInfo.name, mInfo.information);
		newTooltipWidget.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		int i = 0;
		for (int count = mInfo.scoringInformation.Count; i < count; i++)
		{
			MEBasicNode.ExtendedInfo extendedInfo = mInfo.scoringInformation[i];
			PartListTooltipWidget newTooltipWidget2 = GetNewTooltipWidget();
			newTooltipWidget2.Setup(extendedInfo.name, extendedInfo.information);
			newTooltipWidget2.transform.SetParent(extInfoListContainer.transform, worldPositionStays: false);
		}
		GetNewTooltipSpacer().SetParent(extInfoListContainer.transform, worldPositionStays: false);
	}

	public void DisplayExtendedInfo(bool display, string rmbHintText)
	{
		if (HasExtendedInfo)
		{
			if (display && !hasCreatedExtendedInfo)
			{
				CreateExtendedInfo();
				hasCreatedExtendedInfo = true;
			}
			panelExtended.SetActive(display && extInfoListContainer.childCount > 1);
		}
		textRMBHint.text = rmbHintText;
	}

	public void HidePreviousTooltipWidgets()
	{
		for (int i = 0; i < extInfoWidgets.Count; i++)
		{
			if (extInfoWidgets[i].gameObject.activeSelf)
			{
				extInfoWidgets[i].gameObject.SetActive(value: false);
			}
		}
		for (int j = 0; j < extInfoSpacers.Count; j++)
		{
			if (extInfoSpacers[j].gameObject.activeSelf)
			{
				extInfoSpacers[j].gameObject.SetActive(value: false);
			}
		}
	}

	public PartListTooltipWidget GetNewTooltipWidget()
	{
		int num = 0;
		while (true)
		{
			if (num < extInfoWidgets.Count)
			{
				if (!extInfoWidgets[num].gameObject.activeSelf)
				{
					break;
				}
				num++;
				continue;
			}
			PartListTooltipWidget partListTooltipWidget = Object.Instantiate(extInfoWidgetPrefab);
			extInfoWidgets.Add(partListTooltipWidget);
			return partListTooltipWidget;
		}
		extInfoWidgets[num].gameObject.SetActive(value: true);
		return extInfoWidgets[num];
	}

	public RectTransform GetNewTooltipSpacer()
	{
		int num = 0;
		while (true)
		{
			if (num < extInfoSpacers.Count)
			{
				if (!extInfoSpacers[num].gameObject.activeSelf)
				{
					break;
				}
				num++;
				continue;
			}
			RectTransform rectTransform = Object.Instantiate(extInfoListSpacerPrefab);
			extInfoSpacers.Add(rectTransform);
			return rectTransform;
		}
		extInfoSpacers[num].gameObject.SetActive(value: true);
		return extInfoSpacers[num];
	}

	public void OnDestroy()
	{
		for (int i = 0; i < extInfoWidgets.Count; i++)
		{
			Object.Destroy(extInfoWidgets[i].gameObject);
		}
		for (int j = 0; j < extInfoSpacers.Count; j++)
		{
			Object.Destroy(extInfoSpacers[j].gameObject);
		}
	}
}
