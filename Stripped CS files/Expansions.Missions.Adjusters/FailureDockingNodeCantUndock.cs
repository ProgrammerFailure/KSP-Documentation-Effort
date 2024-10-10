using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureDockingNodeCantUndock : AdjusterDockingNodeBase
{
	public FailureDockingNodeCantUndock()
	{
		canBeRepaired = true;
		guiName = "#autoLOC_8100331";
	}

	public FailureDockingNodeCantUndock(MENode node)
		: base(node)
	{
		canBeRepaired = true;
		guiName = "#autoLOC_8100331";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100332"));
	}

	public override bool IsBlockingUndock()
	{
		return true;
	}
}
