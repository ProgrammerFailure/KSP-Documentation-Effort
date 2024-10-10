using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_Category]
public class MEGUIParameterGroup : MEGUIParameter
{
	public MEGUIParameterGroupHeader header;

	public VerticalLayoutGroup mainLayout;

	public RectTransform containerChilden;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	[SerializeField]
	public List<Color> headerColors;

	public Image headerImage;

	public BaseAPFieldList groupFields;

	public int _offset;

	public int _depth;

	public BaseAPField Field => field;

	public BaseAPFieldList GroupFields => groupFields;

	public int Offset
	{
		set
		{
			_offset = value;
			mainLayout.padding.left = _offset;
		}
	}

	public int Depth
	{
		get
		{
			return _depth;
		}
		set
		{
			_depth = value;
			if (headerColors != null && headerColors.Count > 0)
			{
				headerImage.color = headerColors[value % headerColors.Count];
			}
		}
	}

	public MEGUIParameter Create(BaseAPFieldList categoryFields, Transform parent, string name)
	{
		MEGUIParameterGroup obj = Create((BaseAPField)null, parent, name) as MEGUIParameterGroup;
		obj.groupFields = categoryFields;
		obj.headerImage = obj.header.GetComponent<Image>();
		return obj;
	}

	public MEGUIParameter Create(Transform parent, string name)
	{
		return Create((BaseAPField)null, parent, name);
	}

	public override void Setup(string name)
	{
		title.text = name;
		buttonCollapse.onClick.AddListener(OnButtonCollapsePressed);
		if (resetButton != null)
		{
			resetButton.onClick.AddListener(OnParameterGroupReset);
		}
		else
		{
			resetButton.gameObject.SetActive(value: false);
		}
	}

	public override void Display()
	{
		SortChilden();
	}

	public static int SortParametersByOrder(MEGUIParameter p1, MEGUIParameter p2)
	{
		return p1.Order.CompareTo(p2.Order);
	}

	public void CollapseGroup()
	{
		containerChilden.gameObject.SetActive(value: false);
		((Image)buttonCollapse.targetGraphic).sprite = spriteCollapseOn;
	}

	public void OnButtonCollapsePressed()
	{
		containerChilden.gameObject.SetActive(!containerChilden.gameObject.activeSelf);
		((Image)buttonCollapse.targetGraphic).sprite = (containerChilden.gameObject.activeSelf ? spriteCollapseOff : spriteCollapseOn);
	}

	public void SortChilden()
	{
		List<MEGUIParameter> list = new List<MEGUIParameter>();
		bool flag = false;
		for (int i = 0; i < containerChilden.childCount; i++)
		{
			MEGUIParameter component = containerChilden.GetChild(i).GetComponent<MEGUIParameter>();
			if (component != null && component.Order != -1)
			{
				list.Add(component);
			}
			flag = flag || component.gameObject.activeSelf;
		}
		header.gameObject.SetActive(flag);
		if (flag)
		{
			list.Sort(SortParametersByOrder);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].transform.SetSiblingIndex(j + 1);
			}
			if (list.Count > 0)
			{
				base.Order = list[0].Order;
			}
		}
	}

	public void OnParameterGroupReset()
	{
		if (groupFields == null)
		{
			return;
		}
		int i = 0;
		for (int count = groupFields.Count; i < count; i++)
		{
			MEGUIParameter parameterFromFieldID = MissionEditorLogic.Instance.actionPane.GetParameterFromFieldID(groupFields[i].FieldID);
			if (parameterFromFieldID != null)
			{
				parameterFromFieldID.OnParameterReset();
			}
		}
	}
}
