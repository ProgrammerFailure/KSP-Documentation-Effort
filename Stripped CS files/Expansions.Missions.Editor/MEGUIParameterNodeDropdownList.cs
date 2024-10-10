using System;
using System.Collections.Generic;
using TMPro;

namespace Expansions.Missions.Editor;

[MEGUI_NodeSelect]
public class MEGUIParameterNodeDropdownList : MEGUIParameter
{
	public TMP_Dropdown dropdownList;

	public bool ResetToValidNodeWhenNodeIsMissing;

	public DictionaryValueList<Guid, MENode> options;

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

	public int FieldValue
	{
		get
		{
			Guid value = (field.GetValue() as Guid?).Value;
			if (options.Contains(value))
			{
				return options.IndexOf(options[value]);
			}
			if (ResetToValidNodeWhenNodeIsMissing)
			{
				if (options.Count == 0)
				{
					field.SetValue(Guid.Empty);
					return 0;
				}
				dropdownList.value = 0;
				field.SetValue(options.KeysList[dropdownList.value]);
				return dropdownList.value;
			}
			return -1;
		}
		set
		{
			if (value < options.Count)
			{
				MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
				field.SetValue(options.At(value).id);
			}
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
	}

	public void RefreshNodeList()
	{
		Guid value = (field.GetValue() as Guid?).Value;
		dropdownList.ClearOptions();
		options = GetNodeList();
		List<string> list = new List<string>();
		int i = 0;
		for (int count = options.Count; i < count; i++)
		{
			list.Add(options.At(i).Title);
		}
		dropdownList.AddOptions(list);
		if (MissionEditorLogic.Instance.EditorMission.nodes.ContainsKey(value))
		{
			dropdownList.value = FieldValue;
			return;
		}
		TMP_Dropdown.OptionData optionData = null;
		if (!ResetToValidNodeWhenNodeIsMissing && list.Count > 0)
		{
			optionData = new TMP_Dropdown.OptionData("#autoLOC_8007310");
		}
		if (list.Count == 0)
		{
			optionData = new TMP_Dropdown.OptionData("#autoLOC_6003000");
		}
		if (optionData != null)
		{
			dropdownList.options.Add(optionData);
			dropdownList.value = dropdownList.options.Count;
			if (!ResetToValidNodeWhenNodeIsMissing && list.Count > 0)
			{
				dropdownList.options.Remove(optionData);
			}
		}
	}

	public virtual DictionaryValueList<Guid, MENode> GetNodeList()
	{
		return MissionEditorLogic.Instance.EditorMission.nodes;
	}

	public override void Display()
	{
		base.Display();
		RefreshNodeList();
		if (ResetToValidNodeWhenNodeIsMissing)
		{
			dropdownList.value = FieldValue;
		}
	}

	public override void ResetDefaultValue(string value)
	{
		Guid key = new Guid(value);
		if (options.Contains(key))
		{
			FieldValue = options.IndexOf(options[key]);
		}
	}

	public override void RefreshUI()
	{
		dropdownList.value = FieldValue;
	}

	public void OnParameterValueChanged(int value)
	{
		FieldValue = value;
		UpdateNodeBodyUI();
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		Guid value = Guid.Empty;
		if (data.TryGetValue("value", ref value))
		{
			field.SetValue(value);
			dropdownList.value = FieldValue;
		}
	}
}
