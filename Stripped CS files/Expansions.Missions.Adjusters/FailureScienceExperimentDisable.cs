using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureScienceExperimentDisable : AdjusterScienceExperimentBase
{
	public FailureScienceExperimentDisable()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100355";
	}

	public FailureScienceExperimentDisable(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100355";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100356"));
	}
}
