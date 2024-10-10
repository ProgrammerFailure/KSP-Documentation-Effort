using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureControlSurfaceDisableControl : AdjusterControlSurfaceBase
{
	public FailureControlSurfaceDisableControl()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100325");
	}

	public FailureControlSurfaceDisableControl(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100325");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100326"));
	}

	public override float ApplyActuatorSpeedAdjustment(float actuatorSpeed)
	{
		return 0f;
	}
}
