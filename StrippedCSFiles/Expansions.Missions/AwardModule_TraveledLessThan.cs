using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_TraveledLessThan : AwardModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, guiName = "#autoLOC_8100017", Tooltip = "#autoLOC_8004206")]
	public double distance;

	protected double distanceTraveled;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_TraveledLessThan(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_TraveledLessThan(MENode node, AwardDefinition definition)
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
	private void OnVesselWillDestroy(Vessel data)
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
