public class ModuleSampleContainer : PartModule
{
	public PlanetarySample sample;

	[KSPField(isPersistant = true)]
	public bool sampleHeld;

	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_6001872")]
	public string heldSampleName = "N/A";

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001873", guiUnits = "#autoLOC_7001412")]
	public float heldSampleMass;

	public override void OnStart(StartState state)
	{
		if (state != StartState.Editor && state != 0)
		{
			base.Events["Discard"].active = sampleHeld;
		}
	}

	public void TransferSample(PlanetarySample transferee)
	{
		heldSampleName = transferee.sampleName;
		heldSampleMass = transferee.sampleMass;
		sample = transferee;
		sampleHeld = true;
		base.Events["Discard"].active = true;
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001874")]
	public void Discard()
	{
		sample = null;
		heldSampleMass = 0f;
		heldSampleName = "N/A";
		sampleHeld = false;
		base.Events["Discard"].active = false;
	}
}
