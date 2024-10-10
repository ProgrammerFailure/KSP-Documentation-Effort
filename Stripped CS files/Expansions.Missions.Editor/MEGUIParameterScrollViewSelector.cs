using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_ScrollViewSelector]
public class MEGUIParameterScrollViewSelector : MEGUIParameter
{
	public delegate void SelectedElementEvent(int elementIndex);

	public delegate void ExpandEvent(bool isDisplayed);

	public Button buttonExpand;

	public GameObject arrowExpand;

	public GameObject scrollViewParent;

	public RectTransform container;

	public GameObject prefabListElement;

	public List<GameObject> listElements;

	public List<string> listValues;

	public List<SelectedElementEvent> selectedElementEventList;

	public List<ExpandEvent> expandEventList;

	public int FieldValue
	{
		get
		{
			return field.GetValue<int>(field.host);
		}
		set
		{
			field.SetValue(value);
			title.text = listValues[value];
		}
	}

	public MEGUIParameterScrollViewSelector()
	{
		selectedElementEventList = new List<SelectedElementEvent>();
		expandEventList = new List<ExpandEvent>();
	}

	public override void Setup(string name)
	{
		title.text = name;
		listElements = new List<GameObject>();
		buttonExpand.onClick.AddListener(ToggleExpand);
		if (field.FieldType.IsEnum)
		{
			List<string> list = new List<string>();
			foreach (Enum value in Enum.GetValues(field.FieldType))
			{
				list.Add(value.displayDescription());
			}
			FillList(list);
		}
		else
		{
			listValues = new List<string>();
		}
		arrowExpand.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		scrollViewParent.SetActive(value: false);
	}

	public void ToggleExpand()
	{
		bool flag = !scrollViewParent.activeSelf;
		scrollViewParent.SetActive(flag);
		arrowExpand.transform.eulerAngles = (flag ? new Vector3(0f, 0f, 180f) : Vector3.zero);
		for (int i = 0; i < expandEventList.Count; i++)
		{
			expandEventList[i](flag);
		}
	}

	public void FillList(List<string> newValues)
	{
		listValues = newValues;
		for (int i = 0; i < listValues.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(prefabListElement, container);
			gameObject.transform.localPosition = new Vector3(0f, 1f * (float)i, 0f);
			int buttonIndex = i;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				ButtonCallback(buttonIndex);
			});
			gameObject.GetComponentInChildren<TMP_Text>().text = listValues[i];
			listElements.Add(gameObject);
		}
		if (listElements.Count > 0)
		{
			float num = (listElements[0].transform as RectTransform).sizeDelta.y + 2.5f;
			container.sizeDelta = new Vector2(container.sizeDelta.x, (float)listElements.Count * num);
		}
		int fieldValue = FieldValue;
		if (fieldValue != -1 && listValues.Count > 0)
		{
			title.text = listValues[fieldValue];
		}
	}

	public void ButtonCallback(int buttonIndex)
	{
		FieldValue = buttonIndex;
		for (int i = 0; i < selectedElementEventList.Count; i++)
		{
			selectedElementEventList[i](buttonIndex);
		}
	}

	public void OnSelectedElement(SelectedElementEvent call)
	{
		selectedElementEventList.Add(call);
	}

	public void OnExpandView(ExpandEvent call)
	{
		expandEventList.Add(call);
	}

	public bool IsExpanded()
	{
		return scrollViewParent.activeSelf;
	}
}
