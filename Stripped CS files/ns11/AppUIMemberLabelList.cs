using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

[AppUI_LabelList]
public class AppUIMemberLabelList : AppUIMember
{
	public string newGuiName;

	public TextMeshProUGUI value1;

	public TextMeshProUGUI value2;

	public float preferredHeight;

	[SerializeField]
	public GameObject separator1;

	[SerializeField]
	public GameObject separator2;

	public LayoutElement layout;

	public AppUI_Control.HorizontalAlignment horizontalAlignment = AppUI_Control.HorizontalAlignment.Middle;

	public AppUI_Control.VerticalAlignment verticalAlignment;

	public bool showSeparator;

	public override void OnInitialized()
	{
		base.OnInitialized();
		if (_attribs is AppUI_LabelList appUI_LabelList)
		{
			verticalAlignment = appUI_LabelList.textVerticalAlignment;
			showSeparator = appUI_LabelList.showSeparator;
			preferredHeight = appUI_LabelList.preferredHeight;
		}
		layout = GetComponent<LayoutElement>();
	}

	public override void OnRefreshUI()
	{
		if (preferredHeight > 0f && layout != null)
		{
			layout.preferredHeight = preferredHeight;
			layout.minHeight = preferredHeight;
		}
		TransferDataSimpleTopData.APPUIMemberLabelListItem aPPUIMemberLabelListItem = (TransferDataSimpleTopData.APPUIMemberLabelListItem)GetValue();
		newGuiName = aPPUIMemberLabelListItem.column1;
		value1.text = aPPUIMemberLabelListItem.column2;
		value2.text = aPPUIMemberLabelListItem.column3;
		switch (horizontalAlignment)
		{
		case AppUI_Control.HorizontalAlignment.Left:
			switch (verticalAlignment)
			{
			case AppUI_Control.VerticalAlignment.Top:
				value1.alignment = TextAlignmentOptions.TopLeft;
				value2.alignment = TextAlignmentOptions.TopLeft;
				break;
			case AppUI_Control.VerticalAlignment.Bottom:
				value1.alignment = TextAlignmentOptions.BottomLeft;
				value2.alignment = TextAlignmentOptions.BottomLeft;
				break;
			case AppUI_Control.VerticalAlignment.Midline:
				value1.alignment = TextAlignmentOptions.MidlineLeft;
				value2.alignment = TextAlignmentOptions.MidlineLeft;
				break;
			case AppUI_Control.VerticalAlignment.Capline:
				value1.alignment = TextAlignmentOptions.CaplineLeft;
				value2.alignment = TextAlignmentOptions.CaplineLeft;
				break;
			}
			break;
		case AppUI_Control.HorizontalAlignment.Middle:
			switch (verticalAlignment)
			{
			case AppUI_Control.VerticalAlignment.Top:
				value1.alignment = TextAlignmentOptions.Top;
				value2.alignment = TextAlignmentOptions.Top;
				break;
			case AppUI_Control.VerticalAlignment.Bottom:
				value1.alignment = TextAlignmentOptions.Bottom;
				value2.alignment = TextAlignmentOptions.Bottom;
				break;
			case AppUI_Control.VerticalAlignment.Midline:
				value1.alignment = TextAlignmentOptions.Midline;
				value2.alignment = TextAlignmentOptions.Midline;
				break;
			case AppUI_Control.VerticalAlignment.Capline:
				value1.alignment = TextAlignmentOptions.Capline;
				value2.alignment = TextAlignmentOptions.Capline;
				break;
			}
			break;
		case AppUI_Control.HorizontalAlignment.Right:
			switch (verticalAlignment)
			{
			case AppUI_Control.VerticalAlignment.Top:
				value1.alignment = TextAlignmentOptions.TopRight;
				value2.alignment = TextAlignmentOptions.TopRight;
				break;
			case AppUI_Control.VerticalAlignment.Bottom:
				value1.alignment = TextAlignmentOptions.BottomRight;
				value2.alignment = TextAlignmentOptions.BottomRight;
				break;
			case AppUI_Control.VerticalAlignment.Midline:
				value1.alignment = TextAlignmentOptions.MidlineRight;
				value2.alignment = TextAlignmentOptions.MidlineRight;
				break;
			case AppUI_Control.VerticalAlignment.Capline:
				value1.alignment = TextAlignmentOptions.CaplineRight;
				value2.alignment = TextAlignmentOptions.CaplineRight;
				break;
			}
			break;
		}
		if (separator1 != null)
		{
			separator1.gameObject.SetActive(showSeparator);
		}
		if (separator2 != null)
		{
			separator2.gameObject.SetActive(showSeparator);
		}
	}
}
