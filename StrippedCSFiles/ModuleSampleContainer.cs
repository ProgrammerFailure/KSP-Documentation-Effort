using System.Runtime.CompilerServices;

public class ModuleSampleContainer : PartModule
{
	public PlanetarySample sample;

	[KSPField(isPersistant = true)]
	public bool sampleHeld;

	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_6001872")]
	public string heldSampleName;

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001873", guiUnits = "#autoLOC_7001412")]
	public float heldSampleMass;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSampleContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TransferSample(PlanetarySample transferee)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001874")]
	public void Discard()
	{
		throw null;
	}
}
