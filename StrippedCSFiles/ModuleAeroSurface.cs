using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleAeroSurface : ModuleControlSurface, IMultipleDragCube
{
	[KSPField]
	public float uncasedTemp;

	[KSPField]
	public float casedTemp;

	[KSPField]
	public float ctrlRangeFactor;

	[KSPField]
	public bool brakeDeployInvert;

	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = false, guiName = "#autoLOC_6001336")]
	public float aeroAuthorityLimiter;

	[KSPField(guiActiveUnfocused = true, guiFormat = "0", isPersistant = false, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001336", guiUnits = "°")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.05f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float aeroAuthorityLimiterUI;

	[KSPAxisField(unfocusedRange = 25f, isPersistant = true, guiActiveUnfocused = true, maxValue = 100f, incrementalSpeed = 50f, guiFormat = "0", axisMode = KSPAxisMode.Incremental, minValue = -100f, guiActive = true, guiName = "#autoLOC_6013041", guiUnits = "°")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float aeroDeployAngle;

	public Vector2 aeroDeployAngleLimits;

	private static string cacheAutoLOC_6003025;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAeroSurface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyAeroAuthorityLimiter(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ModifyAeroAuthorityLimiterUI(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001329", KSPActionGroup.Brakes)]
	public void ActionToggleBrakes(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CtrlSurfaceUpdate(Vector3 vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CtrlSurfaceEditorUpdate(Vector3 CoM)
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
