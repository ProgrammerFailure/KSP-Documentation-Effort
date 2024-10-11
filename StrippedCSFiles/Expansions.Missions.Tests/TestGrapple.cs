using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestGrapple : TestModule
{
	public enum SpaceObjectChoices
	{
		[Description("#autoLOC_8000046")]
		Asteroid,
		[Description("#autoLOC_6005065")]
		Comet,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	[MEGUI_VesselSelect(resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8005456", Tooltip = "#autoLOC_8005457")]
	public uint grapplingVesselID;

	private ModuleGrappleNode grabbingUnit;

	[MEGUI_Dropdown(onDropDownValueChange = "OnSpaceObjectChoiceValueChange", onControlSetupComplete = "OnSpaceObjectChoiceControlSetup", onControlCreated = "OnSpaceObjectChoiceControlCreated", gapDisplay = false, guiName = "#autoLOC_8005458", Tooltip = "#autoLOC_8005459")]
	public SpaceObjectChoices grappledSpaceObject;

	private MEGUIParameterDropdownList spaceObjectParameterReference;

	[MEGUI_AsteroidSelect(canBePinned = false, onControlCreated = "OnAsteroidControlCreated", resetValue = "0", guiName = "#autoLOC_8000046", Tooltip = "#autoLOC_8005460")]
	public uint grappleAsteroidID;

	[MEGUI_CometSelect(hideOnSetup = true, canBePinned = false, onControlCreated = "OnCometControlCreated", resetValue = "0", guiName = "#autoLOC_6005065", Tooltip = "#autoLOC_8005461")]
	public uint grappleCometID;

	[MEGUI_VesselSelect(addDefaultOption = false, hideOnSetup = true, canBePinned = false, onControlCreated = "OnVesselControlCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_6002525")]
	public uint grappleVesselID;

	private uint grappleSOPartID;

	private MEGUIParameterAsteroidDropdownList grappleAsteroidParameterReference;

	private MEGUIParameterCometDropdownList grappleCometParameterReference;

	private MEGUIParameterVesselDropdownList grappleVesselParameterReference;

	[MEGUI_Dropdown(SetDropDownItems = "SetDockedUndockedDropdown", canBePinned = true, guiName = "#autoLOC_8002012")]
	public string dockedUndocked;

	private bool firstRunTest;

	private bool testSuccess;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestGrapple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSpaceObjectChoiceControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSpaceObjectChoiceControlSetup(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAsteroidControlCreated(MEGUIParameterAsteroidDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCometControlCreated(MEGUIParameterCometDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ParameterSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSpaceObjectChoiceValueChange(MEGUIParameterDropdownList sender, int newIndex)
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
	public override bool Test()
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
