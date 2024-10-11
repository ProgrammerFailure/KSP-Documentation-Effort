using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleAsteroidDrill : BaseDrill
{
	[KSPField(isPersistant = true)]
	public bool DirectAttach;

	[KSPField]
	public float PowerConsumption;

	[KSPField]
	public bool RockOnly;

	protected bool _isValidSituation;

	protected Transform impactTransformCache;

	private ConversionRecipe recipe;

	private double _drilledMass;

	private string _status;

	private RaycastHit impactHitInfo;

	private Part _potato;

	private ModuleAsteroidInfo _info;

	private static string cacheAutoLOC_258405;

	private static string cacheAutoLOC_258412;

	private static string cacheAutoLOC_258419;

	private static string cacheAutoLOC_258428;

	private static string cacheAutoLOC_258436;

	private static string cacheAutoLOC_258443;

	private static string cacheAutoLOC_258451;

	private static string cacheAutoLOC_258501;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAsteroidDrill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ConversionRecipe PrepareRecipe(double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CheckForImpact()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Part GetAttachedPotato()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsSituationValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
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
	protected override void PostProcess(ConverterResults result, double deltaTime)
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
