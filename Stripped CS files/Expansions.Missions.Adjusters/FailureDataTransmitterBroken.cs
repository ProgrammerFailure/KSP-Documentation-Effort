using ns9;

namespace Expansions.Missions.Adjusters;

public class FailureDataTransmitterBroken : AdjusterDataTransmitterBase
{
	public FailureDataTransmitterBroken()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100327";
	}

	public FailureDataTransmitterBroken(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100327";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100328"));
		ModuleDataTransmitter moduleDataTransmitter = adjustedModule as ModuleDataTransmitter;
		if (moduleDataTransmitter != null && moduleDataTransmitter.IsBusy())
		{
			moduleDataTransmitter.StopTransmission();
		}
	}

	public override bool IsDataTransmitterBroken()
	{
		return true;
	}
}
