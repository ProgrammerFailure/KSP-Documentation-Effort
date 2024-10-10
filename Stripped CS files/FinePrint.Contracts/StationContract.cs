using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts;

public class StationContract : Contract, IUpdateWaypoints, ICheckSpecificVessels
{
	public CelestialBody targetBody;

	public int capacity;

	public bool contextual;

	public static List<string> objectiveTypes;

	public override bool Generate()
	{
		if (!AreFacilitiesUnlocked())
		{
			return false;
		}
		if (ContractSystem.Instance.GetCurrentContracts<StationContract>().Length >= ContractDefs.Station.MaximumExistent)
		{
			return false;
		}
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		float fundsMultiplier = 1f;
		float scienceMultiplier = 1f;
		float reputationMultiplier = 1f;
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.ORBIT, progressComplete: true, MannedStatus.MANNED);
		if (base.Prestige == ContractPrestige.Exceptional)
		{
			bodiesProgress.AddRange(ProgressUtilities.GetNextUnreached(2, MannedStatus.MANNED));
			while (bodiesProgress.Contains(Planetarium.fetch.Home))
			{
				bodiesProgress.Remove(Planetarium.fetch.Home);
			}
		}
		else if (base.Prestige == ContractPrestige.Significant)
		{
			bodiesProgress.AddRange(ProgressUtilities.GetNextUnreached(1, MannedStatus.MANNED));
		}
		if (!ContractDefs.Station.AllowSolar)
		{
			while (bodiesProgress.Contains(Planetarium.fetch.Sun))
			{
				bodiesProgress.Remove(Planetarium.fetch.Sun);
			}
		}
		if (bodiesProgress.Count == 0)
		{
			return false;
		}
		targetBody = WeightedBodyChoice(bodiesProgress, kSPRandom);
		Vessel vessel = null;
		int num = 5;
		int num2 = 5 + kSPRandom.Next(1, 8);
		int num3 = 7 + kSPRandom.Next(1, 14);
		List<Vessel> list = VesselUtilities.SpecificVesselClassAt(targetBody, VesselType.Station, requireOwned: true, excludeActive: true, useVesselType: false);
		int num4 = Mathf.RoundToInt(ContractDefs.Station.ContextualChance * (Math.Min(list.Count, ContractDefs.Station.ContextualAssets) / ContractDefs.Station.ContextualAssets));
		if (kSPRandom.Next(0, 100) < num4)
		{
			contextual = true;
			vessel = list[kSPRandom.Next(list.Count)];
			AddParameter(new SpecificVesselParameter(vessel));
			int num5 = VesselUtilities.ActualCrewCapacity(vessel);
			num += num5;
			num2 += num5;
			num3 += num5;
		}
		if (ProgressUtilities.GetBodyProgress(ProgressType.ORBIT, targetBody, MannedStatus.MANNED))
		{
			switch (base.Prestige)
			{
			case ContractPrestige.Trivial:
				capacity = num;
				break;
			case ContractPrestige.Significant:
				capacity = num2;
				break;
			case ContractPrestige.Exceptional:
				capacity = num3;
				break;
			}
		}
		else
		{
			switch (base.Prestige)
			{
			case ContractPrestige.Trivial:
				capacity = num;
				break;
			case ContractPrestige.Significant:
				capacity = num;
				break;
			case ContractPrestige.Exceptional:
				capacity = num2;
				break;
			}
			AddKeywords("Pioneer");
		}
		if (objectiveTypes == null)
		{
			objectiveTypes = new List<string> { "Antenna", "Generator", "Dock" };
		}
		string text = Localizer.Format("#autoLOC_7001017");
		AddParameter(new VesselSystemsParameter(objectiveTypes, new List<string>
		{
			Localizer.Format("#autoLOC_7001075"),
			Localizer.Format("#autoLOC_7001076"),
			Localizer.Format("#autoLOC_7001077")
		}, text, MannedStatus.const_2, !contextual));
		AddParameter(new CrewCapacityParameter(capacity));
		float num6 = 0f;
		string size = "";
		switch (base.Prestige)
		{
		case ContractPrestige.Trivial:
			num6 = ContractDefs.Station.TrivialAsteroidChance;
			size = (SystemUtilities.CoinFlip(kSPRandom) ? "A" : "B");
			break;
		case ContractPrestige.Significant:
			num6 = ContractDefs.Station.SignificantAsteroidChance;
			size = (SystemUtilities.CoinFlip(kSPRandom) ? "B" : "C");
			break;
		case ContractPrestige.Exceptional:
			num6 = ContractDefs.Station.ExceptionalAsteroidChance;
			size = (SystemUtilities.CoinFlip(kSPRandom) ? "D" : "E");
			break;
		}
		ConfigNode node = ContractDefs.config.GetNode("Station");
		if (node != null)
		{
			SystemUtilities.ProcessSideRequests(this, node, targetBody, text, ref fundsMultiplier, ref scienceMultiplier, ref reputationMultiplier, vessel);
		}
		if (AreAsteroidsUnlocked())
		{
			if (contextual && vessel != null && (targetBody.name != "Dres" || VesselUtilities.VesselHasPartName("PotatoRoid", vessel)))
			{
				num6 = 0f;
			}
			if ((float)kSPRandom.Next(0, 100) < num6)
			{
				AddKeywords("Pioneer");
				AddParameter(new AsteroidParameter(size, forStation: true));
				fundsMultiplier *= ContractDefs.Station.Funds.AsteroidMultiplier;
				scienceMultiplier *= ContractDefs.Station.Science.AsteroidMultiplier;
				reputationMultiplier *= ContractDefs.Station.Reputation.AsteroidMultiplier;
			}
		}
		AddParameter(new StabilityParameter(10f));
		AddParameter(new LocationAndSituationParameter(targetBody, Vessel.Situations.ORBITING, text));
		float num7 = (float)Math.Max(FundsCompletion, ContractDefs.Station.Funds.BaseReward);
		float num8 = Math.Max(ScienceCompletion, ContractDefs.Station.Science.BaseReward);
		float num9 = Math.Max(ReputationCompletion, ContractDefs.Station.Reputation.BaseReward);
		SetExpiry(ContractDefs.Station.Expire.MinimumExpireDays, ContractDefs.Station.Expire.MaximumExpireDays);
		SetDeadlineDays(ContractDefs.Station.Expire.DeadlineDays, targetBody);
		SetFunds(Mathf.Round(ContractDefs.Station.Funds.BaseAdvance * fundsMultiplier), Mathf.Round(num7 * fundsMultiplier), Mathf.Round(ContractDefs.Station.Funds.BaseFailure * fundsMultiplier), targetBody);
		SetScience(Mathf.Round(num8 * scienceMultiplier));
		SetReputation(Mathf.Round(num9 * reputationMultiplier), Mathf.Round(ContractDefs.Station.Reputation.BaseFailure * reputationMultiplier));
		if (ContractSystem.Instance.AnyCurrentContracts((StationContract contract) => contract.targetBody == targetBody))
		{
			return false;
		}
		if (ContractSystem.Instance.AnyCurrentContracts((BaseContract contract) => contract.targetBody == targetBody))
		{
			return false;
		}
		return true;
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
		string text = ((!contextual) ? Localizer.Format("#autoLOC_7001020") : Localizer.Format("#autoLOC_7001019", StringUtilities.SpecificVesselName(this)));
		string text2 = ((!(targetBody == Planetarium.fetch.Sun)) ? Localizer.Format("#autoLOC_7001022", targetBody.displayName) : Localizer.Format("#autoLOC_7001021"));
		return Localizer.Format("#autoLOC_6002375", text, text2);
	}

	public override string GetDescription()
	{
		string text = StringUtilities.IntegerToGreek(new KSPRandom(base.MissionSeed).Next(23));
		string topic = (contextual ? StringUtilities.SpecificVesselName(this) : (targetBody.displayName + " Station " + text));
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, topic, targetBody.displayName, base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: true, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (contextual)
		{
			return Localizer.Format("#autoLOC_280652", targetBody.displayName, StringUtilities.SpecificVesselName(this));
		}
		if (targetBody == Planetarium.fetch.Sun)
		{
			return Localizer.Format("#autoLOC_7001071", capacity);
		}
		return Localizer.Format("#autoLOC_7001072", capacity, targetBody.displayName);
	}

	public override string MessageCompleted()
	{
		if (contextual)
		{
			return Localizer.Format("#autoLOC_280660", StringUtilities.SpecificVesselName(this));
		}
		if (targetBody == Planetarium.fetch.Sun)
		{
			return Localizer.Format("#autoLOC_7001073");
		}
		return Localizer.Format("#autoLOC_7001074", targetBody.displayName);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("capacity", capacity);
		node.AddValue("contextual", contextual);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "StationContract", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "StationContract", "capacity", ref capacity, 8);
		SystemUtilities.LoadNode(node, "StationContract", "contextual", ref contextual, defaultValue: false);
	}

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, "Orbit"))
		{
			return false;
		}
		return true;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		return new List<CelestialBody> { targetBody };
	}

	public override void OnAccepted()
	{
		VesselSystemsParameter parameter = GetParameter<VesselSystemsParameter>();
		if (parameter != null && parameter.requireNew)
		{
			parameter.launchID = HighLogic.CurrentGame.launchID;
		}
	}

	public override void OnFinished()
	{
		GetParameter<SpecificVesselParameter>()?.CleanupWaypoints();
	}

	public static bool AreFacilitiesUnlocked()
	{
		if (ResearchAndDevelopment.ResearchedValidContractObjectives("Antenna", "Dock", "Generator"))
		{
			return true;
		}
		return false;
	}

	public static bool AreAsteroidsUnlocked()
	{
		if (!ContractDefs.Station.AllowAsteroid)
		{
			return false;
		}
		if (!GameVariables.Instance.UnlockedSpaceObjectDiscovery(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)))
		{
			return false;
		}
		if (!ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Sun.name, "Orbit"))
		{
			return false;
		}
		if (!ResearchAndDevelopment.ResearchedValidContractObjectives("Grapple"))
		{
			return false;
		}
		return true;
	}
}
