using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestPartDocking : TestModule
{
	[MEGUI_VesselPartSelect(resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8002008", Tooltip = "#autoLOC_8002009")]
	public VesselPartIDPair partOnevesselPartIDs;

	[MEGUI_VesselPartSelect(resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8002010", Tooltip = "#autoLOC_8002011")]
	public VesselPartIDPair partTwovesselPartIDs;

	[MEGUI_Dropdown(SetDropDownItems = "SetDockedUndockedDropdown", canBePinned = true, guiName = "#autoLOC_8002012")]
	public string dockedUndocked;

	private bool firstRunTest;

	private bool testSuccess;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestPartDocking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetDockedUndockedDropdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setPlayerCreatedRootParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Cleared()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartCouple(GameEvents.FromToAction<Part, Part> partAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartUndock(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void testPartVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool findPartVesselId(uint partId, out uint vesselId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselDocking(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RunValidation(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}
}
