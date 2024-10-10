using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureAnchoredDecouplerCantDecouple : AdjusterAnchoredDecouplerBase
{
	public FailureAnchoredDecouplerCantDecouple()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100322");
	}

	public FailureAnchoredDecouplerCantDecouple(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100322");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100323"));
	}

	public override bool IsBlockingDecouple()
	{
		return true;
	}
}
