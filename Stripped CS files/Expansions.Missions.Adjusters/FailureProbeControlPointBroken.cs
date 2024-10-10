namespace Expansions.Missions.Adjusters;

public class FailureProbeControlPointBroken : AdjusterProbeControlPointBase
{
	public FailureProbeControlPointBroken()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100342";
	}

	public FailureProbeControlPointBroken(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100342";
	}

	public override bool IsProbeControlPointBroken()
	{
		return true;
	}
}
