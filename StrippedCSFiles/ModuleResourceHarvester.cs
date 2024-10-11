using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleResourceHarvester : BaseDrill, IResourceConsumer
{
	[KSPField]
	public bool CausesDepletion;

	[KSPField]
	public float DepletionRate;

	[KSPField]
	public float HarvestThreshold;

	[KSPField]
	public int HarvesterType;

	[KSPField]
	public string ResourceName;

	[KSPField]
	public double airSpeedStatic;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "")]
	public string ResourceStatus;

	protected double _resFlow;

	protected double _displayedResFlow;

	protected bool _isValidSituation;

	protected Transform impactTransformCache;

	public string intakeTransformName;

	public Transform intakeTransform;

	protected ConversionRecipe recipe;

	private RaycastHit impactHitInfo;

	public ResourceRatio eInput;

	public bool hasEInput;

	protected bool cachedWasNotAsteroid;

	protected bool cachedWasNotComet;

	protected int partCountCache;

	private static string cacheAutoLOC_259762;

	private static string cacheAutoLOC_259775;

	private static string cacheAutoLOC_259782;

	private static string cacheAutoLOC_259789;

	private static string cacheAutoLOC_259808;

	private static string cacheAutoLOC_259819;

	private static string cacheAutoLOC_259826;

	private static string cacheAutoLOC_259862;

	private static string cacheAutoLOC_259933;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleResourceHarvester()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetLocationString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ConversionRecipe PrepareRecipe(double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetIntakeMultiplier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void PostUpdateCleanup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LoadRecipe(double harvestRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void PostProcess(ConverterResults result, double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CheckForImpact()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsSituationValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void PreProcessing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal new static void CacheLocalStrings()
	{
		throw null;
	}
}
