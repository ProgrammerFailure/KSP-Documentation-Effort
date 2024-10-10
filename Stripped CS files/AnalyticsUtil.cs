using System;
using System.Collections.Generic;
using System.IO;
using Contracts;
using Expansions;
using Expansions.Missions;
using KSPAchievements;
using ns11;
using ns36;
using ns9;
using Steamworks;
using UnityEngine;
using UnityEngine.Analytics;

public static class AnalyticsUtil
{
	public enum cbProgressTypes
	{
		reached,
		orbit,
		landed,
		flight,
		escaped,
		science,
		returned
	}

	public enum SpaceObjectEventTypes
	{
		grappled,
		released,
		landed,
		splashed,
		exploded,
		reached,
		sampled
	}

	public enum steamItemTypes
	{
		mission,
		craft
	}

	public enum steamActions
	{
		create,
		update,
		subscribe,
		unsubscribe,
		query,
		other
	}

	public static bool initialized = false;

	public static bool logErrors = false;

	public static string analyticsFileFlag;

	public static string analyticsFilePath;

	public static bool logToFile;

	public static int optOutValueAtLaunch = -1;

	public static readonly Dictionary<string, object> m_EventData = new Dictionary<string, object>();

	public static List<ProtoCrewMember> vesselCrew;

	public static int OptOutValue
	{
		get
		{
			int @int = PlayerPrefs.GetInt("data.optOut", -1);
			if (@int < 0)
			{
				return optOutValueAtLaunch;
			}
			return @int;
		}
	}

	public static bool Initialize()
	{
		try
		{
			optOutValueAtLaunch = PlayerPrefs.GetInt("data.optOut", -1);
			initialized = true;
			logToFile = false;
			try
			{
				analyticsFileFlag = ((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../.analyticslogflag") : Path.Combine(Application.dataPath, "../.analyticslogflag"));
				analyticsFilePath = ((Application.platform == RuntimePlatform.OSXPlayer) ? Path.Combine(Application.dataPath, "../../.analyticslog.txt") : Path.Combine(Application.dataPath, "../.analyticslog.txt"));
				logToFile = File.Exists(analyticsFileFlag);
			}
			catch (Exception)
			{
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public static void LogCustomEvent(string eventName, Action eventSetupAction)
	{
		try
		{
			m_EventData.Clear();
			eventSetupAction();
			LogCustomEventToFile(eventName);
			AnalyticsEvent.Custom(eventName, m_EventData);
		}
		catch (Exception ex)
		{
			if (logErrors)
			{
				Debug.LogErrorFormat("[Analytics] Unable to send event {0}.\n{1}", eventName, ex.Message);
			}
		}
	}

	public static void LogCustomEventToFile(string eventName)
	{
		if (!logToFile)
		{
			return;
		}
		try
		{
			string text = $"{KSPUtil.SystemDateTime.DateTimeNow():yyyyMMdd HH:mm:ss}: {eventName}\n";
			foreach (KeyValuePair<string, object> eventDatum in m_EventData)
			{
				text += $"\t{eventDatum.Key}={eventDatum.Value}\n";
			}
			text += "\n";
			File.AppendAllText(analyticsFilePath, text);
		}
		catch (Exception ex)
		{
			if (logErrors)
			{
				Debug.LogErrorFormat("[Analytics] Unable to write event {0}.\n{1}", eventName, ex.Message);
			}
		}
	}

	public static void LogTutorialStart(TutorialScenario scenario)
	{
		string className = scenario.GetType().Name;
		className = FixIncorrectTutorialName(className);
		if (className.ToLower().StartsWith("tutorial"))
		{
			LogCustomEvent("tutorial_start", delegate
			{
				m_EventData.Add("tutorial_id", className);
			});
		}
	}

	public static void LogTutorialCompleted(TutorialScenario scenario)
	{
		string className = scenario.GetType().Name;
		className = FixIncorrectTutorialName(className);
		if (className.ToLower().StartsWith("tutorial"))
		{
			LogCustomEvent("tutorial_complete", delegate
			{
				m_EventData.Add("tutorial_id", className);
			});
		}
	}

	public static string FixIncorrectTutorialName(string name)
	{
		return name switch
		{
			"BasicTutorial" => "TutorialMEBasic", 
			"IntermediateTutorial" => "TutorialMEIntermediate", 
			"AdvancedTutorial" => "TutorialMEAdvanced", 
			_ => name, 
		};
	}

	public static void LogGameStart(float loadingSeconds)
	{
		LogCustomEvent("game_start", delegate
		{
			m_EventData.Add("version", VersioningBase.GetVersionString());
			m_EventData.Add("buildid", Versioning.BuildID);
			m_EventData.Add("is_release", Versioning.IsReleaseBuild);
			m_EventData.Add("is_winx64", Versioning.WinX64);
			m_EventData.Add("is_steam", Versioning.IsSteam);
			m_EventData.Add("distribution_name", Versioning.DistributionName);
			m_EventData.Add("lang", Localizer.CurrentLanguage);
			m_EventData.Add("is_modded", GameDatabase.Modded);
			m_EventData.Add("load_time", loadingSeconds);
			m_EventData.Add("analytics_data_optout", OptOutValue == 1);
		});
		LogCustomEvent("game_start_hardware", delegate
		{
			m_EventData.Add("displays_count", (Display.displays == null) ? 1 : Display.displays.Length);
			m_EventData.Add("operating_system", SystemInfo.operatingSystem);
			m_EventData.Add("processor_count", SystemInfo.processorCount);
			m_EventData.Add("processor_frequency", SystemInfo.processorFrequency);
			m_EventData.Add("processor_type", SystemInfo.processorType);
			m_EventData.Add("memory_size", SystemInfo.systemMemorySize);
		});
		LogCustomEvent("game_start_graphics", delegate
		{
			m_EventData.Add("device_name", SystemInfo.graphicsDeviceName);
			m_EventData.Add("device_type", SystemInfo.graphicsDeviceType);
			m_EventData.Add("device_vendor", SystemInfo.graphicsDeviceVendor);
			m_EventData.Add("shader_level", SystemInfo.graphicsShaderLevel);
			m_EventData.Add("memory_size", SystemInfo.graphicsMemorySize);
			m_EventData.Add("texture_size", SystemInfo.maxTextureSize);
			m_EventData.Add("screen_resolution", $"{Screen.currentResolution.width}x{Screen.currentResolution.height}");
			m_EventData.Add("game_resolution", $"{Screen.width}x{Screen.height}");
		});
		LogCustomEvent("game_start_settings", delegate
		{
			m_EventData.Add("advanced_tweakables", GameSettings.ADVANCED_TWEAKABLES);
			m_EventData.Add("extended_burntime", GameSettings.EXTENDED_BURNTIME);
		});
		LogCustomEvent("game_start_expansions", delegate
		{
			m_EventData.Add("making_history", ExpansionsLoader.IsExpansionInstalled("MakingHistory"));
			m_EventData.Add("serenity", ExpansionsLoader.IsExpansionInstalled("Serenity"));
		});
		List<ExpansionsLoader.ExpansionInfo> expansions = ExpansionsLoader.GetInstalledExpansions();
		int i;
		for (i = 0; i < expansions.Count; i++)
		{
			string text = expansions[i].FolderName.ToLower();
			LogCustomEvent("game_start_expansions_" + text, delegate
			{
				m_EventData.Add("version", expansions[i].PersistentObject.Version);
			});
		}
	}

	public static void LogMissionStart(Mission mission)
	{
		LogCustomEvent("mission_start", delegate
		{
			AddMissionGeneralData(mission, full: true);
		});
	}

	public static void LogJoystickUsage(string joystickName)
	{
		LogCustomEvent("joystick_used", delegate
		{
			m_EventData.Add("joystick_name", joystickName);
		});
	}

	public static void LogMissionCompleted(Mission mission)
	{
		LogCustomEvent("mission_complete", delegate
		{
			AddMissionGeneralData(mission, full: true);
			m_EventData.Add("score", mission.currentScore);
		});
	}

	public static void LogMissionFailed(Mission mission)
	{
		LogCustomEvent("mission_fail", delegate
		{
			AddMissionGeneralData(mission, full: true);
			m_EventData.Add("score", mission.currentScore);
		});
	}

	public static void LogMissionExitBuilder(Mission mission, double time)
	{
		LogCustomEvent("mission_exit_builder", delegate
		{
			m_EventData.Add("time", time);
		});
	}

	public static void LogSaveGameCreated(Game game)
	{
		if (game != null)
		{
			LogCustomEvent("savegame_created", delegate
			{
				m_EventData.Add("game_mode", game.Mode);
			});
		}
	}

	public static void LogSaveGameResumed(Game game)
	{
		if (game != null)
		{
			LogCustomEvent("savegame_resumed", delegate
			{
				m_EventData.Add("game_mode", game.Mode);
				m_EventData.Add("time", game.UniversalTime);
				AddSaveVesselDate(game, useFlightState: true);
			});
		}
	}

	public static void LogSaveGameClosed(Game game)
	{
		LogCustomEvent("savegame_closed", delegate
		{
			m_EventData.Add("game_mode", game.Mode);
			m_EventData.Add("time", game.UniversalTime);
			AddSaveVesselDate(game);
		});
	}

	public static void AddSaveVesselDate(Game game, bool useFlightState = false)
	{
		if (FlightGlobals.Vessels == null)
		{
			return;
		}
		int orbitVessels = 0;
		int landedVessels = 0;
		int flyingVessels = 0;
		if (useFlightState && game.flightState != null && game.flightState.protoVessels != null)
		{
			for (int i = 0; i < game.flightState.protoVessels.Count; i++)
			{
				ProtoVessel protoVessel = game.flightState.protoVessels[i];
				CountVesselSituation(protoVessel.vesselType, protoVessel.situation, ref orbitVessels, ref landedVessels, ref flyingVessels);
			}
		}
		else
		{
			if (FlightGlobals.VesselsLoaded != null)
			{
				for (int j = 0; j < FlightGlobals.VesselsLoaded.Count; j++)
				{
					Vessel vessel = FlightGlobals.VesselsLoaded[j];
					CountVesselSituation(vessel.vesselType, vessel.situation, ref orbitVessels, ref landedVessels, ref flyingVessels);
				}
			}
			if (FlightGlobals.VesselsUnloaded != null)
			{
				for (int k = 0; k < FlightGlobals.VesselsUnloaded.Count; k++)
				{
					Vessel vessel2 = FlightGlobals.VesselsUnloaded[k];
					CountVesselSituation(vessel2.protoVessel.vesselType, vessel2.protoVessel.situation, ref orbitVessels, ref landedVessels, ref flyingVessels);
				}
			}
		}
		m_EventData.Add("vessel_count", flyingVessels + orbitVessels + landedVessels);
		m_EventData.Add("vessel_count_flying", flyingVessels);
		m_EventData.Add("vessel_count_orbiting", orbitVessels);
		m_EventData.Add("vessel_count_landed", landedVessels);
	}

	public static void CountVesselSituation(VesselType vesselType, Vessel.Situations situation, ref int orbitVessels, ref int landedVessels, ref int flyingVessels)
	{
		if (vesselType != 0 && vesselType != VesselType.SpaceObject && vesselType != VesselType.Unknown && vesselType != VesselType.Flag && vesselType != VesselType.DeployedSciencePart)
		{
			switch (situation)
			{
			case Vessel.Situations.FLYING:
				flyingVessels++;
				break;
			case Vessel.Situations.LANDED:
			case Vessel.Situations.SPLASHED:
			case Vessel.Situations.PRELAUNCH:
				landedVessels++;
				break;
			case Vessel.Situations.SUB_ORBITAL:
			case Vessel.Situations.ORBITING:
			case Vessel.Situations.ESCAPING:
				orbitVessels++;
				break;
			}
		}
	}

	public static void LogTechTreeNodeUnlocked(ProtoTechNode techNode, float science)
	{
		LogCustomEvent("techTreeNode_unlocked", delegate
		{
			m_EventData.Add("tech_tree_node", techNode.techID);
			m_EventData.Add("science_available", science);
		});
	}

	public static void LogDebugWindow(Game game)
	{
		LogCustomEvent("debug_window", delegate
		{
			if (game != null)
			{
				m_EventData.Add("game_mode", game.Mode);
			}
			else
			{
				m_EventData.Add("game_mode", "NONE");
			}
			m_EventData.Add("game_scene", HighLogic.LoadedScene);
		});
	}

	public static void LogExitEditor(double time, Dictionary<string, double> timeInModes)
	{
		string[] modeNames = Enum.GetNames(typeof(EditorScreen));
		LogCustomEvent("exit_editor", delegate
		{
			m_EventData.Add("time", time);
			m_EventData.Add("editor", EditorDriver.editorFacility);
			for (int i = 0; i < modeNames.Length; i++)
			{
				if (!timeInModes.ContainsKey(modeNames[i]))
				{
					timeInModes.Add(modeNames[i], 0.0);
				}
				m_EventData.Add("time_" + modeNames[i], timeInModes[modeNames[i]]);
			}
		});
	}

	public static void LogExitFlight(double time, Dictionary<string, double> timeInModes)
	{
		string[] modeNames = Enum.GetNames(typeof(CameraManager.CameraMode));
		LogCustomEvent("exit_flight", delegate
		{
			m_EventData.Add("time", time);
			for (int i = 0; i < modeNames.Length; i++)
			{
				if (!timeInModes.ContainsKey(modeNames[i]))
				{
					timeInModes.Add(modeNames[i], 0.0);
				}
				m_EventData.Add("time_" + modeNames[i], timeInModes[modeNames[i]]);
			}
		});
	}

	public static void LogContractAccepted(Contract contract)
	{
		LogCustomEvent("contract_accepted", delegate
		{
			m_EventData.Add("contract_type", contract.Prestige.ToString());
			m_EventData.Add("contract_class", contract.GetType().FullName);
			m_EventData.Add("ut", contract.DateFinished - contract.DateAccepted);
		});
	}

	public static void LogContractCancelled(Contract contract)
	{
		LogCustomEvent("contract_cancelled", delegate
		{
			m_EventData.Add("contract_type", contract.Prestige.ToString());
			m_EventData.Add("contract_class", contract.GetType().FullName);
			m_EventData.Add("ut", contract.DateFinished - contract.DateAccepted);
		});
	}

	public static void LogContractCompleted(Contract contract)
	{
		LogCustomEvent("contract_completed", delegate
		{
			m_EventData.Add("contract_type", contract.Prestige.ToString());
			m_EventData.Add("contract_class", contract.GetType().FullName);
			m_EventData.Add("ut", contract.DateFinished - contract.DateAccepted);
		});
	}

	public static void LogKSPedia()
	{
		LogCustomEvent("open_kspedia", delegate
		{
		});
	}

	public static void AddMissionGeneralData(Mission mission, bool full = false)
	{
		m_EventData.Add("mission_name", mission.name);
		m_EventData.Add("pack_name", mission.packName);
		if (mission.MissionInfo != null)
		{
			m_EventData.Add("mission_type", mission.MissionInfo.missionType);
		}
		if (!full)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		DictionaryValueList<string, int> dictionaryValueList = new DictionaryValueList<string, int>();
		int i = 0;
		for (int count = mission.nodes.Count; i < count; i++)
		{
			MENode mENode = mission.nodes.At(i);
			if (mENode.actionModules.Count > 0)
			{
				num++;
			}
			else if (mENode.IsLogicNode)
			{
				num2++;
			}
			if (!dictionaryValueList.ContainsKey(mENode.category))
			{
				dictionaryValueList.Add(mENode.category, 0);
			}
			dictionaryValueList[mENode.category]++;
		}
		m_EventData.Add("nodes_count", mission.nodes.Count);
		m_EventData.Add("nodes_count_action", num);
		m_EventData.Add("nodes_count_logic", num2);
		if (mission.MissionInfo.missionType == MissionTypes.Steam)
		{
			m_EventData.Add("steam_fileId", mission.steamPublishedFileId.ToString());
		}
		else
		{
			m_EventData.Add("steam_fileId", "0");
		}
	}

	public static void LogEVAChuteDeployed(ModuleEvaChute evaChute)
	{
		LogCustomEvent("evachute_deployed", delegate
		{
			if (evaChute.vessel != null)
			{
				if (evaChute.vessel.mainBody != null)
				{
					m_EventData.Add("celestial_body", evaChute.vessel.mainBody.name);
				}
				if (evaChute.vessel.isEVA && evaChute.vessel.Parts.Count > 0 && evaChute.vessel.Parts[0].protoModuleCrew.Count > 0 && evaChute.vessel.Parts[0].protoModuleCrew[0] != null)
				{
					m_EventData.Add("kerbal", evaChute.vessel.Parts[0].protoModuleCrew[0].name);
				}
			}
		});
	}

	public static void LogVesselLaunched(Game game, Vessel vessel, string landedAt, VesselCrewManifest manifest, ulong fileId)
	{
		LogCustomEvent("vessel_launched", delegate
		{
			if (game != null)
			{
				m_EventData.Add("game_mode", game.Mode);
			}
			if (vessel != null)
			{
				m_EventData.Add("launched_from", landedAt);
				if (vessel.mainBody != null)
				{
					m_EventData.Add("celestial_body", vessel.mainBody.name);
				}
				if (vessel.parts != null)
				{
					m_EventData.Add("vessel_part_count", vessel.Parts.Count);
				}
				if (manifest != null)
				{
					m_EventData.Add("vessel_crew_count", manifest.CrewCount);
					vesselCrew = manifest.GetAllCrew(includeNulls: false);
					int num = 0;
					for (int i = 0; i < vesselCrew.Count; i++)
					{
						if (vesselCrew[i].seat == null)
						{
							num++;
						}
					}
					m_EventData.Add("vessel_crew_count_externalseat", num);
				}
				m_EventData.Add("vessel_type", vessel.vesselType.ToString());
			}
			m_EventData.Add("steam_fileId", fileId.ToString());
		});
		LogCustomEvent("vessel_launched_partstats", delegate
		{
			if (game != null)
			{
				m_EventData.Add("game_mode", game.Mode);
			}
			if (vessel != null)
			{
				m_EventData.Add("launched_from", landedAt);
				m_EventData.Add("mod", vessel.HasModParts());
				m_EventData.Add("making_history", vessel.HasMakingHistoryParts());
				m_EventData.Add("serenity", vessel.HasSerenityParts());
				m_EventData.Add("serenity_robotic", vessel.HasSerenityRoboticParts());
				m_EventData.Add("serenity_robotic_controller", vessel.HasSerenityRoboticController());
			}
		});
		if (!(vessel != null) || !vessel.HasSerenityRoboticParts())
		{
			return;
		}
		LogCustomEvent("vessel_launched_partstats_robotics", delegate
		{
			if (game != null)
			{
				m_EventData.Add("game_mode", game.Mode);
			}
			if (vessel != null)
			{
				m_EventData.Add("launched_from", landedAt);
				m_EventData.Add("serenity_robotic_rotor", vessel.CountSerenityRotorParts());
				m_EventData.Add("serenity_robotic_hinge", vessel.CountSerenityHingeParts());
				m_EventData.Add("serenity_robotic_piston", vessel.CountSerenityPistonParts());
				m_EventData.Add("serenity_robotic_rotationservo", vessel.CountSerenityRotationServoParts());
				m_EventData.Add("serenity_robotic_mod", vessel.CountSerenityModRoboticParts());
				vessel.CountSerenityControllerFields(out var countControllers, out var countAxes, out var countActions);
				m_EventData.Add("serenity_robotic_controller", countControllers);
				m_EventData.Add("serenity_robotic_controller_axes", countAxes);
				m_EventData.Add("serenity_robotic_controller_actions", countActions);
			}
		});
	}

	public static void LogCelestialBodyProgress(CelestialBody body, cbProgressTypes progress)
	{
		LogCustomEvent("celestialbody_" + progress, delegate
		{
			m_EventData.Add("celestial_body", body.name);
		});
	}

	public static void LogPointOfInterestReached(PointOfInterest poi, CelestialBody body)
	{
		LogCustomEvent("pointofinterest_reached", delegate
		{
			m_EventData.Add("celestial_body", body.name);
			m_EventData.Add("point_of_interest", poi.name);
		});
	}

	public static void LogVesselDocked(Vessel v)
	{
		LogCustomEvent("celestialbody_docked", delegate
		{
			m_EventData.Add("celestial_body", v.mainBody.name);
			m_EventData.Add("vessel_part_count", v.Parts.Count);
		});
	}

	public static void LogAsteroidEvent(ModuleAsteroid asteroid, SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		if (asteroid == null)
		{
			return;
		}
		LogCustomEvent("asteroid_event", delegate
		{
			if (game != null)
			{
				m_EventData.Add("game_mode", game.Mode);
			}
			if (vessel != null)
			{
				if (vessel.mainBody != null)
				{
					m_EventData.Add("celestial_body", vessel.mainBody.name);
				}
				if (vessel.parts != null)
				{
					m_EventData.Add("vessel_part_count", vessel.Parts.Count);
				}
			}
			if (asteroid.part != null)
			{
				m_EventData.Add("mass", asteroid.part.mass);
			}
			m_EventData.Add("event_type", eventType);
		});
	}

	public static void LogAsteroidVesselEvent(SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		for (int i = 0; i < vessel.Parts.Count; i++)
		{
			ModuleAsteroid moduleAsteroid = vessel.Parts[i].FindModuleImplementing<ModuleAsteroid>();
			if (moduleAsteroid != null)
			{
				LogAsteroidEvent(moduleAsteroid, eventType, game, vessel);
			}
		}
	}

	public static void LogCometEvent(ModuleComet comet, SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		if (comet == null)
		{
			return;
		}
		LogCustomEvent("comet_event", delegate
		{
			if (game != null)
			{
				m_EventData.Add("game_mode", game.Mode);
			}
			if (vessel != null)
			{
				if (vessel.mainBody != null)
				{
					m_EventData.Add("celestial_body", vessel.mainBody.name);
				}
				if (vessel.parts != null)
				{
					m_EventData.Add("vessel_part_count", vessel.Parts.Count);
				}
			}
			if (comet.part != null)
			{
				m_EventData.Add("mass", comet.part.mass);
			}
			m_EventData.Add("event_type", eventType);
		});
	}

	public static void LogCometVesselEvent(SpaceObjectEventTypes eventType, Game game, Vessel vessel)
	{
		for (int i = 0; i < vessel.Parts.Count; i++)
		{
			ModuleComet moduleComet = vessel.Parts[i].FindModuleImplementing<ModuleComet>();
			if (moduleComet != null)
			{
				LogCometEvent(moduleComet, eventType, game, vessel);
			}
		}
	}

	public static void LogSteamError(steamActions action, steamItemTypes itemType, ulong fileId, string errorCode)
	{
		LogCustomEvent("steam_error", delegate
		{
			m_EventData.Add("action", action.ToString());
			AddCommonSteamItemData(itemType, fileId);
			m_EventData.Add("errorCode", errorCode);
		});
	}

	public static void LogSteamError(steamActions action, steamItemTypes itemType, ulong fileId, EResult errorCode)
	{
		string text = "";
		text = ((!(SteamManager.Instance != null)) ? errorCode.ToString() : SteamManager.Instance.GetUGCFailureReason(errorCode));
		LogSteamError(action, itemType, fileId, text);
	}

	public static void LogSteamError(steamActions action, steamItemTypes itemType, PublishedFileId_t fileId, string errorCode)
	{
		LogSteamError(action, itemType, fileId.m_PublishedFileId, errorCode);
	}

	public static void LogSteamError(steamActions action, steamItemTypes itemType, PublishedFileId_t fileId, EResult errorCode)
	{
		LogSteamError(action, itemType, fileId.m_PublishedFileId, errorCode);
	}

	public static void LogSteamItemCreated(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		LogCustomEvent("steam_item_created", delegate
		{
			AddCommonSteamItemData(itemType, fileId.m_PublishedFileId);
		});
	}

	public static void LogSteamItemUpdated(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		LogCustomEvent("steam_item_updated", delegate
		{
			AddCommonSteamItemData(itemType, fileId.m_PublishedFileId);
		});
	}

	public static void LogSteamItemSubscribed(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		LogCustomEvent("steam_item_subscribed", delegate
		{
			AddCommonSteamItemData(itemType, fileId.m_PublishedFileId);
		});
	}

	public static void LogSteamItemUnsubscribed(steamItemTypes itemType, PublishedFileId_t fileId)
	{
		LogCustomEvent("steam_item_unsubscribed", delegate
		{
			AddCommonSteamItemData(itemType, fileId.m_PublishedFileId);
		});
	}

	public static void AddCommonSteamItemData(steamItemTypes itemType, ulong fileId)
	{
		m_EventData.Add("itemType", itemType.ToString());
		m_EventData.Add("fileId", fileId.ToString());
	}

	public static void ManeuverModeUsage(Game game, bool mapMode, ManeuverNodeEditorManager.UsageStats usage)
	{
		LogCustomEvent("maneuvermode_usage", delegate
		{
			m_EventData.Add("game_mode", game.Mode);
			m_EventData.Add("map_mode", mapMode);
			m_EventData.Add("left_tab", usage.leftTabChange);
			m_EventData.Add("node_selection", usage.nodeSelector);
			m_EventData.Add("vector_handle", usage.vectorHandle);
			m_EventData.Add("vector_text", usage.vectorText);
			m_EventData.Add("ut_handle", usage.utHandle);
			m_EventData.Add("ut_text", usage.utText);
			m_EventData.Add("precision", usage.precisionSlider);
			m_EventData.Add("orbit_selection", usage.orbitSelection);
		});
	}

	public static void AltimeterModeUsage(Game game, AltitudeTumbler.UsageStats usage)
	{
		LogCustomEvent("altimetermode_usage", delegate
		{
			m_EventData.Add("game_mode", game.Mode);
			m_EventData.Add("asl_selected", usage.aslSelected);
			m_EventData.Add("agl_selected", usage.aglSelected);
		});
	}

	public static void DeltaVAppUsage(Game game, GameScenes scene, DeltaVApp.UsageStats usage)
	{
		LogCustomEvent("deltavapp_usage", delegate
		{
			m_EventData.Add("game_mode", game.Mode);
			m_EventData.Add("game_scene", scene);
			m_EventData.Add("body_selected", usage.body);
			m_EventData.Add("situation_selected", usage.situation);
			m_EventData.Add("atmosphere_pressure", usage.atmosphereSlider);
			m_EventData.Add("fields_selected", usage.displayFields);
		});
	}

	public static void DeltaVStageUsage(Game game, GameScenes scene, StageManager.DVUsageStats usage)
	{
		LogCustomEvent("deltavapp_usage", delegate
		{
			m_EventData.Add("game_mode", game.Mode);
			m_EventData.Add("game_scene", scene);
			m_EventData.Add("stage_shown", usage.stageHide);
			m_EventData.Add("stage_hidden", usage.stageShow);
			m_EventData.Add("stages_all_selected", usage.allStagesShow);
			m_EventData.Add("stages_all_hidden", usage.allStagesHide);
		});
	}

	public static void LogSuitWindowUsage(Game game, ProtoCrewMember crew, SuitCombo oldSuit, SuitCombo newSuit, string oldComboId, string newComboId)
	{
		LogCustomEvent("suitpicker_change", delegate
		{
			if (game != null)
			{
				m_EventData.Add("game_mode", game.Mode);
			}
			if (crew != null)
			{
				m_EventData.Add("gender", crew.gender);
			}
			if (oldSuit != null)
			{
				m_EventData.Add("old_type", oldSuit.suitType);
			}
			if (newSuit != null)
			{
				m_EventData.Add("new_type", newSuit.suitType);
			}
			m_EventData.Add("old_combo_id", oldComboId);
			m_EventData.Add("new_combo_id", newComboId);
		});
	}
}
