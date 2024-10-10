using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_ListOrder]
public class MEGUIParameterListOrder : MEGUIParameter
{
	public Button buttonExpand;

	public GameObject arrowExpand;

	public GameObject scrollViewParent;

	public RectTransform container;

	public MEGUIListOrderItem prefabListElement;

	public List<MEGUIListOrderItem> listElements;

	public bool isInitialized;

	public IList FieldValue
	{
		get
		{
			return field.GetValue() as IList;
		}
		set
		{
			field.SetValue(value);
		}
	}

	public override void OnEnable()
	{
		if (isInitialized)
		{
			FillList();
		}
	}

	public void OnDisable()
	{
		if (scrollViewParent.activeSelf)
		{
			ToggleExpand();
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		listElements = new List<MEGUIListOrderItem>();
		buttonExpand.onClick.AddListener(ToggleExpand);
		if (field.FieldType.Name.Equals("List`1"))
		{
			FillList();
		}
		arrowExpand.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		scrollViewParent.SetActive(value: false);
		isInitialized = true;
	}

	public string GetTitle(object item, string fieldTitle)
	{
		if (string.IsNullOrEmpty(fieldTitle))
		{
			return item.ToString();
		}
		PropertyInfo property = item.GetType().GetProperty(fieldTitle, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (property != null)
		{
			return property.GetValue(item, null) as string;
		}
		return "";
	}

	public void FillList()
	{
		CleanList();
		IList fieldValue = FieldValue;
		int i = 0;
		for (int count = fieldValue.Count; i < count; i++)
		{
			if (i < listElements.Count)
			{
				MEGUIListOrderItem mEGUIListOrderItem = listElements[i];
				mEGUIListOrderItem.title.text = GetTitle(fieldValue[i], ((MEGUI_ListOrder)field.Attribute).nameField);
				mEGUIListOrderItem.upButton.interactable = i > 0;
				mEGUIListOrderItem.downButton.interactable = i + 1 < fieldValue.Count;
				mEGUIListOrderItem.transform.SetAsLastSibling();
				mEGUIListOrderItem.gameObject.SetActive(value: true);
				continue;
			}
			MEGUIListOrderItem listElement = Object.Instantiate(prefabListElement, container);
			listElement.upButton.onClick.AddListener(delegate
			{
				UpButton(listElement);
			});
			listElement.downButton.onClick.AddListener(delegate
			{
				DownButton(listElement);
			});
			listElement.title.text = GetTitle(fieldValue[i], ((MEGUI_ListOrder)field.Attribute).nameField);
			listElement.upButton.interactable = i > 0;
			listElement.downButton.interactable = i + 1 < fieldValue.Count;
			listElements.Add(listElement);
		}
	}

	public void CleanList()
	{
		int i = 0;
		for (int count = listElements.Count; i < count; i++)
		{
			listElements[i].gameObject.SetActive(value: false);
		}
	}

	public void UpButton(MEGUIListOrderItem item)
	{
		int siblingIndex = item.transform.GetSiblingIndex();
		object value = FieldValue[siblingIndex];
		item.transform.SetSiblingIndex(siblingIndex - 1);
		FieldValue.RemoveAt(siblingIndex);
		FieldValue.Insert(siblingIndex - 1, value);
		item.upButton.interactable = siblingIndex - 1 > 0;
		item.downButton.interactable = true;
		MEGUIListOrderItem component = container.GetChild(siblingIndex).GetComponent<MEGUIListOrderItem>();
		component.upButton.interactable = siblingIndex > 0;
		component.downButton.interactable = siblingIndex + 1 < FieldValue.Count;
	}

	public void DownButton(MEGUIListOrderItem item)
	{
		int siblingIndex = item.transform.GetSiblingIndex();
		object value = FieldValue[siblingIndex];
		item.transform.SetSiblingIndex(siblingIndex + 1);
		FieldValue.RemoveAt(siblingIndex);
		item.upButton.interactable = true;
		if (siblingIndex + 1 == FieldValue.Count)
		{
			FieldValue.Add(value);
			item.downButton.interactable = false;
		}
		else
		{
			FieldValue.Insert(siblingIndex + 1, value);
		}
		MEGUIListOrderItem component = container.GetChild(siblingIndex).GetComponent<MEGUIListOrderItem>();
		component.upButton.interactable = siblingIndex > 0;
		component.downButton.interactable = siblingIndex + 1 < FieldValue.Count;
	}

	public void ToggleExpand()
	{
		bool flag = !scrollViewParent.activeSelf;
		scrollViewParent.SetActive(flag);
		arrowExpand.transform.eulerAngles = (flag ? new Vector3(0f, 0f, 180f) : Vector3.zero);
	}
}
