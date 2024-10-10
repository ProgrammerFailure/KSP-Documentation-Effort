using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Contracts;
using FinePrint;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace SentinelMission;

public class SentinelContract : Contract, IUpdateWaypoints
{
	public KSPRandom generator;

	public CelestialBody innerBody;

	public CelestialBody outerBody;

	public int asteroidCount = 3;

	public override bool Generate()
	{
		if (!ProgressUtilities.HavePartTech(SentinelUtilities.SentinelPartName, logging: false))
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		SentinelContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<SentinelContract>();
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
		if (num < ContractDefs.Sentinel.MaximumAvailable && num2 < ContractDefs.Sentinel.MaximumActive)
		{
			int seed = SystemUtilities.SuperSeed(this);
			generator = new KSPRandom(seed);
			float num4 = 1f;
			SentinelScanType sentinelScanType = SentinelScanType.NONE;
			UntrackedObjectClass untrackedObjectClass = ((!(ScenarioDiscoverableObjects.Instance != null)) ? ((UntrackedObjectClass)(Enum.GetNames(typeof(UntrackedObjectClass)).Length - 1)) : ScenarioDiscoverableObjects.Instance.maxAsteroidClass);
			double num5 = 0.0;
			double num6 = 0.0;
			asteroidCount = generator.Next(ContractDefs.Sentinel.Trivial.MinAsteroids, ContractDefs.Sentinel.Trivial.MaxAsteroids);
			switch (prestige)
			{
			default:
				SetFocusBody(Planetarium.fetch.Home);
				break;
			case ContractPrestige.Exceptional:
			{
				List<CelestialBody> list = (from cb in Contract.GetBodies_Reached(includeKerbin: true, includeSun: false)
					where cb.referenceBody == Planetarium.fetch.Sun
					select cb).ToList();
				if (list.Count <= 0)
				{
					return false;
				}
				SetFocusBody(list[generator.Next(0, list.Count)]);
				asteroidCount = generator.Next(ContractDefs.Sentinel.Exceptional.MinAsteroids, ContractDefs.Sentinel.Exceptional.MaxAsteroids);
				sentinelScanType = SentinelUtilities.RandomScanType(generator);
				break;
			}
			case ContractPrestige.Significant:
				SetFocusBody(Planetarium.fetch.Home);
				asteroidCount = generator.Next(ContractDefs.Sentinel.Significant.MinAsteroids, ContractDefs.Sentinel.Significant.MaxAsteroids);
				sentinelScanType = SentinelUtilities.RandomScanType(generator);
				break;
			}
			switch (sentinelScanType)
			{
			case SentinelScanType.NONE:
				num4 = ContractDefs.Sentinel.ScanTypeBaseMultiplier;
				break;
			case SentinelScanType.CLASS:
			{
				untrackedObjectClass = SentinelUtilities.WeightedAsteroidClass(generator);
				double num7 = Enum.GetNames(typeof(UntrackedObjectClass)).Length - 1;
				num4 = (float)((num7 + 1.0 + (double)untrackedObjectClass) / ((num7 + 1.0) * 2.0));
				num4 *= ContractDefs.Sentinel.ScanTypeBaseMultiplier;
				break;
			}
			case SentinelScanType.ECCENTRICITY:
				num5 = SentinelUtilities.WeightedRandom(generator, SentinelUtilities.MinAsteroidEccentricity, SentinelUtilities.MaxAsteroidEccentricity);
				num4 = (float)(0.5 + num5 / 2.0);
				num4 *= ContractDefs.Sentinel.ScanTypeEccentricityMultiplier;
				break;
			case SentinelScanType.INCLINATION:
				num6 = SentinelUtilities.WeightedRandom(generator, SentinelUtilities.MinAsteroidInclination, SentinelUtilities.MaxAsteroidInclination);
				num4 = (float)(0.5 + num6 / 2.0);
				num4 *= ContractDefs.Sentinel.ScanTypeInclinationMultiplier;
				break;
			}
			num4 *= Mathf.Round((float)asteroidCount / 2f);
			ConfigNode configNode = new ConfigNode("PART_REQUEST");
			configNode.AddValue("Article", "a");
			configNode.AddValue("PartDescription", SentinelUtilities.SentinelPartTitle);
			configNode.AddValue("VesselDescription", Localizer.Format("#autoLOC_6002407"));
			configNode.AddValue("Part", SentinelUtilities.SentinelPartName);
			AddParameter(new PartRequestParameter(configNode));
			Orbit orbit = Orbit.CreateRandomOrbitNearby(outerBody.orbit);
			if (innerBody == Planetarium.fetch.Sun)
			{
				orbit.semiMajorAxis = CelestialUtilities.GetMinimumOrbitalDistance(innerBody, 1f) * SentinelUtilities.RandomRange(generator, 1.05, 1.2);
			}
			else
			{
				orbit.semiMajorAxis = innerBody.orbit.semiMajorAxis * SentinelUtilities.RandomRange(generator, 1.05, 1.2);
			}
			switch (prestige)
			{
			default:
				AddParameter(new SpecificOrbitParameter(OrbitType.RANDOM, orbit.inclination, orbit.eccentricity, orbit.semiMajorAxis, orbit.double_0, orbit.argumentOfPeriapsis, orbit.meanAnomalyAtEpoch, orbit.epoch, Planetarium.fetch.Sun, ContractDefs.Sentinel.TrivialDeviation));
				break;
			case ContractPrestige.Exceptional:
				AddParameter(new SpecificOrbitParameter(OrbitType.RANDOM, orbit.inclination, orbit.eccentricity, orbit.semiMajorAxis, orbit.double_0, orbit.argumentOfPeriapsis, orbit.meanAnomalyAtEpoch, orbit.epoch, Planetarium.fetch.Sun, ContractDefs.Sentinel.ExceptionalDeviation));
				break;
			case ContractPrestige.Significant:
				AddParameter(new SpecificOrbitParameter(OrbitType.RANDOM, orbit.inclination, orbit.eccentricity, orbit.semiMajorAxis, orbit.double_0, orbit.argumentOfPeriapsis, orbit.meanAnomalyAtEpoch, orbit.epoch, Planetarium.fetch.Sun, ContractDefs.Sentinel.SignificantDeviation));
				break;
			}
			AddParameter(new SentinelParameter(outerBody, sentinelScanType, untrackedObjectClass, num5, num6, asteroidCount));
			AddKeywords("Pioneer", "Scientific");
			SetExpiry(ContractDefs.Sentinel.Expire.MinimumExpireDays, ContractDefs.Sentinel.Expire.MaximumExpireDays);
			SetDeadlineDays(Mathf.RoundToInt((float)ContractDefs.Sentinel.Expire.DeadlineDays * num4), innerBody);
			switch (prestige)
			{
			default:
				SetScience(Mathf.RoundToInt(ContractDefs.Sentinel.Science.BaseReward * num4 * ContractDefs.Sentinel.Trivial.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.Sentinel.Reputation.BaseReward * num4 * ContractDefs.Sentinel.Trivial.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Reputation.BaseFailure * num4 * ContractDefs.Sentinel.Trivial.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseAdvance * num4 * ContractDefs.Sentinel.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseReward * num4 * ContractDefs.Sentinel.Trivial.FundsMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseFailure * num4 * ContractDefs.Sentinel.Trivial.FundsMultiplier), innerBody);
				break;
			case ContractPrestige.Exceptional:
				SetScience(Mathf.RoundToInt(ContractDefs.Sentinel.Science.BaseReward * num4 * ContractDefs.Sentinel.Exceptional.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.Sentinel.Reputation.BaseReward * num4 * ContractDefs.Sentinel.Exceptional.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Reputation.BaseFailure * num4 * ContractDefs.Sentinel.Exceptional.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseAdvance * num4 * ContractDefs.Sentinel.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseReward * num4 * ContractDefs.Sentinel.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseFailure * num4 * ContractDefs.Sentinel.Exceptional.FundsMultiplier), innerBody);
				break;
			case ContractPrestige.Significant:
				SetScience(Mathf.RoundToInt(ContractDefs.Sentinel.Science.BaseReward * num4 * ContractDefs.Sentinel.Significant.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.Sentinel.Reputation.BaseReward * num4 * ContractDefs.Sentinel.Significant.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Reputation.BaseFailure * num4 * ContractDefs.Sentinel.Significant.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseAdvance * num4 * ContractDefs.Sentinel.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseReward * num4 * ContractDefs.Sentinel.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.Sentinel.Funds.BaseFailure * num4 * ContractDefs.Sentinel.Significant.FundsMultiplier), innerBody);
				break;
			}
			currentContracts = ContractSystem.Instance.GetCurrentContracts<SentinelContract>();
			int num8 = currentContracts.Length;
			do
			{
				if (num8-- <= 0)
				{
					return true;
				}
			}
			while (!(currentContracts[num8].outerBody == outerBody));
			return false;
		}
		return false;
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
		if (outerBody == Planetarium.fetch.Home)
		{
			return Localizer.Format("#autoLOC_6002297", asteroidCount, outerBody.displayName, SentinelUtilities.SentinelPartTitle);
		}
		string text = Localizer.Format("#autoLOC_7001310");
		return Localizer.Format("#autoLOC_6002298", asteroidCount, outerBody.displayName, text, SentinelUtilities.SentinelPartTitle);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, Localizer.Format("#autoLOC_6002299"), outerBody.displayName, base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (outerBody == Planetarium.fetch.Home)
		{
			string text = Localizer.Format("#autoLOC_7001311");
			return Localizer.Format("#autoLOC_6002300", outerBody.displayName, text, SentinelUtilities.SentinelPartTitle, innerBody.displayName);
		}
		string text2 = Localizer.Format("#autoLOC_7001310");
		return Localizer.Format("#autoLOC_6002301", SentinelUtilities.SentinelPartTitle, Planetarium.fetch.Home.displayName, text2, outerBody.displayName);
	}

	public override string MessageCompleted()
	{
		string text = Localizer.Format("#autoLOC_7001310");
		return Localizer.Format("#autoLOC_6002302", outerBody.displayName, text);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "SentinelContract", "innerBody", ref innerBody, Planetarium.fetch.Sun);
		SystemUtilities.LoadNode(node, "SentinelContract", "outerBody", ref outerBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "SentinelContract", "asteroidCount", ref asteroidCount, 3);
	}

	public override void OnSave(ConfigNode node)
	{
		int flightGlobalsIndex = innerBody.flightGlobalsIndex;
		node.AddValue("innerBody", flightGlobalsIndex);
		int flightGlobalsIndex2 = outerBody.flightGlobalsIndex;
		node.AddValue("outerBody", flightGlobalsIndex2);
		node.AddValue("asteroidCount", asteroidCount);
	}

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete(Planetarium.fetch.Sun.name, "Orbit"))
		{
			return false;
		}
		if (GameVariables.Instance.GetOrbitDisplayMode(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) < GameVariables.OrbitDisplayMode.AllOrbits)
		{
			return false;
		}
		return true;
	}

	public void SetFocusBody(CelestialBody body)
	{
		if (!SentinelUtilities.IsOnSolarOrbit(body))
		{
			innerBody = Planetarium.fetch.Sun;
			outerBody = Planetarium.fetch.Home;
		}
		else
		{
			SentinelUtilities.FindInnerAndOuterBodies(body.orbit.semiMajorAxis - 1.0, out innerBody, out outerBody);
		}
	}
}
