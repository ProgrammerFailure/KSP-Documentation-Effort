using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts;

public class BaseContract : Contract, IUpdateWaypoints, ICheckSpecificVessels
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
		if (ContractSystem.Instance.GetCurrentContracts<BaseContract>().Length >= ContractDefs.Base.MaximumExistent)
		{
			return false;
		}
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		float fundsMultiplier = 1f;
		float scienceMultiplier = 1f;
		float reputationMultiplier = 1f;
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.LANDING, progressComplete: true, MannedStatus.MANNED, (CelestialBody cb) => cb.hasSolidSurface && cb != Planetarium.fetch.Sun && cb != Planetarium.fetch.Home);
		if (base.Prestige == ContractPrestige.Significant)
		{
			bodiesProgress.AddRange(ProgressUtilities.GetNextUnreached(1, MannedStatus.MANNED, (CelestialBody cb) => cb.hasSolidSurface && cb != Planetarium.fetch.Sun && cb != Planetarium.fetch.Home));
		}
		else if (base.Prestige == ContractPrestige.Exceptional)
		{
			bodiesProgress.AddRange(ProgressUtilities.GetNextUnreached(2, MannedStatus.MANNED, (CelestialBody cb) => cb.hasSolidSurface && cb != Planetarium.fetch.Sun && cb != Planetarium.fetch.Home));
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
		List<Vessel> list = VesselUtilities.SpecificVesselClassAt(targetBody, VesselType.Base, requireOwned: true, excludeActive: true, useVesselType: false);
		int num4 = Mathf.RoundToInt(ContractDefs.Base.ContextualChance * (Math.Min(list.Count, ContractDefs.Base.ContextualAssets) / ContractDefs.Base.ContextualAssets));
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
		if (ProgressUtilities.GetBodyProgress(ProgressType.LANDING, targetBody, MannedStatus.MANNED))
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
		if (CelestialUtilities.IsGasGiant(targetBody))
		{
			List<CelestialBody> neighbors = CelestialUtilities.GetNeighbors(targetBody, (CelestialBody cb) => cb != Planetarium.fetch.Sun && !CelestialUtilities.IsGasGiant(cb));
			if (neighbors.Count <= 0)
			{
				return false;
			}
			targetBody = WeightedBodyChoice(neighbors, kSPRandom);
		}
		if (objectiveTypes == null)
		{
			objectiveTypes = new List<string> { "Antenna", "Generator", "Dock" };
		}
		AddParameter(new VesselSystemsParameter(objectiveTypes, new List<string>
		{
			Localizer.Format("#autoLOC_7001075"),
			Localizer.Format("#autoLOC_7001076"),
			Localizer.Format("#autoLOC_7001077")
		}, Localizer.Format("#autoLOC_7001078"), MannedStatus.const_2, !contextual));
		AddParameter(new CrewCapacityParameter(capacity));
		float num6 = 0f;
		switch (base.Prestige)
		{
		case ContractPrestige.Trivial:
			num6 = ContractDefs.Base.TrivialMobileChance;
			break;
		case ContractPrestige.Significant:
			num6 = ContractDefs.Base.SignificantMobileChance;
			break;
		case ContractPrestige.Exceptional:
			num6 = ContractDefs.Base.ExceptionalMobileChance;
			break;
		}
		ConfigNode node = ContractDefs.config.GetNode("Base");
		if (node != null)
		{
			SystemUtilities.ProcessSideRequests(this, node, targetBody, Localizer.Format("#autoLOC_278368"), ref fundsMultiplier, ref scienceMultiplier, ref reputationMultiplier, vessel);
		}
		if (!contextual && ResearchAndDevelopment.ResearchedValidContractObjectives("Wheel") && ContractDefs.Base.AllowMobile && (float)kSPRandom.Next(0, 100) < num6)
		{
			AddKeywords("Pioneer");
			AddParameter(new MobileBaseParameter());
			fundsMultiplier *= ContractDefs.Base.Funds.MobileMultiplier;
			scienceMultiplier *= ContractDefs.Base.Science.MobileMultiplier;
			reputationMultiplier *= ContractDefs.Base.Reputation.MobileMultiplier;
		}
		AddParameter(new StabilityParameter(10f));
		AddParameter(new LocationAndSituationParameter(targetBody, Vessel.Situations.LANDED, Localizer.Format("#autoLOC_278346")));
		float num7 = (float)Math.Max(FundsCompletion, ContractDefs.Base.Funds.BaseReward);
		float num8 = Math.Max(ScienceCompletion, ContractDefs.Base.Science.BaseReward);
		float num9 = Math.Max(ReputationCompletion, ContractDefs.Base.Reputation.BaseReward);
		SetExpiry(ContractDefs.Base.Expire.MinimumExpireDays, ContractDefs.Base.Expire.MaximumExpireDays);
		SetDeadlineDays(ContractDefs.Base.Expire.DeadlineDays, targetBody);
		SetFunds(Mathf.Round(ContractDefs.Base.Funds.BaseAdvance * fundsMultiplier), Mathf.Round(num7 * fundsMultiplier), Mathf.Round(ContractDefs.Base.Funds.BaseFailure * fundsMultiplier), targetBody);
		SetScience(Mathf.Round(num8 * scienceMultiplier));
		SetReputation(Mathf.Round(num9 * reputationMultiplier), Mathf.Round(ContractDefs.Base.Reputation.BaseFailure * reputationMultiplier));
		if (ContractSystem.Instance.AnyCurrentContracts((BaseContract contract) => contract.targetBody == targetBody))
		{
			return false;
		}
		if (ContractSystem.Instance.AnyCurrentContracts((StationContract contract) => contract.targetBody == targetBody))
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
		if (contextual)
		{
			return Localizer.Format("#autoLOC_7001079", StringUtilities.SpecificVesselName(this), targetBody.displayName);
		}
		return Localizer.Format("#autoLOC_7001080", targetBody.displayName);
	}

	public override string GetDescription()
	{
		string text = StringUtilities.IntegerToGreek(new KSPRandom(base.MissionSeed).Next(23));
		string topic = (contextual ? StringUtilities.SpecificVesselName(this) : (targetBody.displayName + " Base " + text));
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, topic, targetBody.displayName + "Srf", base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (contextual)
		{
			return Localizer.Format("#autoLOC_278436", targetBody.displayName, StringUtilities.SpecificVesselName(this));
		}
		return Localizer.Format("#autoLOC_278438", capacity, targetBody.displayName);
	}

	public override string MessageCompleted()
	{
		if (contextual)
		{
			return Localizer.Format("#autoLOC_278444", StringUtilities.SpecificVesselName(this));
		}
		return Localizer.Format("#autoLOC_278446", targetBody.displayName);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("capacity", capacity);
		node.AddValue("contextual", contextual);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "BaseContract", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "BaseContract", "capacity", ref capacity, 8);
		SystemUtilities.LoadNode(node, "BaseContract", "contextual", ref contextual, defaultValue: false);
	}

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Home.name, Localizer.Format("#autoLOC_278465")))
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
}
