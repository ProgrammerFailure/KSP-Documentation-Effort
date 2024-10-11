using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleParachute : PartModule, IMultipleDragCube
{
	public enum deploymentStates
	{
		STOWED,
		ACTIVE,
		SEMIDEPLOYED,
		DEPLOYED,
		CUT
	}

	public enum deploymentSafeStates
	{
		SAFE,
		RISKY,
		UNSAFE,
		NONE
	}

	public deploymentStates deploymentState;

	public deploymentSafeStates deploymentSafeState;

	[KSPField]
	public bool invertCanopy;

	[KSPField]
	public string semiDeployedAnimation;

	[KSPField]
	public string fullyDeployedAnimation;

	[KSPField]
	public float autoCutSpeed;

	public float rotationSpeedDPS;

	[KSPField]
	public string capName;

	[KSPField]
	public string canopyName;

	[KSPField(isPersistant = true)]
	public string persistentState;

	[KSPField]
	public float stowedDrag;

	[KSPField]
	public float semiDeployedDrag;

	[KSPField]
	public float fullyDeployedDrag;

	[KSPField(isPersistant = true)]
	public float animTime;

	[KSPField]
	public float clampMinAirPressure;

	[UI_FloatRange(stepIncrement = 0.01f, maxValue = 0.75f, minValue = 0.01f)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001340")]
	public float minAirPressureToOpen;

	private static double pressureRecip;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_438841")]
	[UI_FloatRange(stepIncrement = 50f, maxValue = 5000f, minValue = 50f)]
	public float deployAltitude;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 10f, minValue = 0f)]
	[KSPField(guiActiveEditor = true, isPersistant = true, guiActive = true, advancedTweakable = true, guiName = "#autoLOC_6001341")]
	public float spreadAngle;

	[KSPField]
	public float deploymentSpeed;

	[KSPField]
	public float deploymentCurve;

	[KSPField]
	public float semiDeploymentSpeed;

	[KSPField]
	public double chuteMaxTemp;

	[KSPField]
	public double chuteThermalMassPerArea;

	[KSPField]
	public double startingTemp;

	[KSPField]
	public double chuteEmissivity;

	[KSPField]
	public double chuteTemp;

	[KSPField(guiName = "#autoLOC_6001342")]
	public string deploySafe;

	[KSPField]
	public double machHeatMultBase;

	[KSPField]
	public double machHeatMultScalar;

	[KSPField]
	public double machHeatMultPow;

	[KSPField]
	public double machHeatDensityFadeoutMult;

	[KSPField]
	public double secondsForRisky;

	[KSPField]
	public double safeMult;

	[UI_Cycle(stateNames = new string[] { "#autoLOC_6001344", "#autoLOC_6001345", "#autoLOC_6001346" }, controlEnabled = true, scene = UI_Scene.All, affectSymCounterparts = UI_Scene.All)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001343")]
	public int automateSafeDeploy;

	[KSPField]
	public bool shieldedCanDeploy;

	[KSPField]
	public double refDensity;

	[KSPField]
	public double refSpeedOfSound;

	protected Transform canopy;

	protected Transform cap;

	private FXGroup deployFx;

	private float lerpTime;

	private float baseDrag;

	public int symmetryCount;

	public Animation Anim;

	public Quaternion lastRot;

	public double areaSemi;

	public double areaDeployed;

	public double maxSafeSpeedAtRef;

	public double invThermalMass;

	public double machHeatMult;

	public double convectivekW;

	public double shockTemp;

	public double convectionArea;

	public double finalTemp;

	protected bool dontRotateParachute;

	protected bool deactivateOnRepack;

	private double timeUnpacked;

	private Vessel currentVessel;

	private BaseField deployfield;

	private static string cacheAutoLOC_7003409;

	private static string cacheAutoLOC_7003410;

	private static string cacheAutoLOC_7003411;

	private static string cacheAutoLOC_6003048;

	public bool IsMultipleCubesActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleParachute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ModuleParachute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001348", activeEditor = false)]
	public void DeployAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001347", activeEditor = false)]
	public void CutAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001348")]
	public void Deploy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001349")]
	public virtual void CutParachute()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4f, guiName = "#autoLOC_6001350")]
	public void Repack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001351")]
	public void Disarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
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
	protected void SetUIEVents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupAnimation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetArea(DragCube cube)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalcBaseStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnParachuteSemiDeployed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnParachuteFullyDeployed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool ShouldDeploy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool IsMovingFastEnoughToDeploy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool PassedAdditionalDeploymentChecks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool IsAnimationPlaying()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetAnimationNormalizedTime(string animationName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void PlayDeployAnimation(string animationName, float deploymentSpeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnVesselWasModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSymmetry(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartUnpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDragCubes_Stowed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDragCube_SemiDeployed(float semiDeployed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDragCube_Deployed(float deployed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetDragCubeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssumeDragCubePosition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UsesProceduralDragCubes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetConvectiveStats(double density, double extTemp, double mach, double convCoeff)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string DeploySafe(out deploymentSafeStates state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateCut()
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
