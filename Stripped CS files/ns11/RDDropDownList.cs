using System;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns11;

public class RDDropDownList : MonoBehaviour
{
	public RDDropDownList listPrefab;

	public RDDropDownListItem listItemPrefab;

	public int headerHeight = 24;

	public UIList scrollList;

	public TextMeshProUGUI nameField;

	[NonSerialized]
	public new string name;

	[NonSerialized]
	public bool open;

	[NonSerialized]
	public string sortingBy = "";

	[NonSerialized]
	public UIListItem selectedItem;

	public OnOpen callbackOpenClose;

	public OnSelectItem callbackSelectItem;

	public bool over;

	public int radioGroup;

	public void Close(bool force = false, bool callback = true)
	{
		CloseInternal(force, callback);
	}

	public void CloseInternal(bool force = false, bool callback = true)
	{
		if (open || force)
		{
			open = false;
			scrollList.Clear(destroyElements: true);
			sortingBy = "";
			selectedItem = null;
			if (callback)
			{
				callbackOpenClose(this, opening: false);
			}
		}
	}

	public void Open(bool force = false, bool callback = true)
	{
		OpenInternal(force, callback);
	}

	public void OpenInternal(bool force = false, bool callback = true)
	{
		if (!open || force)
		{
			open = true;
			if (callback)
			{
				callbackOpenClose(this, opening: true);
			}
		}
	}

	public void SetStandardToggleButtonInputDelegate()
	{
	}

	public RDDropDownList CreateInstanceFromPrefab(string name, string displayName, OnOpen listCallback, OnSelectItem itemCallback, int radioGroup)
	{
		RDDropDownList rDDropDownList = UnityEngine.Object.Instantiate(listPrefab);
		rDDropDownList.gameObject.SetActive(value: true);
		rDDropDownList.radioGroup = radioGroup;
		rDDropDownList.SetStandardToggleButtonInputDelegate();
		rDDropDownList.name = name;
		rDDropDownList.nameField.text = displayName;
		rDDropDownList.callbackOpenClose = listCallback;
		rDDropDownList.callbackSelectItem = itemCallback;
		return rDDropDownList;
	}

	public void ClearList(bool removeFiltering = true)
	{
		scrollList.Clear(destroyElements: true);
		if (removeFiltering)
		{
			sortingBy = "";
			selectedItem = null;
		}
	}

	public UIListItem AddItem(string id, string header, string description)
	{
		RDDropDownListItem rDDropDownListItem = UnityEngine.Object.Instantiate(listItemPrefab);
		rDDropDownListItem.radioButton.Data = id;
		rDDropDownListItem.radioButton.SetGroup(radioGroup);
		rDDropDownListItem.radioButton.onTrue.AddListener(OnTrue);
		rDDropDownListItem.radioButton.onFalse.AddListener(OnFalse);
		rDDropDownListItem.Setup(header, description);
		UIListItem component = rDDropDownListItem.GetComponent<UIListItem>();
		scrollList.AddItem(component);
		return component;
	}

	public void UpdateSelectedItem(string description, bool append = false)
	{
		if (append)
		{
			selectedItem.GetComponent<RDDropDownListItem>().description.text += description;
		}
		else
		{
			selectedItem.GetComponent<RDDropDownListItem>().description.text = description;
		}
	}

	public void UpdateItem(int index, string description)
	{
		scrollList.GetUilistItemAt(index).GetComponent<RDDropDownListItem>().description.text = description;
	}

	public void AddSelectedItem(string id, string header, string description)
	{
		AddItem(id, header, description).GetComponent<RDDropDownListItem>().radioButton.Value = true;
	}

	public void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		if (callType == UIRadioButton.CallType.USER)
		{
			UIRadioButton component = data.pointerPress.GetComponent<UIRadioButton>();
			sortingBy = component.Data as string;
			selectedItem = component.GetComponent<UIListItem>();
			callbackSelectItem(this, selected: true);
		}
	}

	public void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		if (callType == UIRadioButton.CallType.USER)
		{
			sortingBy = "";
			selectedItem = null;
			callbackSelectItem(this, selected: false);
		}
	}

	public Func<RDArchivesController.ReportData, bool> GetFilter()
	{
		if (sortingBy != string.Empty)
		{
			return (RDArchivesController.ReportData a) => a.id.Contains(sortingBy);
		}
		return (RDArchivesController.ReportData a) => true;
	}

	public Func<RDArchivesController.ReportData, bool> GetBiomeFilter()
	{
		if (sortingBy != string.Empty)
		{
			return (RDArchivesController.ReportData a) => a.biome == sortingBy;
		}
		return (RDArchivesController.ReportData a) => true;
	}
}
