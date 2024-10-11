using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using Expansions.Missions;
using KSP.UI.Screens;
using KSP.UI.Screens.Flight;
using KSPAchievements;
using Steamworks;

internal static class AnalyticsUtil
{
	internal enum cbProgressTypes
	{
		reached,
		orbit,
		landed,
		flight,
		escaped,
		science,
		returned
	}

	internal enum SpaceObjectEventTypes
	{
		grappled,
		released,
		landed,
		splashed,
		exploded,
		reached,
		sampled
	}

	internal enum steamItemTypes
	{
		mission,
		craft
	}

	internal enum steamActions
	{
		create,
		update,
		subscribe,
		unsubscribe,
		query,
		other
	}

	private static bool initialized;

	private static bool logErrors;

	private static string analyticsFileFlag;

	private static string analyticsFilePath;

	private static bool logToFile;

	internal static int optOutValueAtLaunch;

	private static readonly Dictionary<string, object> m_EventData;

	private static List<ProtoCrewMember> vesselCrew;

	internal static int OptOutValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AnalyticsUtil()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogCustomEvent(string eventName, Action eventSetupAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LogCustomEventToFile(string eventName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogTutorialStart(TutorialScenario scenario)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogTutorialCompleted(TutorialScenario scenario)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string FixIncorrectTutorialName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogGameStart(float loadingSeconds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogMissionStart(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogJoystickUsage(string joystickName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogMissionCompleted(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogMissionFailed(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogMissionExitBuilder(Mission mission, double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSaveGameCreated(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSaveGameResumed(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSaveGameClosed(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddSaveVesselDate(Game game, bool useFlightState = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CountVesselSituation(VesselType vesselType, Vessel.Situations situation, ref int orbitVessels, ref int landedVessels, ref int flyingVessels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogTechTreeNodeUnlocked(ProtoTechNode techNode, float science)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogDebugWindow(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogExitEditor(double time, Dictionary<string, double> timeInModes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogExitFlight(double time, Dictionary<string, double> timeInModes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogContractAccepted(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogContractCancelled(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogContractCompleted(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogKSPedia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddMissionGeneralData(Mission mission, bool full = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogEVAChuteDeployed(ModuleEvaChute evaChute)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogVesselLaunched(Game game, Vessel vessel, string landedAt, VesselCrewManifest manifest, ulong fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogCelestialBodyProgress(CelestialBody body, cbProgressTypes progress)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogPointOfInterestReached(PointOfInterest poi, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogVesselDocked(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogAsteroidEvent(ModuleAsteroid asteroid, SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogAsteroidVesselEvent(SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogCometEvent(ModuleComet comet, SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogCometVesselEvent(SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamError(steamActions action, steamItemTypes itemType, ulong fileId, string errorCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamError(steamActions action, steamItemTypes itemType, ulong fileId, EResult errorCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamError(steamActions action, steamItemTypes itemType, PublishedFileId_t fileId, string errorCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamError(steamActions action, steamItemTypes itemType, PublishedFileId_t fileId, EResult errorCode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamItemCreated(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamItemUpdated(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamItemSubscribed(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSteamItemUnsubscribed(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddCommonSteamItemData(steamItemTypes itemType, ulong fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void ManeuverModeUsage(Game game, bool mapMode, ManeuverNodeEditorManager.UsageStats usage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void AltimeterModeUsage(Game game, AltitudeTumbler.UsageStats usage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void DeltaVAppUsage(Game game, GameScenes scene, DeltaVApp.UsageStats usage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void DeltaVStageUsage(Game game, GameScenes scene, StageManager.DVUsageStats usage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LogSuitWindowUsage(Game game, ProtoCrewMember crew, SuitCombo oldSuit, SuitCombo newSuit, string oldComboId, string newComboId)
	{
		throw null;
	}
}
