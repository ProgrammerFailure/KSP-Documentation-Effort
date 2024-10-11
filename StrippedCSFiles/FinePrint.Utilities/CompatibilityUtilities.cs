using System.Runtime.CompilerServices;

namespace FinePrint.Utilities;

public class CompatibilityUtilities
{
	public static int majorVersion;

	public static int minorVersion;

	public static int revisionVersion;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CompatibilityUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CompatibilityUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool OldCareerSave(Game.Modes mode, int saveMajor, int saveMinor, int saveRevision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool OldCareerSave(Game.Modes mode, string versionString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UpdateCareerSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SurveyDefinition FindSurveyDefinition(string experiment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SavePriorTo(int major, int minor, int revision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string UpgradeVesselSituation(string situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CleanUpUnsanitaryEVAKerbals(ConfigNode flightState)
	{
		throw null;
	}
}
