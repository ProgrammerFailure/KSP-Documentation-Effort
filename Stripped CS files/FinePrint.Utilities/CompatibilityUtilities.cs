using System.Collections.Generic;
using Contracts;
using UnityEngine;

namespace FinePrint.Utilities;

public class CompatibilityUtilities
{
	public static int majorVersion = int.MaxValue;

	public static int minorVersion = int.MaxValue;

	public static int revisionVersion = int.MaxValue;

	public static bool OldCareerSave(Game.Modes mode, int saveMajor, int saveMinor, int saveRevision)
	{
		majorVersion = saveMajor;
		minorVersion = saveMinor;
		revisionVersion = saveRevision;
		if (mode != Game.Modes.CAREER)
		{
			return false;
		}
		if (Versioning.version_major > saveMajor)
		{
			return true;
		}
		if (Versioning.version_major == saveMajor)
		{
			if (Versioning.version_minor > saveMinor)
			{
				return true;
			}
			if (Versioning.version_minor == saveMinor && Versioning.Revision > saveRevision)
			{
				return true;
			}
		}
		return false;
	}

	public static bool OldCareerSave(Game.Modes mode, string versionString)
	{
		string[] array = versionString.Split('.');
		if (array.Length != 3)
		{
			return true;
		}
		if (!int.TryParse(array[0], out var result))
		{
			return true;
		}
		if (!int.TryParse(array[1], out var result2))
		{
			return true;
		}
		if (!int.TryParse(array[2], out var result3))
		{
			return true;
		}
		return OldCareerSave(mode, result, result2, result3);
	}

	public static void UpdateCareerSave(ConfigNode node)
	{
		if (!node.HasValue("name") || node.GetValue("name") != "ContractSystem")
		{
			return;
		}
		ConfigNode node2 = node.GetNode("CONTRACTS");
		if (node2 == null)
		{
			return;
		}
		bool flag = false;
		int count = node2.nodes.Count;
		while (count-- > 0)
		{
			ConfigNode configNode = node2.nodes[count];
			if ((configNode.name != "CONTRACT" && configNode.name != "CONTRACT_FINISHED") || !configNode.HasValue("type"))
			{
				continue;
			}
			string value = configNode.GetValue("type");
			switch (value)
			{
			case "BaseContract":
				if (!configNode.HasValue("contextual"))
				{
					configNode.AddValue("contextual", value: false);
					flag = true;
				}
				break;
			case "StationContract":
				if (!configNode.HasValue("contextual"))
				{
					configNode.AddValue("contextual", value: false);
					flag = true;
				}
				break;
			case "PartTest":
				if (!configNode.HasValue("haul"))
				{
					configNode.AddValue("haul", value: false);
					flag = true;
				}
				break;
			case "SurveyContract":
				if (!configNode.HasValue("contextual"))
				{
					configNode.AddValue("contextual", value: false);
					flag = true;
				}
				if (!configNode.HasValue("targetBand"))
				{
					configNode.AddValue("targetBand", FlightBand.NONE);
					flag = true;
				}
				if (!configNode.HasValue("locationName"))
				{
					CelestialBody value5 = Planetarium.fetch.Home;
					SystemUtilities.LoadNode(configNode, "CompatibilityUtilities", "targetBody", ref value5, Planetarium.fetch.Home, logging: false);
					configNode.AddValue("locationName", value5.displayName);
					flag = true;
				}
				if (!configNode.HasValue("dataName") || !configNode.HasValue("anomalyName") || !configNode.HasValue("reportName"))
				{
					SurveyDefinition surveyDefinition = null;
					int count2 = configNode.nodes.Count;
					while (count2-- > 0)
					{
						ConfigNode configNode3 = configNode.nodes[count2];
						if (configNode3.HasValue("name") && !(configNode3.GetValue("name") != "SurveyWaypointParameter"))
						{
							if (configNode3.HasValue("experiment"))
							{
								surveyDefinition = FindSurveyDefinition(configNode3.GetValue("experiment"));
							}
							if (surveyDefinition != null)
							{
								break;
							}
						}
					}
					if (surveyDefinition != null)
					{
						if (!configNode.HasValue("dataName"))
						{
							configNode.AddValue("dataName", surveyDefinition.DataName);
						}
						if (!configNode.HasValue("anomalyName"))
						{
							configNode.AddValue("anomalyName", surveyDefinition.AnomalyName);
						}
						if (!configNode.HasValue("reportName"))
						{
							configNode.AddValue("reportName", surveyDefinition.ResultName);
						}
						flag = true;
					}
				}
				configNode.RemoveValues("surveyTitle");
				configNode.RemoveValues("surveyBriefing");
				configNode.RemoveValues("surveyDebriefing");
				break;
			case "RecordTrackContract":
				node2.nodes.Remove(configNode);
				flag = true;
				break;
			case "ISRUContract":
				if (configNode.HasValue("deliverySituation") && SavePriorTo(1, 0, 6))
				{
					string newValue = UpgradeVesselSituation(configNode.GetValue("deliverySituation"));
					configNode.SetValue("deliverySituation", newValue);
					flag = true;
				}
				break;
			case "SatelliteContract":
				if (!configNode.HasValue("placementMode"))
				{
					configNode.AddValue("placementMode", "0");
					flag = true;
				}
				break;
			case "TourismContract":
				if (!configNode.HasValue("preposition"))
				{
					Contract.ContractPrestige value2 = Contract.ContractPrestige.Trivial;
					SystemUtilities.LoadNode(configNode, "CompatibilityUtilities", "prestige", ref value2, Contract.ContractPrestige.Trivial, logging: false);
					string value3 = "";
					if (value2 <= Contract.ContractPrestige.Significant)
					{
						CelestialBody value4 = null;
						ConfigNode[] nodes = configNode.GetNodes("PARAM");
						ConfigNode configNode2 = null;
						int num = nodes.Length;
						while (num-- > 0)
						{
							if (nodes[num].HasValue("name") && nodes[num].GetValue("name") == "KerbalTourParameter")
							{
								configNode2 = nodes[num];
								break;
							}
						}
						if (configNode2 != null)
						{
							ConfigNode[] nodes2 = configNode2.GetNodes("PARAM");
							int num2 = nodes2.Length - 1;
							while (num2 >= 0)
							{
								if (!nodes2[num2].HasValue("name") || !(nodes2[num2].GetValue("name") == "KerbalDestinationParameter"))
								{
									num2--;
									continue;
								}
								SystemUtilities.LoadNode(node, "CompatibilityUtilities", "targetBody", ref value4, Planetarium.fetch.Home, logging: false);
								break;
							}
						}
						if (value4 != null)
						{
							value3 = (value4.isHomeWorld ? ("right here on " + value4.displayName) : ("around " + value4.displayName));
						}
					}
					else
					{
						value3 = "of the solar system";
					}
					configNode.AddValue("preposition", value3);
					flag = true;
				}
				if (!configNode.HasValue("homeDestinations"))
				{
					configNode.AddValue("homeDestinations", value: false);
					flag = true;
				}
				if (configNode.GetValues("kerbalName").Length != 0)
				{
					SystemUtilities.CondenseNodeList(configNode, "kerbalName", "tourists");
					flag = true;
				}
				break;
			}
			ConfigNode[] nodes3 = configNode.GetNodes("PARAM");
			int num3 = nodes3.Length;
			while (num3-- > 0)
			{
				ConfigNode configNode4 = nodes3[num3];
				if (!configNode4.HasValue("name"))
				{
					continue;
				}
				string value6 = configNode4.GetValue("name");
				switch (value6)
				{
				case "VesselSystemsParameter":
				{
					if (!configNode4.HasValue("checkModuleTypes") || !SavePriorTo(1, 1, 4))
					{
						break;
					}
					string[] array = configNode4.GetValue("checkModuleTypes").Split('|');
					int i = 0;
					for (int num4 = array.Length; i < num4; i++)
					{
						switch (array[i])
						{
						case "Dock":
							array[i] = "Dock";
							break;
						case "Power":
							array[i] = "Generator";
							break;
						case "Antenna":
							array[i] = "Antenna";
							break;
						}
					}
					configNode4.SetValue("checkModuleTypes", string.Join("|", array));
					flag = true;
					break;
				}
				case "SpecificOrbitParameter":
					if (!configNode4.HasValue("TargetBody"))
					{
						string value15 = "0";
						SystemUtilities.LoadNode(configNode4, "CompatibilityUtilities", "targetBody", ref value15, "0", logging: false);
						configNode4.RemoveValues("targetBody");
						configNode4.AddValue("TargetBody", value15);
						flag = true;
					}
					break;
				case "ProbeSystemsParameter":
				case "FacilitySystemsParameter":
					configNode4.SetValue("name", "VesselSystemsParameter", createIfNotFound: true);
					flag = true;
					if (!configNode4.HasValue("typeString"))
					{
						string value19 = "vessel";
						switch (value)
						{
						case "BaseContract":
							value19 = "outpost";
							break;
						case "StationContract":
							value19 = "station";
							break;
						case "SatelliteContract":
							value19 = "satellite";
							break;
						}
						configNode4.AddValue("typeString", value19);
					}
					if (!configNode4.HasValue("mannedStatus"))
					{
						configNode4.AddValue("mannedStatus", (value6 == "ProbeSystemsParameter") ? MannedStatus.UNMANNED : MannedStatus.const_2);
					}
					if (!configNode4.HasValue("requireNew"))
					{
						configNode4.AddValue("requireNew", value: true);
					}
					if (!configNode4.HasValue("checkModuleTypes"))
					{
						configNode4.AddValue("checkModuleTypes", (value6 == "ProbeSystemsParameter") ? "Antenna|Power" : "Antenna|Power|Dock");
					}
					if (!configNode4.HasValue("checkModuleDescriptions"))
					{
						configNode4.AddValue("checkModuleDescriptions", (value6 == "ProbeSystemsParameter") ? "has an antenna|can generate power" : "has an antenna|has a docking port|can generate power");
					}
					configNode4.RemoveValues("hasAntenna");
					configNode4.RemoveValues("hasPowerGenerator");
					configNode4.RemoveValues("hasDockingPort");
					break;
				case "LocationAndSituationParameter":
					if (configNode4.HasValue("targetSituation") && SavePriorTo(1, 0, 6))
					{
						string newValue2 = UpgradeVesselSituation(configNode4.GetValue("targetSituation"));
						configNode4.SetValue("targetSituation", newValue2);
						flag = true;
					}
					break;
				case "SurveyWaypointParameter":
					if (!configNode4.HasValue("contextual"))
					{
						configNode4.AddValue("contextual", value: false);
						flag = true;
					}
					break;
				case "PartTest":
					if (!configNode4.HasValue("body"))
					{
						CelestialBody value16 = null;
						SystemUtilities.LoadNode(configNode, "CompatibilityUtilities", "dest", ref value16, null, logging: false);
						if (value16 != null)
						{
							configNode4.AddValue("body", value16.bodyName);
							flag = true;
						}
					}
					if (!configNode4.HasValue("situation"))
					{
						string value17 = "ORBITING";
						SystemUtilities.LoadNode(configNode, "CompatibilityUtilities", "sit", ref value17, "ORBITING", logging: false);
						configNode4.AddValue("situation", value17);
						flag = true;
					}
					if (!configNode4.HasValue("uniqueID"))
					{
						int value18 = 0;
						SystemUtilities.LoadNode(configNode, "CompatibilityUtilities", "seed", ref value18, 0, logging: false);
						KSPRandom kSPRandom = new KSPRandom(value18);
						configNode4.AddValue("uniqueID", kSPRandom.NextDouble().ToString("G17"));
						flag = true;
					}
					if (!configNode4.HasValue("haul"))
					{
						configNode4.AddValue("haul", value: false);
						flag = true;
					}
					if (!configNode4.HasValue("repeatability"))
					{
						configNode4.AddValue("repeatability", PartTestConstraint.TestRepeatability.ONCEPERPART.ToString());
						flag = true;
					}
					break;
				case "RecordTrackingParameter":
					configNode.nodes.Remove(configNode4);
					flag = true;
					break;
				case "ResourceExtractionParameter":
					if (!configNode4.HasValue("modules"))
					{
						SystemUtilities.SaveNodeList(configNode4, "modules", new List<string> { "ModuleResourceHarvester" });
						flag = true;
					}
					break;
				case "StationaryPointParameter":
					if (!configNode4.HasValue("deviation"))
					{
						configNode4.AddValue("deviation", 12.5);
						flag = true;
					}
					break;
				case "PartRequestParameter":
					if (configNode4.HasValue("partNames"))
					{
						string value7 = configNode4.GetValue("partNames");
						if (value7.Contains(","))
						{
							List<string> list = new List<string>(value7.Split(','));
							configNode4.RemoveValues("partNames");
							SystemUtilities.SaveNodeList(configNode4, "partNames", list);
							flag = true;
						}
					}
					if (configNode4.HasValue("moduleNames"))
					{
						string value8 = configNode4.GetValue("moduleNames");
						if (value8.Contains(","))
						{
							List<string> list2 = new List<string>(value8.Split(','));
							configNode4.RemoveValues("moduleNames");
							SystemUtilities.SaveNodeList(configNode4, "moduleNames", list2);
							flag = true;
						}
					}
					if (!configNode4.HasValue("partDescription"))
					{
						string value9 = "part";
						string value10 = configNode4.GetValue("partNames");
						string value11 = configNode4.GetValue("moduleNames");
						List<string> partNames = new List<string>(value10.Split('|'));
						List<string> moduleNames = new List<string>(value11.Split('|'));
						Part part = VesselUtilities.FindFirstPartOrModuleName(partNames, moduleNames);
						if (part != null)
						{
							value9 = part.partInfo.title;
						}
						configNode4.AddValue("partDescription", value9);
					}
					if (!configNode4.HasValue("article"))
					{
						string value12 = "a";
						string value13 = configNode4.GetValue("partDescription");
						if (value13.Length > 0 && StringUtilities.IsVowel(value13[0]))
						{
							value12 = "an";
						}
						configNode4.AddValue("article", value12);
					}
					if (!configNode4.HasValue("vesselDescription"))
					{
						string value14 = "vessel";
						switch (value)
						{
						case "SatelliteContract":
							value14 = "satellite";
							break;
						case "BaseContract":
							value14 = "outpost";
							break;
						case "StationContract":
							value14 = "station";
							break;
						}
						configNode4.AddValue("vesselDescription", value14);
					}
					configNode4.RemoveValues("title");
					break;
				}
			}
		}
		if (flag)
		{
			Debug.Log("Contract Log: Outdated save file detected! All contracts in save file were updated!");
		}
	}

	public static SurveyDefinition FindSurveyDefinition(string experiment)
	{
		ConfigNode configNode = null;
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("Contracts");
		int num = configNodes.Length;
		while (num-- > 0)
		{
			configNode = configNodes[num];
		}
		if (configNode == null)
		{
			Debug.LogWarning("Contract Log: Failed to update outdated survey contract - invalid configuration!");
			return null;
		}
		ConfigNode node = configNode.GetNode("Survey");
		if (node == null)
		{
			Debug.LogWarning("Contract Log: Failed to update outdated survey contract - invalid configuration!");
			return null;
		}
		ConfigNode[] nodes = node.GetNodes("SURVEY_DEFINITION");
		int num2 = nodes.Length;
		while (num2-- > 0)
		{
			ConfigNode configNode2 = nodes[num2];
			ConfigNode[] nodes2 = configNode2.GetNodes("PARAM");
			int num3 = nodes2.Length;
			while (num3-- > 0)
			{
				ConfigNode configNode3 = nodes2[num3];
				if (configNode3.HasValue("Experiment") && configNode3.GetValue("Experiment") == experiment)
				{
					return new SurveyDefinition(configNode2);
				}
			}
		}
		Debug.LogWarning("Contract Log: Failed to update outdated survey contract - survey definition not found!");
		return null;
	}

	public static bool SavePriorTo(int major, int minor, int revision)
	{
		return KSPUtil.CheckVersion(majorVersion, minorVersion, revisionVersion, major, minor, revision) == VersionCompareResult.INCOMPATIBLE_TOO_EARLY;
	}

	public static string UpgradeVesselSituation(string situation)
	{
		int result = -1;
		if (!int.TryParse(situation, out result))
		{
			return situation;
		}
		if (SavePriorTo(1, 0, 5))
		{
			switch (result)
			{
			case 0:
				return Vessel.Situations.LANDED.ToString();
			case 1:
				return Vessel.Situations.SPLASHED.ToString();
			case 2:
				return Vessel.Situations.PRELAUNCH.ToString();
			case 3:
				return Vessel.Situations.FLYING.ToString();
			case 4:
				return Vessel.Situations.SUB_ORBITAL.ToString();
			case 5:
				return Vessel.Situations.ORBITING.ToString();
			case 6:
				return Vessel.Situations.ESCAPING.ToString();
			case 7:
				return Vessel.Situations.DOCKED.ToString();
			}
		}
		else if (SavePriorTo(1, 0, 6))
		{
			switch (result)
			{
			case 32:
				return Vessel.Situations.ORBITING.ToString();
			case 0:
				return Vessel.Situations.LANDED.ToString();
			case 1:
				return Vessel.Situations.LANDED.ToString();
			case 2:
				return Vessel.Situations.SPLASHED.ToString();
			case 3:
				return Vessel.Situations.FLYING.ToString();
			case 4:
				return Vessel.Situations.PRELAUNCH.ToString();
			case 5:
				return Vessel.Situations.ORBITING.ToString();
			case 6:
				return Vessel.Situations.ESCAPING.ToString();
			case 7:
				return Vessel.Situations.DOCKED.ToString();
			case 8:
				return Vessel.Situations.FLYING.ToString();
			case 16:
				return Vessel.Situations.SUB_ORBITAL.ToString();
			case 128:
				return Vessel.Situations.DOCKED.ToString();
			case 64:
				return Vessel.Situations.ESCAPING.ToString();
			}
		}
		return situation;
	}

	public static void CleanUpUnsanitaryEVAKerbals(ConfigNode flightState)
	{
		int num = int.Parse(flightState.GetValue("activeVessel"));
		ConfigNode[] nodes = flightState.GetNodes("VESSEL");
		int num2 = nodes.Length;
		while (num2-- > 0)
		{
			ConfigNode configNode = nodes[num2];
			if (!(configNode.GetValue("type") == "EVA"))
			{
				continue;
			}
			ConfigNode[] nodes2 = configNode.GetNodes("PART");
			int i = 0;
			for (int num3 = nodes2.Length; i < num3; i++)
			{
				ConfigNode configNode2 = nodes2[i];
				if (configNode2.GetValue("name") == "kerbalEVA" && configNode2.GetValue("crew") == null)
				{
					flightState.RemoveNode(configNode);
					if (num2 > num)
					{
						num--;
					}
					break;
				}
			}
		}
		flightState.SetValue("activeVessel", num.ToString());
	}
}
