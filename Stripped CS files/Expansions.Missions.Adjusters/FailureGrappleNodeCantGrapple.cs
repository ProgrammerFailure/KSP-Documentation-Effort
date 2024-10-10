using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureGrappleNodeCantGrapple : AdjusterGrappleNodeBase
{
	public FailureGrappleNodeCantGrapple()
	{
		canBeRepaired = true;
		guiName = "#autoLOC_8100339";
	}

	public FailureGrappleNodeCantGrapple(MENode node)
		: base(node)
	{
		canBeRepaired = true;
		guiName = "#autoLOC_8100339";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100340"));
	}

	public override bool IsBlockingGrappleGrab()
	{
		return true;
	}
}
