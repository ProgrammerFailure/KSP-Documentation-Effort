using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class MissionKerbal
{
	public delegate void UpdateNodeBodyUI();

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "SetKerbalTypeList", onDropDownValueChange = "KerbalTypeChanged", guiName = "#autoLOC_8002023", Tooltip = "#autoLOC_8002024")]
	private ProtoCrewMember.KerbalType typeToShow;

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "SetKerbalList", onControlCreated = "OnKerbalControlCreated", onDropDownValueChange = "KerbalChanged", gapDisplay = true, guiName = "#autoLOC_8000022", Tooltip = "#autoLOC_8000153")]
	private ProtoCrewMember kerbal;

	private bool settingKerbal;

	private MENode node;

	private ProtoCrewMember selectedKerbal;

	private MEGUIParameterDropdownList kerbalDropdown;

	public ProtoCrewMember.RosterStatus statusToShow;

	public bool showStrandedOnly;

	public bool showAnyKerbal;

	public bool anyTextIsActiveKerbal;

	public bool showAllRosterStatus;

	public UpdateNodeBodyUI updateNodeBodyUI;

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ProtoCrewMember Kerbal
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public ProtoCrewMember.KerbalType TypeToShow
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool AnyValid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionKerbal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionKerbal(ProtoCrewMember kerbal, MENode node, UpdateNodeBodyUI updateNodeBodyUI, ProtoCrewMember.KerbalType typetoShow = ProtoCrewMember.KerbalType.Crew, bool showAnyKerbal = true, bool anyTextIsActiveKerbal = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(ProtoCrewMember kerbal, MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsValid(ProtoCrewMember targetKerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKerbalControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalStatusChange(ProtoCrewMember crew, ProtoCrewMember.RosterStatus prevStatus, ProtoCrewMember.RosterStatus newStatus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalAdded(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalTypeChange(ProtoCrewMember crew, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalNameChange(ProtoCrewMember crew, string oldName, string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onKerbalRemoved(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RebuildDropDownOnChange(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetKerbalList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetKerbalTypeList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KerbalChanged(MEGUIParameterDropdownList sender, int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KerbalTypeChanged(MEGUIParameterDropdownList sender, int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setcurrentKerbalAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeBodyParameterString()
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
