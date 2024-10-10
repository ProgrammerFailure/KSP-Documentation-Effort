using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using CommNet;
using CompoundParts;
using Expansions;
using Expansions.Missions;
using Expansions.Serenity;
using FinePrint.Utilities;
using KerbNet;
using Lingoona;
using ModuleWheels;
using ns11;
using ns12;
using ns13;
using ns17;
using ns23;
using ns36;
using PreFlightTests;
using SentinelMission;
using Strategies;
using UnityEngine;

namespace ns9;

public class Localizer
{
	public static Dictionary<string, string> LanguageIds;

	public Dictionary<string, string> tagValues = new Dictionary<string, string>();

	public string _currentLanguage = "";

	public bool _showKeysInGame;

	public bool _overrideMELock;

	public static List<string> missingKeysList = new List<string>();

	public bool _debugWriteMissingKeysToLog;

	public string missingKeyPrefix = "Localization.Format Missed Key in:";

	public List<string> outputForReplaceTags = new List<string>();

	public static Localizer Instance { get; set; }

	public static int TagsLength => Instance.tagValues.Count;

	public static Dictionary<string, string> Tags => Instance.tagValues;

	public static string CurrentLanguage => Instance._currentLanguage;

	public static bool ShowKeysOnScreen
	{
		get
		{
			if (Instance != null)
			{
				return Instance._showKeysInGame;
			}
			return false;
		}
		set
		{
			Instance._showKeysInGame = value;
		}
	}

	public static bool OverrideMELock
	{
		get
		{
			if (Instance != null)
			{
				return Instance._overrideMELock;
			}
			return false;
		}
		set
		{
			Instance._overrideMELock = value;
		}
	}

	public static bool debugWriteMissingKeysToLog
	{
		get
		{
			if (Instance != null)
			{
				return Instance._debugWriteMissingKeysToLog;
			}
			return false;
		}
		set
		{
			if (Instance != null)
			{
				if (value && !Instance._debugWriteMissingKeysToLog)
				{
					missingKeysList = new List<string>();
				}
				Instance._debugWriteMissingKeysToLog = value;
			}
		}
	}

	public static Localizer Init()
	{
		LanguageIds = new Dictionary<string, string>();
		LanguageIds.Add("en", "en");
		LanguageIds.Add("es", "es");
		LanguageIds.Add("ja", "jp");
		LanguageIds.Add("ru", "ru");
		LanguageIds.Add("zh", "cn");
		LanguageIds.Add("de", "de");
		LanguageIds.Add("fr", "fr");
		LanguageIds.Add("it", "it");
		LanguageIds.Add("pt", "pt");
		if (Instance != null)
		{
			return Instance;
		}
		Instance = new Localizer();
		SwitchToLanguage(GetLanguageIdFromFile());
		debugWriteMissingKeysToLog = GameSettings.LOG_MISSING_KEYS_TO_FILE;
		ShowKeysOnScreen = GameSettings.SHOW_TRANSLATION_KEYS_ON_SCREEN;
		return Instance;
	}

	public static string GetStringByTag(string tag)
	{
		return Instance.tagValues[tag];
	}

	public static string Format(string template, params string[] list)
	{
		return Instance._Format(template, list);
	}

	public static string Format(string template, params object[] list)
	{
		string[] array = new string[list.Length];
		for (int i = 0; i < list.Length; i++)
		{
			array[i] = list[i].ToString();
		}
		return Instance._Format(template, array);
	}

	public static string Format(string template)
	{
		string result = "";
		if (Instance != null)
		{
			result = Instance._Format(template, new string[0]);
		}
		return result;
	}

	public static bool TryGetStringByTag(string tagName, out string value)
	{
		return Instance.tagValues.TryGetValue(tagName, out value);
	}

	public static void TranslateBranch(ConfigNode branchRoot)
	{
		ConfigNode[] nodes = branchRoot.GetNodes();
		for (int i = 0; i < nodes.Length; i++)
		{
			TranslateBranch(nodes[i]);
		}
		ConfigNode.ValueList values = branchRoot.values;
		for (int j = 0; j < values.Count; j++)
		{
			values[j].value = Format(values[j].value);
		}
	}

	public string _Format(string template, string[] parameterList)
	{
		if (ShowKeysOnScreen)
		{
			return template;
		}
		string result = "";
		if (string.IsNullOrEmpty(template))
		{
			return result;
		}
		result = ((parameterList.Length != 0) ? FormatWithLingoona(template, parameterList) : ReplaceSingleTagIfFound(template));
		result = unescapeFormattedString(result);
		if (debugWriteMissingKeysToLog && !result.Contains(missingKeyPrefix) && result.ToLower().Contains("#autoloc") && !missingKeysList.Contains(result))
		{
			missingKeysList.Add(result);
			Debug.LogWarning(missingKeyPrefix + " " + result);
		}
		return result;
	}

	public string FormatWithLingoona(string template, string[] parameterList)
	{
		template = ReplaceSingleTagIfFound(template);
		List<string> parameters = ReplaceTags(parameterList);
		return Grammar.useGrammar(template, parameters);
	}

	public string ReplaceSingleTagIfFound(string tag)
	{
		string value = "";
		if (string.IsNullOrEmpty(tag))
		{
			return value;
		}
		if (tagValues.TryGetValue(tag, out value))
		{
			return value;
		}
		return tag;
	}

	public List<string> ReplaceTags(string[] list)
	{
		outputForReplaceTags.Clear();
		for (int i = 0; i < list.Length; i++)
		{
			outputForReplaceTags.Add(ReplaceSingleTagIfFound(list[i]));
		}
		return outputForReplaceTags;
	}

	public static string GetLanguageIdFromFile()
	{
		string text = "buildID64.txt";
		string result = "en-us";
		if (File.Exists(KSPUtil.ApplicationRootPath + "/" + text))
		{
			string[] array = File.ReadAllLines(KSPUtil.ApplicationRootPath + "/" + text);
			foreach (string text2 in array)
			{
				if (text2.Contains("language") && text2.Contains("="))
				{
					string[] array2 = text2.Split('=');
					if (array2.Length == 2)
					{
						result = array2[1].Trim();
					}
				}
			}
		}
		return result;
	}

	public static bool SwitchToLanguage(string lang)
	{
		try
		{
			Instance._SwitchToLanguage(lang);
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to switch language to " + lang + "\nDetails:" + ex.Message);
			return false;
		}
		try
		{
			Instance._CacheLocalStrings();
		}
		catch (Exception ex2)
		{
			Debug.LogError("Failed to run through CacheLocalStrings\nDetails:" + ex2.Message);
			return false;
		}
		GameEvents.onLanguageSwitched.Fire();
		return true;
	}

	public void _SwitchToLanguage(string lang)
	{
		_currentLanguage = lang;
		RefreshTagValues();
		RefreshRuleSet();
	}

	public void _CacheLocalStrings()
	{
		KSPUtil.CacheLocalStrings();
		StringUtilities.CacheLocalStrings();
		Part.CacheLocalStrings();
		KerbNetDialog.CacheLocalStrings();
		METDisplay.CacheLocalStrings();
		TelemetryUpdate.CacheLocalStrings();
		ModuleProbeControlPoint.CacheLocalStrings();
		TooltipController_SignalStrength.CacheLocalStrings();
		FlightOverlays.CacheLocalStrings();
		QuickSaveLoad.CacheLocalStrings();
		InternalSpeed.CacheLocalStrings();
		ModuleAeroSurface.CacheLocalStrings();
		ModuleControlSurface.CacheLocalStrings();
		ModuleLiftingSurface.CacheLocalStrings();
		ModuleParachute.CacheLocalStrings();
		ModuleAnimateGeneric.CacheLocalStrings();
		ModuleAnimationSetter.CacheLocalStrings();
		ModuleColorChanger.CacheLocalStrings();
		CModuleFuelLine.CacheLocalStrings();
		CModuleStrut.CacheLocalStrings();
		ModuleCommand.CacheLocalStrings();
		ModuleReactionWheel.CacheLocalStrings();
		ModuleSAS.CacheLocalStrings();
		ModuleEngines.CacheLocalStrings();
		ModuleProceduralFairing.CacheLocalStrings();
		ModuleLight.CacheLocalStrings();
		ModuleAsteroid.CacheLocalStrings();
		ModuleComet.CacheLocalStrings();
		ModuleTestSubject.CacheLocalStrings();
		ModuleActiveRadiator.CacheLocalStrings();
		ModuleDeployableRadiator.CacheLocalStrings();
		ModuleDeployablePart.CacheLocalStrings();
		ModuleDeployableSolarPanel.CacheLocalStrings();
		ModuleGenerator.CacheLocalStrings();
		ModuleResourceIntake.CacheLocalStrings();
		ModuleEnviroSensor.CacheLocalStrings();
		BaseConverter.CacheLocalStrings();
		ModuleScienceConverter.CacheLocalStrings();
		SentinelModule.CacheLocalStrings();
		ModuleWheelDeployment.CacheLocalStrings();
		ModuleWheelMotor.CacheLocalStrings();
		ModuleAnimationGroup.CacheLocalStrings();
		ModuleAsteroidDrill.CacheLocalStrings();
		ModuleGPS.CacheLocalStrings();
		ModuleOrbitalScanner.CacheLocalStrings();
		ModuleOverheatDisplay.CacheLocalStrings();
		ModuleResourceHarvester.CacheLocalStrings();
		ModuleResourceScanner.CacheLocalStrings();
		ResourceConverter.CacheLocalStrings();
		ModuleRCS.CacheLocalStrings();
		ModuleRCSFX.CacheLocalStrings();
		KerbalSeat.CacheLocalStrings();
		ModuleEnginesFX.CacheLocalStrings();
		ModuleGimbal.CacheLocalStrings();
		ModuleTripLogger.CacheLocalStrings();
		Vessel.CacheLocalStrings();
		TimeWarp.CacheLocalStrings();
		KerbalPortrait.CacheLocalStrings();
		PartListTooltip.CacheLocalStrings();
		EngineersReport.CacheLocalStrings();
		NavBallBurnVector.CacheLocalStrings();
		MapNode.CacheLocalStrings();
		OrbitRendererBase.CacheLocalStrings();
		OrbitTargeter.CacheLocalStrings();
		KbApp_UnownedInfo.CacheLocalStrings();
		KbApp_VesselInfo.CacheLocalStrings();
		EditorLogicBase.CacheLocalStrings();
		ProtoVessel.CacheLocalStrings();
		ModuleKerbNetAccess.CacheLocalStrings();
		CanAffordLaunchTest.CacheLocalStrings();
		CraftWithinMassLimits.CacheLocalStrings();
		CraftWithinPartCountLimit.CacheLocalStrings();
		CraftWithinSizeLimits.CacheLocalStrings();
		AirBreathingEnginesForIntakes.CacheLocalStrings();
		AntennaPresent.CacheLocalStrings();
		ComOffset.CacheLocalStrings();
		ContractEquipment.CacheLocalStrings();
		DecouplerFacing.CacheLocalStrings();
		DecouplersBeforeClamps.CacheLocalStrings();
		DockingPortAsDecoupler.CacheLocalStrings();
		DockingPortFacing.CacheLocalStrings();
		DockingPortRCS.CacheLocalStrings();
		ElectricBatteryAndNoCharge.CacheLocalStrings();
		ElectricChargeAndNoBattery.CacheLocalStrings();
		ElectricChargeAndNoConsumer.CacheLocalStrings();
		ElectricConsumerAndNoCharge.CacheLocalStrings();
		EnginesJettisonedBeforeUse.CacheLocalStrings();
		HatchObstructed.CacheLocalStrings();
		InputAuthority.CacheLocalStrings();
		IntakesForAirBreathingEngines.CacheLocalStrings();
		LadderPresent.CacheLocalStrings();
		LandingGearPresent.CacheLocalStrings();
		MissingCrew.CacheLocalStrings();
		NonRootCmdMissaligned.CacheLocalStrings();
		ParachuteOnEngineStage.CacheLocalStrings();
		ParachuteOnFirstStage.CacheLocalStrings();
		ParachutePresent.CacheLocalStrings();
		RequiredIScienceDataTransmitter.CacheLocalStrings();
		StationHubAttachments.CacheLocalStrings();
		KerbNetModeTerrain.CacheLocalStrings();
		KerbNetModeBiome.CacheLocalStrings();
		KerbNetModeResource.CacheLocalStrings();
		CelestialBody.CacheLocalStrings();
		Strategy.CacheLocalStrings();
		VesselPrecalculate.CacheLocalStrings();
		TooltipController_CrewAC.CacheLocalStrings();
		TrackingStationWidget.CacheLocalStrings();
		DiscoveryInfo.CacheLocalStrings();
		StageGroup.CacheLocalStrings();
		StageManager.CacheLocalStrings();
		DeltaVAppSituation.CacheLocalStrings();
		ModuleGroundSciencePart.CacheLocalStrings();
		ModuleGroundExpControl.CacheLocalStrings();
		ModuleGroundExperiment.CacheLocalStrings();
		ManeuverNodeEditorManager.CacheLocalStrings();
		ManeuverNodeEditorTabIntercept.CacheLocalStrings();
		ManeuverNodeEditorTabOrbitBasic.CacheLocalStrings();
		ManeuverNodeEditorTabVectorHandles.CacheLocalStrings();
		ManeuverNodeEditorTabVectorInput.CacheLocalStrings();
		BaseServo.CacheLocalStrings();
		ModuleRoboticServoPiston.CacheLocalStrings();
		BaseField.CacheLocalStrings();
		ModuleWheelDamage.CacheLocalStrings();
		ModuleGroundPart.CacheLocalStrings();
	}

	public void RefreshRuleSet()
	{
		if (_currentLanguage.Length >= 2)
		{
			string text = _currentLanguage.Substring(0, 2);
			if (LanguageIds.ContainsKey(text))
			{
				text = LanguageIds[text];
			}
			Grammar.setLanguage(text);
		}
	}

	public void RefreshTagValues()
	{
		tagValues.Clear();
		AddTagValuesForLanguage("en-us");
		AddTagValuesForLanguage(_currentLanguage);
	}

	public void AddTagValuesForLanguage(string lang)
	{
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("Localization");
		int num = configNodes.Length;
		while (num-- > 0)
		{
			ConfigNode[] nodes = configNodes[num].GetNodes(lang);
			int num2 = nodes.Length;
			while (num2-- > 0)
			{
				AddTagValuesFromNode(nodes[num2]);
			}
		}
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			AppendMissionTags(lang);
		}
	}

	public static void AppendMissionTags(string lang)
	{
		ConfigNode[] array = MissionLocalizer.LoadMissionLocalizationFiles().ToArray();
		int num = array.Length;
		while (num-- > 0)
		{
			ConfigNode[] nodes = array[num].GetNodes(lang);
			int num2 = nodes.Length;
			while (num2-- > 0)
			{
				Instance.AddTagValuesFromNode(nodes[num2]);
			}
		}
	}

	public void AddTagValuesFromNode(ConfigNode node)
	{
		ConfigNode.ValueList values = node.values;
		int count = values.Count;
		while (count-- > 0)
		{
			ConfigNode.Value value = values[count];
			tagValues[value.name] = unescapeStringToLingoona(value.value);
		}
	}

	public string unescapeStringToLingoona(string originalString)
	{
		string text = originalString;
		text = text.Replace("｢", "{");
		text = text.Replace("｣", "}");
		return new Regex("\\\\[uU]([0-9A-F]{4})").Replace(text, (Match match) => ((char)int.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
	}

	public string unescapeFormattedString(string formattedString)
	{
		return formattedString.Replace("\\n", "\n").Replace("\\\"", "\"").Replace("\\t", "\t");
	}
}
