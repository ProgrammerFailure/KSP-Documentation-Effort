using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Contracts;
using FinePrint.Contracts.Parameters;
using UnityEngine;

namespace FinePrint.Utilities;

public class VesselUtilities
{
	public static int highestPodCapacity = int.MinValue;

	public static int HighestPodCapacity
	{
		get
		{
			if (highestPodCapacity != int.MinValue)
			{
				return highestPodCapacity;
			}
			int count = PartLoader.LoadedPartsList.Count;
			while (count-- > 0)
			{
				Part partPrefab = PartLoader.LoadedPartsList[count].partPrefab;
				if (partPrefab != null && partPrefab.isControlSource > Vessel.ControlLevel.NONE)
				{
					highestPodCapacity = ((partPrefab.CrewCapacity > highestPodCapacity) ? partPrefab.CrewCapacity : highestPodCapacity);
				}
			}
			return highestPodCapacity;
		}
	}

	public static bool ActiveVesselFallback(ref Vessel v, bool logging = true)
	{
		if (v == null)
		{
			if (!HighLogic.LoadedSceneIsFlight || !FlightGlobals.ready || FlightGlobals.ActiveVessel == null)
			{
				if (logging)
				{
					Debug.LogWarning("Contract Log: Vessel check cannot fall back to active vessel.");
				}
				return false;
			}
			v = FlightGlobals.ActiveVessel;
		}
		return true;
	}

	public static bool VesselHasPartName(string partName, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		partName = partName.Replace('_', '.');
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				if (GetPartName(v.Parts[count]) == partName)
				{
					return true;
				}
			}
		}
		else if (v.protoVessel != null)
		{
			int count2 = v.protoVessel.protoPartSnapshots.Count;
			while (count2-- > 0)
			{
				if (v.protoVessel.protoPartSnapshots[count2].partName == partName)
				{
					return true;
				}
			}
		}
		else if (v.Parts.Count > 0)
		{
			int count3 = v.Parts.Count;
			while (count3-- > 0)
			{
				if (GetPartName(v.Parts[count3]) == partName)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool VesselHasModuleName(string moduleName, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				int count2 = v.Parts[count].Modules.Count;
				while (count2-- > 0)
				{
					if (v.Parts[count].Modules[count2].moduleName == moduleName)
					{
						return true;
					}
				}
			}
		}
		else
		{
			int count3 = v.protoVessel.protoPartSnapshots.Count;
			while (count3-- > 0)
			{
				int count4 = v.protoVessel.protoPartSnapshots[count3].modules.Count;
				while (count4-- > 0)
				{
					if (v.protoVessel.protoPartSnapshots[count3].modules[count4].moduleName == moduleName)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static double VesselResourceAmount(string resourceName, Vessel v = null)
	{
		double num = 0.0;
		if (!ActiveVesselFallback(ref v))
		{
			return num;
		}
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				PartResource partResource = v.Parts[count].Resources[resourceName];
				if (partResource != null)
				{
					num += partResource.amount;
				}
			}
		}
		else
		{
			int count2 = v.protoVessel.protoPartSnapshots.Count;
			while (count2-- > 0)
			{
				for (int num2 = v.protoVessel.protoPartSnapshots[count2].resources.Count - 1; num2 >= 0; num2--)
				{
					if (!(v.protoVessel.protoPartSnapshots[count2].resources[num2].resourceName != resourceName))
					{
						num += v.protoVessel.protoPartSnapshots[count2].resources[num2].amount;
					}
				}
			}
		}
		return num;
	}

	public static bool VesselHasAnyParts(List<string> partList, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		if (partList != null && partList.Count > 0)
		{
			if (v.loaded)
			{
				int count = v.Parts.Count;
				while (count-- > 0)
				{
					int count2 = partList.Count;
					while (count2-- > 0)
					{
						string text = partList[count2].Replace('_', '.');
						if (GetPartName(v.Parts[count]) == text)
						{
							return true;
						}
					}
				}
			}
			else
			{
				int count3 = v.protoVessel.protoPartSnapshots.Count;
				while (count3-- > 0)
				{
					int count4 = partList.Count;
					while (count4-- > 0)
					{
						string text2 = partList[count4].Replace('_', '.');
						if (v.protoVessel.protoPartSnapshots[count3].partName == text2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	public static bool VesselHasAnyModules(List<string> moduleList, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		if (moduleList != null && moduleList.Count > 0)
		{
			if (v.loaded)
			{
				int count = v.Parts.Count;
				while (count-- > 0)
				{
					int count2 = v.Parts[count].Modules.Count;
					while (count2-- > 0)
					{
						int count3 = moduleList.Count;
						while (count3-- > 0)
						{
							if (v.Parts[count].Modules[count2].moduleName == moduleList[count3])
							{
								return true;
							}
						}
					}
				}
			}
			else
			{
				int count4 = v.protoVessel.protoPartSnapshots.Count;
				while (count4-- > 0)
				{
					int count5 = v.protoVessel.protoPartSnapshots[count4].modules.Count;
					while (count5-- > 0)
					{
						int count6 = moduleList.Count;
						while (count6-- > 0)
						{
							if (v.protoVessel.protoPartSnapshots[count4].modules[count5].moduleName == moduleList[count6])
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	public static bool VesselHasAnyPartsOrModules(List<string> partList, List<string> moduleList, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		bool flag = partList == null || partList.Count <= 0;
		bool flag2 = moduleList == null || moduleList.Count <= 0;
		if (flag && flag2)
		{
			return false;
		}
		if (flag2)
		{
			return VesselHasAnyParts(partList, v);
		}
		if (flag)
		{
			return VesselHasAnyModules(moduleList, v);
		}
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				int count2 = partList.Count;
				while (count2-- > 0)
				{
					string text = partList[count2].Replace('_', '.');
					if (GetPartName(v.Parts[count]) == text)
					{
						return true;
					}
				}
				int count3 = v.Parts[count].Modules.Count;
				while (count3-- > 0)
				{
					int count4 = moduleList.Count;
					while (count4-- > 0)
					{
						if (v.Parts[count].Modules[count3].moduleName == moduleList[count4])
						{
							return true;
						}
					}
				}
			}
		}
		else
		{
			int count5 = v.protoVessel.protoPartSnapshots.Count;
			while (count5-- > 0)
			{
				int count6 = partList.Count;
				while (count6-- > 0)
				{
					string text2 = partList[count6].Replace('_', '.');
					if (v.protoVessel.protoPartSnapshots[count5].partName == text2)
					{
						return true;
					}
				}
				int count7 = v.protoVessel.protoPartSnapshots[count5].modules.Count;
				while (count7-- > 0)
				{
					int count8 = moduleList.Count;
					while (count8-- > 0)
					{
						if (v.protoVessel.protoPartSnapshots[count5].modules[count7].moduleName == moduleList[count8])
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public static int VesselPartAndModuleCount(List<string> partList, List<string> moduleList, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return 0;
		}
		int num = 0;
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				int count2 = partList.Count;
				while (count2-- > 0)
				{
					string text = partList[count2].Replace('_', '.');
					if (GetPartName(v.Parts[count]) == text)
					{
						num++;
					}
				}
				int count3 = v.Parts[count].Modules.Count;
				while (count3-- > 0)
				{
					int count4 = moduleList.Count;
					while (count4-- > 0)
					{
						if (v.Parts[count].Modules[count3].moduleName == moduleList[count4])
						{
							num++;
							break;
						}
					}
				}
			}
		}
		else
		{
			int count5 = v.protoVessel.protoPartSnapshots.Count;
			while (count5-- > 0)
			{
				int count6 = partList.Count;
				while (count6-- > 0)
				{
					string text2 = partList[count6].Replace('_', '.');
					if (v.protoVessel.protoPartSnapshots[count5].partName == text2)
					{
						num++;
					}
				}
				int count7 = v.protoVessel.protoPartSnapshots[count5].modules.Count;
				while (count7-- > 0)
				{
					int count8 = moduleList.Count;
					while (count8-- > 0)
					{
						if (v.protoVessel.protoPartSnapshots[count5].modules[count7].moduleName == moduleList[count8])
						{
							num++;
							break;
						}
					}
				}
			}
		}
		return num;
	}

	public static List<uint> GetPartIDList(Vessel v = null)
	{
		List<uint> list = new List<uint>();
		if (!ActiveVesselFallback(ref v))
		{
			return list;
		}
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				list.Add(v.Parts[count].flightID);
			}
		}
		else
		{
			int count2 = v.protoVessel.protoPartSnapshots.Count;
			while (count2-- > 0)
			{
				list.Add(v.protoVessel.protoPartSnapshots[count2].flightID);
			}
		}
		return list;
	}

	public static Vessel FindVesselWithPartIDs(List<uint> partIDs)
	{
		if (partIDs != null && partIDs.Count > 0)
		{
			int num = 0;
			Vessel result = null;
			int count = FlightGlobals.Vessels.Count;
			while (count-- > 0)
			{
				Vessel vessel = FlightGlobals.Vessels[count];
				int num2 = 0;
				if (vessel.loaded)
				{
					int count2 = vessel.Parts.Count;
					while (count2-- > 0)
					{
						if (partIDs.Contains(vessel.Parts[count2].flightID))
						{
							num2++;
						}
					}
				}
				else
				{
					int count3 = vessel.protoVessel.protoPartSnapshots.Count;
					while (count3-- > 0)
					{
						if (partIDs.Contains(vessel.protoVessel.protoPartSnapshots[count3].flightID))
						{
							num2++;
						}
					}
				}
				if (num2 > num)
				{
					num = num2;
					result = vessel;
				}
			}
			return result;
		}
		return null;
	}

	public static VesselType ClassifyVesselType(Vessel v = null, bool useVesselType = true)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return VesselType.Unknown;
		}
		int count;
		int num;
		Vessel.Situations situation;
		VesselType vesselType;
		if (v.loaded)
		{
			count = v.Parts.Count;
			num = v.GetCrewCount();
			situation = v.situation;
			vesselType = v.vesselType;
		}
		else
		{
			count = v.protoVessel.protoPartSnapshots.Count;
			num = v.protoVessel.GetVesselCrew().Count;
			situation = v.protoVessel.situation;
			vesselType = v.protoVessel.vesselType;
		}
		if (count > 1 && VesselHasModuleName("ModuleCommand", v))
		{
			int num2 = ActualCrewCapacity(v);
			if (num2 <= HighestPodCapacity && VesselHasWheelsOnGround(v, WheelType.MOTORIZED))
			{
				return VesselType.Rover;
			}
			if (v.HasValidContractObjectives("Generator", "Antenna"))
			{
				if (num2 > HighestPodCapacity && v.HasValidContractObjectives("Dock"))
				{
					switch (situation)
					{
					case Vessel.Situations.ORBITING:
						return VesselType.Station;
					case Vessel.Situations.LANDED:
						return VesselType.Base;
					}
				}
				if (num + num2 <= 0)
				{
					return VesselType.Probe;
				}
			}
		}
		if (!useVesselType)
		{
			return VesselType.Unknown;
		}
		return vesselType;
	}

	public static Dictionary<Vessel, VesselType> ClassifyAllVesselsAt(CelestialBody body, bool useVesselType = true)
	{
		Dictionary<Vessel, VesselType> dictionary = new Dictionary<Vessel, VesselType>();
		if (body == null)
		{
			return dictionary;
		}
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			if (vessel.loaded ? (vessel.orbit.referenceBody == body) : (FlightGlobals.Bodies[vessel.protoVessel.orbitSnapShot.ReferenceBodyIndex] == body))
			{
				dictionary.Add(vessel, ClassifyVesselType(vessel, useVesselType));
			}
		}
		return dictionary;
	}

	public static List<Vessel> SpecificVesselClassAt(CelestialBody body, VesselType vesselType, bool requireOwned = false, bool excludeActive = false, bool useVesselType = true)
	{
		List<Vessel> list = new List<Vessel>();
		if (body == null)
		{
			return list;
		}
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			if ((!excludeActive || !HighLogic.LoadedSceneIsFlight || !FlightGlobals.ready || !(vessel == FlightGlobals.ActiveVessel)) && !((vessel.loaded ? vessel.orbit.referenceBody : FlightGlobals.Bodies[vessel.protoVessel.orbitSnapShot.ReferenceBodyIndex]) != body) && (!requireOwned || VesselIsOwned(vessel)) && ClassifyVesselType(vessel, useVesselType) == vesselType)
			{
				list.Add(vessel);
			}
		}
		return list;
	}

	public static List<Vessel> SpecificVesselClassAt(Vessel.Situations situation, CelestialBody body, VesselType vesselType, bool requireOwned = false, bool excludeActive = false, bool useVesselType = true)
	{
		List<Vessel> list = new List<Vessel>();
		if (body == null)
		{
			return list;
		}
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			if ((!excludeActive || !HighLogic.LoadedSceneIsFlight || !FlightGlobals.ready || !(vessel == FlightGlobals.ActiveVessel)) && (vessel.loaded ? vessel.situation : vessel.protoVessel.situation) == situation && !((vessel.loaded ? vessel.orbit.referenceBody : FlightGlobals.Bodies[vessel.protoVessel.orbitSnapShot.ReferenceBodyIndex]) != body) && (!requireOwned || VesselIsOwned(vessel)) && ClassifyVesselType(vessel, useVesselType) == vesselType)
			{
				list.Add(vessel);
			}
		}
		return list;
	}

	public static bool VesselHasWheelsOnGround(Vessel v = null, params WheelType[] validWheelTypes)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		bool num;
		if (!v.loaded)
		{
			if (v.protoVessel.situation != Vessel.Situations.LANDED)
			{
				num = v.protoVessel.situation != Vessel.Situations.PRELAUNCH;
				goto IL_0050;
			}
		}
		else if (v.situation != Vessel.Situations.LANDED)
		{
			num = v.situation != Vessel.Situations.PRELAUNCH;
			goto IL_0050;
		}
		goto IL_0054;
		IL_0054:
		if (v.loaded)
		{
			int count = v.Parts.Count;
			while (count-- > 0)
			{
				int count2 = v.Parts[count].Modules.Count;
				while (count2-- > 0)
				{
					ModuleWheelBase moduleWheelBase = v.Parts[count].Modules[count2] as ModuleWheelBase;
					if (moduleWheelBase == null || !moduleWheelBase.isGrounded)
					{
						continue;
					}
					int num2 = validWheelTypes.Length;
					while (num2-- > 0)
					{
						if (moduleWheelBase.wheelType == validWheelTypes[num2])
						{
							return true;
						}
					}
				}
			}
		}
		else
		{
			int count3 = v.protoVessel.protoPartSnapshots.Count;
			while (count3-- > 0)
			{
				int count4 = v.protoVessel.protoPartSnapshots[count3].modules.Count;
				while (count4-- > 0)
				{
					ProtoPartModuleSnapshot protoPartModuleSnapshot = v.protoVessel.protoPartSnapshots[count3].modules[count4];
					if (protoPartModuleSnapshot.moduleName != "ModuleWheelBase")
					{
						continue;
					}
					bool result = false;
					if (protoPartModuleSnapshot.moduleValues.HasValue("isGrounded"))
					{
						bool.TryParse(protoPartModuleSnapshot.moduleValues.GetValue("isGrounded"), out result);
					}
					if (!result || !protoPartModuleSnapshot.moduleValues.HasValue("wheelType"))
					{
						continue;
					}
					string value = protoPartModuleSnapshot.moduleValues.GetValue("wheelType");
					int num3 = validWheelTypes.Length;
					while (num3-- > 0)
					{
						if (value == validWheelTypes[num3].ToString())
						{
							return true;
						}
					}
				}
			}
		}
		return false;
		IL_0050:
		if (num)
		{
			return false;
		}
		goto IL_0054;
	}

	public static FlightBand GetFlightBand(double threshold, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return FlightBand.NONE;
		}
		if (v.loaded)
		{
			if (v.LandedOrSplashed)
			{
				return FlightBand.GROUND;
			}
			if (v.altitude > threshold)
			{
				return FlightBand.HIGH;
			}
			if (v.altitude <= threshold)
			{
				return FlightBand.const_2;
			}
		}
		else
		{
			if (v.protoVessel.situation == Vessel.Situations.LANDED || v.protoVessel.situation == Vessel.Situations.SPLASHED)
			{
				return FlightBand.GROUND;
			}
			Orbit orbit = v.protoVessel.orbitSnapShot.Load();
			orbit.UpdateFromUT(Planetarium.GetUniversalTime());
			if (orbit.altitude > threshold)
			{
				return FlightBand.HIGH;
			}
			if (orbit.altitude <= threshold)
			{
				return FlightBand.const_2;
			}
		}
		return FlightBand.NONE;
	}

	public static int ActualCrewCapacity(Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return 0;
		}
		if (v.loaded)
		{
			return v.GetCrewCapacity();
		}
		int num = 0;
		int count = v.protoVessel.protoPartSnapshots.Count;
		while (count-- > 0)
		{
			if (v.protoVessel.protoPartSnapshots[count].partInfo != null)
			{
				Part partPrefab = v.protoVessel.protoPartSnapshots[count].partInfo.partPrefab;
				if (partPrefab != null)
				{
					num += partPrefab.CrewCapacity;
				}
			}
		}
		return num;
	}

	public static bool VesselLaunchedAfterID(uint launchID, Vessel v, params string[] ignore)
	{
		if (v == null)
		{
			return false;
		}
		int num = ignore.Length;
		while (num-- > 0)
		{
			ignore[num] = ignore[num].Replace('_', '.');
		}
		if (v.loaded)
		{
			List<Part> parts = v.Parts;
			if (parts.Count <= 0)
			{
				return false;
			}
			int count = parts.Count;
			while (count-- > 0)
			{
				Part part = parts[count];
				bool flag = false;
				int num2 = ignore.Length;
				while (num2-- > 0)
				{
					if (!(GetPartName(part) != ignore[num2]))
					{
						flag = true;
						break;
					}
				}
				if (!flag && part.launchID < launchID)
				{
					return false;
				}
			}
		}
		else
		{
			List<ProtoPartSnapshot> protoPartSnapshots = v.protoVessel.protoPartSnapshots;
			if (protoPartSnapshots.Count <= 0)
			{
				return false;
			}
			int count2 = protoPartSnapshots.Count;
			while (count2-- > 0)
			{
				ProtoPartSnapshot protoPartSnapshot = protoPartSnapshots[count2];
				bool flag2 = false;
				int num3 = ignore.Length;
				while (num3-- > 0)
				{
					if (!(protoPartSnapshot.partName != ignore[num3]))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && protoPartSnapshot.launchID < launchID)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool VesselIsOwned(Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		if (v.loaded)
		{
			return v.DiscoveryInfo.Level == DiscoveryLevels.Owned;
		}
		ProtoVessel protoVessel = v.protoVessel;
		DiscoveryLevels discoveryLevels = DiscoveryLevels.None;
		if (protoVessel.discoveryInfo.HasValue("state"))
		{
			discoveryLevels = (DiscoveryLevels)int.Parse(protoVessel.discoveryInfo.GetValue("state"));
		}
		return discoveryLevels == DiscoveryLevels.Owned;
	}

	public static void DiscoverVessel(Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return;
		}
		if (v.loaded)
		{
			if (v.DiscoveryInfo.Level != DiscoveryLevels.Owned)
			{
				v.DiscoveryInfo.SetLevel(DiscoveryLevels.Owned);
			}
			return;
		}
		ProtoVessel protoVessel = v.protoVessel;
		DiscoveryLevels discoveryLevels = DiscoveryLevels.None;
		if (protoVessel.discoveryInfo.HasValue("state"))
		{
			discoveryLevels = (DiscoveryLevels)int.Parse(protoVessel.discoveryInfo.GetValue("state"));
		}
		if (discoveryLevels != DiscoveryLevels.Owned)
		{
			protoVessel.discoveryInfo.SetValue("state", (-1).ToString(CultureInfo.InvariantCulture), createIfNotFound: true);
		}
	}

	public static uint VesselID(Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return 0u;
		}
		if (v.loaded)
		{
			return v.rootPart.flightID;
		}
		if (v.protoVessel.protoPartSnapshots.Count < 0)
		{
			return 0u;
		}
		return v.protoVessel.protoPartSnapshots[0].flightID;
	}

	public static List<ProtoCrewMember> VesselCrewWithTrait(string trait, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return new List<ProtoCrewMember>();
		}
		List<ProtoCrewMember> list = new List<ProtoCrewMember>(v.GetVesselCrew());
		int count = list.Count;
		while (count-- > 0)
		{
			if (list[count].experienceTrait.TypeName != trait)
			{
				list.Remove(list[count]);
			}
		}
		return list;
	}

	public static int VesselCrewWithTraitCount(string trait, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return 0;
		}
		int num = 0;
		List<ProtoCrewMember> vesselCrew = v.GetVesselCrew();
		int count = vesselCrew.Count;
		while (count-- > 0)
		{
			if (vesselCrew[count].experienceTrait.TypeName == trait)
			{
				num++;
			}
		}
		return num;
	}

	public static int CrewTraitMissionAvailability(string trait, CelestialBody targetBody = null, Vessel excludeVessel = null)
	{
		int num = 0;
		if (trait == "Tourist")
		{
			if (targetBody == null)
			{
				IEnumerator<ProtoCrewMember> enumerator = HighLogic.CurrentGame.CrewRoster.Kerbals(ProtoCrewMember.KerbalType.Tourist, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
				while (enumerator.MoveNext())
				{
					num++;
				}
				return num;
			}
			TourismContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<TourismContract>();
			int num2 = currentContracts.Length;
			while (num2-- > 0)
			{
				IEnumerator<ContractParameter> enumerator2 = currentContracts[num2].AllParameters.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (!(enumerator2.Current is KerbalTourParameter kerbalTourParameter))
					{
						continue;
					}
					IEnumerator<ContractParameter> enumerator3 = kerbalTourParameter.AllParameters.GetEnumerator();
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current is KerbalDestinationParameter kerbalDestinationParameter && CelestialUtilities.GetHostPlanet(kerbalDestinationParameter.targetBody) == targetBody)
						{
							num++;
							break;
						}
					}
				}
			}
			return num;
		}
		IEnumerator<ProtoCrewMember> enumerator4 = HighLogic.CurrentGame.CrewRoster.Kerbals(ProtoCrewMember.KerbalType.Crew, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
		while (enumerator4.MoveNext())
		{
			if (enumerator4.Current != null && enumerator4.Current.experienceTrait.TypeName == trait)
			{
				num++;
			}
		}
		if (targetBody == null)
		{
			return num;
		}
		CelestialBody hostPlanet = CelestialUtilities.GetHostPlanet(targetBody);
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			Vessel vessel = FlightGlobals.Vessels[count];
			if (vessel == null || vessel == excludeVessel)
			{
				continue;
			}
			List<ProtoCrewMember> vesselCrew = vessel.GetVesselCrew();
			if (vesselCrew.Count <= 0)
			{
				continue;
			}
			CelestialBody hostPlanet2 = CelestialUtilities.GetHostPlanet(vessel.loaded ? vessel.mainBody : FlightGlobals.Bodies[vessel.protoVessel.orbitSnapShot.ReferenceBodyIndex]);
			if (hostPlanet != hostPlanet2)
			{
				continue;
			}
			int count2 = vesselCrew.Count;
			while (count2-- > 0)
			{
				if (vesselCrew[count2].type == ProtoCrewMember.KerbalType.Crew && vesselCrew[count2].rosterStatus == ProtoCrewMember.RosterStatus.Assigned && vesselCrew[count2].experienceTrait.TypeName == trait)
				{
					num++;
				}
			}
		}
		return num;
	}

	public static Orbit GenerateAdjustedVesselOrbit(double minimumDeviation, double modificationLength, int numberOfModifications, System.Random generator = null, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return new Orbit();
		}
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		Orbit orbit = (v.loaded ? v.orbit : v.protoVessel.orbitSnapShot.Load());
		if (!(orbit.eccentricity >= 1.0) && orbit.ApR < orbit.referenceBody.sphereOfInfluence)
		{
			Orbit orbit2 = new Orbit(orbit.inclination, orbit.eccentricity, orbit.semiMajorAxis, orbit.double_0, orbit.argumentOfPeriapsis, orbit.meanAnomalyAtEpoch, orbit.epoch, orbit.referenceBody);
			List<OrbitalProperties> list = new List<OrbitalProperties>
			{
				OrbitalProperties.const_0,
				OrbitalProperties.const_5,
				OrbitalProperties.const_6
			};
			if (Math.Abs(orbit.inclination) % 180.0 >= 1.0)
			{
				list.Add(OrbitalProperties.const_2);
			}
			if (orbit.eccentricity > 0.05)
			{
				list.Add(OrbitalProperties.const_1);
			}
			SystemUtilities.ShuffleList(ref list, (kSPRandom != null) ? kSPRandom : generator);
			while (list.Count > numberOfModifications)
			{
				list.RemoveAt(0);
			}
			int count = list.Count;
			while (count-- > 0)
			{
				double num = (minimumDeviation + modificationLength + (modificationLength * 1.5 - modificationLength) * (kSPRandom?.NextDouble() ?? generator.NextDouble())) / 100.0;
				double num2 = ((!SystemUtilities.CoinFlip((kSPRandom != null) ? kSPRandom : generator)) ? 1 : (-1));
				switch (list[count])
				{
				case OrbitalProperties.const_0:
					orbit2.inclination += num2 * num * 90.0;
					orbit2.inclination = UtilMath.WrapAround(orbit2.inclination, -180.0, 180.0);
					if (orbit2.inclination < 0.0)
					{
						orbit2.inclination = Math.Abs(orbit2.inclination);
						orbit2.double_0 += 180.0;
						orbit2.argumentOfPeriapsis -= 180.0;
						orbit2.double_0 = UtilMath.WrapAround(orbit2.double_0, 0.0, 360.0);
						orbit2.argumentOfPeriapsis = UtilMath.WrapAround(orbit2.argumentOfPeriapsis, 0.0, 360.0);
					}
					break;
				case OrbitalProperties.const_1:
					orbit2.argumentOfPeriapsis += num2 * num * 360.0;
					orbit2.argumentOfPeriapsis = UtilMath.WrapAround(orbit2.argumentOfPeriapsis, 0.0, 360.0);
					break;
				case OrbitalProperties.const_2:
					orbit2.double_0 += num2 * num * 360.0;
					orbit2.argumentOfPeriapsis -= num2 * num * 360.0;
					orbit2.double_0 = UtilMath.WrapAround(orbit2.double_0, 0.0, 360.0);
					orbit2.argumentOfPeriapsis = UtilMath.WrapAround(orbit2.argumentOfPeriapsis, 0.0, 360.0);
					break;
				case OrbitalProperties.const_4:
				case OrbitalProperties.const_5:
				case OrbitalProperties.const_6:
				{
					double num3 = orbit2.semiMajorAxis * (1.0 - orbit2.eccentricity);
					double num4 = orbit2.semiMajorAxis * (1.0 + orbit2.eccentricity);
					List<double> list2 = new List<double>();
					if (list[count] == OrbitalProperties.const_5 || list[count] == OrbitalProperties.const_4)
					{
						double num5 = num3 * num;
						if (num3 + num5 <= orbit2.referenceBody.sphereOfInfluence)
						{
							list2.Add(num3 + num5);
						}
						if (num3 - num5 >= orbit2.referenceBody.minOrbitalDistance * 1.05)
						{
							list2.Add(num3 - num5);
						}
						if (list2.Count > 0)
						{
							num3 = list2[kSPRandom?.Next(0, list2.Count) ?? generator.Next(0, list2.Count)];
						}
					}
					if (list[count] == OrbitalProperties.const_6 || list[count] == OrbitalProperties.const_4)
					{
						double num6 = num4 * num;
						if (num4 + num6 <= orbit2.referenceBody.sphereOfInfluence)
						{
							list2.Add(num4 + num6);
						}
						if (num4 - num6 >= orbit2.referenceBody.minOrbitalDistance * 1.05)
						{
							list2.Add(num4 - num6);
						}
						if (list2.Count > 0)
						{
							num4 = list2[kSPRandom?.Next(0, list2.Count) ?? generator.Next(0, list2.Count)];
						}
					}
					double val = num3;
					double val2 = num4;
					num3 = Math.Min(val, val2);
					num4 = Math.Max(val, val2);
					orbit2.semiMajorAxis = (num4 + num3) / 2.0;
					orbit2.eccentricity = (num4 - num3) / (num4 + num3);
					break;
				}
				}
			}
			return orbit2;
		}
		return orbit;
	}

	public static bool VesselAtOrbit(Orbit o, double deviationWindow, Vessel v = null)
	{
		if (!ActiveVesselFallback(ref v))
		{
			return false;
		}
		if (o == null)
		{
			return false;
		}
		if ((v.loaded ? v.situation : v.protoVessel.situation) != Vessel.Situations.ORBITING)
		{
			return false;
		}
		Orbit orbit = (v.loaded ? v.orbit : v.protoVessel.orbitSnapShot.Load());
		if (orbit.referenceBody != o.referenceBody)
		{
			return false;
		}
		if (!SystemUtilities.WithinDeviation(orbit.PeA, o.PeA, deviationWindow) && Math.Abs(orbit.PeA - o.PeA) > ContractDefs.Satellite.MinimumDeviationWindow)
		{
			return false;
		}
		if (!SystemUtilities.WithinDeviation(orbit.ApA, o.ApA, deviationWindow) && Math.Abs(orbit.ApA - o.ApA) > ContractDefs.Satellite.MinimumDeviationWindow)
		{
			return false;
		}
		if (!SystemUtilities.WithinDeviationByValue(Math.Abs(orbit.inclination), Math.Abs(o.inclination), deviationWindow, 90.0))
		{
			return false;
		}
		float num = 0f;
		if (Math.Abs(o.inclination) % 180.0 < 1.0)
		{
			double num2 = 389.0;
			double num3 = 32.0;
			if (Math.Abs(o.inclination) % 360.0 < 1.0)
			{
				num2 = orbit.double_0 + orbit.argumentOfPeriapsis;
				num3 = o.double_0 + o.argumentOfPeriapsis;
			}
			else
			{
				num2 = orbit.double_0 - orbit.argumentOfPeriapsis;
				num3 = o.double_0 - o.argumentOfPeriapsis;
			}
			if (num2 > 360.0)
			{
				num2 -= 360.0;
			}
			else if (num2 < 0.0)
			{
				num2 += 360.0;
			}
			if (num3 > 360.0)
			{
				num2 -= 360.0;
			}
			else if (num3 < 0.0)
			{
				num2 += 360.0;
			}
			num = (float)Math.Abs(num2 - num3) % 360f;
		}
		else
		{
			double num4 = orbit.double_0;
			double num5 = o.double_0;
			if (orbit.inclination < 0.0)
			{
				num4 = (num4 + 180.0) % 360.0;
			}
			if (o.inclination < 0.0)
			{
				num5 = (num5 + 180.0) % 360.0;
			}
			float num6 = (float)Math.Abs(num4 - num5) % 360f;
			if (num6 > 180f)
			{
				num6 = 360f - num6;
			}
			if ((double)num6 > deviationWindow / 100.0 * 360.0)
			{
				return false;
			}
			num = (float)Math.Abs(orbit.argumentOfPeriapsis - o.argumentOfPeriapsis) % 360f;
		}
		if (num > 180f)
		{
			num = 360f - num;
		}
		if (o.eccentricity > 0.05 && (double)num > deviationWindow / 100.0 * 360.0)
		{
			return false;
		}
		return true;
	}

	public static double VesselAtOrbitAccuracy(Orbit o, double deviationWindow, Vessel v = null)
	{
		if (!VesselAtOrbit(o, deviationWindow, v))
		{
			return 0.0;
		}
		Orbit obj = (v.loaded ? v.orbit : v.protoVessel.orbitSnapShot.Load());
		double num = SystemUtilities.WithinDeviationAccuracy(obj.PeA, o.PeA, deviationWindow);
		double num2 = SystemUtilities.WithinDeviationAccuracy(obj.ApA, o.ApA, deviationWindow);
		double num3 = SystemUtilities.WithinDeviationByReferenceAccuracy(obj.inclination, o.inclination, 180.0, deviationWindow);
		return (num + num2 + num3) / 3.0;
	}

	public static Vector3 EstimatePartSize(Part p)
	{
		if (p == null)
		{
			return Vector3.zero;
		}
		List<MeshRenderer> list = p.FindModelComponents<MeshRenderer>();
		List<SkinnedMeshRenderer> list2 = p.FindModelComponents<SkinnedMeshRenderer>();
		List<Bounds> list3 = new List<Bounds>();
		int count = list.Count;
		while (count-- > 0)
		{
			list3.Add(list[count].bounds);
		}
		int count2 = list2.Count;
		while (count2-- > 0)
		{
			Bounds bounds = list2[count2].bounds;
			list3.Add((bounds.size == Vector3.zero) ? list2[count2].localBounds : bounds);
		}
		return PartGeometryUtil.MergeBounds(list3.ToArray(), p.transform).size;
	}

	public static Part FindFirstPartOrModuleName(List<string> partNames, List<string> moduleNames)
	{
		int count = PartLoader.LoadedPartsList.Count;
		while (count-- > 0)
		{
			Part partPrefab = PartLoader.LoadedPartsList[count].partPrefab;
			if (partNames != null)
			{
				int count2 = partNames.Count;
				while (count2-- > 0)
				{
					if (GetPartName(partPrefab) == partNames[count2].Replace('_', '.'))
					{
						return partPrefab;
					}
				}
			}
			if (moduleNames == null || moduleNames.Count <= 0)
			{
				continue;
			}
			int count3 = partPrefab.Modules.Count;
			while (count3-- > 0)
			{
				PartModule partModule = partPrefab.Modules[count3];
				if (moduleNames.Contains(partModule.moduleName))
				{
					return partPrefab;
				}
			}
		}
		return null;
	}

	public static string GetPartName(Part p)
	{
		if (!(p == null) && p.partInfo != null)
		{
			return p.partInfo.name;
		}
		return "";
	}
}
