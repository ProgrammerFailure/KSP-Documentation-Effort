using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureGPSNoDisplay : AdjusterGPSBase
{
	public FailureGPSNoDisplay()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100337");
	}

	public FailureGPSNoDisplay(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100337");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100338"));
	}
}
