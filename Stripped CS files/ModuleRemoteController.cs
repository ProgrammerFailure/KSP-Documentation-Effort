public class ModuleRemoteController : PartModule
{
	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_6001352")]
	public string controllerActive = "Inactive";

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_502068")]
	public void Toggle()
	{
		if (controllerActive == "Inactive")
		{
			controllerActive = "Active";
		}
		else if (controllerActive == "Active")
		{
			controllerActive = "Inactive";
		}
	}
}
