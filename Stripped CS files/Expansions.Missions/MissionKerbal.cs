using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Actions;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public class MissionKerbal
{
	public delegate void UpdateNodeBodyUI();

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "SetKerbalTypeList", onDropDownValueChange = "KerbalTypeChanged", guiName = "#autoLOC_8002023", Tooltip = "#autoLOC_8002024")]
	public ProtoCrewMember.KerbalType typeToShow;

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "SetKerbalList", onControlCreated = "OnKerbalControlCreated", onDropDownValueChange = "KerbalChanged", gapDisplay = true, guiName = "#autoLOC_8000022", Tooltip = "#autoLOC_8000153")]
	public ProtoCrewMember kerbal;

	public bool settingKerbal;

	public MENode node;

	public ProtoCrewMember selectedKerbal;

	public MEGUIParameterDropdownList kerbalDropdown;

	public ProtoCrewMember.RosterStatus statusToShow;

	public bool showStrandedOnly;

	public bool showAnyKerbal;

	public bool anyTextIsActiveKerbal;

	public bool showAllRosterStatus;

	public UpdateNodeBodyUI updateNodeBodyUI;

	public string Name
	{
		get
		{
			if (kerbal != null)
			{
				return kerbal.name;
			}
			return Localizer.Format("#autoLOC_8000152");
		}
	}

	public ProtoCrewMember Kerbal
	{
		get
		{
			return kerbal;
		}
		set
		{
			kerbal = value;
			if (updateNodeBodyUI != null)
			{
				updateNodeBodyUI();
			}
		}
	}

	public ProtoCrewMember.KerbalType TypeToShow
	{
		get
		{
			return typeToShow;
		}
		set
		{
			typeToShow = value;
		}
	}

	public bool AnyValid
	{
		get
		{
			return kerbal == null;
		}
		set
		{
			kerbal = null;
		}
	}

	public MissionKerbal()
	{
	}

	public MissionKerbal(ProtoCrewMember kerbal, MENode node, UpdateNodeBodyUI updateNodeBodyUI, ProtoCrewMember.KerbalType typetoShow = ProtoCrewMember.KerbalType.Crew, bool showAnyKerbal = true, bool anyTextIsActiveKerbal = false)
	{
		this.kerbal = kerbal;
		selectedKerbal = kerbal;
		this.node = node;
		typeToShow = typetoShow;
		this.updateNodeBodyUI = updateNodeBodyUI;
		this.showAnyKerbal = showAnyKerbal;
		this.anyTextIsActiveKerbal = anyTextIsActiveKerbal;
	}

	public void Initialize(ProtoCrewMember kerbal, MENode node)
	{
		this.kerbal = kerbal;
		selectedKerbal = kerbal;
		this.node = node;
	}

	public bool IsValid(ProtoCrewMember targetKerbal)
	{
		bool flag = true;
		if (AnyValid)
		{
			if (!showAnyKerbal)
			{
				return false;
			}
			return targetKerbal.type == typeToShow;
		}
		return kerbal.name == targetKerbal.name && kerbal.type == targetKerbal.type;
	}

	public void OnKerbalControlCreated(MEGUIParameterDropdownList parameter)
	{
		kerbalDropdown = parameter;
	}

	public void onKerbalStatusChange(ProtoCrewMember crew, ProtoCrewMember.RosterStatus prevStatus, ProtoCrewMember.RosterStatus newStatus)
	{
		RebuildDropDownOnChange(crew);
	}

	public void onKerbalAdded(ProtoCrewMember crew)
	{
		RebuildDropDownOnChange(crew);
	}

	public void onKerbalTypeChange(ProtoCrewMember crew, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType)
	{
		RebuildDropDownOnChange(crew);
	}

	public void onKerbalNameChange(ProtoCrewMember crew, string oldName, string newName)
	{
		RebuildDropDownOnChange(crew);
	}

	public void onKerbalRemoved(ProtoCrewMember crew)
	{
		if (selectedKerbal != null && selectedKerbal.name == crew.name)
		{
			selectedKerbal = null;
		}
		RebuildDropDownOnChange(crew);
	}

	public void RebuildDropDownOnChange(ProtoCrewMember crew)
	{
		if (!settingKerbal && kerbalDropdown != null && (selectedKerbal == null || (selectedKerbal != null && selectedKerbal.name != crew.name)))
		{
			kerbalDropdown.RebuildDropDown();
		}
	}

	public List<MEGUIDropDownItem> SetKerbalList()
	{
		settingKerbal = true;
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		List<ActionCreateKerbal> allActionModules = node.mission.GetAllActionModules<ActionCreateKerbal>();
		IEnumerator<ProtoCrewMember> enumerator = node.mission.situation.crewRoster.Crew.GetEnumerator();
		if (showAnyKerbal)
		{
			if (typeToShow == ProtoCrewMember.KerbalType.Tourist)
			{
				list.Add(new MEGUIDropDownItem(Localizer.Format("#autoLOC_8002021"), null, Localizer.Format("#autoLOC_8002021")));
			}
			else if (!anyTextIsActiveKerbal)
			{
				list.Add(new MEGUIDropDownItem(Localizer.Format("#autoLOC_8000152"), null, Localizer.Format("#autoLOC_8000152")));
			}
			else
			{
				list.Add(new MEGUIDropDownItem(Localizer.Format("#autoLOC_8004218"), null, Localizer.Format("#autoLOC_8004218")));
			}
		}
		switch (typeToShow)
		{
		case ProtoCrewMember.KerbalType.Crew:
			enumerator = node.mission.situation.crewRoster.Crew.GetEnumerator();
			break;
		case ProtoCrewMember.KerbalType.Applicant:
			enumerator = node.mission.situation.crewRoster.Applicants.GetEnumerator();
			break;
		case ProtoCrewMember.KerbalType.Unowned:
			enumerator = node.mission.situation.crewRoster.Unowned.GetEnumerator();
			break;
		case ProtoCrewMember.KerbalType.Tourist:
			enumerator = node.mission.situation.crewRoster.Tourist.GetEnumerator();
			break;
		}
		try
		{
			while (enumerator.MoveNext())
			{
				ProtoCrewMember current = enumerator.Current;
				if (current.rosterStatus != statusToShow && (kerbal == null || !(kerbal.name == current.name)) && !showAllRosterStatus)
				{
					continue;
				}
				if (showStrandedOnly)
				{
					for (int i = 0; i < allActionModules.Count; i++)
					{
						ActionCreateKerbal actionCreateKerbal = allActionModules[i];
						if (actionCreateKerbal.missionKerbal != null && actionCreateKerbal.missionKerbal.Kerbal != null && actionCreateKerbal.missionKerbal.kerbal.name == current.name && actionCreateKerbal.isStranded)
						{
							list.Add(new MEGUIDropDownItem(current.name, current, current.name));
							break;
						}
					}
				}
				else
				{
					list.Add(new MEGUIDropDownItem(current.name, current, current.displayName));
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		if (list.Count == 0)
		{
			list.Add(new MEGUIDropDownItem(Localizer.Format("#autoLOC_6003000"), null, Localizer.Format("#autoLOC_6003000")));
		}
		if (updateNodeBodyUI != null)
		{
			updateNodeBodyUI();
		}
		selectedKerbal = kerbal;
		settingKerbal = false;
		return list;
	}

	public List<MEGUIDropDownItem> SetKerbalTypeList()
	{
		settingKerbal = true;
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		IEnumerator enumerator = Enum.GetValues(typeToShow.GetType()).GetEnumerator();
		while (enumerator.MoveNext())
		{
			Enum @enum = (Enum)enumerator.Current;
			if (@enum.ToString() != "Applicant" && @enum.ToString() != "Unowned")
			{
				list.Add(new MEGUIDropDownItem(@enum.ToString(), @enum, @enum.displayDescription()));
			}
		}
		settingKerbal = false;
		return list;
	}

	public void KerbalChanged(MEGUIParameterDropdownList sender, int newIndex)
	{
		bool flag = settingKerbal;
		settingKerbal = true;
		setcurrentKerbalAvailable();
		if (sender.SelectedValue != null && node != null && node.IsVesselNode)
		{
			((ProtoCrewMember)sender.SelectedValue).rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
			selectedKerbal = (ProtoCrewMember)sender.SelectedValue;
		}
		if (updateNodeBodyUI != null)
		{
			updateNodeBodyUI();
		}
		settingKerbal = flag;
	}

	public void KerbalTypeChanged(MEGUIParameterDropdownList sender, int newIndex)
	{
		bool flag = settingKerbal;
		settingKerbal = true;
		if (kerbalDropdown != null)
		{
			kerbalDropdown.RebuildDropDown();
		}
		settingKerbal = flag;
	}

	public void setcurrentKerbalAvailable()
	{
		if (selectedKerbal != null)
		{
			selectedKerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			selectedKerbal = null;
		}
	}

	public string GetNodeBodyParameterString()
	{
		if (kerbal != null)
		{
			return Localizer.Format("#autoLOC_8002022", kerbal.displayName);
		}
		if (typeToShow == ProtoCrewMember.KerbalType.Tourist)
		{
			return Localizer.Format("#autoLOC_8002022", Localizer.Format("#autoLOC_8002021"));
		}
		return Localizer.Format("#autoLOC_8002022", Localizer.Format("#autoLOC_8000152"));
	}

	public override bool Equals(object obj)
	{
		if (!(obj is MissionKerbal missionKerbal))
		{
			return false;
		}
		if (typeToShow.Equals(missionKerbal.typeToShow))
		{
			return kerbal.Equals(missionKerbal.kerbal);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return typeToShow.GetHashCode() ^ kerbal.GetHashCode();
	}

	public void Save(ConfigNode node)
	{
		if (kerbal != null)
		{
			node.AddValue("kerbalName", kerbal.name);
		}
		node.AddValue("kerbalType", typeToShow);
	}

	public void Load(ConfigNode node)
	{
		string value = string.Empty;
		kerbal = null;
		selectedKerbal = null;
		node.TryGetValue("kerbalName", ref value);
		if (!string.IsNullOrEmpty(value))
		{
			ProtoCrewMember protoCrewMember = this.node.mission.situation.crewRoster[value];
			if (protoCrewMember != null)
			{
				kerbal = protoCrewMember;
				selectedKerbal = kerbal;
			}
			else
			{
				Debug.LogFormat("Unable to find kerbal ({0}) in mission roster, setting to Any Kerbal", value);
			}
		}
		node.TryGetEnum("kerbalType", ref typeToShow, ProtoCrewMember.KerbalType.Crew);
	}
}
