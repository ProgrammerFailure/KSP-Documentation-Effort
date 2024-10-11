using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleSampleCollector : PartModule
{
	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_6001872")]
	public string collectedSampleName;

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001873", guiUnits = "#autoLOC_7001412")]
	public float collectedSampleMass;

	[KSPField]
	public float sampleMassMin;

	[KSPField]
	public float sampleMassMax;

	[KSPField(isPersistant = true)]
	public bool sampleCollected;

	public PlanetarySample sample;

	private ScreenMessage warningMessage;

	public List<ModuleSampleContainer> containers;

	private bool showTransferDialog;

	private Rect transferDialogRect;

	private Vector2 scrollPos;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSampleCollector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001890")]
	public void Collect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001891")]
	public void Discard()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void drawTransferDialog(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_900235")]
	public void Transfer()
	{
		throw null;
	}
}
