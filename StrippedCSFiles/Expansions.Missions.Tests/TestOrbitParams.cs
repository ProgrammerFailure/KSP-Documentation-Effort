using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
public class TestOrbitParams : TestVessel, IScoreableObjective
{
	[MEGUI_Checkbox(group = "Apoapsis", onValueChange = "onCheckApoapsisValueChanged", order = 10, groupDisplayName = "#autoLOC_8100059", onControlCreated = "onCheckApoapsisCreated", guiName = "#autoLOC_8002064", Tooltip = "#autoLOC_8002065")]
	public bool checkApoapsis;

	[MEGUI_NumberRange(onControlSetupComplete = "onApoapsisMinCreated", groupDisplayName = "#autoLOC_8200059", canBePinned = false, group = "Apoapsis", resetValue = "70000", onValueChange = "onApMinValueChanged", order = 11, hideOnSetup = true, guiName = "#autoLOC_8002068", Tooltip = "#autoLOC_8002069")]
	public double apMinValue;

	[MEGUI_NumberRange(onControlSetupComplete = "onApoapsisMaxCreated", groupDisplayName = "#autoLOC_8200059", canBePinned = false, group = "Apoapsis", resetValue = "100000", onValueChange = "onApMaxValueChanged", order = 12, hideOnSetup = true, guiName = "#autoLOC_8002070", Tooltip = "#autoLOC_8002071")]
	public double apMaxValue;

	[MEGUI_Checkbox(group = "Periapsis", onValueChange = "onCheckPeriapsisValueChanged", order = 13, groupDisplayName = "#autoLOC_8100060", guiName = "#autoLOC_8002066", Tooltip = "#autoLOC_8002067")]
	public bool checkPeriapsis;

	[MEGUI_NumberRange(onControlSetupComplete = "onPeriapsisMinCreated", groupDisplayName = "#autoLOC_8100060", canBePinned = false, group = "Periapsis", resetValue = "70000", onValueChange = "onPeMinValueChanged", order = 14, hideOnSetup = true, guiName = "#autoLOC_8002072", Tooltip = "#autoLOC_8002073")]
	public double peMinValue;

	[MEGUI_NumberRange(onControlSetupComplete = "onPeriapsisMaxCreated", groupDisplayName = "#autoLOC_8100060", canBePinned = false, group = "Periapsis", resetValue = "100000", onValueChange = "onPeMaxValueChanged", order = 15, hideOnSetup = true, guiName = "#autoLOC_8002074", Tooltip = "#autoLOC_8002075")]
	public double peMaxValue;

	[MEGUI_Checkbox(group = "Inclination", onValueChange = "onCheckIncValueChanged", order = 16, groupDisplayName = "#autoLOC_8100062", guiName = "#autoLOC_8002076", Tooltip = "#autoLOC_8002077")]
	public bool checkInclination;

	[MEGUI_NumberRange(resetValue = "0", onControlSetupComplete = "onInclinationCreated", clampTextInput = true, maxValue = 180f, groupDisplayName = "#autoLOC_8100062", canBePinned = false, displayUnits = "°", minValue = 0f, group = "Inclination", displayFormat = "0.##", onValueChange = "onIncValueChanged", order = 17, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_8100062", Tooltip = "#autoLOC_8002078")]
	public double inclination;

	[MEGUI_NumberRange(resetValue = "90", onControlCreated = "onInclinationAccuracyCreated", clampTextInput = true, maxValue = 100f, groupDisplayName = "#autoLOC_8100062", canBePinned = false, displayUnits = "%", minValue = 0f, group = "Inclination", displayFormat = "0.##", onValueChange = "OnInclinationAccuracyChanged", order = 18, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_8002079", Tooltip = "#autoLOC_8002080")]
	public double inclinationAccuracy;

	[MEGUI_Checkbox(group = "LAN", onValueChange = "onCheckLANValueChanged", order = 19, groupDisplayName = "#autoLOC_900371", guiName = "#autoLOC_8002081", Tooltip = "#autoLOC_8002082")]
	public bool checkLAN;

	[MEGUI_NumberRange(resetValue = "90", onControlSetupComplete = "onLANCreated", clampTextInput = true, maxValue = 360f, groupDisplayName = "#autoLOC_900371", canBePinned = false, displayUnits = "°", minValue = 0f, group = "LAN", displayFormat = "0.##", onValueChange = "onLANValueChanged", order = 20, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_900371", Tooltip = "#autoLOC_8002083")]
	public double LAN;

	[MEGUI_NumberRange(resetValue = "90", onControlCreated = "onLANAccuracyCreated", clampTextInput = true, maxValue = 100f, groupDisplayName = "#autoLOC_900371", canBePinned = false, displayUnits = "%", minValue = 0f, group = "LAN", displayFormat = "0.##", onValueChange = "OnLANAccuracyChanged", order = 21, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_8002084", Tooltip = "#autoLOC_8002085")]
	public double LANAccuracy;

	[MEGUI_CelestialBody(gapDisplay = false, order = 22, onControlSetupComplete = "onBodyCreated", canBePinned = true, resetValue = "0", showAnySOIoption = true, guiName = "#autoLOC_8000263", Tooltip = "#autoLOC_8000157")]
	public MissionCelestialBody body;

	[MEGUI_Time(order = 23, canBePinned = true, resetValue = "5", guiName = "#autoLOC_8003019", Tooltip = "#autoLOC_8003061")]
	protected double stabilizationTime;

	[MEGUI_Checkbox(order = 24, canBePinned = true, resetValue = "False", guiName = "#autoLOC_8003062", Tooltip = "#autoLOC_8003063")]
	protected bool underThrust;

	protected bool situationSuccess;

	protected bool orbitSuccess;

	protected double successStartTime;

	private MEGUIParameterNumberRange apMinInstance;

	private MEGUIParameterNumberRange apMaxInstance;

	private MEGUIParameterNumberRange peMinInstance;

	private MEGUIParameterNumberRange peMaxInstance;

	private MEGUIParameterNumberRange incInstance;

	private MEGUIParameterNumberRange inclinationAccuracyInstance;

	private MEGUIParameterNumberRange lanInstance;

	private MEGUIParameterNumberRange lanAccuracyInstance;

	private MEGUIParameterCelestialBody bodyInstance;

	private MEGUIParameterDropdownList bodyDropdownInstance;

	public double InclinationDeviation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double LANDeviation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestOrbitParams()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
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
	private void onApoapsisMinCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onApoapsisMaxCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPeriapsisMinCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPeriapsisMaxCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onInclinationCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLANCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onInclinationAccuracyCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLANAccuracyCreated(MEGUIParameterNumberRange parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onBodyCreated(MEGUIParameterCelestialBody parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCheckApoapsisValueChanged(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCheckPeriapsisValueChanged(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCheckIncValueChanged(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCheckLANValueChanged(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onApMinValueChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onApMaxValueChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPeMinValueChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPeMaxValueChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onIncValueChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLANValueChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInclinationAccuracyChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLANAccuracyChanged(double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBodyChanged(int newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetMaxAp(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void toggleApoapsisParms(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void togglePeriapsisParms(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void toggleIncParms(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void togglelLANParms(bool on)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetScoreModifier(Type scoreModule)
	{
		throw null;
	}
}
