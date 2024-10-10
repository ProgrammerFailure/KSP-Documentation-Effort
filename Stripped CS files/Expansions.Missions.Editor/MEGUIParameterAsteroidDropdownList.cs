using System.Collections.Generic;
using Expansions.Missions.Actions;
using TMPro;

namespace Expansions.Missions.Editor;

[MEGUI_AsteroidSelect]
public class MEGUIParameterAsteroidDropdownList : MEGUIParameter
{
	public TMP_Dropdown dropdownList;

	public DictionaryValueList<uint, string> dropdownOptions;

	public int FieldValue
	{
		get
		{
			uint value = (field.GetValue() as uint?).Value;
			if (dropdownOptions.KeysList.Contains(value))
			{
				return dropdownOptions.KeysList.IndexOf(value);
			}
			field.SetValue(dropdownOptions.KeysList[dropdownList.value]);
			return dropdownList.value;
		}
		set
		{
			if (value < dropdownOptions.Count && value > -1)
			{
				MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
				field.SetValue(dropdownOptions.KeysList[value]);
			}
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		SetDropdownValues();
		dropdownList.value = FieldValue;
		dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
		GameEvents.Mission.onBuilderNodeAdded.Add(onBuilderNodeAdded);
		GameEvents.Mission.onBuilderNodeDeleted.Add(onBuilderNodeDeleted);
	}

	public void OnDestroy()
	{
		GameEvents.Mission.onBuilderNodeAdded.Remove(onBuilderNodeAdded);
		GameEvents.Mission.onBuilderNodeDeleted.Remove(onBuilderNodeDeleted);
	}

	public void onBuilderNodeAdded(MENode node)
	{
		if (node.IsVesselNode)
		{
			SetDropdownValues();
		}
	}

	public void onBuilderNodeDeleted(MENode node)
	{
		if (node.IsVesselNode)
		{
			SetDropdownValues();
		}
	}

	public void SetDropdownValues()
	{
		dropdownList.ClearOptions();
		dropdownOptions = new DictionaryValueList<uint, string>();
		List<string> list = new List<string>();
		List<ActionCreateAsteroid> allActionModules = MissionEditorLogic.Instance.EditorMission.GetAllActionModules<ActionCreateAsteroid>();
		int i = 0;
		for (int count = allActionModules.Count; i < count; i++)
		{
			dropdownOptions.Add(allActionModules[i].PersistentID, allActionModules[i].asteroid.name);
			list.Add(allActionModules[i].asteroid.name);
		}
		if (list.Count == 0)
		{
			dropdownOptions.Add(0u, "#autoLOC_6003000");
			list.Add("#autoLOC_6003000");
		}
		dropdownList.AddOptions(list);
		if (dropdownList.value >= dropdownOptions.Count)
		{
			dropdownList.value = dropdownOptions.Count - 1;
		}
	}

	public override void RefreshUI()
	{
		dropdownList.value = FieldValue;
	}

	public override void Display()
	{
		base.Display();
		dropdownList.onValueChanged.RemoveListener(OnParameterValueChanged);
		bool flag = true;
		List<ActionCreateAsteroid> allActionModules = MissionEditorLogic.Instance.EditorMission.GetAllActionModules<ActionCreateAsteroid>();
		int i = 0;
		for (int count = allActionModules.Count; i < count; i++)
		{
			if (allActionModules[i].PersistentID == dropdownOptions.KeysList[dropdownList.value])
			{
				flag = false;
			}
		}
		if (flag)
		{
			SetDropdownValues();
			dropdownList.value = 0;
		}
		else
		{
			SetDropdownValues();
			dropdownList.value = FieldValue;
		}
		dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
	}

	public void OnParameterValueChanged(int value)
	{
		FieldValue = value;
		UpdateNodeBodyUI();
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		uint value = 0u;
		if (data.TryGetValue("value", ref value))
		{
			field.SetValue(value);
			dropdownList.value = FieldValue;
		}
	}
}
