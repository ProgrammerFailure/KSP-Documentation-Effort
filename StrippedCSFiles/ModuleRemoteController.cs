using System.Runtime.CompilerServices;

public class ModuleRemoteController : PartModule
{
	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_6001352")]
	public string controllerActive;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleRemoteController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_502068")]
	public void Toggle()
	{
		throw null;
	}
}
