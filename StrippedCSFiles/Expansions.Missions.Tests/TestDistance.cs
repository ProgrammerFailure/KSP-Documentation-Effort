using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using TMPro;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
internal class TestDistance : TestModule, IScoreableObjective
{
	public enum DistanceFromChoices
	{
		[Description("#autoLOC_7000020")]
		Kerbal,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	public enum DistanceToChoices
	{
		[Description("#autoLOC_8000046")]
		Asteroid,
		[Description("#autoLOC_6005065")]
		Comet,
		[Description("#autoLOC_8000263")]
		CelestialBody,
		[Description("#autoLoc_6002179")]
		Flag,
		[Description("#autoLOC_7000020")]
		Kerbal,
		[Description("#autoLOC_8000067")]
		LaunchSite,
		[Description("#autoLOC_8004145")]
		Location,
		[Description("#autoLOC_8004199")]
		NodeLabelNode,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	public enum DistanceCalculationType
	{
		[Description("#autoLOC_8004207")]
		StraightLine,
		[Description("#autoLOC_8004208")]
		GreatCircle
	}

	[MEGUI_Dropdown(onDropDownValueChange = "OnDistanceFromValueChange", onControlCreated = "OnDistanceFromTargetControlCreated", gapDisplay = false, guiName = "#autoLOC_8004196", Tooltip = "#autoLOC_8004197")]
	public DistanceFromChoices distanceFromTarget;

	private MEGUIParameterDropdownList distanceFromTargetParameterReference;

	[MEGUI_MissionKerbal(onControlCreated = "OnDistanceFromKerbalControlCreated", statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, canBePinned = false, showAllRosterStatus = true, hideOnSetup = true, guiName = "#autoLOC_7000020")]
	public MissionKerbal distanceFromKerbal;

	private MEGUIParameterMissionKerbal distanceFromKerbalParameterReference;

	private Vessel distanceFromKerbalVessel;

	[MEGUI_VesselSelect(onControlCreated = "OnDistanceFromVesselControlCreated", resetValue = "0", canBePinned = false, gapDisplay = true, hideOnSetup = true, addDefaultOption = true, defaultOptionIsActiveVessel = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8004198")]
	public uint distanceFromVesselID;

	private MEGUIParameterVesselDropdownList distanceFromVesselParameterReference;

	private Vessel distanceToKerbalVessel;

	[MEGUI_Dropdown(onDropDownValueChange = "OnDistanceToValueChange", onControlSetupComplete = "OnDistanceToTargetControlSetup", onControlCreated = "OnDistanceToTargetControlCreated", gapDisplay = false, guiName = "#autoLOC_8004200", Tooltip = "#autoLOC_8004201")]
	public DistanceToChoices distanceToTarget;

	private MEGUIParameterDropdownList distanceToTargetParameterReference;

	[MEGUI_MissionKerbal(onControlCreated = "OnDistanceToKerbalControlCreated", statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, canBePinned = false, showAllRosterStatus = true, hideOnSetup = true, guiName = "#autoLOC_7000020")]
	public MissionKerbal distanceToKerbal;

	private MEGUIParameterMissionKerbal distanceToKerbalParameterReference;

	[MEGUI_VesselSelect(resetValue = "0", addDefaultOption = false, canBePinned = false, onControlCreated = "OnDistanceToVesselControlCreated", hideOnSetup = true, gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8004211")]
	public uint distanceToVesselID;

	private MEGUIParameterVesselDropdownList distanceToVesselParameterReference;

	[MEGUI_VesselGroundLocation(DisableRotationX = true, onControlCreated = "OnDistanceToLocationControlCreated", DisableRotationZ = true, DisableRotationY = true, canBePinned = false, gapDisplay = true, hideOnSetup = true, guiName = "#autoLOC_8004145")]
	public VesselGroundLocation distanceToLocation;

	private MEGUIParameterCelestialBody_VesselGroundLocation distanceToLocationParameterReference;

	[MEGUI_CelestialBody(gapDisplay = true, canBePinned = false, onControlCreated = "OnDistanceToCelestialBodyControlCreated", resetValue = "0", showAnySOIoption = true, guiName = "#autoLOC_8000263")]
	public MissionCelestialBody distanceToCelestialBody;

	private MEGUIParameterCelestialBody distanceToCelestialBodyParameterReference;

	[MEGUI_AsteroidSelect(canBePinned = false, onControlCreated = "OnDistanceToAsteroidControlCreated", resetValue = "0", guiName = "#autoLOC_8000046", Tooltip = "#autoLOC_8004202")]
	public uint distanceToAsteroidID;

	[MEGUI_CometSelect(canBePinned = false, onControlCreated = "OnDistanceToCometControlCreated", resetValue = "0", guiName = "#autoLOC_6005065", Tooltip = "#autoLOC_8005447")]
	public uint distanceToCometID;

	private MEGUIParameterAsteroidDropdownList distanceToAsteroidParameterReference;

	private MEGUIParameterCometDropdownList distanceToCometParameterReference;

	[MEGUI_FlagSelect(canBePinned = false, onControlCreated = "OnDistanceToFlagControlCreated", resetValue = "0", guiName = "#autoLoc_6002179", Tooltip = "#autoLOC_8004203")]
	public uint distanceToFlagID;

	private MEGUIParameterFlagDropdownList distanceToFlagParameterReference;

	[MEGUI_LaunchSiteSelect(canBePinned = false, onControlCreated = "OnDistanceToLaunchSiteControlCreated", resetValue = "LaunchPad", guiName = "#autoLOC_8000067", Tooltip = "#autoLOC_8004204")]
	public string distanceToLaunchSiteName;

	private MEGUIParameterLaunchSiteDropdownList distanceToLaunchSiteParameterReference;

	private PSystemSetup.SpaceCenterFacility distanceToFacility;

	private LaunchSite distanceToLaunchSite;

	[MEGUI_NodeLabelNodeSelect(canBePinned = false, onControlCreated = "OnDistanceToNodeControlCreated", guiName = "#autoLOC_8004199", Tooltip = "#autoLOC_8004205")]
	public Guid distanceToNode;

	private MEGUIParameterNodeLabelNodeDropdownList distanceToNodeParameterReference;

	private ITestNodeLabel distanceToNodeLabelInterface;

	[MEGUI_Dropdown(canBePinned = true, resetValue = "StraightLine", canBeReset = true, guiName = "#autoLOC_8004209", Tooltip = "#autoLOC_8004210")]
	public DistanceCalculationType calculationType;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "LessThan", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8100017", Tooltip = "#autoLOC_8004206")]
	public double distance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestDistance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceFromTargetControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceFromKerbalControlCreated(MEGUIParameterMissionKerbal parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceFromVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceFromValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToTargetControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToTargetControlSetup(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int SortDropDownItemByText(TMP_Dropdown.OptionData a, TMP_Dropdown.OptionData b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToKerbalControlCreated(MEGUIParameterMissionKerbal parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToLocationControlCreated(MEGUIParameterCelestialBody_VesselGroundLocation parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToCelestialBodyControlCreated(MEGUIParameterCelestialBody parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToAsteroidControlCreated(MEGUIParameterAsteroidDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToCometControlCreated(MEGUIParameterCometDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToFlagControlCreated(MEGUIParameterFlagDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToLaunchSiteControlCreated(MEGUIParameterLaunchSiteDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToNodeControlCreated(MEGUIParameterNodeLabelNodeDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDistanceToValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(TestGroup testGroup, string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ParameterSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TestPointsDistance(Vector3d pointA, Vector3d pointB, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GetVesselLocation(uint vesselID, ref Vector3d location, ref CelestialBody celestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GetKerbalLocation(MissionKerbal missionKerbal, ref Vessel currentVessel, ref Vector3d location, ref CelestialBody celestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckCrew(Vessel v, MissionKerbal missionKerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RunValidation(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetScoreModifier(Type scoreModule)
	{
		throw null;
	}
}
