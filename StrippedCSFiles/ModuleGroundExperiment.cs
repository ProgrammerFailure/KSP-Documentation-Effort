using System.Runtime.CompilerServices;
using Expansions.Serenity.DeployedScience.Runtime;
using UnityEngine;

public class ModuleGroundExperiment : ModuleGroundSciencePart
{
	[KSPField]
	public string experimentId;

	[KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false)]
	[SerializeField]
	protected float scienceLimit;

	[SerializeField]
	protected float scienceValue;

	[KSPField(unfocusedRange = 20f, guiActiveUnfocused = true, isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8002248", guiUnits = "#autoLOC_8002249")]
	[SerializeField]
	protected float scienceValueDisplay;

	[KSPField(guiActiveUnfocused = true, guiFormat = "F2", isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002250", guiUnits = "%")]
	public float ScienceCompletedPercentage;

	[KSPField(guiActiveUnfocused = true, guiFormat = "F2", isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002257", guiUnits = "%")]
	public float ScienceTransmittedPercentage;

	[KSPField(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002245", guiUnits = "%")]
	public float ScienceModifierRate;

	[KSPField]
	public FloatCurve distanceCurve;

	private DeployedScienceExperiment scienceExperiment;

	private DeployedSciencePart deployedSciencePart;

	private ScienceExperiment experiment;

	private static string cacheAutoLOC_8002251;

	public float ScienceLimit
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

	public float ScienceValue
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
	public ModuleGroundExperiment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceGenerated(DeployedScienceExperiment scienceExperiment, DeployedSciencePart sciencePart, DeployedScienceCluster cluster, float scienceGenerated)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceTransmitted(DeployedScienceExperiment scienceExperiment, DeployedSciencePart sciencePart, DeployedScienceCluster cluster, float scienceTransmitted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdateModuleUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RetrievePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
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
