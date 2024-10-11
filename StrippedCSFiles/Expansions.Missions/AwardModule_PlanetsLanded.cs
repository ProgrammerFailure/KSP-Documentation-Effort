using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_PlanetsLanded : AwardModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.IntegerNumber, guiName = "#autoLOC_8100022")]
	public int planetsLanded;

	protected List<string> landedOn;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_PlanetsLanded(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_PlanetsLanded(MENode node, AwardDefinition definition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool EvaluateCondition(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void StartTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void StopTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSitutationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
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
