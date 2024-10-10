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

public class OrbitalConstructionContract : Contract, IUpdateWaypoints
{
	public CelestialBody targetBody;

	public uint constructionVesselId;

	public string constructionPartName;

	public double orbitEccentricity;

	public double orbitAltitudeFactor = 0.5;

	public double orbitInclinationFactor = 0.5;

	public string vesselName;

	public ContractOrbitRenderer orbitRenderer;

	public Orbit orbit;

	public AvailablePart constructionPart;

	public bool waitingForVslSpawn;

	public PreBuiltCraftDefinition craftDefinition;

	public string[] VesselNameStrings = new string[12]
	{
		"#autoLOC_6002605", "#autoLOC_6002606", "#autoLOC_6002607", "#autoLOC_6002608", "#autoLOC_6002609", "#autoLOC_6002610", "#autoLOC_6002611", "#autoLOC_6002612", "#autoLOC_6002613", "#autoLOC_6002614",
		"#autoLOC_6002615", "#autoLOC_6002616"
	};

	public CelestialBody TargetBody => targetBody;

	public uint ConstructionVesselId => constructionVesselId;

	public string ConstructionPartName => constructionPartName;

	public double OrbitEccentricity => orbitEccentricity;

	public double OrbitAltitudeFactor => orbitAltitudeFactor;

	public double OrbitInclinationFactor => orbitInclinationFactor;

	public string VesselName => vesselName;

	public override bool Generate()
	{
		int num = 0;
		int num2 = 0;
		OrbitalConstructionContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<OrbitalConstructionContract>();
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
		if (num < ContractDefs.OrbitalConstruction.MaximumAvailable && num2 < ContractDefs.OrbitalConstruction.MaximumActive)
		{
			List<PreBuiltCraftDefinition> list = ContractDefs.PreBuiltCraftDefs.CraftForContractType("OrbitalConstruction");
			if (list.Count <= 0)
			{
				return false;
			}
			List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.FLYBY, bodyReached: true, progressComplete: true, MannedStatus.MANNED);
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
				int count2 = bodiesProgress.Count;
				while (count2-- > 0)
				{
					if (!bodiesProgress[count2].HasParent(homeBody))
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
			case ContractPrestige.Exceptional:
			{
				int count = bodiesProgress.Count;
				while (count-- > 0)
				{
					if (bodiesProgress[count].isHomeWorld || bodiesProgress[count].isStar || bodiesProgress[count].HasParent(homeBody))
					{
						bodiesProgress.RemoveAt(count);
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
			craftDefinition = list[index];
			List<ConstructionPart> list2 = ContractDefs.constructionParts.PartsForContractType("OrbitalConstruction");
			int count3 = list2.Count;
			while (count3-- > 0)
			{
				if (list2[count3].availablePart == null || !ResearchAndDevelopment.PartModelPurchased(list2[count3].availablePart))
				{
					list2.RemoveAt(count3);
				}
			}
			if (list2.Count <= 0)
			{
				return false;
			}
			int index2 = kSPRandom.Next(0, list2.Count - 1);
			constructionPartName = list2[index2].partName;
			constructionPart = list2[index2].availablePart;
			orbitEccentricity = 0.0;
			orbitAltitudeFactor = 0.5;
			orbitInclinationFactor = 0.5;
			switch (prestige)
			{
			default:
				orbitEccentricity = Math.Max(0.0, Math.Min(ContractDefs.OrbitalConstruction.Trivial.MaxOrbitEccentricityFactor, kSPRandom.NextDouble() / 2.0));
				orbitAltitudeFactor = ContractDefs.OrbitalConstruction.Trivial.OrbitAltitudeFactor;
				orbitInclinationFactor = ContractDefs.OrbitalConstruction.Trivial.OrbitInclinationFactor;
				break;
			case ContractPrestige.Exceptional:
				orbitEccentricity = Math.Max(0.0, Math.Min(ContractDefs.OrbitalConstruction.Exceptional.MaxOrbitEccentricityFactor, kSPRandom.NextDouble() / 2.0));
				orbitAltitudeFactor = ContractDefs.OrbitalConstruction.Exceptional.OrbitAltitudeFactor;
				orbitInclinationFactor = ContractDefs.OrbitalConstruction.Exceptional.OrbitInclinationFactor;
				break;
			case ContractPrestige.Significant:
				orbitEccentricity = Math.Max(0.0, Math.Min(ContractDefs.OrbitalConstruction.Significant.MaxOrbitEccentricityFactor, kSPRandom.NextDouble() / 2.0));
				orbitAltitudeFactor = ContractDefs.OrbitalConstruction.Significant.OrbitAltitudeFactor;
				orbitInclinationFactor = ContractDefs.OrbitalConstruction.Significant.OrbitInclinationFactor;
				break;
			}
			orbit = OrbitUtilities.GenerateOrbit(base.MissionSeed, targetBody, OrbitType.RANDOM, orbitAltitudeFactor, orbitInclinationFactor, orbitEccentricity);
			if (orbit == null)
			{
				return false;
			}
			vesselName = CreateVesselName(targetBody.bodyDisplayName);
			float num4 = 1f;
			AddParameter(new CrewTraitParameter("Engineer", 1, Localizer.Format("#autoLOC_8004217")));
			AddParameter(new ConstructionParameter(constructionPartName, constructionVesselId, vesselName, targetBody));
			AddKeywords("Commercial");
			SetExpiry(ContractDefs.RoverConstruction.Expire.MinimumExpireDays, ContractDefs.RoverConstruction.Expire.MaximumExpireDays);
			SetDeadlineDays((float)ContractDefs.RoverConstruction.Expire.DeadlineDays * num4, targetBody);
			switch (prestige)
			{
			default:
				SetScience(Mathf.RoundToInt(ContractDefs.RoverConstruction.Science.BaseReward * num4 * ContractDefs.RoverConstruction.Trivial.ScienceMultiplier), targetBody);
				SetReputation(Mathf.RoundToInt(ContractDefs.RoverConstruction.Reputation.BaseReward * num4 * ContractDefs.RoverConstruction.Trivial.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Reputation.BaseFailure * num4 * ContractDefs.RoverConstruction.Trivial.ReputationMultiplier), targetBody);
				SetFunds(Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseAdvance * num4 * ContractDefs.RoverConstruction.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseReward * num4 * ContractDefs.RoverConstruction.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseFailure * num4 * ContractDefs.RoverConstruction.Trivial.FundsMultiplier), targetBody);
				break;
			case ContractPrestige.Exceptional:
				SetScience(Mathf.RoundToInt(ContractDefs.RoverConstruction.Science.BaseReward * num4 * ContractDefs.RoverConstruction.Exceptional.ScienceMultiplier), targetBody);
				SetReputation(Mathf.RoundToInt(ContractDefs.RoverConstruction.Reputation.BaseReward * num4 * ContractDefs.RoverConstruction.Exceptional.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Reputation.BaseFailure * num4 * ContractDefs.RoverConstruction.Exceptional.ReputationMultiplier), targetBody);
				SetFunds(Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseAdvance * num4 * ContractDefs.RoverConstruction.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseReward * num4 * ContractDefs.RoverConstruction.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseFailure * num4 * ContractDefs.RoverConstruction.Exceptional.FundsMultiplier), targetBody);
				break;
			case ContractPrestige.Significant:
				SetScience(Mathf.RoundToInt(ContractDefs.RoverConstruction.Science.BaseReward * num4 * ContractDefs.RoverConstruction.Significant.ScienceMultiplier), targetBody);
				SetReputation(Mathf.RoundToInt(ContractDefs.RoverConstruction.Reputation.BaseReward * num4 * ContractDefs.RoverConstruction.Significant.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Reputation.BaseFailure * num4 * ContractDefs.RoverConstruction.Significant.ReputationMultiplier), targetBody);
				SetFunds(Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseAdvance * num4 * ContractDefs.RoverConstruction.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseReward * num4 * ContractDefs.RoverConstruction.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.RoverConstruction.Funds.BaseFailure * num4 * ContractDefs.RoverConstruction.Significant.FundsMultiplier), targetBody);
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
		return Localizer.Format("#autoLOC_6002592", targetBody.displayName);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories("OrbitConstruction", base.Agent.Title, targetBody.displayName, "", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
	}

	public override string GetSynopsys()
	{
		string text = "";
		if (constructionPart == null)
		{
			constructionPart = PartLoader.getPartInfoByName(constructionPartName);
		}
		if (constructionPart != null)
		{
			text = constructionPart.title;
		}
		return Localizer.Format("#autoLOC_6002599", text);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_6002603");
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("bodyName", ref value))
		{
			targetBody = FlightGlobals.GetBodyByName(value);
		}
		string value2 = "";
		if (node.TryGetValue("constructionCraftDef", ref value2))
		{
			craftDefinition = ContractDefs.PreBuiltCraftDefs.CraftByURL(value2);
		}
		node.TryGetValue("constructionVslId", ref constructionVesselId);
		node.TryGetValue("constructionPartName", ref constructionPartName);
		if (!string.IsNullOrEmpty(constructionPartName))
		{
			constructionPart = PartLoader.getPartInfoByName(constructionPartName);
		}
		node.TryGetValue("orbitEccentricity", ref orbitEccentricity);
		node.TryGetValue("orbitAltitudeFactor", ref orbitAltitudeFactor);
		node.TryGetValue("orbitInclinationFactor", ref orbitInclinationFactor);
		node.TryGetValue("vesselName", ref vesselName);
		if (targetBody != null)
		{
			orbit = OrbitUtilities.GenerateOrbit(base.MissionSeed, targetBody, OrbitType.RANDOM, orbitAltitudeFactor, orbitInclinationFactor, orbitEccentricity);
			if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && base.ContractState == State.Offered && ContractDefs.DisplayOfferedOrbits)
			{
				SetupRenderer();
			}
		}
		FlightGlobals.FindVessel(constructionVesselId, out var vessel);
		if (!(vessel == null))
		{
			return;
		}
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			if (FlightGlobals.Vessels[i].vesselName.Equals(vesselName))
			{
				constructionVesselId = FlightGlobals.Vessels[i].persistentId;
				break;
			}
		}
		if (GetParameter(typeof(ConstructionParameter)) is ConstructionParameter constructionParameter)
		{
			constructionParameter.UpdatePartInfo("", constructionVesselId, vesselName, null);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("bodyName", targetBody.bodyName);
		if (craftDefinition != null)
		{
			node.AddValue("constructionCraftDef", craftDefinition.craftURL);
		}
		if (constructionVesselId != 0)
		{
			node.AddValue("constructionVslId", constructionVesselId);
		}
		if (!string.IsNullOrEmpty(constructionPartName))
		{
			node.AddValue("constructionPartName", constructionPartName);
		}
		node.AddValue("orbitEccentricity", orbitEccentricity);
		node.AddValue("orbitAltitudeFactor", orbitAltitudeFactor);
		node.AddValue("orbitInclinationFactor", orbitInclinationFactor);
		node.AddValue("vesselName", vesselName);
	}

	public override bool MeetRequirements()
	{
		if (ResearchAndDevelopment.GetTechnologyState(ContractDefs.OrbitalConstruction.TechNodeRequired) != RDTech.State.Available)
		{
			return false;
		}
		return true;
	}

	public override void OnAccepted()
	{
		VesselSituation vesselSituation = new VesselSituation();
		VesselLocation vesselLocation = (vesselSituation.location = new VesselLocation());
		new VesselGroundLocation().targetBody = targetBody;
		orbit = OrbitUtilities.GenerateOrbit(base.MissionSeed, targetBody, OrbitType.RANDOM, orbitAltitudeFactor, orbitInclinationFactor, orbitEccentricity);
		vesselSituation.location.orbitSnapShot = new MissionOrbit(orbit);
		vesselLocation.situation = MissionSituation.VesselStartSituations.ORBITING;
		vesselSituation.playerCreated = false;
		vesselSituation.craftFile = craftDefinition.craftURL;
		vesselSituation.focusonSpawn = false;
		vesselSituation.vesselName = vesselName;
		waitingForVslSpawn = true;
		ContractSystem.Instance.SpawnPreBuiltShip(craftDefinition, vesselSituation);
	}

	public string CreateVesselName(string bodyName)
	{
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		return Localizer.Format(VesselNameStrings[kSPRandom.Next(0, VesselNameStrings.Length)], bodyName, StringUtilities.AlphaNumericDesignation(base.MissionSeed));
	}

	public void OnContractPreBuiltVesselSpawned(Vessel vsl)
	{
		if (waitingForVslSpawn)
		{
			constructionVesselId = vsl.persistentId;
			if (GetParameter(typeof(ConstructionParameter)) is ConstructionParameter constructionParameter)
			{
				constructionParameter.UpdatePartInfo(constructionPartName, constructionVesselId, vesselName, targetBody);
			}
			waitingForVslSpawn = false;
		}
	}

	public void VesselIdChanged(uint oldId, uint newId)
	{
		if (constructionVesselId == oldId)
		{
			constructionVesselId = newId;
		}
	}

	public void OnVesselDestroy(Vessel vsl)
	{
		if (vsl.persistentId == constructionVesselId)
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
