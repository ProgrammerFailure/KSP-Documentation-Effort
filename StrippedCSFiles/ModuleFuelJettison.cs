using System.Runtime.CompilerServices;

public class ModuleFuelJettison : PartModule
{
	[KSPField]
	public string ResourceName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleFuelJettison()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 5f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001475")]
	public void FuelJettison()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dump(PartResource res)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001475", activeEditor = false)]
	public void FuelJettisonAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}
}
