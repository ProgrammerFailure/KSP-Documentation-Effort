using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleDecouplerBase : PartModule, IModuleInfo, IStageSeparator, IStageSeparatorChild, IAirstreamShield
{
	[KSPField]
	public float ejectionForce;

	[UI_FloatRange(minValue = 0f, stepIncrement = 1f, maxValue = 100f)]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001442")]
	public float ejectionForcePercent;

	[KSPField(isPersistant = true)]
	public bool isDecoupled;

	[KSPField]
	public bool staged;

	[KSPField]
	public bool partDecoupled;

	[KSPField]
	public bool isEnginePlate;

	[KSPField]
	public string fxGroupName;

	[KSPField]
	public bool isOmniDecoupler;

	[KSPField]
	public string explosiveNodeID;

	protected FXGroup fx;

	protected AttachNode explosiveNode;

	protected ModuleJettison jettisonModule;

	protected bool shroudOn;

	protected bool refreshStaging;

	private List<AdjusterDecoupleBase> adjusterCache;

	public AttachNode ExplosiveNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDecouplerBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001443", activeEditor = false)]
	public void DecoupleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001443")]
	public void Decouple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselWasModified(Vessel vsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckShielded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnJettison(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAirstream(bool enclosed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDecouple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<Rect> GetDrawModulePanelCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPrimaryField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStageIndex(int fallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PartDetaches(out List<Part> decoupledParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsEnginePlate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ClosedAndLocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vessel GetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part GetPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingEnabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingToggleEnabledEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool StagingToggleEnabledFlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingEnableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingDisableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool IsAdjusterBlockingDecouple()
	{
		throw null;
	}
}
