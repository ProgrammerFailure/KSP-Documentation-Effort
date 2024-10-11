using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleEnviroSensor : PartModule, IResourceConsumer
{
	public enum SensorType
	{
		TEMP,
		GRAV,
		ACC,
		PRES
	}

	[KSPField]
	public SensorType sensorType;

	[KSPField(guiFormat = "F3", guiActive = true, guiName = "#autoLOC_237023", guiUnits = "")]
	public string readoutInfo;

	[KSPField(isPersistant = true)]
	public bool sensorActive;

	private List<PartResourceDefinition> consumedResources;

	private static string cacheAutoLOC_7001406;

	private static string cacheAutoLOC_6004058;

	private static string cacheAutoLOC_7001413;

	private static string cacheAutoLOC_7001410;

	private static string cacheAutoLOC_6004059;

	private static string cacheAutoLOC_6004060;

	private static string cacheAutoLOC_237153;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleEnviroSensor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001431")]
	public void ToggleAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001431")]
	public void Toggle()
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
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
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
