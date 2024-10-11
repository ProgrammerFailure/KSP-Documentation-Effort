using System.Runtime.CompilerServices;

public class ModuleScienceConverter : BaseConverter
{
	[KSPField(guiActive = true, guiName = "#autoLOC_6001432")]
	public string sciString;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001433")]
	public string datString;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001434")]
	public string rateString;

	[KSPField]
	public float scientistBonus;

	[KSPField]
	public int researchTime;

	[KSPField]
	public int scienceMultiplier;

	[KSPField]
	public float dataProcessingMultiplier;

	[KSPField]
	public int scienceCap;

	[KSPField]
	public float powerRequirement;

	protected ModuleScienceLab _lab;

	private static string cacheAutoLOC_237911;

	private static string cacheAutoLOC_237928;

	private static string cacheAutoLOC_237931;

	public virtual ModuleScienceLab Lab
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleScienceConverter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string DataExpectationSummary(float dataAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CalculateScienceAmount(float dataAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CalculateScienceRate(float dataAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double CalculateResearchTime(float dataAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateDisplayStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsSituationValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ConversionRecipe PrepareRecipe(double deltatime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void PreProcessing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void PostProcess(ConverterResults result, double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetScientists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal new static void CacheLocalStrings()
	{
		throw null;
	}
}
