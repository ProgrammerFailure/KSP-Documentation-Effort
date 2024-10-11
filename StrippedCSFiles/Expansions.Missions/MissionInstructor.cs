using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class MissionInstructor
{
	public delegate void UpdateNodeBodyUI();

	[MEGUI_Dropdown(checkpointValidation = CheckpointValidationType.None, SetDropDownItems = "SetInstructors", resetValue = "", gapDisplay = true, guiName = "#autoLOC_8006002", Tooltip = "#autoLOC_8006003")]
	public string instructorName;

	[MEGUI_Checkbox(checkpointValidation = CheckpointValidationType.None, hideOnSetup = true, guiName = "#autoLOC_8002109", Tooltip = "#autoLOC_8002110")]
	public bool vintageSuit;

	private MEGUIParameterDropdownList instructorDropdown;

	public UpdateNodeBodyUI updateNodeBodyUI;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionInstructor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionInstructor(MENode node, UpdateNodeBodyUI updateNodeBodyUI)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetInstructors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
