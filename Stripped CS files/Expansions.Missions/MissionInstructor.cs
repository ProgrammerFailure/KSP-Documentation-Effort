using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class MissionInstructor
{
	public delegate void UpdateNodeBodyUI();

	[MEGUI_Dropdown(checkpointValidation = CheckpointValidationType.None, SetDropDownItems = "SetInstructors", resetValue = "", gapDisplay = true, guiName = "#autoLOC_8006002", Tooltip = "#autoLOC_8006003")]
	public string instructorName;

	[MEGUI_Checkbox(checkpointValidation = CheckpointValidationType.None, hideOnSetup = true, guiName = "#autoLOC_8002109", Tooltip = "#autoLOC_8002110")]
	public bool vintageSuit;

	public MEGUIParameterDropdownList instructorDropdown;

	public UpdateNodeBodyUI updateNodeBodyUI;

	public MissionInstructor()
	{
		instructorName = "Instructor_Gene";
		vintageSuit = false;
	}

	public MissionInstructor(MENode node, UpdateNodeBodyUI updateNodeBodyUI)
	{
		this.updateNodeBodyUI = updateNodeBodyUI;
	}

	public List<MEGUIDropDownItem> SetInstructors()
	{
		return new List<MEGUIDropDownItem>
		{
			new MEGUIDropDownItem("Wernher", "Instructor_Wernher", "#autoLOC_900000"),
			new MEGUIDropDownItem("Gene", "Instructor_Gene", "#autoLOC_900001"),
			new MEGUIDropDownItem("Gus", "Strategy_MechanicGuy", "#autoLOC_900002"),
			new MEGUIDropDownItem("Mortimer", "Strategy_Mortimer", "#autoLOC_900004"),
			new MEGUIDropDownItem("Walt", "Strategy_PRGuy", "#autoLOC_900005"),
			new MEGUIDropDownItem("Linus", "Strategy_ScienceGuy", "#autoLOC_900006"),
			new MEGUIDropDownItem("Jeb", "Instructor_Jeb_MaleVeteran", "#autoLOC_20803"),
			new MEGUIDropDownItem("Bill", "Instructor_Bill_MaleVeteran", "#autoLOC_20811"),
			new MEGUIDropDownItem("Bob", "Instructor_Bob_MaleVeteran", "#autoLOC_20819"),
			new MEGUIDropDownItem("Val", "Instructor_Val_FemaleVeteran", "#autoLOC_20827")
		};
	}

	public string GetNodeBodyParameterString(BaseAPField field)
	{
		string text = "";
		if (instructorName == "Instructor_Wernher")
		{
			return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900000");
		}
		if (instructorName == "Instructor_Gene")
		{
			return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900001");
		}
		if (instructorName == "Strategy_MechanicGuy")
		{
			return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900002");
		}
		if (instructorName == "Strategy_Mortimer")
		{
			return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900004");
		}
		if (instructorName == "Strategy_PRGuy")
		{
			return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900005");
		}
		if (instructorName == "Strategy_ScienceGuy")
		{
			return Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_900006");
		}
		if (instructorName == "Instructor_Jeb_MaleVeteran")
		{
			text = Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_20803");
			return text + "\n" + Localizer.Format("#autoLOC_8002109") + ": " + vintageSuit;
		}
		if (instructorName == "Instructor_Bill_MaleVeteran")
		{
			text = Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_20811");
			return text + "\n" + Localizer.Format("#autoLOC_8002109") + ": " + vintageSuit;
		}
		if (instructorName == "Instructor_Bob_MaleVeteran")
		{
			text = Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_20819");
			return text + "\n" + Localizer.Format("#autoLOC_8002109") + ": " + vintageSuit;
		}
		if (instructorName == "Instructor_Val_FemaleVeteran")
		{
			text = Localizer.Format("#autoLOC_8006002") + ": " + Localizer.Format("#autoLOC_20827");
			return text + "\n" + Localizer.Format("#autoLOC_8002109") + ": " + vintageSuit;
		}
		return "";
	}

	public override bool Equals(object obj)
	{
		if (!(obj is MissionInstructor missionInstructor))
		{
			return false;
		}
		if (instructorName.Equals(missionInstructor.instructorName))
		{
			return vintageSuit.Equals(missionInstructor.vintageSuit);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return instructorName.GetHashCode_Net35() ^ vintageSuit.GetHashCode();
	}

	public void Save(ConfigNode node)
	{
		if (!string.IsNullOrEmpty(instructorName))
		{
			node.AddValue("instructorName", instructorName);
		}
		node.AddValue("vintageSuit", vintageSuit);
	}

	public void Load(ConfigNode node)
	{
		instructorName = string.Empty;
		node.TryGetValue("instructorName", ref instructorName);
		node.TryGetValue("vintageSuit", ref vintageSuit);
	}
}
