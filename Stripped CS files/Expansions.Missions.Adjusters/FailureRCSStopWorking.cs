using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureRCSStopWorking : AdjusterRCSBase
{
	public FailureRCSStopWorking()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100345";
	}

	public FailureRCSStopWorking(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100345";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100346"));
	}

	public override bool IsRCSBroken()
	{
		return true;
	}
}
