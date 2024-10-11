using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleOverheatDisplay : PartModule
{
	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001487")]
	public string heatDisplay;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001488")]
	public string coreTempDisplay;

	private double oldHeat;

	private double oldTemp;

	private double oldGoal;

	protected List<IOverheatDisplay> _modules;

	private static string cacheAutoLOC_259510;

	private static string cacheAutoLOC_7001406;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleOverheatDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
