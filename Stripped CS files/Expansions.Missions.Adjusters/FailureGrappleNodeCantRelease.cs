using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureGrappleNodeCantRelease : AdjusterGrappleNodeBase
{
	public FailureGrappleNodeCantRelease()
	{
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100320");
	}

	public FailureGrappleNodeCantRelease(MENode node)
		: base(node)
	{
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100320");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100321"));
		ModuleGrappleNode moduleGrappleNode = adjustedModule as ModuleGrappleNode;
		if (moduleGrappleNode != null)
		{
			moduleGrappleNode.SetUpAdjusterBlockingGrappleNodeAbilityToRelease();
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
		ModuleGrappleNode moduleGrappleNode = adjustedModule as ModuleGrappleNode;
		if (moduleGrappleNode != null)
		{
			moduleGrappleNode.RemoveAdjusterBlockingGrappleNodeAbilityToRelease();
		}
	}

	public override bool IsBlockingGrappleRelease()
	{
		return true;
	}
}
