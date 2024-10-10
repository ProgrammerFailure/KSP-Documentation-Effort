using System;
using ns9;
using UnityEngine;

namespace Upgradeables;

[Serializable]
public class KSCFacilityLevelText
{
	public string textBase;

	public SpaceCenterFacility facility;

	public string linePrefix = "*";

	public static string[] FacilityNames = Enum.GetNames(typeof(SpaceCenterFacility));

	public string GetText(float normLevel)
	{
		string text = (string.IsNullOrEmpty(textBase) ? "" : textBase);
		while (text.Contains("["))
		{
			text = ProcessTextTags(text, normLevel);
		}
		while (text.Contains("\n\n"))
		{
			text = text.Replace("\n\n", "\n");
		}
		if (text != "" && !string.IsNullOrEmpty(linePrefix))
		{
			text = linePrefix + text.Trim().Replace("\n", "\n" + linePrefix);
		}
		return text;
	}

	public string ProcessTextTags(string src, float level)
	{
		if (src.Contains("["))
		{
			int num = src.LastIndexOf('[');
			while (num != -1)
			{
				int num2 = src.IndexOf(']', num);
				if (num2 != -1)
				{
					int startIndex = num + 1;
					int length = num2 - num - 1;
					string text = src.Substring(startIndex, length);
					if (!text.Contains("[") && !text.Contains("]"))
					{
						src = src.Remove(num, num2 - num + 1);
						string value = GetValue(text, level);
						src = src.Insert(num, value);
						num = src.LastIndexOf('[');
						continue;
					}
					Debug.LogError("KSCUpgradeableLevelText Syntax Error: Mismatched brackets on replacement block: " + src + ". nesting is not supported, ensure all opening brackets are closed.");
					return src;
				}
				Debug.LogError("KSCUpgradeableLevelText Syntax Error: Missing closing bracket on replacement block: " + src);
				return src;
			}
		}
		return src;
	}

	public string GetValue(string valueTag, float facilityLevel)
	{
		if (valueTag.ToLower().Contains("#autoloc"))
		{
			return Localizer.Format(valueTag);
		}
		switch (valueTag)
		{
		default:
		{
			string text;
			string text2;
			if (valueTag.Contains(":"))
			{
				string[] array = valueTag.Split(':');
				text = array[0];
				text2 = array[1];
				bool flag = false;
				int num = FacilityNames.Length;
				while (num-- > 0)
				{
					if (FacilityNames[num] == text2)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					facilityLevel = ScenarioUpgradeableFacilities.GetFacilityLevel(text2);
				}
				else
				{
					Debug.LogError("[KSCUpgradeableFacilityText]: Parse Error. No KSC Facility exists called " + text2 + "!");
					text2 = facility.ToString();
					if (facilityLevel == -1f)
					{
						facilityLevel = ScenarioUpgradeableFacilities.GetFacilityLevel(facility);
					}
				}
			}
			else
			{
				text = valueTag;
				text2 = facility.ToString();
				if (facilityLevel == -1f)
				{
					facilityLevel = ScenarioUpgradeableFacilities.GetFacilityLevel(facility);
				}
			}
			string metersText;
			Vector3 craftSize;
			switch (text)
			{
			case "TrackedObjects":
				return GetValueOrFallback(GameVariables.Instance.GetTrackedObjectLimit(facilityLevel), 2.1474836E+09f, "0", Localizer.Format("#autoLOC_416922"));
			case "ActionGroupsStock":
				if (!GameVariables.Instance.UnlockedActionGroupsStock(facilityLevel, facility == SpaceCenterFacility.VehicleAssemblyBuilding))
				{
					return "";
				}
				return Localizer.Format("#autoLOC_6002244");
			case "UnownedObjectsMinSize":
				if (!GameVariables.Instance.UnlockedSpaceObjectDiscovery(facilityLevel))
				{
					return "";
				}
				return Localizer.Format("#autoLOC_6002242") + " " + GameVariables.Instance.MinTrackedObjectSize(facilityLevel).ToString() + " " + Localizer.Format("#autoLOC_6002243");
			case "FlightPlanning":
				if (!GameVariables.Instance.UnlockedFlightPlanning(facilityLevel))
				{
					return "";
				}
				if (GameVariables.Instance.GetOrbitDisplayMode(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) != GameVariables.OrbitDisplayMode.PatchedConics)
				{
					return "<color=" + XKCDColors.HexFormat.Yellow + ">" + Localizer.Format("#autoLOC_6002238") + "</color>";
				}
				return Localizer.Format("#autoLOC_6002239");
			case "ScienceDataRatio":
				return KSPUtil.LocalizeNumber(GameVariables.Instance.GetDataToScienceRatio(facilityLevel) * 100f, "0.0##") + "%";
			case "ManeuverTool":
				if (!GameVariables.Instance.ManeuverToolAvailable(facilityLevel))
				{
					return "";
				}
				if (!GameVariables.Instance.UnlockedFlightPlanning(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.MissionControl)))
				{
					return Localizer.Format("#autoLOC_6002699");
				}
				return Localizer.Format("#autoLOC_6002675");
			case "facility":
				return facility.displayDescription();
			case "CrewLevel":
				return GetValueOrFallback(GameVariables.Instance.GetCrewLevelLimit(facilityLevel), float.MaxValue, "", Localizer.Format("#autoLOC_6002245"), (float s) => Localizer.Format("#autoLOC_6002246") + " " + KSPUtil.LocalizeNumber(s * 5f, "0"));
			case "EVAClamber":
				if (!GameVariables.Instance.UnlockedEVAClamber(facilityLevel))
				{
					return "";
				}
				return Localizer.Format("#autoLOC_6002240");
			case "EVAFlags":
				if (!GameVariables.Instance.UnlockedEVAFlags(facilityLevel))
				{
					return "";
				}
				return Localizer.Format("#autoLOC_416979");
			case "DSNRange":
				return Localizer.Format("#autoLOC_416970", KSPUtil.PrintSI(GameVariables.Instance.GetDSNRange(facilityLevel), string.Empty));
			case "PatchesAhead":
				return GetValueOrFallback(GameVariables.Instance.GetPatchesAheadLimit(facilityLevel), 2.1474836E+09f, "0", Localizer.Format("#autoLOC_416922"));
			case "StrategyLevel":
				return GetValueOrFallback(GameVariables.Instance.GetStrategyLevelLimit(facilityLevel), float.MaxValue, "", Localizer.Format("#autoLOC_6002245"), (float s) => Localizer.Format("#autoLOC_6002246") + " " + KSPUtil.LocalizeNumber(s * 5f, "0"));
			case "FuelTransfer":
				if (!GameVariables.Instance.UnlockedFuelTransfer(facilityLevel))
				{
					return "";
				}
				return Localizer.Format("#autoLOC_417008");
			case "ContractCount":
				return GetValueOrFallback(GameVariables.Instance.GetActiveContractsLimit(facilityLevel), 2.1474836E+09f, "0", Localizer.Format("#autoLOC_416922"));
			case "EVASrfSample":
			{
				ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment("surfaceSample");
				if (experiment != null)
				{
					if (GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
					{
						if (!(GameVariables.Instance.GetExperimentLevel(facilityLevel) >= experiment.requiredExperimentLevel))
						{
							return "";
						}
						return Localizer.Format("#autoLOC_6002241");
					}
					if (!(GameVariables.Instance.GetExperimentLevel(facilityLevel) >= experiment.requiredExperimentLevel))
					{
						return "";
					}
					return "<color=" + XKCDColors.HexFormat.Yellow + ">" + Localizer.Format("#autoLOC_416994") + "</color>";
				}
				return "";
			}
			case "ActionGroupsCustom":
				if (!GameVariables.Instance.UnlockedActionGroupsCustom(facilityLevel, facility == SpaceCenterFacility.VehicleAssemblyBuilding))
				{
					return "";
				}
				return Localizer.Format("#autoLOC_417015");
			case "CrewCount":
				return GetValueOrFallback(GameVariables.Instance.GetActiveCrewLimit(facilityLevel), 2.1474836E+09f, "0", Localizer.Format("#autoLOC_416922"));
			case "ActionGroups":
				if (!GameVariables.Instance.UnlockedActionGroupsStock(facilityLevel, facility == SpaceCenterFacility.VehicleAssemblyBuilding))
				{
					return "";
				}
				if (!GameVariables.Instance.UnlockedActionGroupsCustom(facilityLevel, facility == SpaceCenterFacility.VehicleAssemblyBuilding))
				{
					return Localizer.Format("#autoLOC_5050026");
				}
				return Localizer.Format("#autoLOC_417011");
			case "level":
				return (facilityLevel * (float)ScenarioUpgradeableFacilities.GetFacilityLevelCount(text2)).ToString("0");
			case "StrategyRange":
				return KSPUtil.LocalizeNumber(GameVariables.Instance.GetStrategyCommitRange(facilityLevel) * 100f, "0.0#") + "%";
			case "CraftParts":
				return GetValueOrFallback(GameVariables.Instance.GetPartCountLimit(facilityLevel, facility == SpaceCenterFacility.VehicleAssemblyBuilding), 2.1474836E+09f, "0", Localizer.Format("#autoLOC_416922"));
			case "UnownedObjects":
				if (!GameVariables.Instance.UnlockedSpaceObjectDiscovery(facilityLevel))
				{
					return "";
				}
				return Localizer.Format("#autoLOC_417002");
			case "CraftSizeYXZ":
			case "CraftSize":
				metersText = Localizer.Format("#autoLOC_7001411");
				craftSize = GameVariables.Instance.GetCraftSizeLimit(facilityLevel, facility == SpaceCenterFacility.LaunchPad);
				return GetValueOrFallback(craftSize.magnitude, float.MaxValue, "", Localizer.Format("#autoLOC_416922"), (float s) => KSPUtil.LocalizeNumber(craftSize.y, "0.0") + metersText + ", " + KSPUtil.LocalizeNumber(craftSize.x, "0.0") + metersText + ", " + KSPUtil.LocalizeNumber(craftSize.z, "0.0") + metersText);
			case "CraftMass":
				return GetValueOrFallback(GameVariables.Instance.GetCraftMassLimit(facilityLevel, facility == SpaceCenterFacility.LaunchPad), float.MaxValue, "", Localizer.Format("#autoLOC_416922"), (float s) => KSPUtil.LocalizeNumber(s, "0.0#") + Localizer.Format("#autoLOC_7001407"));
			case "ScienceCost":
				return GetValueOrFallback(GameVariables.Instance.GetScienceCostLimit(facilityLevel), float.MaxValue, "0", Localizer.Format("#autoLOC_416922"));
			case "ContractLevel":
				return GetValueOrFallback(GameVariables.Instance.GetContractLevelLimit(facilityLevel), float.MaxValue, "", Localizer.Format("#autoLOC_6002245"), (float s) => Localizer.Format("#autoLOC_6002246") + " " + KSPUtil.LocalizeNumber(s * 5f, "0"));
			case "CraftSizeHgtDiam":
				metersText = Localizer.Format("#autoLOC_7001411");
				craftSize = GameVariables.Instance.GetCraftSizeLimit(facilityLevel, facility == SpaceCenterFacility.LaunchPad);
				return GetValueOrFallback(craftSize.magnitude, float.MaxValue, "", Localizer.Format("#autoLOC_416922"), (float s) => KSPUtil.LocalizeNumber(craftSize.y, "0.0") + metersText + ", " + KSPUtil.LocalizeNumber(Mathf.Sqrt(Mathf.Pow(craftSize.x, 2f) + Mathf.Pow(craftSize.z, 2f)), "0.0") + metersText);
			case "EVA":
				if (!GameVariables.Instance.UnlockedEVA(facilityLevel))
				{
					return Localizer.Format("#autoLOC_7003404");
				}
				return Localizer.Format("#autoLOC_416977");
			case "StrategyCount":
				return GetValueOrFallback(GameVariables.Instance.GetActiveStrategyLimit(facilityLevel), 2.1474836E+09f, "0", Localizer.Format("#autoLOC_416922"));
			default:
				return "";
			case "MapMode":
				return GameVariables.Instance.GetOrbitDisplayMode(facilityLevel) switch
				{
					GameVariables.OrbitDisplayMode.CelestialBodyOrbits => "Map View Available", 
					GameVariables.OrbitDisplayMode.AllOrbits => Localizer.Format("#autoLOC_416965"), 
					GameVariables.OrbitDisplayMode.PatchedConics => Localizer.Format("#autoLOC_416967"), 
					_ => "Map View Available", 
				};
			}
		}
		case "K":
		case "ja":
		case "ru":
		case "zh":
		case "es":
		case "fr":
		case "it":
		case "de":
		case "pt":
			return "{" + valueTag + "}";
		}
	}

	public string GetValueOrFallback(float value, float threshold, string format, string fallback, Func<float, string> postProcess = null)
	{
		if (value < threshold)
		{
			if (postProcess == null)
			{
				return value.ToString(format);
			}
			return postProcess(value);
		}
		return fallback;
	}

	public static KSCFacilityLevelText CreateFromAsset(KSCUpgradeableLevelText asset)
	{
		return new KSCFacilityLevelText
		{
			linePrefix = asset.linePrefix,
			textBase = asset.textBase,
			facility = asset.facility
		};
	}
}
