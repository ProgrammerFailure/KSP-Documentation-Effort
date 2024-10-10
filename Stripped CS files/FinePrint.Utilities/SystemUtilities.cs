using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Contracts;
using Contracts.Templates;
using FinePrint.Contracts;
using FinePrint.Contracts.Parameters;
using ns9;
using UnityEngine;

namespace FinePrint.Utilities;

public class SystemUtilities
{
	public const int frameSuccessDelay = 5;

	public static Texture2D LoadTexture(string textureName)
	{
		textureName = (textureName.Contains("/") ? textureName : ("Squad/Contracts/Icons/" + textureName));
		Texture2D texture2D = GameDatabase.Instance.GetTexture(textureName, asNormalMap: false);
		if (texture2D == null)
		{
			texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, Color.white);
			texture2D.Apply();
		}
		return texture2D;
	}

	public static Color RandomColor(int seed = 0, float alpha = 1f, float saturation = 1f, float brightness = 0.5f)
	{
		return new ColorHSV((float)((seed == 0) ? new KSPRandom() : new KSPRandom(seed)).NextDouble(), Mathf.Clamp01(saturation), Mathf.Clamp01(brightness), Mathf.Clamp01(alpha)).ToColor();
	}

	public static bool TryConvert<T>(string input, out T value, ref string error)
	{
		if (typeof(T).IsEnum)
		{
			try
			{
				value = (T)Enum.Parse(typeof(T), input);
				return true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
				value = default(T);
				return false;
			}
		}
		if (typeof(T) == typeof(Guid))
		{
			try
			{
				value = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(input);
				return true;
			}
			catch (Exception ex2)
			{
				error = ex2.Message;
				value = default(T);
				return false;
			}
		}
		try
		{
			value = (T)Convert.ChangeType(input, typeof(T));
			return true;
		}
		catch (Exception ex3)
		{
			error = ex3.Message;
			value = default(T);
			return false;
		}
	}

	public static PreBuiltCraftDefinition ValidateLoadCraftNode(string url, string className, bool brokenParts)
	{
		return ValidateLoadCraftNode(url, className, brokenParts, bypassTechCheck: false);
	}

	public static PreBuiltCraftDefinition ValidateLoadCraftNode(string url, string className, bool brokenParts, bool bypassTechCheck)
	{
		if (File.Exists(url))
		{
			ConfigNode configNode = ConfigNode.Load(url);
			if (configNode == null)
			{
				Debug.LogWarning("Contract Log: " + className + " prebuilt craft at " + url + " is invalid.");
				return null;
			}
			PreBuiltCraftDefinition preBuiltCraftDefinition = new PreBuiltCraftDefinition(url, configNode);
			preBuiltCraftDefinition.craftInfo.LoadDetailsFromCraftFile(configNode, url, brokenParts, bypassTechCheck);
			if (preBuiltCraftDefinition.craftInfo.CraftHasErrors())
			{
				Debug.LogWarning("Contract Log: " + className + " prebuilt craft at " + url + " invalid.");
				Debug.LogWarning(preBuiltCraftDefinition.craftInfo.GetErrorMessage());
				return null;
			}
			if (brokenParts && preBuiltCraftDefinition.craftInfo.BrokenShipParts.Count < 1)
			{
				Debug.LogWarning("Contract Log: " + className + " prebuilt craft at " + url + " is supposed to contain broken parts, but it does not. So craft is invalid.");
				return null;
			}
			return preBuiltCraftDefinition;
		}
		Debug.LogWarning("Contract Log: " + className + " unable to find a prebuilt craft at " + url + " Craft will not be added to contract possibilities.");
		return null;
	}

	public static void LoadNode<T>(ConfigNode node, string className, string valueName, ref T value, T defaultValue, bool logging = true)
	{
		LoadResult loadResult = LoadResult.NULL;
		string error = "";
		string name = "";
		if (node == null)
		{
			loadResult = LoadResult.NULL;
		}
		else
		{
			name = StringUtilities.ShortName(valueName);
			if (node.HasValue(name))
			{
				if (!(typeof(T) == typeof(CelestialBody)))
				{
					loadResult = (TryConvert<T>(node.GetValue(name), out value, ref error) ? LoadResult.SUCCESS : LoadResult.INVALID);
				}
				else
				{
					int value2 = 0;
					if (!TryConvert<int>(node.GetValue(name), out value2, ref error))
					{
						loadResult = LoadResult.INVALID;
					}
					else
					{
						CelestialBody celestialBody = FlightGlobals.Bodies[value2];
						if (celestialBody == null)
						{
							error = Localizer.Format("#autoLOC_290713");
							loadResult = LoadResult.INVALID;
						}
						else
						{
							value = (T)(object)celestialBody;
							loadResult = LoadResult.SUCCESS;
						}
					}
				}
			}
			else
			{
				loadResult = LoadResult.NOVALUE;
			}
		}
		if (loadResult == LoadResult.SUCCESS)
		{
			return;
		}
		if (logging)
		{
			switch (loadResult)
			{
			case LoadResult.NULL:
				Debug.LogWarning(string.Concat("Contract Log: ", className, " cannot load ", valueName, " from a null node. Initializing with default of ", defaultValue, "!"));
				break;
			case LoadResult.NOVALUE:
				Debug.LogWarning(string.Concat("Contract Log: ", className, " cannot load ", valueName, ", it is not in the node. Initializing with default of ", defaultValue, "!"));
				break;
			case LoadResult.INVALID:
				Debug.LogWarning(string.Concat("Contract Log: ", className, " parsed an invalid value \"", node.GetValue(name), "\" from ", valueName, ". (", error, "). Initializing with default of ", defaultValue, "!"));
				break;
			}
		}
		value = defaultValue;
	}

	public static void LoadNodeList<T>(ConfigNode node, string className, string valueName, ref List<T> list)
	{
		if (node != null)
		{
			string value = "";
			LoadNode(node, className, valueName, ref value, "", logging: false);
			list = StringUtilities.UnpackDelimitedString<T>(value);
		}
	}

	public static void SaveNodeList<T>(ConfigNode node, string listName, List<T> list)
	{
		if (node != null && list != null && list.Count > 0)
		{
			node.AddValue(listName, StringUtilities.PackDelimitedString(list));
		}
	}

	public static void CondenseNodeList(ConfigNode node, string valueName, string listName)
	{
		if (node != null)
		{
			List<string> list = new List<string>(node.GetValues(valueName));
			SaveNodeList(node, listName, list);
			node.RemoveValues(valueName);
		}
	}

	public static void LoadNodePair<T1, T2>(ConfigNode node, string className, string valueName, ref KeyValuePair<T1, T2> pair)
	{
		if (node != null)
		{
			string value = "";
			LoadNode(node, className, valueName, ref value, "", logging: false);
			pair = StringUtilities.UnpackDelimitedPair<T1, T2>(value);
		}
	}

	public static bool FlightIsReady(bool checkVessel = false, CelestialBody targetBody = null)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return false;
		}
		if (!FlightGlobals.ready)
		{
			return false;
		}
		if (!checkVessel)
		{
			return true;
		}
		if (FlightGlobals.ActiveVessel == null)
		{
			return false;
		}
		if (targetBody == null)
		{
			return true;
		}
		return FlightGlobals.ActiveVessel.mainBody == targetBody;
	}

	public static bool FlightIsReady(Contract.State currentState, Contract.State targetState, bool checkVessel = false, CelestialBody targetBody = null)
	{
		if (currentState != targetState)
		{
			return false;
		}
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return false;
		}
		if (!FlightGlobals.ready)
		{
			return false;
		}
		if (!checkVessel)
		{
			return true;
		}
		if (FlightGlobals.ActiveVessel == null)
		{
			return false;
		}
		if (targetBody == null)
		{
			return true;
		}
		return FlightGlobals.ActiveVessel.mainBody == targetBody;
	}

	public static bool CoinFlip(System.Random generator = null)
	{
		KSPRandom kSPRandom = null;
		return (((generator != null) ? (generator as KSPRandom) : new KSPRandom())?.Next(2) ?? generator.Next(2)) == 1;
	}

	public static uint HashNumber(uint x)
	{
		x = ((x >> 16) ^ x) * 73244475;
		x = ((x >> 16) ^ x) * 73244475;
		x = (x >> 16) ^ x;
		return x;
	}

	public static int SuperSeed(Contract c)
	{
		return KSPUtil.GenerateSuperSeed(c.ContractGuid, c.MissionSeed);
	}

	public static void ProcessSideRequests(Contract contract, ConfigNode contractNode, CelestialBody targetBody, string vesselName, ref float fundsMultiplier, ref float scienceMultiplier, ref float reputationMultiplier, Vessel vessel = null)
	{
		KSPRandom kSPRandom = new KSPRandom(contract.MissionSeed);
		int num;
		string name;
		switch (contract.Prestige)
		{
		case Contract.ContractPrestige.Exceptional:
			num = kSPRandom.Next(2, 4);
			name = "Exceptional";
			break;
		default:
			num = kSPRandom.Next(0, 2);
			name = "Trivial";
			break;
		case Contract.ContractPrestige.Significant:
			num = kSPRandom.Next(1, 3);
			name = "Significant";
			break;
		}
		ConfigNode[] nodes = contractNode.GetNodes();
		List<KeyValuePair<ConfigNode, int>> list = new List<KeyValuePair<ConfigNode, int>>();
		int num2 = nodes.Length;
		while (num2-- > 0)
		{
			if (nodes[num2].name != "PART_REQUEST" && nodes[num2].name != "RESOURCE_REQUEST" && nodes[num2].name != "CREW_REQUEST")
			{
				continue;
			}
			List<string> list2 = new List<string>(nodes[num2].GetValues("Part"));
			List<string> list3 = new List<string>(nodes[num2].GetValues("Module"));
			if (!ProgressUtilities.HaveAnyTech(list2, list3, logging: false))
			{
				continue;
			}
			if (nodes[num2].name == "CREW_REQUEST")
			{
				string value = "Pilot";
				LoadNode(nodes[num2], "ProcessSideRequests", "Trait", ref value, "Pilot", logging: false);
				int num3 = VesselUtilities.CrewTraitMissionAvailability(value, targetBody, vessel);
				int num4 = ((!(vessel == null)) ? VesselUtilities.VesselCrewWithTraitCount(value, vessel) : 0);
				int num5 = (int)(contract.Prestige + 1);
				if (num5 > num3)
				{
					continue;
				}
				if (vessel != null && list2.Count + list3.Count > 0)
				{
					int value2 = 1;
					LoadNode(nodes[num2], "ProcessSideRequests", "Crew", ref value2, 1, logging: false);
					int num6 = VesselUtilities.ActualCrewCapacity(vessel) / 2 / GameDatabase.Instance.ExperienceConfigs.Categories.Count;
					num6 += VesselUtilities.VesselPartAndModuleCount(list2, list3, vessel) * value2;
					if (num4 + num5 > num6)
					{
						continue;
					}
				}
			}
			int value3 = 1;
			LoadNode(nodes[num2].GetNode(name), "ProcessSideRequests", "Weight", ref value3, 1, logging: false);
			list.Add(new KeyValuePair<ConfigNode, int>(nodes[num2], value3));
		}
		ShuffleList(ref list, kSPRandom);
		int num7 = num;
		while (num7-- > 0 && list.Count > 0)
		{
			ConfigNode configNode = null;
			int num8 = 0;
			int count = list.Count;
			while (count-- > 0)
			{
				num8 += list[count].Value;
			}
			if (num8 <= 0)
			{
				continue;
			}
			int num9 = kSPRandom.Next(num8);
			int count2 = list.Count;
			while (count2-- > 0)
			{
				num9 -= list[count2].Value;
				if (num9 <= 0)
				{
					configNode = list[count2].Key;
					list.Remove(list[count2]);
					break;
				}
			}
			if (configNode == null)
			{
				continue;
			}
			float value4 = 1f;
			float value5 = 1f;
			float value6 = 1f;
			float value7 = 0f;
			float value8 = 0f;
			float value9 = 0f;
			string value10 = "";
			ConfigNode node = configNode.GetNode(name);
			LoadNode(node, "ProcessSideRequests", "FundsMultiplier", ref value4, 1f, logging: false);
			LoadNode(node, "ProcessSideRequests", "ScienceMultiplier", ref value5, 1f, logging: false);
			LoadNode(node, "ProcessSideRequests", "ReputationMultiplier", ref value6, 1f, logging: false);
			LoadNode(configNode, "ProcessSideRequests", "Keyword", ref value10, "", logging: false);
			LoadNode(configNode, "ProcessSideRequests", "MinimumFunds", ref value7, 0f, logging: false);
			LoadNode(configNode, "ProcessSideRequests", "MinimumScience", ref value8, 0f, logging: false);
			LoadNode(configNode, "ProcessSideRequests", "MinimumReputation", ref value9, 0f, logging: false);
			fundsMultiplier *= value4;
			scienceMultiplier *= value5;
			reputationMultiplier *= value6;
			contract.FundsCompletion = Math.Max(contract.FundsCompletion, value7);
			contract.ScienceCompletion = Math.Max(contract.ScienceCompletion, value8);
			contract.ReputationCompletion = Math.Max(contract.ReputationCompletion, value9);
			if (value10 != string.Empty && !contract.Keywords.Contains(value10))
			{
				contract.Keywords.Add(value10);
			}
			switch (configNode.name)
			{
			case "CREW_REQUEST":
			{
				string value14 = "Pilot";
				LoadNode(configNode, "ProcessSideRequests", "Trait", ref value14, "Pilot", logging: false);
				int num10 = ((!(vessel == null)) ? VesselUtilities.VesselCrewWithTraitCount(value14, vessel) : 0);
				num10 = (int)(num10 + (contract.Prestige + 1));
				contract.AddParameter(new CrewTraitParameter(value14, num10, vesselName));
				break;
			}
			case "RESOURCE_REQUEST":
			{
				string value11 = "Matter";
				string value12 = "matter";
				float value13 = 1000f;
				LoadNode(configNode, "ProcessSideRequests", "Name", ref value11, "Matter", logging: false);
				LoadNode(configNode, "ProcessSideRequests", "Title", ref value12, "matter", logging: false);
				LoadNode(node, "ProcessSideRequests", "Amount", ref value13, 1000f, logging: false);
				if (vessel != null)
				{
					value13 += (float)VesselUtilities.VesselResourceAmount(value11, vessel);
				}
				contract.AddParameter(new ResourcePossessionParameter(value11, value12, vesselName, value13));
				break;
			}
			case "PART_REQUEST":
				contract.AddParameter(new PartRequestParameter(configNode, vessel));
				break;
			}
		}
	}

	public static void ExpungeKerbal(ProtoCrewMember pcm)
	{
		if (HighLogic.CurrentGame.CrewRoster[pcm.name] == null)
		{
			return;
		}
		int crewIndex = HighLogic.CurrentGame.CrewRoster.IndexOf(pcm);
		string crewName = pcm.name;
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			if (vessel == null)
			{
				continue;
			}
			if (vessel.loaded && !vessel.packed)
			{
				int count2 = vessel.Parts.Count;
				while (count2-- > 0)
				{
					Part part = vessel.Parts[count2];
					if (part.protoModuleCrew.Contains(pcm))
					{
						part.RemoveCrewmember(pcm);
						part.protoPartSnapshot.protoModuleCrew.RemoveAll((ProtoCrewMember crew) => crew == pcm);
						part.protoPartSnapshot.protoCrewNames.RemoveAll((string name) => name == crewName);
						part.protoPartSnapshot.protoCrewIndicesBackup.RemoveAll((int index) => index == crewIndex);
						vessel.protoVessel.RemoveCrew(pcm);
						vessel.RemoveCrew(pcm);
						if (vessel.isEVA && part.protoPartSnapshot.protoModuleCrew.Count <= 0)
						{
							ExpungeVessel(vessel);
						}
					}
				}
				continue;
			}
			int count3 = vessel.protoVessel.protoPartSnapshots.Count;
			while (count3-- > 0)
			{
				ProtoPartSnapshot protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[count3];
				if (protoPartSnapshot.protoModuleCrew.Contains(pcm))
				{
					protoPartSnapshot.protoModuleCrew.RemoveAll((ProtoCrewMember crew) => crew == pcm);
					protoPartSnapshot.protoCrewNames.RemoveAll((string name) => name == crewName);
					protoPartSnapshot.protoCrewIndicesBackup.RemoveAll((int index) => index == crewIndex);
					vessel.protoVessel.RemoveCrew(pcm);
					if (vessel.isEVA && protoPartSnapshot.protoModuleCrew.Count <= 0)
					{
						ExpungeVessel(vessel);
					}
				}
			}
		}
		HighLogic.CurrentGame.CrewRoster.Remove(pcm);
		pcm.seat = null;
		pcm.seatIdx = -1;
		pcm = null;
	}

	public static void ExpungeVessel(Vessel v)
	{
		if (!(v == null))
		{
			v.Die();
			if (v.loaded)
			{
				v.Unload();
			}
			HighLogic.CurrentGame.flightState.protoVessels.Remove(v.protoVessel);
			FlightGlobals.RemoveVessel(v);
			v.gameObject.DestroyGameObject();
		}
	}

	public static bool WithinDeviation(double v1, double v2, double deviation)
	{
		double deviationAsFraction = GetDeviationAsFraction(deviation);
		return MeasureDeviation(v1, v2) <= deviationAsFraction;
	}

	public static bool WithinDeviationByValue(double v1, double v2, double deviation, double fullValue)
	{
		double deviationAsFraction = GetDeviationAsFraction(deviation);
		return MeasureDeviationByValue(v1, v2, fullValue) <= deviationAsFraction;
	}

	public static double MeasureDeviation(double v1, double v2)
	{
		return Math.Abs(v1 - v2) / ((v1 + v2) / 2.0);
	}

	public static double MeasureDeviationByValue(double v1, double v2, double fullValue)
	{
		return Math.Abs(v1 - v2) / fullValue;
	}

	public static double GetDeviationAsFraction(double deviation)
	{
		deviation = ((deviation > 100.0) ? 100.0 : deviation);
		deviation = ((deviation < 0.0) ? 0.0 : deviation);
		deviation = ((deviation > 1.0) ? (deviation / 100.0) : deviation);
		return deviation;
	}

	public static double WithinDeviationAccuracy(double v1, double v2, double deviation)
	{
		deviation = ((deviation > 100.0) ? 100.0 : deviation);
		deviation = ((deviation < 0.0) ? 0.0 : deviation);
		deviation = ((deviation > 1.0) ? (deviation / 100.0) : deviation);
		return UtilMath.InverseLerp(deviation, 0.0, Math.Abs(v1 - v2) / ((v1 + v2) / 2.0));
	}

	public static double WithinDeviationByReferenceAccuracy(double v1, double v2, double reference, double deviation)
	{
		deviation = ((deviation > 100.0) ? 100.0 : deviation);
		deviation = ((deviation < 0.0) ? 0.0 : deviation);
		deviation = ((deviation > 1.0) ? (deviation / 100.0) : deviation);
		return UtilMath.InverseLerp(deviation, 0.0, Math.Abs(v1 - v2) / reference);
	}

	public static void ShuffleList<T>(ref List<T> list, System.Random generator = null)
	{
		KSPRandom kSPRandom = null;
		kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		for (int num = list.Count - 1; num > 0; num--)
		{
			int index = kSPRandom?.Next(0, num + 1) ?? generator.Next(0, num + 1);
			T value = list[num];
			list[num] = list[index];
			list[index] = value;
		}
	}

	public static T RandomSplitChoice<T>(List<List<T>> listOfLists, System.Random generator = null)
	{
		KSPRandom kSPRandom = null;
		kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		if (listOfLists == null)
		{
			return default(T);
		}
		int num = 0;
		int count = listOfLists.Count;
		while (count-- > 0)
		{
			List<T> list = listOfLists[count];
			if (list == null)
			{
				listOfLists.RemoveAt(count);
			}
			else
			{
				num += list.Count;
			}
		}
		if (listOfLists.Count <= 0)
		{
			return default(T);
		}
		int num2 = kSPRandom?.Next(num) ?? generator.Next(num);
		int count2 = listOfLists.Count;
		while (count2-- > 0)
		{
			int count3 = listOfLists[count2].Count;
			if (num2 >= count3)
			{
				num2 -= count3;
				continue;
			}
			num2 = count2;
			break;
		}
		List<T> list2 = listOfLists[num2];
		return list2[kSPRandom?.Next(list2.Count) ?? generator.Next(list2.Count)];
	}

	public static bool CheckTouristRecoveryContractKerbals(string kerbalName)
	{
		TourismContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<TourismContract>();
		RecoverAsset[] currentContracts2 = ContractSystem.Instance.GetCurrentContracts<RecoverAsset>();
		for (int i = 0; i < currentContracts.Length; i++)
		{
			for (int j = 0; j < currentContracts[i].Tourists.Count; j++)
			{
				if (currentContracts[i].Tourists[j].Contains(kerbalName))
				{
					return true;
				}
			}
		}
		int num = 0;
		while (true)
		{
			if (num < currentContracts2.Length)
			{
				if (currentContracts2[num].RecoveryKerbal != null && currentContracts2[num].RecoveryKerbal.name.Contains(kerbalName))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
