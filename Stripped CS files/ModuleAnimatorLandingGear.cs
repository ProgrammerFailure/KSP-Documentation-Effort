public class ModuleAnimatorLandingGear : PartModule
{
	[KSPField(isPersistant = true)]
	public float gearExtension;

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001868")]
	public void InputGearToggle()
	{
		if (gearExtension == 0f)
		{
			gearExtension = 1f;
			base.Events["InputGearToggle"].guiName = "#autoLOC_6001869";
		}
		else
		{
			gearExtension = 0f;
			base.Events["InputGearToggle"].guiName = "#autoLOC_6001870";
		}
	}

	public override void OnStart(StartState state)
	{
		if (gearExtension == 0f)
		{
			base.Events["InputGearToggle"].guiName = "#autoLOC_6001870";
		}
		else
		{
			base.Events["InputGearToggle"].guiName = "#autoLOC_6001869";
		}
	}
}
