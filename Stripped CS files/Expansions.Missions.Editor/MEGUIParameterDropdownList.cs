using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_Dropdown]
public class MEGUIParameterDropdownList : MEGUIParameter
{
	public delegate bool ValueComparer(object value, object listValue);

	public TMP_Dropdown dropdownList;

	public ValueComparer customValueComparer;

	public GAPPrefabDisplay prefabDisplay;

	public GameObject prefabToDisplay;

	public bool isInitialized;

	public List<string> optionsToExclude;

	public string selectedKey = "";

	public override bool IsInteractable
	{
		get
		{
			return dropdownList.interactable;
		}
		set
		{
			dropdownList.interactable = value;
		}
	}

	public MethodInfo OnDropDownValueChanged { get; set; }

	public MethodInfo OnGAPPrefabInstantiated { get; set; }

	public MethodInfo SetDropDownItems { get; set; }

	public MethodInfo SetGAPPrefab { get; set; }

	public int FieldValue
	{
		get
		{
			int num = GetItemIndex(field.GetValue());
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
		set
		{
			if (value < Items.Count)
			{
				MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
				selectedKey = Items.KeyAt(value);
				field.SetValue(Items[selectedKey].value);
			}
		}
	}

	public string SelectedKey => selectedKey;

	public DictionaryValueList<string, MEGUIDropDownItem> Items { get; set; }

	public object SelectedValue
	{
		get
		{
			if (dropdownList.value >= Items.Count)
			{
				return null;
			}
			return Items[Items.KeyAt(dropdownList.value)].value;
		}
	}

	public void SetExcludedOptions(List<string> optionNames)
	{
		optionsToExclude = optionNames;
		RebuildDropDown();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (isInitialized)
		{
			RefreshUI();
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		gapDisplayPartner = (RectTransform)dropdownList.gameObject.transform;
		if (!string.IsNullOrEmpty(((MEGUI_Dropdown)field.Attribute).onDropDownValueChange))
		{
			OnDropDownValueChanged = field.host.GetType().GetMethod(((MEGUI_Dropdown)field.Attribute).onDropDownValueChange, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		if (!string.IsNullOrEmpty(((MEGUI_Dropdown)field.Attribute).SetDropDownItems))
		{
			SetDropDownItems = field.host.GetType().GetMethod(((MEGUI_Dropdown)field.Attribute).SetDropDownItems, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		if (!string.IsNullOrEmpty(((MEGUI_Dropdown)field.Attribute).SetGAPPrefab))
		{
			SetGAPPrefab = field.host.GetType().GetMethod(((MEGUI_Dropdown)field.Attribute).SetGAPPrefab, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		if (!string.IsNullOrEmpty(((MEGUI_Dropdown)field.Attribute).onGAPPrefabInstantiated))
		{
			OnGAPPrefabInstantiated = field.host.GetType().GetMethod(((MEGUI_Dropdown)field.Attribute).onGAPPrefabInstantiated, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		BuildDropDownItemsList();
		dropdownList.value = FieldValue;
		dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
		isInitialized = true;
	}

	public void RebuildDropDown()
	{
		BuildDropDownItemsList();
		RefreshUI();
	}

	public virtual void BuildDropDownItemsList()
	{
		int itemIndex = GetItemIndex(field.GetValue());
		Items = new DictionaryValueList<string, MEGUIDropDownItem>();
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		if (field.FieldType.BaseType == typeof(Enum) && SetDropDownItems == null)
		{
			IEnumerator enumerator = Enum.GetValues(field.FieldType).GetEnumerator();
			while (enumerator.MoveNext())
			{
				Enum @enum = (Enum)enumerator.Current;
				list.Add(new MEGUIDropDownItem(@enum.ToString(), @enum, @enum.displayDescription()));
			}
		}
		else
		{
			if (((MEGUI_Dropdown)field.Attribute).addDefaultOption)
			{
				MEGUIDropDownItem mEGUIDropDownItem = new MEGUIDropDownItem("", null, "#autoLOC_8100310");
				if (!string.IsNullOrEmpty(((MEGUI_Dropdown)field.Attribute).defaultDisplayString))
				{
					mEGUIDropDownItem.displayString = ((MEGUI_Dropdown)field.Attribute).defaultDisplayString;
				}
				list.Add(mEGUIDropDownItem);
			}
			if (SetDropDownItems != null)
			{
				list.AddRange((List<MEGUIDropDownItem>)SetDropDownItems.Invoke(field.host, new object[0]));
			}
			else if (list.Count < 1)
			{
				Debug.LogWarningFormat("[MEGUIParameterDropdownList]: {0} - Type is not an Enum and no SetDropDownValues delegate defined. List is empty", field.name);
			}
		}
		int count = list.Count;
		while (count-- > 0)
		{
			if (optionsToExclude != null && optionsToExclude.Contains(list[count].key))
			{
				list.RemoveAt(count);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			Items.Add(list[i].key, list[i]);
		}
		dropdownList.ClearOptions();
		dropdownList.AddOptions(list.GetDisplayStrings());
		int itemIndex2 = GetItemIndex(field.GetValue());
		if (itemIndex != itemIndex2 || itemIndex2 < 0)
		{
			selectedKey = Items.KeyAt((itemIndex2 >= 0) ? itemIndex2 : 0);
			field.SetValue(Items[selectedKey].value);
		}
	}

	public int GetItemIndex(object value)
	{
		if (Items == null)
		{
			return -1;
		}
		int count = Items.Count;
		while (true)
		{
			if (count-- > 0)
			{
				if (customValueComparer == null)
				{
					string text = value as string;
					if (value != null && (text == null || !(text == "")))
					{
						if (Items.ValuesList[count].value != null && Items.ValuesList[count].value.Equals(value))
						{
							return count;
						}
					}
					else if (Items.ValuesList[count].value == null)
					{
						return count;
					}
				}
				else if (customValueComparer(value, Items.ValuesList[count].value))
				{
					break;
				}
				continue;
			}
			return -1;
		}
		return count;
	}

	public override void ResetDefaultValue(string value)
	{
		if (Items.ContainsKey(value))
		{
			FieldValue = Items.KeysList.IndexOf(value);
		}
	}

	public override void RefreshUI()
	{
		dropdownList.value = FieldValue;
		UpdateNodeBodyUI();
	}

	public void OnParameterValueChanged(int value)
	{
		FieldValue = value;
		if (OnDropDownValueChanged != null)
		{
			OnDropDownValueChanged.Invoke(field.host, new object[2] { this, value });
		}
		UpdateNodeBodyUI();
		RefreshGapPrefab();
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		RefreshUI();
		prefabDisplay = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPPrefabDisplay>();
		RefreshGapPrefab();
	}

	public void RefreshGapPrefab()
	{
		if (!(SetGAPPrefab != null))
		{
			return;
		}
		prefabToDisplay = (GameObject)SetGAPPrefab.Invoke(field.host, new object[0]);
		if (prefabDisplay != null)
		{
			prefabDisplay.Setup(prefabToDisplay, ((MEGUI_Dropdown)field.Attribute).GAPCameraDistance);
			if (OnGAPPrefabInstantiated != null)
			{
				OnGAPPrefabInstantiated.Invoke(field.host, new object[1] { prefabDisplay.PrefabInstance });
			}
		}
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("pinned", isPinned);
		configNode.AddValue("value", selectedKey);
		return configNode;
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		string value = "";
		if (data.TryGetValue("value", ref value))
		{
			BuildDropDownItemsList();
			if (Items.ContainsKey(value))
			{
				field.SetValue(Items[value].value);
			}
			dropdownList.value = FieldValue;
			RefreshUI();
		}
	}
}
