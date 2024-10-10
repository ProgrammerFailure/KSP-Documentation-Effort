using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureDecoupleCantDecouple : AdjusterDecoupleBase
{
	public FailureDecoupleCantDecouple()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100329";
	}

	public FailureDecoupleCantDecouple(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100329";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100341"));
	}

	public override bool IsBlockingDecouple()
	{
		return true;
	}
}
