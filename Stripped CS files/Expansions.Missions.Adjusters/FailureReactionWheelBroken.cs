using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureReactionWheelBroken : AdjusterReactionWheelBase
{
	public FailureReactionWheelBroken()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100347";
	}

	public FailureReactionWheelBroken(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100347";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100348"));
	}

	public override bool IsReactionWheelBroken()
	{
		return true;
	}
}
