using System.Collections.Generic;
using System.Globalization;
using Expansions.Missions;
using FinePrint;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Contracts.Templates;

public class RoverConstructionContract : Contract, IUpdateWaypoints
{
	public CelestialBody targetBody;

	public double craftStartLatitude;

	public double craftStartLongitude;

	public double craftEndLatitude;

	public double craftEndLongitude;

	public uint roverVesselId;

	public PreBuiltCraftDefinition roverCraftDefinition;

	public bool waitingForVslSpawn;

	public string[] VesselNameStrings = new string[9] { "#autoLOC_6002628", "#autoLOC_6002629", "#autoLOC_6002630", "#autoLOC_6002631", "#autoLOC_6002632", "#autoLOC_6002633", "#autoLOC_6002634", "#autoLOC_6002635", "#autoLOC_6002636" };

	public CelestialBody TargetBody => targetBody;

	public double CraftStartLatitude => craftStartLatitude;

	public double CraftStartLongitude => craftStartLongitude;

	public double CraftEndLatitude => craftEndLatitude;

	public double CraftEndLongitude => craftEndLongitude;

	public uint RoverVesselId => roverVesselId;

	public override bool Generate()
	{
		int num = 0;
		int num2 = 0;
		RoverConstructionContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<RoverConstructionContract>();
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
		if (num < ContractDefs.RoverConstruction.MaximumAvailable && num2 < ContractDefs.RoverConstruction.MaximumActive)
		{
			List<PreBuiltCraftDefinition> list = ContractDefs.PreBuiltCraftDefs.CraftForContractType("RoverConstruction");
			if (list.Count <= 0)
			{
				return false;
			}
			List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.ORBIT, bodyReached: true, progressComplete: true, MannedStatus.const_2, (CelestialBody cb) => cb != Planetarium.fetch.Home);
			int count = bodiesProgress.Count;
			while (count-- > 0)
			{
				if (!bodiesProgress[count].hasSolidSurface)
				{
					bodiesProgress.RemoveAt(count);
				}
				else if (bodiesProgress[count].GeeASL < (double)ContractDefs.RoverConstruction.MinimumGeeASL || bodiesProgress[count].GeeASL > (double)ContractDefs.RoverConstruction.MaximumGeeASL)
				{
					bodiesProgress.RemoveAt(count);
				}
			}
			targetBody = WeightedBodyChoice(bodiesProgress);
			KSPRandom kSPRandom = new KSPRandom();
			int index = kSPRandom.Next(0, list.Count - 1);
			roverCraftDefinition = list[index];
			bool flag = false;
			if (roverCraftDefinition.usePreBuiltPositions && ContractDefs.PreBuiltCraftPositions.ContainsKey(targetBody.bodyName))
			{
				List<PreBuiltCraftPosition> list2 = ContractDefs.PreBuiltCraftPositions[targetBody.bodyName];
				int index2 = kSPRandom.Next(0, list2.Count - 1);
				double centerLatitude = list2[index2].CenterLatitude;
				double centerLongitude = list2[index2].CenterLongitude;
				float searchRadius = list2[index2].SearchRadius;
				WaypointManager.ChooseRandomPositionNear(out craftStartLatitude, out craftStartLongitude, centerLatitude, centerLongitude, targetBody.bodyName, searchRadius, roverCraftDefinition.allowWater);
				flag = true;
			}
			if (!flag)
			{
				WaypointManager.ChooseRandomPosition(out craftStartLatitude, out craftStartLongitude, targetBody.bodyName, roverCraftDefinition.allowWater, equatorial: false, kSPRandom);
			}
			float wayPointMinDistance = ContractDefs.RoverConstruction.WayPointMinDistance;
			float wayPointMaxDistance = ContractDefs.RoverConstruction.WayPointMaxDistance;
			switch (prestige)
			{
			default:
				wayPointMinDistance *= ContractDefs.RoverConstruction.Trivial.WayPointMultiplier;
				wayPointMaxDistance *= ContractDefs.RoverConstruction.Trivial.WayPointMultiplier;
				break;
			case ContractPrestige.Exceptional:
				wayPointMinDistance *= ContractDefs.RoverConstruction.Exceptional.WayPointMultiplier;
				wayPointMaxDistance *= ContractDefs.RoverConstruction.Exceptional.WayPointMultiplier;
				break;
			case ContractPrestige.Significant:
				wayPointMinDistance *= ContractDefs.RoverConstruction.Significant.WayPointMultiplier;
				wayPointMaxDistance *= ContractDefs.RoverConstruction.Significant.WayPointMultiplier;
				break;
			}
			CalculateWaypoint(wayPointMinDistance, wayPointMaxDistance);
			float num4 = 1f;
			AddParameter(new RoverWayPointParameter(craftStartLatitude, craftStartLongitude, craftEndLatitude, craftEndLongitude, targetBody));
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

	public void CalculateWaypoint(float minRadius, float maxRadius)
	{
		Waypoint waypoint = new Waypoint
		{
			celestialName = TargetBody.bodyName,
			latitude = craftStartLatitude,
			longitude = craftStartLongitude
		};
		KSPRandom generator = new KSPRandom();
		waypoint.RandomizeNear(waypoint.latitude, waypoint.longitude, minRadius, maxRadius, roverCraftDefinition.allowWater, generator);
		craftEndLatitude = waypoint.latitude;
		craftEndLongitude = waypoint.longitude;
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
		return Localizer.Format("#autoLOC_6002550", targetBody.displayName);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories("RoverConstruction", base.Agent.Title, targetBody.displayName, "", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
	}

	public override string GetSynopsys()
	{
		return Localizer.Format("#autoLOC_6002551");
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_6002552");
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
			roverCraftDefinition = ContractDefs.PreBuiltCraftDefs.CraftByURL(value2);
		}
		node.TryGetValue("craftStartLat", ref craftStartLatitude);
		node.TryGetValue("craftStartLon", ref craftStartLongitude);
		node.TryGetValue("craftEndLat", ref craftEndLatitude);
		node.TryGetValue("craftEndLon", ref craftEndLongitude);
		node.TryGetValue("roverVslId", ref roverVesselId);
		FlightGlobals.FindVessel(roverVesselId, out var vessel);
		if (!(vessel == null))
		{
			return;
		}
		string value3 = CreateVesselName(targetBody.bodyDisplayName);
		if (string.IsNullOrEmpty(value3))
		{
			return;
		}
		for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
		{
			if (FlightGlobals.Vessels[i].vesselName.Equals(value3))
			{
				roverVesselId = FlightGlobals.Vessels[i].persistentId;
				break;
			}
		}
		FlightGlobals.FindVessel(roverVesselId, out vessel);
		RoverWayPointParameter roverWayPointParameter = GetParameter(typeof(RoverWayPointParameter)) as RoverWayPointParameter;
		if (vessel != null)
		{
			roverWayPointParameter?.UpdateRoverInfo(vessel);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("bodyName", targetBody.bodyName);
		if (roverCraftDefinition != null)
		{
			node.AddValue("roverCraftDef", roverCraftDefinition.craftURL);
		}
		node.AddValue("craftStartLat", craftStartLatitude);
		node.AddValue("craftStartLon", craftStartLongitude);
		node.AddValue("craftEndLat", craftEndLatitude);
		node.AddValue("craftEndLon", craftEndLongitude);
		if (roverVesselId != 0)
		{
			node.AddValue("roverVslId", roverVesselId);
		}
	}

	public override bool MeetRequirements()
	{
		if (ProgressUtilities.GetBodiesProgress(ProgressType.ORBIT, bodyReached: true, progressComplete: true, MannedStatus.const_2, (CelestialBody cb) => cb != Planetarium.fetch.Home).Count <= 0)
		{
			return false;
		}
		if (ResearchAndDevelopment.GetTechnologyState(ContractDefs.RoverConstruction.TechNodeRequired) != RDTech.State.Available)
		{
			return false;
		}
		return true;
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
		vesselSituation.location.orbitSnapShot = new MissionOrbit(targetBody);
		vesselSituation.playerCreated = false;
		vesselSituation.craftFile = roverCraftDefinition.craftURL;
		vesselSituation.focusonSpawn = false;
		vesselSituation.vesselName = CreateVesselName(targetBody.bodyDisplayName);
		vesselLocation.situation = MissionSituation.VesselStartSituations.LANDED;
		waitingForVslSpawn = true;
		ContractSystem.Instance.SpawnPreBuiltShip(roverCraftDefinition, vesselSituation);
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
			roverVesselId = vsl.persistentId;
			if (GetParameter(typeof(RoverWayPointParameter)) is RoverWayPointParameter roverWayPointParameter)
			{
				roverWayPointParameter.UpdateRoverInfo(vsl);
			}
			waitingForVslSpawn = false;
		}
	}

	public void VesselIdChanged(uint oldId, uint newId)
	{
		if (roverVesselId == oldId)
		{
			roverVesselId = newId;
		}
	}

	public void OnVesselDestroy(Vessel vsl)
	{
		if (vsl.persistentId == roverVesselId)
		{
			Fail();
		}
	}
}
