using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_MissionTime : AwardModule
{
	[MEGUI_Time(guiName = "#autoLOC_8000188")]
	public double missionTime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_MissionTime(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_MissionTime(MENode node, AwardDefinition definition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool EvaluateCondition(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}
}
