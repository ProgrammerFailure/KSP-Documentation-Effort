using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using FinePrint;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
internal class TestKerbalEVA : TestModule, IScoreableObjective, INodeWaypoint, ITestNodeLabel
{
	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	[MEGUI_ParameterSwitchCompound(excludeParamFields = "launchsiteName", guiName = "#autoLOC_8000149")]
	private ParamChoices_CelestialBodySurface situation;

	[MEGUI_VesselSelect(resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_8000074")]
	public uint vesselID;

	private bool eventFound;

	protected Vessel vessel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestKerbalEVA()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(TestGroup testGroup, string title)
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
	public void OnEVA(GameEvents.FromToAction<Part, Part> fromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool ShouldCreateCheckpoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeWaypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Waypoint GetNodeWaypoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeLabel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasWorldPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetWorldPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetExtraText()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetScoreModifier(Type scoreModule)
	{
		throw null;
	}
}
