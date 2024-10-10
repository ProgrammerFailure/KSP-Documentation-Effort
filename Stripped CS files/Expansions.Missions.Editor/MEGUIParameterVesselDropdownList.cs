using System;
using System.Collections.Generic;
using ns9;
using TMPro;

namespace Expansions.Missions.Editor;

[MEGUI_VesselSelect]
public class MEGUIParameterVesselDropdownList : MEGUIParameterVessel
{
	public TMP_Dropdown dropdownList;

	public bool craftOnly;

	public DictionaryValueList<uint, string> dropdownOptions;

	public GAPVesselDisplay vesselDisplay;

	public bool overrideDefaultOptionIsActiveVessel;

	public bool defaultOptionIsActiveVessel;

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
		GameEvents.Mission.onVesselSituationChanged.Add(SetDropdownValues);
	}

	public void SetCraftOnly(bool craftOnly = true)
	{
		GameEvents.Mission.onVesselSituationChanged.Remove(SetDropdownValues);
		this.craftOnly = craftOnly;
		SetDropdownValues();
		dropdownList.value = FieldValue;
		GameEvents.Mission.onVesselSituationChanged.Add(SetDropdownValues);
	}

	public new void OnDestroy()
	{
		GameEvents.Mission.onBuilderNodeAdded.Remove(onBuilderNodeAdded);
		GameEvents.Mission.onBuilderNodeDeleted.Remove(onBuilderNodeDeleted);
		GameEvents.Mission.onVesselSituationChanged.Remove(SetDropdownValues);
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
		defaultOptionIsActiveVessel = ((!overrideDefaultOptionIsActiveVessel) ? ((MEGUI_VesselSelect)field.Attribute).defaultOptionIsActiveVessel : defaultOptionIsActiveVessel);
		if (((MEGUI_VesselSelect)field.Attribute).addDefaultOption)
		{
			if (!defaultOptionIsActiveVessel)
			{
				dropdownOptions.Add(0u, "#autoLOC_8001004");
				list.Add("#autoLOC_8001004");
			}
			else
			{
				dropdownOptions.Add(0u, "#autoLOC_8004217");
				list.Add("#autoLOC_8004217");
			}
		}
		int i = 0;
		for (int count = base.VesselList.Count; i < count; i++)
		{
			dropdownOptions.Add(base.VesselList[i].persistentId, base.VesselList[i].vesselName);
			MissionCraft craftBySituationsVesselID = MissionEditorLogic.Instance.EditorMission.GetCraftBySituationsVesselID(base.VesselList[i].persistentId);
			if (craftBySituationsVesselID != null && MissionEditorLogic.Instance.incompatibleCraft.Contains(craftBySituationsVesselID.craftFile))
			{
				list.Add(Localizer.Format("#autoLOC_8004245", base.VesselList[i].vesselName));
			}
			else
			{
				list.Add(base.VesselList[i].vesselName);
			}
			if (craftOnly)
			{
				continue;
			}
			for (int j = 0; j < base.MappedVesselList.Count; j++)
			{
				if (base.MappedVesselList[j].currentVesselPersistentId == base.VesselList[i].persistentId && craftBySituationsVesselID != null)
				{
					dropdownOptions.Add(base.MappedVesselList[j].mappedVesselPersistentId, Localizer.Format(base.MappedVesselList[j].partVesselName));
					if (MissionEditorLogic.Instance.incompatibleCraft.Contains(craftBySituationsVesselID.craftFile))
					{
						list.Add(">> " + Localizer.Format("#autoLOC_8004245", Localizer.Format(base.MappedVesselList[j].partVesselName)));
					}
					else
					{
						list.Add(">> " + Localizer.Format(base.MappedVesselList[j].partVesselName));
					}
				}
			}
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

	public void OverrideDefaultValue(bool defaultOptionIsActiveVessel)
	{
		overrideDefaultOptionIsActiveVessel = true;
		this.defaultOptionIsActiveVessel = defaultOptionIsActiveVessel;
	}

	public override void ResetDefaultValue(string value)
	{
		uint result = 0u;
		if (uint.TryParse(value, out result) && dropdownOptions.KeysList.Contains(result))
		{
			FieldValue = dropdownOptions.KeysList.IndexOf(result);
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
		int i = 0;
		for (int count = base.VesselList.Count; i < count; i++)
		{
			if (base.VesselList[i].vesselName == dropdownList.options[dropdownList.value].text)
			{
				flag = false;
			}
		}
		if (flag)
		{
			string text = dropdownList.options[dropdownList.value].text;
			if (text.IndexOf(">> ", StringComparison.InvariantCulture) != -1)
			{
				text = text.Substring(3, text.Length - 3);
			}
			if (base.MappedVesselList.MappedVesselName(text) != -1)
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
		if (vesselDisplay != null && vesselDisplay.displayImage != null && value < dropdownOptions.Count)
		{
			vesselDisplay.SetupVessel(mission.GetCraftBySituationsVesselID(dropdownOptions.KeyAt(value)), mission.GetVesselSituationByVesselID(dropdownOptions.KeyAt(value), processMappedVessels: true), this);
		}
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		vesselDisplay = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPVesselDisplay>();
		vesselDisplay.SetupVessel(mission.GetCraftBySituationsVesselID(dropdownOptions.KeyAt(FieldValue)), mission.GetVesselSituationByVesselID(dropdownOptions.KeyAt(FieldValue), processMappedVessels: true), this);
	}

	public override void OnNextVessel()
	{
		dropdownList.value = (dropdownList.value + 1) % dropdownList.options.Count;
	}

	public override void OnPrevVessel()
	{
		dropdownList.value = ((dropdownList.value - 1 < 0) ? (dropdownList.options.Count - 1) : (dropdownList.value - 1));
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
