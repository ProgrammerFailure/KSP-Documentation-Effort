using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleDeployableSolarPanel : ModuleDeployablePart, IContractObjectiveModule
{
	public enum PanelType
	{
		FLAT,
		CYLINDRICAL,
		SPHERICAL
	}

	[KSPField]
	public PanelType panelType;

	[KSPField]
	public float raycastOffset;

	[KSPField]
	public string resourceName;

	[KSPField]
	public float chargeRate;

	[KSPField]
	public string raycastTransformName;

	[KSPField]
	public bool useRaycastForTrackingDot;

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001420", guiUnits = " ")]
	public float sunAOA;

	[KSPField(guiFormat = "F3", guiActive = true, guiName = "#autoLOC_6001421", guiUnits = " ")]
	public float flowRate;

	public double _flowRate;

	[KSPField]
	public string flowUnits;

	[KSPField]
	public bool flowUnitsUseSpace;

	[KSPField]
	public string flowFormat;

	[KSPField]
	public float flowMult;

	[KSPField]
	public bool showInfo;

	[KSPField]
	public double resMultForGetInfo;

	[KSPField]
	public FloatCurve powerCurve;

	[KSPField]
	public FloatCurve temperatureEfficCurve;

	[KSPField]
	public FloatCurve timeEfficCurve;

	[KSPField(isPersistant = true)]
	public float efficiencyMult;

	private float originalEfficiencyMultiplier;

	[KSPField(isPersistant = true)]
	public double launchUT;

	public double _distMult;

	public double _efficMult;

	public Transform trackingDotTransform;

	protected int planetLayerMask;

	protected int defaultLayerMask;

	public RaycastHit hit;

	private List<AdjusterDeployableSolarPanelBase> adjusterCache;

	private static string cacheAutoLOC_438839;

	private static string cacheAutoLOC_235418;

	private static string cacheAutoLOC_235468;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDeployableSolarPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private new void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PostFSMUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CalculateTrackingLOS(Vector3 trackingDirection, ref string blocker)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PostCalculateTracking(bool trackingLOS, Vector3 trackingDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetContractObjectiveType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckContractObjectiveValidity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartRepaired(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
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
	protected float ApplyEfficiencyAdjustments(float efficiency)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal new static void CacheLocalStrings()
	{
		throw null;
	}
}
