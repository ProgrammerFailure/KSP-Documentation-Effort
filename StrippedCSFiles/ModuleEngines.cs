using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using KSP.UI.Screens;
using UnityEngine;

public class ModuleEngines : PartModule, IEngineStatus, IModuleInfo, IResourceConsumer, IThrustProvider, IConstruction
{
	public List<Propellant> propellants;

	protected Dictionary<Propellant, ProtoStageIconInfo> PropellantGauges;

	public float mixtureDensity;

	public double mixtureDensityRecip;

	public double ratioSum;

	private int displayNameLimit;

	private bool isThrusting;

	[KSPField]
	public string engineID;

	[KSPField]
	public bool throttleLocked;

	[KSPField]
	public bool exhaustDamage;

	[KSPField]
	public bool exhaustDamageLogEvent;

	[KSPField]
	public bool exhaustSplashbackDamage;

	[KSPField]
	public double exhaustDamageMultiplier;

	[KSPField]
	public double exhaustDamageFalloffPower;

	[KSPField]
	public double exhaustDamageSplashbackFallofPower;

	[KSPField]
	public double exhaustDamageSplashbackMult;

	[KSPField]
	public double exhaustDamageSplashbackMaxMutliplier;

	[KSPField]
	public double exhaustDamageDistanceOffset;

	[KSPField]
	public float exhaustDamageMaxRange;

	[KSPField]
	public double exhaustDamageMaxMutliplier;

	public float minThrust;

	public float maxThrust;

	public float maxFuelFlow;

	public float minFuelFlow;

	[KSPField]
	public float ignitionThreshold;

	[KSPField]
	public bool clampPropReceived;

	[KSPField]
	public double clampPropReceivedMinLowerAmount;

	[KSPField]
	public bool allowRestart;

	[KSPField]
	public bool allowShutdown;

	[KSPField]
	public bool shieldedCanActivate;

	[KSPField]
	public bool autoPositionFX;

	[KSPField]
	public string fxGroupPrefix;

	[KSPField]
	public Vector3 fxOffset;

	[KSPField]
	public float heatProduction;

	[KSPField]
	public FloatCurve atmosphereCurve;

	[KSPField]
	public bool atmChangeFlow;

	[KSPField]
	public FloatCurve atmCurve;

	[KSPField]
	public bool useAtmCurve;

	[KSPField]
	public FloatCurve velCurve;

	[KSPField]
	public bool useVelCurve;

	[KSPField]
	public float CLAMP;

	[KSPField]
	public FloatCurve atmCurveIsp;

	[KSPField]
	public bool useAtmCurveIsp;

	[KSPField]
	public FloatCurve velCurveIsp;

	[KSPField]
	public bool useVelCurveIsp;

	[KSPField]
	public float machLimit;

	[KSPField]
	public float flameoutBar;

	[KSPField]
	public float machHeatMult;

	[KSPField]
	public float flowMultCap;

	[KSPField]
	public bool normalizeHeatForFlow;

	[KSPField]
	public float flowMultCapSharpness;

	[KSPField]
	public float multFlow;

	[KSPField]
	public float multIsp;

	[KSPField]
	public bool useThrustCurve;

	[KSPField]
	public FloatCurve thrustCurve;

	[KSPField]
	public bool useThrottleIspCurve;

	[KSPField]
	public FloatCurve throttleIspCurveAtmStrength;

	[KSPField]
	public FloatCurve throttleIspCurve;

	[KSPField(guiFormat = "P3", isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001830")]
	public float thrustCurveDisplay;

	[KSPField(guiFormat = "P3", isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001831")]
	public float thrustCurveRatio;

	[KSPField]
	public bool useEngineResponseTime;

	[KSPField]
	public float engineAccelerationSpeed;

	[KSPField]
	public float engineDecelerationSpeed;

	[KSPField]
	public bool throttleUseAlternate;

	public float throttleMin;

	[KSPField]
	public float throttleResponseRate;

	[KSPField]
	public float throttleIgniteLevelMult;

	[KSPField]
	public float throttleStartupMult;

	[KSPField]
	public float throttleStartedMult;

	[KSPField]
	public bool throttleInstantShutdown;

	[KSPField]
	public float throttleShutdownMult;

	[KSPField]
	public bool throttleInstant;

	[KSPField]
	public double throttlingBaseRate;

	[KSPField]
	public double throttlingBaseClamp;

	[KSPField]
	public double throttlingBaseDivisor;

	[KSPField]
	public string thrustVectorTransformName;

	public List<Transform> thrustTransforms;

	public List<float> thrustTransformMultipliers;

	[KSPField(guiFormat = "F5", guiActive = true, guiName = "#autoLOC_6001375", guiUnits = "#autoLOC_7001409")]
	public float fuelFlowGui;

	[KSPField(guiFormat = "F2", guiActive = false, guiName = "#autoLOC_6001376", guiUnits = "%")]
	public float propellantReqMet;

	[KSPField(guiFormat = "F1", guiActive = true, guiName = "#autoLOC_6001377", guiUnits = "#autoLOC_7001408")]
	public float finalThrust;

	[KSPField(guiFormat = "F1", guiActive = true, guiName = "#autoLOC_6001378", guiUnits = "#autoLOC_7001400")]
	public float realIsp;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
	public string status;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001379")]
	public string statusL2;

	[UI_Toggle(disabledText = "#autoLOC_6013010", scene = UI_Scene.All, enabledText = "#autoLOC_6005051", affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_900770")]
	public bool independentThrottle;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPAxisField(incrementalSpeed = 20f, isPersistant = true, maxValue = 100f, minValue = 0f, guiFormat = "F1", axisMode = KSPAxisMode.Incremental, guiActiveEditor = true, guiActive = true, guiName = "#autoLOC_900770")]
	public float independentThrottlePercentage;

	private UI_FloatRange independentThrottlePercentField;

	[KSPField]
	public bool disableUnderwater;

	[KSPField]
	public bool nonThrustMotor;

	public float resultingThrust;

	public float flowMultiplier;

	public float requestedMassFlow;

	public float requestedThrottle;

	protected static int damageLayerMask;

	protected BaseField statusL2Field;

	protected RaycastHit hit;

	public EngineType engineType;

	public float g;

	private BaseEvent shutdownEvent;

	private BaseEvent activateEvent;

	private BaseField propellantReqMetField;

	[KSPField(isPersistant = true)]
	public bool staged;

	[KSPField(isPersistant = true)]
	public bool flameout;

	[KSPField(isPersistant = true)]
	public bool EngineIgnited;

	[KSPField(isPersistant = true)]
	public bool engineShutdown;

	[KSPField(isPersistant = true)]
	public float currentThrottle;

	[UI_FloatRange(requireFullControl = true, stepIncrement = 0.5f, maxValue = 100f, minValue = 0f)]
	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001363")]
	public float thrustPercentage;

	[KSPField(isPersistant = true)]
	public bool manuallyOverridden;

	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public bool includeinDVCalcs;

	protected double massFlow;

	private bool listenersSet;

	public FXGroup flameoutGroup;

	public FXGroup runningGroup;

	public FXGroup powerGroup;

	public FXGroup engageGroup;

	public FXGroup disengageGroup;

	public List<FXGroup> runningGroups;

	public List<FXGroup> flameoutGroups;

	public List<FXGroup> powerGroups;

	public AudioSource powerSfx;

	protected List<PartResourceDefinition> consumedResources;

	private List<AdjusterEnginesBase> adjusterCache;

	private static string cacheAutoLOC_220473;

	private static string cacheAutoLOC_220477;

	private static string cacheAutoLOC_219016;

	private static string cacheAutoLOC_219034;

	private static string cacheAutoLOC_219636;

	private static string cacheAutoLOC_220370;

	private static string cacheAutoLOC_220377;

	private static string cacheAutoLOC_220657;

	private static string cacheAutoLOC_6001000;

	private static string cacheAutoLOC_6001001;

	private static string cacheAutoLOC_220748;

	private static string cacheAutoLOC_220761;

	private static string cacheAutoLOC_220762;

	public float normalizedThrustOutput
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool getFlameoutState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool getIgnitionState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isOperational
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float normalizedOutput
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float throttleSetting
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string engineName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleEngines()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupPropellant()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdatePropellantStatus(bool doGauge = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdatePropellantGauge(Propellant p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Flameout(string message, bool statusOnly = false, bool showFX = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UnFlameout(bool showFX = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CheckDeprived(double requiredPropellant, out string propName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestPropellant(double mass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001382")]
	public virtual void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001381")]
	public virtual void Shutdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "")]
	public virtual void ToggleIncludeinDV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001380", activeEditor = false)]
	public virtual void OnAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001381", activeEditor = false)]
	public virtual void ShutdownAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001382", activeEditor = false)]
	public virtual void ActivateAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6005052")]
	public virtual void ToggleThrottle(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void thrustPercentChanged(BaseField field, object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void throttleModeChanged(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BurstFlameoutGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetRunningGroupsActive(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPowerGroupsActive(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void HijackFX(FXGroup group, string groupName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupFXGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AutoPlaceFXGroup(FXGroup group, Transform thruster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void InitializeFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DeactivateLoopingFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DeactivateRunningFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DeactivatePowerFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateRunningFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivatePowerFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PlayEngageFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PlayShutdownFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PlayFlameoutFX(bool flamingOut)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FXReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FXUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void EngineExhaustDamage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateThrottle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double RequiredPropellantMass(float throttleAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float CalculateThrust()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool CheckTransformsUnderwater()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool TimeWarping()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ThrustUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnStageIconDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetListener()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ParticlesStop(List<ParticleSystem> particles)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateIncludeDVUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float MaxThrustOutputAtm(bool runningActive = false, bool useThrustLimiter = true, float atmPressure = 1f, double atmTemp = 310.0, double atmDensity = 1.225000023841858)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float MaxThrustOutputVac(bool useThrustLimiter = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double MassFlow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<Rect> GetDrawModulePanelCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetInfoThrust(bool mainInfoWindow)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPrimaryField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float getMaxFuelFlow(Propellant p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float getFuelFlow(Propellant p, float fuelFlow)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float getExhaustVelocity(float isp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetEngineThrust(float isp, float throttle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetThrottlingMult(float atm, float throttle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float ModifyFlow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnCenterOfThrustQuery(CenterOfThrustQuery qry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetMaxThrust()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCurrentThrust()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EngineType GetEngineType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
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
	internal float ApplyThrottleAdjustments(float throttle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsEngineDead()
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
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
