using CompoundParts;
using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureFuelLineBlocked : AdjusterFuelLineBase
{
	public FailureFuelLineBlocked()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100335";
	}

	public FailureFuelLineBlocked(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100335";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100336"));
		CModuleFuelLine cModuleFuelLine = adjustedModule as CModuleFuelLine;
		if (cModuleFuelLine != null)
		{
			cModuleFuelLine.CloseFuelLine();
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
		CModuleFuelLine cModuleFuelLine = adjustedModule as CModuleFuelLine;
		if (cModuleFuelLine != null)
		{
			cModuleFuelLine.OnTargetSet(cModuleFuelLine.target);
		}
	}
}
