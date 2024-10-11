using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleGimbal : PartModule, ITorqueProvider
{
	[KSPField]
	public string gimbalTransformName;

	[KSPField]
	public string actionSuffix;

	[UI_Toggle(disabledText = "#autoLOC_7000036", scene = UI_Scene.All, enabledText = "#autoLOC_7000035", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = true, guiName = "#autoLoc_6003043")]
	public bool gimbalLock;

	[KSPField(isPersistant = true, guiActive = true, guiName = "#autoLOC_6001383")]
	[UI_FloatRange(minValue = 0f, stepIncrement = 1f, maxValue = 100f, affectSymCounterparts = UI_Scene.All)]
	public float gimbalLimiter;

	[KSPField]
	public float gimbalRange;

	[KSPField]
	public float gimbalRangeXP;

	[KSPField]
	public float gimbalRangeYP;

	[KSPField]
	public float gimbalRangeXN;

	[KSPField]
	public float gimbalRangeYN;

	[KSPField]
	public bool flipYZ;

	[KSPField]
	public float xMult;

	[KSPField]
	public float yMult;

	[KSPField]
	public bool showToggles;

	[KSPField(isPersistant = true)]
	public bool currentShowToggles;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001331")]
	public bool enableYaw;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001330")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enablePitch;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001332")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enableRoll;

	[KSPField]
	public float minRollOffset;

	[KSPField]
	public bool useGimbalResponseSpeed;

	[KSPField]
	public float gimbalResponseSpeed;

	[KSPField(isPersistant = true)]
	public bool gimbalActive;

	public List<Transform> gimbalTransforms;

	public List<Quaternion> initRots;

	public Vector3 actuationLocal;

	public Vector3[] oldActuationLocal;

	public Vector3 actuation;

	private Transform t;

	private float targetAngleYaw;

	private float targetAnglePitch;

	private float targetAngleRoll;

	private Vector3 rCoM;

	private Vector3 toRollAxis;

	public List<List<KeyValuePair<ModuleEngines, float>>> engineMultsList;

	private List<AdjusterGimbalBase> adjusterCache;

	private static string cacheAutoLOC_221352;

	private static string cacheAutoLOC_7000023;

	private static string cacheAutoLOC_221424;

	private static string cacheAutoLOC_6003043;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleGimbal()
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
	[KSPAction("#autoLOC_6001385")]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001386")]
	public void LockAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001387")]
	public void FreeAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001388", KSPActionGroup.None, true)]
	public void TogglePitchAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001389", KSPActionGroup.None, true)]
	public void ToggleYawAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001390", KSPActionGroup.None, true)]
	public void ToggleRollAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GimbalRotation(Transform t, Vector3 PYR, Vector3 localCoM)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateToggles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetActionsSuffix()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePAWUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateEngineList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
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
	protected Vector3 ApplyControlAdjustments(Vector3 control)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
