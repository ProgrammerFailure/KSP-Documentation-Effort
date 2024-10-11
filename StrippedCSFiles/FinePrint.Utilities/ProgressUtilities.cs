using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;

namespace FinePrint.Utilities;

public class ProgressUtilities
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProgressUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HavePartTech(string partName, bool logging = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HaveModuleTech(string moduleName, string excludeModule = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HaveAnyTech(IList<string> partNames, IList<string> moduleNames, bool logging = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool GetBodyProgress(ProgressType progress, CelestialBody body, MannedStatus manned = MannedStatus.ANY)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool GetAnyBodyProgress(CelestialBody body, MannedStatus manned = MannedStatus.ANY)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool bodyReached, bool progressComplete, MannedStatus manned, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool bodyReached, bool progressComplete, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool progressComplete, MannedStatus manned, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetBodiesProgress(ProgressType type, bool progressComplete, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetBodiesProgress(bool bodyReached, MannedStatus manned, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetBodiesProgress(bool bodyReached, Func<CelestialBody, bool> where = null, List<CelestialBody> bodies = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetNextUnreached(int count, MannedStatus manned, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<CelestialBody> GetNextUnreached(int count, Func<CelestialBody, bool> where = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Dictionary<CelestialBody, int> CelestialCrewCounts(List<Vessel.Situations> situations)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double CurrentTrackRecord(RecordTrackType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float AverageFacilityLevel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetProgressLevel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IntroWorldFirstContract(ProgressMilestone milestone)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool OutlierWorldFirstContract(ProgressType type, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool OutlierWorldFirstContract(ProgressMilestone milestone)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float ScoreProgressType(ProgressType type, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Contract.ContractPrestige ProgressTypePrestige(ProgressType type, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Contract.ContractPrestige ProgressTypePrestige(ProgressMilestone milestone)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float WorldFirstStandardReward(ProgressRewardType reward, Currency currency, ProgressType progress, CelestialBody body = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float WorldFirstIntervalReward(ProgressRewardType reward, Currency currency, ProgressType progress, CelestialBody body = null, int currentInterval = 1, int totalIntervals = 10)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ExperimentPossibleAt(string experimentID, CelestialBody body, double latitude, double longitude, double altitude, double terrainHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ReachedHomeBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double FindNextRecord(double currentRecord, double maximumRecord, double roundValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double FindNextRecord(double currentRecord, double maximumRecord, double roundValue, ref int interval)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool VisitedSurfaceOf(CelestialBody body, MannedStatus manned = MannedStatus.ANY)
	{
		throw null;
	}
}
