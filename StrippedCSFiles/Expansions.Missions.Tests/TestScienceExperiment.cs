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
public class TestScienceExperiment : TestModule, IScoreableObjective, INodeBody
{
	[MEGUI_Dropdown(addDefaultOption = true, SetDropDownItems = "TSEExpId_SetDropDownValues", defaultDisplayString = "#autoLOC_8002050", guiName = "#autoLOC_8000134")]
	public string experimentID;

	[MEGUI_Dropdown(addDefaultOption = true, SetDropDownItems = "TSEExpSit_SetDropDownValues", defaultDisplayString = "#autoLOC_8000258", guiName = "#autoLOC_8000135")]
	public string experimentSituation;

	[MEGUI_CelestialBody_Biomes(gapDisplay = true, guiName = "#autoLOC_8000136")]
	public MissionBiome biomeData;

	private bool testsSuccessful;

	[MEGUI_Dropdown(addDefaultOption = false, SetDropDownItems = "TSEDoR_SetDropDownValues", guiName = "#autoLOC_8000137", Tooltip = "#autoLOC_8003055")]
	public string DeployorReceived;

	private bool passedTests;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestScienceExperiment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
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
	public void OnScienceReceived(float scienceValue, ScienceSubject scienceSubjectIn, ProtoVessel vessel, bool reverseEngineered)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnExperimentDeployed(ScienceData scienceDataIn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConductScienceTest(string inputExperimentId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> TSEExpId_SetDropDownValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> TSEExpSit_SetDropDownValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> TSEDoR_SetDropDownValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBody GetNodeBody()
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
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetAppObjectiveInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RunValidation(MissionEditorValidator validator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetScoreModifier(Type scoreModule)
	{
		throw null;
	}
}
