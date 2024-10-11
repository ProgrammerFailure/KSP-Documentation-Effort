using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;

public class ModuleGenerator : PartModule, IContractObjectiveModule
{
	[KSPField(isPersistant = true)]
	public bool generatorIsActive;

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001422", guiUnits = "")]
	public float efficiency;

	[KSPField]
	public string efficiencyGUIName;

	[KSPField]
	public string activateGUIName;

	[KSPField]
	public string shutdownGUIName;

	[KSPField]
	public string toggleGUIName;

	[KSPField]
	public bool isAlwaysActive;

	[KSPField]
	public bool isGroundFixture;

	[KSPField]
	public bool requiresAllInputs;

	[KSPField]
	public float resourceThreshold;

	[KSPField]
	public bool isThrottleControlled;

	[KSPField(isPersistant = true)]
	public float throttle;

	[KSPField(guiActive = true, guiName = "#autoLOC_235532")]
	public string displayStatus;

	[KSPField]
	public string status;

	[KSPField]
	public string statusGUIName;

	private ModuleResource currentResource;

	private double inputRecieved;

	private bool lackingResources;

	private List<AdjusterGeneratorBase> adjusterCache;

	private static string cacheAutoLOC_219034;

	private static string cacheAutoLOC_220477;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleGenerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_235508")]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_235502")]
	public void ActivateAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_235505")]
	public void ShutdownAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_235502")]
	public void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_235505")]
	public void Shutdown()
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
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetContractObjectiveType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckContractObjectiveValidity()
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
	protected float ApplyEfficiencyAdjustments(float efficiency)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
