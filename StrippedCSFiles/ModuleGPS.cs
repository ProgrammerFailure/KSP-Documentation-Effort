using System.Runtime.CompilerServices;

public class ModuleGPS : PartModule, IAnimatedModule
{
	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001476")]
	public string body;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_438890")]
	public string bioName;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001477")]
	public string lat;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001478")]
	public string lon;

	protected bool isConnectedToAsteroid;

	protected bool isConnectedToComet;

	private float secondsSinceLastDisplayUpdate;

	private float secondsUpdateRate;

	private static string cacheAutoLOC_6005065;

	private static string cacheAutoLOC_258910;

	private static string cacheAutoLOC_258911;

	private static string cacheAutoLOC_258912;

	private static string cacheAutoLOC_258913;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleGPS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CheckForAsteroidConnect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CheckForCometConnect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void EnableModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DisableModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ModuleIsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsSituationValid()
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
