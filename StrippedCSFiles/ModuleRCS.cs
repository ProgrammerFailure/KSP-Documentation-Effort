using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using KSP.UI.Screens;
using UnityEngine;

public class ModuleRCS : PartModule, IResourceConsumer, ITorqueProvider, IConstruction
{
	public List<Transform> thrusterTransforms;

	public List<FXGroup> thrusterFX;

	public float[] thrustForces;

	[KSPField]
	public string thrusterTransformName;

	[KSPField]
	public bool useZaxis;

	[KSPField]
	public float thrusterPower;

	[KSPField]
	public string resourceName;

	[KSPField]
	public string fxPrefabName;

	[KSPField]
	public string fxPrefix;

	[KSPField]
	public Vector3 fxOffset;

	[KSPField]
	public Vector3 fxOffsetRot;

	[KSPField]
	public bool isJustForShow;

	[KSPField]
	public bool requiresFuel;

	[KSPField]
	public bool shieldedCanThrust;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001362")]
	[UI_Toggle(disabledText = "#autoLOC_6001071", enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.Editor)]
	public bool rcsEnabled;

	[UI_FloatRange(stepIncrement = 0.5f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001363")]
	public float thrustPercentage;

	[KSPField]
	public float fullThrustMin;

	[KSPField]
	public bool useLever;

	[KSPField]
	public float precisionFactor;

	[KSPField]
	public bool useThrustCurve;

	[KSPField]
	public FloatCurve thrustCurve;

	[KSPField(guiFormat = "P3", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001830")]
	public float thrustCurveDisplay;

	[KSPField(guiFormat = "P3", guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001831")]
	public float thrustCurveRatio;

	[KSPField]
	public bool showToggles;

	[KSPField(isPersistant = true)]
	public bool currentShowToggles;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001331")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enableYaw;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001330")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enablePitch;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001332")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enableRoll;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001364")]
	public bool enableX;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001365")]
	public bool enableY;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001366")]
	public bool enableZ;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001367")]
	public bool useThrottle;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001368")]
	public bool fullThrust;

	[KSPField]
	public float EPSILON;

	[KSPField(guiFormat = "F1", guiActive = true, guiName = "#autoLOC_6001369", guiUnits = "#autoLOC_7001400")]
	public float realISP;

	[KSPField]
	public string resourceFlowMode;

	public double G;

	[KSPField]
	public FloatCurve atmosphereCurve;

	private float leverDistance;

	private Vector3 inputRot;

	private Vector3 inputLin;

	private Vector3 rot;

	protected int tC;

	private bool usePrecision;

	public bool rcs_active;

	public bool flameout;

	private double exhaustVel;

	public double maxFuelFlow;

	public double flowMult;

	public double ispMult;

	private float currentThrustForce;

	private float totalThrustForce;

	private float thrustForceRecip;

	private Transform currentThruster;

	private Vector3 predictedCOM;

	private Vector3 direction;

	private bool isOperating;

	private List<PartResourceDefinition> consumedResources;

	private Propellant p;

	public List<Propellant> propellants;

	protected Dictionary<Propellant, ProtoStageIconInfo> PropellantGauges;

	public double mixtureDensity;

	public double mixtureDensityRecip;

	private float ignitionThreshold;

	private List<AdjusterRCSBase> adjusterCache;

	private static string cacheAutoLOC_217939;

	private static string cacheAutoLOC_217950;

	private static string cacheAutoLOC_7003240;

	private static string cacheAutoLOC_7003241;

	private static string cacheAutoLOC_218287;

	private static string cacheAutoLOC_218288;

	private static string cacheAutoLOC_6003050;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleRCS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001384")]
	public void ToggleToggles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001370")]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetLeverDistanceOriginal(Vector3 leverPivotPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetLeverDistance(Transform trf, Vector3 axis, Vector3 leverPivotPosition)
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
	public override void OnLoad(ConfigNode node)
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
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetResource(string resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindThrusters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double getMaxFuelFlow(float Isp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double getMaxFuelFlow(Propellant p, float Isp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateToggles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float CalculateThrust(float totalForce, out bool success)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupPropellant(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePropellantStatus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePropellantGauge(Propellant p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RequestPropellant(double mass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
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
	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdatePowerFX(bool running, int idx, float power)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DeactivateFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DeactivatePowerFX()
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
	protected Vector3 ApplyInputRotationAdjustments(Vector3 inputRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 ApplyInputLinearAdjustments(Vector3 inputLinear)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool IsAdjusterBreakingRCS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
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
