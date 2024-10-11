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
internal class TestKerbalRescue : TestModule, IScoreableObjective
{
	[MEGUI_MissionKerbal(statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = true, showAllRosterStatus = true, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	[MEGUI_Checkbox(canBePinned = true, guiName = "#autoLOC_8002025", Tooltip = "#autoLOC_8002026")]
	public bool mustBoardVessel;

	private bool eventFound;

	private List<ProtoCrewMember> pcm;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestKerbalRescue()
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
	private void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckNewKerbal(string rescuedName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onKnowledgeChanged(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> kChg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test()
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
