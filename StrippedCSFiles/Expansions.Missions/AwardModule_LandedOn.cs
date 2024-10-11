using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class AwardModule_LandedOn : AwardModule
{
	[MEGUI_VesselSelect(gapDisplay = false, guiName = "#autoLOC_8000001")]
	public uint vesselID;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, guiName = "#autoLOC_8100017")]
	public double distance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_LandedOn(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AwardModule_LandedOn(MENode node, AwardDefinition definition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool EvaluateCondition(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vessel GetVessel()
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
