using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using Radiators;

public class ModuleActiveRadiator : PartModule
{
	[KSPField(isPersistant = true)]
	public bool IsCooling;

	[KSPField]
	public double maxEnergyTransfer;

	private double originalMaxEnergyTranfer;

	[KSPField]
	public double overcoolFactor;

	[KSPField]
	public double energyTransferScale;

	[KSPField]
	public bool isCoreRadiator;

	[KSPField]
	public bool parentCoolingOnly;

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001414")]
	public string status;

	protected ModuleDeployableRadiator _depRad;

	private double statusValue;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001854")]
	public string D_CoolParts;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001855")]
	public string D_RadCount;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001856")]
	public string D_HeadRoom;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001857")]
	public string D_Excess;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001858")]
	public string D_XferBase;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001859")]
	public string D_XferFin;

	protected BaseField Dfld_RadCount;

	protected BaseField Dfld_CoolParts;

	protected BaseField Dfld_HeadRoom;

	protected BaseField Dfld_Excess;

	protected BaseField Dfld_XferBase;

	protected BaseField Dfld_XferFin;

	protected List<RadiatorData> coolParts;

	protected List<Part> hotParts;

	private List<AdjusterActiveRadiatorBase> adjusterCache;

	protected List<Part> activeRadiatorParts;

	protected List<Part> nonRadiatorParts;

	protected bool partCachesDirty;

	private static string cacheAutoLOC_6001415;

	private static string cacheAutoLOC_232021;

	private static string cacheAutoLOC_232036;

	private static string cacheAutoLOC_232067;

	private static string cacheAutoLOC_232071;

	private static string cacheAutoLOC_7000029;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleActiveRadiator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001416")]
	public virtual void ToggleRadiatorAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyRadiatorActivation(KSPActionType action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001417")]
	public virtual void ActivateAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001418")]
	public virtual void ShutdownAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001417")]
	public virtual void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001418")]
	public virtual void Shutdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateStatus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckIfDeployable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckDebugFields()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void InternalCooling(RadiatorData rad, int radCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsSibling(Part targetPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CheckResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
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
	protected bool IsAdjusterBlockingCooling()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double ApplyMaxEnergyTransferAdjustments(double maximumEnergyTransfer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnVesselStandardModification(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckPartCaches()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
