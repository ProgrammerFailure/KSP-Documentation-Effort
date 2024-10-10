using System.Collections.Generic;
using TMPro;

namespace Expansions.Missions.Editor;

[MEGUI_CrewList]
public class MEGUIParameterCrewList : MEGUIParameter
{
	public TMP_Dropdown dropdownList;

	public KerbalRoster kerbalRoster;

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

	public ProtoCrewMember FieldValue
	{
		get
		{
			return (ProtoCrewMember)field.GetValue(field.host);
		}
		set
		{
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		List<string> list = new List<string>();
		kerbalRoster = MissionEditorLogic.Instance.EditorMission.situation.crewRoster;
		for (int i = 0; i < kerbalRoster.Count; i++)
		{
			if (kerbalRoster[i].KerbalRef.rosterStatus == ProtoCrewMember.RosterStatus.Assigned || kerbalRoster[i].KerbalRef.rosterStatus == ProtoCrewMember.RosterStatus.Available)
			{
				list.Add(kerbalRoster[i].KerbalRef.crewMemberName);
			}
		}
		dropdownList.AddOptions(list);
		dropdownList.value = 0;
		dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
	}

	public override void ResetDefaultValue(string value)
	{
	}

	public override void RefreshUI()
	{
		dropdownList.value = 0;
	}

	public void OnParameterValueChanged(int value)
	{
		FieldValue = kerbalRoster[value];
		UpdateNodeBodyUI();
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
	}
}
