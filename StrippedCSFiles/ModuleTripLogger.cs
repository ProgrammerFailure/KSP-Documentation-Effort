using System.Runtime.CompilerServices;

public class ModuleTripLogger : PartModule
{
	public FlightLog Log;

	private const string moduleString = "ModuleTripLogger";

	private const string logString = "Log";

	private static string cacheAutoLOC_6003061;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleTripLogger()
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
	private void LoadDeprecated(string oldName, FlightLog.EntryType newName, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSOIChanged(GameEvents.HostedFromToAction<Vessel, CelestialBody> vc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InvokeLogVessel(float delay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogVesselSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogNewSituation(Vessel.Situations sit, CelestialBody where)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LogSituation(FlightLog.EntryType type, string target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ForceVesselLog(Vessel v, FlightLog.EntryType type, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ForceLogEntry(Vessel v, FlightLog.EntryType type, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ForceLogEntry(ProtoVessel pv, FlightLog.EntryType type, CelestialBody body)
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
