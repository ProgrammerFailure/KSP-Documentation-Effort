using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Experience;

public class ScienceUtil
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ExperimentSituations GetExperimentSituation(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetExperimentBiome(CelestialBody body, double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetExperimentBiomeLocalized(CelestialBody body, double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetExperimentBodyName(string subjectID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GetExperimentFieldsFromScienceID(string subjectID, out string BodyName, out ExperimentSituations Situation, out string Biome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void GetExperimentFieldsFromScienceID(string subjectID, out string BodyName, out string Situation, out string Biome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateLocalizedTitle(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string GenerateLocalizedTitle(string id, bool skipAsteroidsComets)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string generateRecoveryLocalizedTitle(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateScienceSubjectTitle(ScienceExperiment exp, ExperimentSituations sit, CelestialBody body, string biome = "", string displaybiome = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string GenerateScienceSubjectRecoveryTitle(ScienceExperiment exp, RecoverySituations sit, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateScienceSubjectTitle(ScienceExperiment exp, ExperimentSituations sit, string sourceUid, string sourceTitle, CelestialBody body, string biome = "", string displaybiome = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetTransmitterScore(IScienceDataTransmitter t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use GetBestTransmitter(Vessel v) instead")]
	public static IScienceDataTransmitter GetBestTransmitter(List<IScienceDataTransmitter> vesselTransmitters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IScienceDataTransmitter GetBestTransmitter(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetLabScore(ModuleScienceLab lab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool RequiredUsageInternalAvailable(Vessel v, Part p, ExperimentUsageReqs req, ScienceExperiment exp, ref string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool CrewListHasType(List<ProtoCrewMember> crew, ProtoCrewMember.KerbalType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool CrewListHasEffect<T>(List<ProtoCrewMember> crew) where T : ExperienceEffect
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool RequiredUsageExternalAvailable(Vessel v, Vessel vExt, ExperimentUsageReqs req, ScienceExperiment exp, ref string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool BiomeIsUnlisted(CelestialBody body, string biome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetBiomedisplayName(CelestialBody body, string biome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetBiomedisplayName(CelestialBody body, string biome, bool formatted)
	{
		throw null;
	}
}
