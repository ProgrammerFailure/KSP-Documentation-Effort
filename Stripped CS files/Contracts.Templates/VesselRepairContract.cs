using System;
using System.Collections.Generic;
using System.Globalization;
using Expansions.Missions;
using FinePrint;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Contracts.Templates;

public class VesselRepairContract : Contract
{
	public CelestialBody targetBody;

	public PreBuiltCraftDefinition repairCraftDefinition;

	public double craftStartLatitude;

	public double craftStartLongitude;

	public uint repairVesselId;

	public uint repairPartId;

	public double orbitEccentricity;

	public double orbitAltitudeFactor = 0.5;

	public double orbitInclinationFactor = 0.5;

	public string vesselName;

	public ContractOrbitRenderer orbitRenderer;

	public Orbit orbit;

	public bool spawnOnGround;

	public bool spawnInOrbit;

	public string repairPartName;

	public bool waitingForVslSpawn;

	public AvailablePart repairPart;

	public string[] VesselNameStrings = new string[11]
	{
		"#autoLOC_6002581", "#autoLOC_6002582", "#autoLOC_6002583", "#autoLOC_6002584", "#autoLOC_6002585", "#autoLOC_6002586", "#autoLOC_6002587", "#autoLOC_6002588", "#autoLOC_6002589", "#autoLOC_6002590",
		"#autoLOC_6002591"
	};

	public CelestialBody TargetBody => targetBody;

	public double CraftStartLatitude => craftStartLatitude;

	public double CraftStartLongitude => craftStartLongitude;

	public uint RepairVesselId => repairVesselId;

	public uint RepairPartId => repairPartId;

	public double OrbitEccentricity => orbitEccentricity;

	public double OrbitAltitudeFactor => orbitAltitudeFactor;

	public double OrbitInclinationFactor => orbitInclinationFactor;

	public string VesselName => vesselName;

	public string RepairPartName => repairPartName;

	public override bool Generate()
	{
		int num = 0;
		int num2 = 0;
		VesselRepairContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<VesselRepairContract>();
		int num3 = currentContracts.Length;
		while (num3-- > 0)
		{
			switch (currentContracts[num3].ContractState)
			{
			case State.Active:
				num2++;
				break;
			case State.Offered:
				num++;
				break;
			}
		}
		if (num < ContractDefs.VesselRepair.MaximumAvailable && num2 < ContractDefs.VesselRepair.MaximumActive)
		{
			List<PreBuiltCraftDefinition> list = ContractDefs.PreBuiltCraftDefs.CraftForContractType("VesselRepair");
			int count = list.Count;
			while (count-- > 0)
			{
				if (list[count].brokenPartNames.Count <= 0 || list[count].brokenPartNames.Count > 1)
				{
					list.RemoveAt(count);
				}
			}
			if (list.Count <= 0)
			{
				return false;
			}
			List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.ORBIT, bodyReached: true, progressComplete: true, MannedStatus.MANNED);
			if (bodiesProgress.Count <= 0)
			{
				return false;
			}
			CelestialBody homeBody = FlightGlobals.GetHomeBody();
			switch (prestige)
			{
			case ContractPrestige.Trivial:
				if (ProgressUtilities.GetBodyProgress(ProgressType.ORBIT, homeBody, MannedStatus.MANNED))
				{
					targetBody = homeBody;
					break;
				}
				return false;
			case ContractPrestige.Significant:
			{
				int count3 = bodiesProgress.Count;
				while (count3-- > 0)
				{
					if (!bodiesProgress[count3].HasParent(homeBody))
					{
						bodiesProgress.RemoveAt(count3);
					}
				}
				if (bodiesProgress.Count <= 0)
				{
					return false;
				}
				targetBody = WeightedBodyChoice(bodiesProgress);
				break;
			}
			case ContractPrestige.Exceptional:
			{
				int count2 = bodiesProgress.Count;
				while (count2-- > 0)
				{
					if (bodiesProgress[count2].isHomeWorld || bodiesProgress[count2].isStar || bodiesProgress[count2].HasParent(homeBody))
					{
						bodiesProgress.RemoveAt(count2);
					}
				}
				if (bodiesProgress.Count <= 0)
				{
					return false;
				}
				targetBody = WeightedBodyChoice(bodiesProgress);
				break;
			}
			}
			if (targetBody == null)
			{
				return false;
			}
			KSPRandom kSPRandom = new KSPRandom();
			int index = kSPRandom.Next(0, list.Count - 1);
			repairCraftDefinition = list[index];
			int index2 = kSPRandom.Next(0, repairCraftDefinition.brokenPartNames.Count - 1);
			repairPartName = repairCraftDefinition.brokenPartNames[index2];
			repairPart = PartLoader.getPartInfoByName(repairPartName);
			if (repairPart == null)
			{
				return false;
			}
			if (repairCraftDefinition.allowGround && repairCraftDefinition.allowOrbit)
			{
				if (kSPRandom.Next(0, 1) == 1)
				{
					spawnOnGround = true;
				}
				else
				{
					spawnInOrbit = true;
				}
			}
			else if (repairCraftDefinition.allowGround)
			{
				spawnOnGround = true;
			}
			else if (repairCraftDefinition.allowOrbit)
			{
				spawnInOrbit = true;
			}
			if (!spawnInOrbit && !spawnOnGround)
			{
				return false;
			}
			if (spawnOnGround)
			{
				bool flag = false;
				if (repairCraftDefinition.usePreBuiltPositions && ContractDefs.PreBuiltCraftPositions.ContainsKey(targetBody.bodyName))
				{
					List<PreBuiltCraftPosition> list2 = ContractDefs.PreBuiltCraftPositions[targetBody.bodyName];
					int index3 = kSPRandom.Next(0, list2.Count - 1);
					double centerLatitude = list2[index3].CenterLatitude;
					double centerLongitude = list2[index3].CenterLongitude;
					float searchRadius = list2[index3].SearchRadius;
					WaypointManager.ChooseRandomPositionNear(out craftStartLatitude, out craftStartLongitude, centerLatitude, centerLongitude, targetBody.bodyName, searchRadius, repairCraftDefinition.allowWater);
					flag = true;
				}
				if (!flag)
				{
					WaypointManager.ChooseRandomPosition(out craftStartLatitude, out craftStartLongitude, targetBody.bodyName, repairCraftDefinition.allowWater, equatorial: false, kSPRandom);
				}
			}
			else if (spawnInOrbit)
			{
				orbitEccentricity = 0.0;
				orbitAltitudeFactor = 0.5;
				orbitInclinationFactor = 0.5;
				switch (prestige)
				{
				default:
					orbitEccentricity = Math.Max(0.0, Math.Min(ContractDefs.VesselRepair.Trivial.MaxOrbitEccentricityFactor, kSPRandom.NextDouble() / 2.0));
					orbitAltitudeFactor = ContractDefs.VesselRepair.Trivial.OrbitAltitudeFactor;
					orbitInclinationFactor = ContractDefs.VesselRepair.Trivial.OrbitInclinationFactor;
					break;
				case ContractPrestige.Exceptional:
					orbitEccentricity = Math.Max(0.0, Math.Min(ContractDefs.VesselRepair.Exceptional.MaxOrbitEccentricityFactor, kSPRandom.NextDouble() / 2.0));
					orbitAltitudeFactor = ContractDefs.VesselRepair.Exceptional.OrbitAltitudeFactor;
					orbitInclinationFactor = ContractDefs.VesselRepair.Exceptional.OrbitInclinationFactor;
					break;
				case ContractPrestige.Significant:
					orbitEccentricity = Math.Max(0.0, Math.Min(ContractDefs.VesselRepair.Significant.MaxOrbitEccentricityFactor, kSPRandom.NextDouble() / 2.0));
					orbitAltitudeFactor = ContractDefs.VesselRepair.Significant.OrbitAltitudeFactor;
					orbitInclinationFactor = ContractDefs.VesselRepair.Significant.OrbitInclinationFactor;
					break;
				}
				orbit = OrbitUtilities.GenerateOrbit(base.MissionSeed, targetBody, OrbitType.RANDOM, orbitAltitudeFactor, orbitInclinationFactor, orbitEccentricity);
				if (orbit == null)
				{
					return false;
				}
			}
			vesselName = CreateVesselName(targetBody.bodyDisplayName);
			float num4 = 1f;
			AddParameter(new CrewTraitParameter("Engineer", 1, Localizer.Format("#autoLOC_8004217")));
			AddParameter(new RepairPartParameter(0u, repairPartName, 0u, vesselName, targetBody));
			AddKeywords("Commercial");
			SetExpiry(ContractDefs.VesselRepair.Expire.MinimumExpireDays, ContractDefs.VesselRepair.Expire.MaximumExpireDays);
			SetDeadlineDays((float)ContractDefs.VesselRepair.Expire.DeadlineDays * num4, targetBody);
			switch (prestige)
			{
			default:
				SetScience(Mathf.RoundToInt(ContractDefs.VesselRepair.Science.BaseReward * num4 * ContractDefs.VesselRepair.Trivial.ScienceMultiplier), targetBody);
				SetReputation(Mathf.RoundToInt(ContractDefs.VesselRepair.Reputation.BaseReward * num4 * ContractDefs.VesselRepair.Trivial.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Reputation.BaseFailure * num4 * ContractDefs.VesselRepair.Trivial.ReputationMultiplier), targetBody);
				SetFunds(Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseAdvance * num4 * ContractDefs.VesselRepair.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseReward * num4 * ContractDefs.VesselRepair.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseFailure * num4 * ContractDefs.VesselRepair.Trivial.FundsMultiplier), targetBody);
				break;
			case ContractPrestige.Exceptional:
				SetScience(Mathf.RoundToInt(ContractDefs.VesselRepair.Science.BaseReward * num4 * ContractDefs.VesselRepair.Exceptional.ScienceMultiplier), targetBody);
				SetReputation(Mathf.RoundToInt(ContractDefs.VesselRepair.Reputation.BaseReward * num4 * ContractDefs.VesselRepair.Exceptional.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Reputation.BaseFailure * num4 * ContractDefs.VesselRepair.Exceptional.ReputationMultiplier), targetBody);
				SetFunds(Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseAdvance * num4 * ContractDefs.VesselRepair.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseReward * num4 * ContractDefs.VesselRepair.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseFailure * num4 * ContractDefs.VesselRepair.Exceptional.FundsMultiplier), targetBody);
				break;
			case ContractPrestige.Significant:
				SetScience(Mathf.RoundToInt(ContractDefs.VesselRepair.Science.BaseReward * num4 * ContractDefs.VesselRepair.Significant.ScienceMultiplier), targetBody);
				SetReputation(Mathf.RoundToInt(ContractDefs.VesselRepair.Reputation.BaseReward * num4 * ContractDefs.VesselRepair.Significant.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Reputation.BaseFailure * num4 * ContractDefs.VesselRepair.Significant.ReputationMultiplier), targetBody);
				SetFunds(Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseAdvance * num4 * ContractDefs.VesselRepair.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseReward * num4 * ContractDefs.VesselRepair.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.VesselRepair.Funds.BaseFailure * num4 * ContractDefs.VesselRepair.Significant.FundsMultiplier), targetBody);
				break;
			}
			return true;
		}
		return false;
	}

	public override void OnRegister()
	{
		GameEvents.Contract.onContractPreBuiltVesselSpawned.Add(OnContractPreBuiltVesselSpawned);
		GameEvents.onVesselPersistentIdChanged.Add(VesselIdChanged);
		GameEvents.onVesselDestroy.Add(OnVesselDestroy);
	}

	public override void OnUnregister()
	{
		GameEvents.Contract.onContractPreBuiltVesselSpawned.Remove(OnContractPreBuiltVesselSpawned);
		GameEvents.onVesselPersistentIdChanged.Remove(VesselIdChanged);
		GameEvents.onVesselDestroy.Remove(OnVesselDestroy);
		CleanupRenderer();
	}

	public override bool CanBeCancelled()
	{
		return true;
	}

	public override bool CanBeDeclined()
	{
		return true;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(this).ToString(CultureInfo.InvariantCulture);
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_6002568", targetBody.displayName);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories("VesselRepair", base.Agent.Title, targetBody.displayName, "", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
	}

	public override string GetSynopsys()
	{
		string text = "";
		if (repairPart == null)
		{
			repairPart = PartLoader.getPartInfoByName(repairPartName);
		}
		if (repairPart != null)
		{
			text = repairPart.title;
		}
		return Localizer.Format("#autoLOC_6002576", text);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_6002579");
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("bodyName", ref value))
		{
			targetBody = FlightGlobals.GetBodyByName(value);
		}
		string value2 = "";
		if (node.TryGetValue("roverCraftDef", ref value2))
		{
			repairCraftDefinition = ContractDefs.PreBuiltCraftDefs.CraftByURL(value2);
		}
		node.TryGetValue("vesselName", ref vesselName);
		node.TryGetValue("craftStartLat", ref craftStartLatitude);
		node.TryGetValue("craftStartLon", ref craftStartLongitude);
		node.TryGetValue("orbitEccentricity", ref orbitEccentricity);
		node.TryGetValue("orbitAltitudeFactor", ref orbitAltitudeFactor);
		node.TryGetValue("orbitInclinationFactor", ref orbitInclinationFactor);
		node.TryGetValue("repairVslId", ref repairVesselId);
		node.TryGetValue("repairPartId", ref repairPartId);
		node.TryGetValue("repairPartName", ref repairPartName);
		spawnInOrbit = true;
		spawnOnGround = false;
		string value3 = "Orbit";
		node.TryGetValue("spawnOrbit", ref value3);
		if (value3 == "Landed")
		{
			spawnInOrbit = false;
			spawnOnGround = true;
		}
		if (spawnInOrbit && targetBody != null)
		{
			orbit = OrbitUtilities.GenerateOrbit(base.MissionSeed, targetBody, OrbitType.RANDOM, orbitAltitudeFactor, orbitInclinationFactor, orbitEccentricity);
			if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && base.ContractState == State.Offered && ContractDefs.DisplayOfferedOrbits)
			{
				SetupRenderer();
			}
		}
		FlightGlobals.FindVessel(repairVesselId, out var vessel);
		if (!(vessel == null))
		{
			return;
		}
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			if (FlightGlobals.Vessels[i].vesselName.Equals(vesselName))
			{
				repairVesselId = FlightGlobals.Vessels[i].persistentId;
				break;
			}
		}
		FlightGlobals.FindVessel(repairVesselId, out vessel);
		bool flag = false;
		Part partout = null;
		FlightGlobals.FindLoadedPart(repairPartId, out partout);
		if (partout == null)
		{
			ProtoPartSnapshot partout2 = null;
			FlightGlobals.FindUnloadedPart(repairPartId, out partout2);
			if (partout2 != null && partout2.pVesselRef != null && partout2.pVesselRef.persistentId == repairVesselId)
			{
				flag = true;
			}
		}
		else if (partout.vessel != null && partout.vessel.persistentId == repairVesselId)
		{
			flag = true;
		}
		if (!flag && vessel != null)
		{
			if (vessel.loaded)
			{
				for (int j = 0; j < vessel.parts.Count; j++)
				{
					if (vessel.parts[j].partName == repairPartName)
					{
						for (int k = 0; k < vessel.parts[j].Modules.Count; k++)
						{
							ModuleDeployablePart moduleDeployablePart = vessel.parts[j].Modules[k] as ModuleDeployablePart;
							if (moduleDeployablePart != null && moduleDeployablePart.deployState == ModuleDeployablePart.DeployState.BROKEN)
							{
								repairPartId = vessel.parts[j].persistentId;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			else
			{
				for (int l = 0; l < vessel.protoVessel.protoPartSnapshots.Count; l++)
				{
					if (vessel.protoVessel.protoPartSnapshots[l].partName == repairPartName)
					{
						for (int m = 0; m < vessel.protoVessel.protoPartSnapshots[l].modules.Count; m++)
						{
							string value4 = "";
							if (vessel.protoVessel.protoPartSnapshots[l].modules[m].moduleValues.TryGetValue("deployState", ref value4) && value4 == "BROKEN")
							{
								repairPartId = vessel.parts[l].persistentId;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
		}
		RepairPartParameter repairPartParameter = GetParameter(typeof(RepairPartParameter)) as RepairPartParameter;
		if (vessel != null)
		{
			repairPartParameter?.UpdatePartInfo(repairPartId, "", repairVesselId, "", null);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("bodyName", targetBody.bodyName);
		if (repairCraftDefinition != null)
		{
			node.AddValue("roverCraftDef", repairCraftDefinition.craftURL);
		}
		node.AddValue("vesselName", vesselName);
		node.AddValue("craftStartLat", craftStartLatitude);
		node.AddValue("craftStartLon", craftStartLongitude);
		node.AddValue("orbitEccentricity", orbitEccentricity);
		node.AddValue("orbitAltitudeFactor", orbitAltitudeFactor);
		node.AddValue("orbitInclinationFactor", orbitInclinationFactor);
		if (repairVesselId != 0)
		{
			node.AddValue("repairVslId", repairVesselId);
		}
		if (repairPartId != 0)
		{
			node.AddValue("repairPartId", repairPartId);
		}
		if (!string.IsNullOrEmpty(repairPartName))
		{
			node.AddValue("repairPartName", repairPartName);
		}
		if (spawnOnGround)
		{
			node.AddValue("spawnOrbit", "Landed");
		}
	}

	public override bool MeetRequirements()
	{
		AvailablePart partInfoByName = PartLoader.getPartInfoByName("evaRepairKit");
		if (partInfoByName != null && ResearchAndDevelopment.PartModelPurchased(partInfoByName))
		{
			return true;
		}
		return false;
	}

	public override void OnAccepted()
	{
		VesselSituation vesselSituation = new VesselSituation();
		VesselLocation vesselLocation = (vesselSituation.location = new VesselLocation());
		VesselGroundLocation vesselGroundLocation = new VesselGroundLocation();
		vesselGroundLocation.targetBody = targetBody;
		vesselGroundLocation.latitude = craftStartLatitude;
		vesselGroundLocation.longitude = craftStartLongitude;
		vesselLocation.vesselGroundLocation = vesselGroundLocation;
		if (spawnOnGround)
		{
			vesselSituation.location.orbitSnapShot = new MissionOrbit(targetBody);
			vesselLocation.situation = MissionSituation.VesselStartSituations.LANDED;
		}
		if (spawnInOrbit)
		{
			orbit = OrbitUtilities.GenerateOrbit(base.MissionSeed, targetBody, OrbitType.RANDOM, orbitAltitudeFactor, orbitInclinationFactor, orbitEccentricity);
			vesselSituation.location.orbitSnapShot = new MissionOrbit(orbit);
			vesselLocation.situation = MissionSituation.VesselStartSituations.ORBITING;
		}
		vesselSituation.playerCreated = false;
		vesselSituation.craftFile = repairCraftDefinition.craftURL;
		vesselSituation.focusonSpawn = false;
		vesselSituation.vesselName = vesselName;
		waitingForVslSpawn = true;
		ContractSystem.Instance.SpawnPreBuiltShip(repairCraftDefinition, vesselSituation);
	}

	public string CreateVesselName(string bodyName)
	{
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		return Localizer.Format(VesselNameStrings[kSPRandom.Next(0, VesselNameStrings.Length)], bodyName, StringUtilities.AlphaNumericDesignation(base.MissionSeed));
	}

	public void OnContractPreBuiltVesselSpawned(Vessel vsl)
	{
		if (!waitingForVslSpawn)
		{
			return;
		}
		repairVesselId = vsl.protoVessel.persistentId;
		if (repairPart == null)
		{
			repairPart = PartLoader.getPartInfoByName(repairPartName);
		}
		ProtoPartSnapshot protoPartSnapshot = null;
		bool flag = false;
		for (int i = 0; i < vsl.protoVessel.protoPartSnapshots.Count; i++)
		{
			if (vsl.protoVessel.protoPartSnapshots[i].partName == repairPart.name)
			{
				for (int j = 0; j < vsl.protoVessel.protoPartSnapshots[i].modules.Count; j++)
				{
					string value = "";
					if (vsl.protoVessel.protoPartSnapshots[i].modules[j].moduleValues.TryGetValue("deployState", ref value) && value == "BROKEN")
					{
						protoPartSnapshot = vsl.protoVessel.protoPartSnapshots[i];
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (GetParameter(typeof(RepairPartParameter)) is RepairPartParameter repairPartParameter)
		{
			repairPartParameter.UpdatePartInfo(protoPartSnapshot?.persistentId ?? 0, repairPartName, repairVesselId, vesselName, targetBody);
		}
		waitingForVslSpawn = false;
	}

	public void VesselIdChanged(uint oldId, uint newId)
	{
		if (repairVesselId == oldId)
		{
			repairVesselId = newId;
		}
	}

	public void OnVesselDestroy(Vessel vsl)
	{
		if (vsl.persistentId == repairVesselId)
		{
			Fail();
		}
	}

	public void SetupRenderer()
	{
		if (!(orbitRenderer != null) && orbit != null)
		{
			orbitRenderer = ContractOrbitRenderer.Setup(base.Root, orbit);
		}
	}

	public void CleanupRenderer()
	{
		if (orbitRenderer != null)
		{
			orbitRenderer.Cleanup();
		}
	}
}
