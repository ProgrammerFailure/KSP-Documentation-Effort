using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleResourceIntake : PartModule
{
	[KSPField]
	public string resourceName;

	[KSPField(guiFormat = "F2", isPersistant = false, guiActive = true, guiName = "#autoLOC_6001423", guiUnits = "#autoLOC_7001409")]
	public float airFlow;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
	public string status;

	[KSPField]
	public double area;

	[KSPField]
	public bool checkForOxygen;

	[KSPField(guiFormat = "F2", isPersistant = false, guiActive = true, guiName = "#autoLOC_6001424", guiUnits = "#autoLOC_7001415")]
	public float airSpeedGui;

	[KSPField]
	public double intakeSpeed;

	[KSPField]
	public string intakeTransformName;

	public Transform intakeTransform;

	[KSPField(isPersistant = true)]
	public bool intakeEnabled;

	[KSPField]
	public string occludeNode;

	[KSPField]
	public double unitScalar;

	[KSPField]
	public FloatCurve machCurve;

	[KSPField]
	public double kPaThreshold;

	[KSPField]
	public bool disableUnderwater;

	[KSPField]
	public bool underwaterOnly;

	public int resourceId;

	public double resourceUnits;

	public PartResourceDefinition resourceDef;

	public double densityRecip;

	public PartResource res;

	public AttachNode node;

	public bool checkNode;

	private static string cacheAutoLOC_235899;

	private static string cacheAutoLOC_235936;

	private static string cacheAutoLOC_235946;

	private static string cacheAutoLOC_235952;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleResourceIntake()
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
	[KSPAction("#autoLOC_6001425")]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleAction(KSPActionType action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001426")]
	public void Deactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001427")]
	public void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
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
