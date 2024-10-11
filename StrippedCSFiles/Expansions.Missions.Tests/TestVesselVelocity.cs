using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestVesselVelocity : TestVessel
{
	public enum SpeedType
	{
		[Description("#autoLOC_8004167")]
		SurfaceVelocity,
		[Description("#autoLOC_8004168")]
		OrbitalVelocity
	}

	public enum VelocityReferenceFrame
	{
		[Description("#autoLOC_8005455")]
		Default,
		[Description("#autoLOC_8000046")]
		Asteroid,
		[Description("#autoLOC_6005065")]
		Comet,
		[Description("#autoLOC_7000020")]
		Kerbal,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8004169", Tooltip = "#autoLOC_8004170")]
	public float velocity;

	[MEGUI_Dropdown(canBePinned = false, resetValue = "GreaterThan", canBeReset = true, guiName = "#autoLOC_8000052", Tooltip = "#autoLOC_8000053")]
	public TestComparisonLessGreaterOnly comparisonOperator;

	[MEGUI_Dropdown(onDropDownValueChange = "OnVelocityWRTBodyValueChange", onControlSetupComplete = "OnVelocityWRTBodyControlSetup", onControlCreated = "OnVelocityWRTBodyControlCreated", gapDisplay = false, guiName = "#autoLOC_8005449", Tooltip = "#autoLOC_8005450")]
	public VelocityReferenceFrame referenceBody;

	private MEGUIParameterDropdownList velocityWRTBodyParameterReference;

	[MEGUI_Dropdown(onControlSetupComplete = "OnSpeedTypeControlSetup", canBePinned = true, onControlCreated = "OnSpeedTypeControlCreated", resetValue = "SurfaceVelocity", canBeReset = true, guiName = "#autoLOC_8004171", Tooltip = "#autoLOC_8004172")]
	public SpeedType speedType;

	private MEGUIParameterDropdownList speedTypeParameterReference;

	[MEGUI_MissionKerbal(onControlCreated = "OnVelocityWRTKerbalControlCreated", statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, canBePinned = false, showAllRosterStatus = true, hideOnSetup = true, guiName = "#autoLOC_7000020")]
	public MissionKerbal velocityKerbal;

	private MEGUIParameterMissionKerbal kerbalVelocityReference;

	[MEGUI_VesselSelect(resetValue = "0", addDefaultOption = false, canBePinned = false, onControlCreated = "OnVelocityWRTVesselControlCreated", hideOnSetup = true, gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8005451")]
	public uint velocityVesselID;

	private MEGUIParameterVesselDropdownList vesselVelocityReference;

	[MEGUI_AsteroidSelect(canBePinned = false, onControlCreated = "OnVelocityWRTAsteroidControlCreated", resetValue = "0", guiName = "#autoLOC_8000046", Tooltip = "#autoLOC_8005452")]
	public uint velocityAsteroidID;

	[MEGUI_CometSelect(canBePinned = false, onControlCreated = "OnVelocityWRTCometControlCreated", resetValue = "0", guiName = "#autoLOC_6005065", Tooltip = "#autoLOC_8005453")]
	public uint velocityCometID;

	private MEGUIParameterAsteroidDropdownList velocityAsteroidParameterReference;

	private MEGUIParameterCometDropdownList velocityCometParameterReference;

	private Vessel vesselRefBody;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestVesselVelocity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVelocityWRTBodyControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVelocityWRTBodyControlSetup(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSpeedTypeControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSpeedTypeControlSetup(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVelocityWRTKerbalControlCreated(MEGUIParameterMissionKerbal parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVelocityWRTVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVelocityWRTAsteroidControlCreated(MEGUIParameterAsteroidDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVelocityWRTCometControlCreated(MEGUIParameterCometDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVelocityWRTBodyValueChange(MEGUIParameterDropdownList sender, int newIndex)
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
	private bool CheckCrew(Vessel v, MissionKerbal missionKerbal)
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
