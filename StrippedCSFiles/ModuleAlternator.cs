using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using Expansions.Serenity;

public class ModuleAlternator : PartModule
{
	private IEngineStatus engine;

	private ModuleRoboticServoRotor rotor;

	[KSPField]
	public bool roboticRotorMode;

	[KSPField]
	public double singleTickThreshold;

	[KSPField]
	public bool preferMultiMode;

	[KSPField]
	public int engineIndex;

	[KSPField]
	public string engineName;

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001419", guiUnits = "#autoLOC_7001414")]
	public float outputRate;

	[KSPField]
	public string outputName;

	[KSPField]
	public string outputUnits;

	[KSPField]
	public string outputFormat;

	protected BaseField fld;

	private List<AdjusterAlternatorBase> adjusterCache;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAlternator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
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
	private void FixedUpdate()
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
	protected float ApplyOutputAdjustments(float output)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
