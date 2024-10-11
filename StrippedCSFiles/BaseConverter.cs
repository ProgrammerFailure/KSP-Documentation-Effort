using System.Collections.Generic;
using System.Runtime.CompilerServices;

public abstract class BaseConverter : PartModule, IAnimatedModule, IOverheatDisplay, IConstruction
{
	public List<ResourceRatio> inputList;

	public List<ResourceRatio> outputList;

	public List<ResourceRatio> reqList;

	[KSPField]
	public string ConverterName;

	[KSPField]
	public bool GeneratesHeat;

	[KSPField]
	public bool UseSpecialistBonus;

	[KSPField]
	public bool UseSpecialistHeatBonus;

	[KSPField]
	public float SpecialistBonusBase;

	[KSPField]
	public bool AutoShutdown;

	[KSPField]
	public bool DirtyFlag;

	[KSPField(isPersistant = true)]
	public float EfficiencyBonus;

	[KSPField]
	public float FillAmount;

	[KSPField(isPersistant = true)]
	public bool IsActivated;

	[KSPField]
	public string StartActionName;

	[KSPField]
	public string StopActionName;

	[KSPField]
	public string ToggleActionName;

	[KSPField]
	public string resourceOutputName;

	[KSPField]
	public float TakeAmount;

	[KSPField]
	public bool AlwaysActive;

	[KSPField]
	public FloatCurve ThermalEfficiency;

	[KSPField]
	public FloatCurve TemperatureModifier;

	[KSPField]
	public float SpecialistEfficiencyFactor;

	[KSPField]
	public float SpecialistHeatFactor;

	[KSPField]
	public float DefaultShutoffTemp;

	[KSPField]
	public string ExperienceEffect;

	protected IResourceBroker _resBroker;

	protected ResourceConverter _resConverter;

	protected double lastUpdateTime;

	public double lastHeatFlux;

	public double lastTimeFactor;

	protected double statusPercent;

	protected BaseEvent startEvt;

	protected BaseEvent stopEvt;

	protected bool _preCalculateEfficiency;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "")]
	public string status;

	public Dictionary<string, float> EfficiencyModifiers;

	protected float _totalEfficiencyModifiers;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001883")]
	public string debugEffBonus;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001884")]
	public string debugDelta;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001885")]
	public string debugTimeFac;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001886")]
	public string debugFinBon;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001887")]
	public string debugCrewBon;

	protected ModuleAnimationGroup partAnimationModule;

	protected bool hasAnimationGroup;

	private int catchupRetries;

	private const int MAX_CATCHUP_RETRIES = 10;

	private static string cacheAutoLOC_257237;

	public virtual ResourceConverter ResConverter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual IResourceBroker ResBroker
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected BaseConverter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateDebugInfo(ConverterResults result, double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetCrewEfficiencyBonus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetCrewHeatBonus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupDebugging()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void EnableModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DisableModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ModuleIsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsAnimationGroupDeployed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = false, guiActive = true, guiName = "#autoLOC_6001471")]
	public virtual void StartResourceConverter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = false, guiActive = true, guiName = "#autoLOC_6001472")]
	public virtual void StopResourceConverter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001472")]
	public virtual void StopResourceConverterAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001471")]
	public virtual void StartResourceConverterAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001473")]
	public virtual void ToggleResourceConverterAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetEfficiencyBonus(float bonus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual double GetDeltaTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double GetBestDeltaTime(double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateConverterStatus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupLabels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetEfficiencyMultiplier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void TallyEfficiencyModifiers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ConvertRecipeToUnits(ConversionRecipe recipe)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetHeatThrottle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CheckForShutdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual int HasSpecialist(string effect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsOverheating()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PreProcessing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PostUpdateCleanup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PostProcess(ConverterResults result, double deltaTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual ConversionRecipe PrepareRecipe(double deltatime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsSituationValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnOverheat(double amount = 1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetFlux()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool DisplayCoreHeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetCoreTemperature()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetGoalTemperature()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeDetached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanBeRotated()
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
