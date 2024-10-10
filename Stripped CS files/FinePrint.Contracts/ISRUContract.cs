using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace FinePrint.Contracts;

public class ISRUContract : Contract
{
	public CelestialBody targetBody;

	public CelestialBody deliveryBody;

	public Vessel.Situations deliverySituation = Vessel.Situations.ORBITING;

	public bool isDelivering;

	public float gatherGoal = 1000f;

	public string targetResource = "Matter";

	public string resourceTitle = "matter";

	public override bool Generate()
	{
		if (ContractSystem.Instance.GetCurrentContracts<ISRUContract>().Length >= ContractDefs.ISRU.MaximumExistent)
		{
			return false;
		}
		List<ConfigNode> list = PossibleResources();
		if (list.Count == 0)
		{
			return false;
		}
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		ConfigNode configNode = list[kSPRandom.Next(0, list.Count)];
		targetResource = "Matter";
		resourceTitle = "matter";
		SystemUtilities.LoadNode(configNode, "ISRUContract", "Name", ref targetResource, "Matter", logging: false);
		string defaultValue = ((!(PartResourceLibrary.Instance != null) || !PartResourceLibrary.Instance.resourceDefinitions.Contains(targetResource)) ? targetResource : PartResourceLibrary.Instance.GetDefinition(targetResource).displayName);
		SystemUtilities.LoadNode(configNode, "ISRUContract", "Title", ref resourceTitle, defaultValue, logging: false);
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(bodyReached: true, (CelestialBody cb) => cb != Planetarium.fetch.Home);
		if (base.Prestige == ContractPrestige.Significant)
		{
			bodiesProgress.AddRange(ProgressUtilities.GetNextUnreached(1, (CelestialBody cb) => cb != Planetarium.fetch.Home));
		}
		else if (base.Prestige == ContractPrestige.Exceptional)
		{
			bodiesProgress.AddRange(ProgressUtilities.GetNextUnreached(2, (CelestialBody cb) => cb != Planetarium.fetch.Home));
		}
		if (bodiesProgress.Count == 0)
		{
			return false;
		}
		while (true)
		{
			targetBody = WeightedBodyChoice(bodiesProgress, kSPRandom);
			if (!CelestialIsForbidden(targetBody, configNode))
			{
				break;
			}
			bodiesProgress.Remove(targetBody);
			if (bodiesProgress.Count == 0)
			{
				return false;
			}
		}
		float value = 1f;
		float value2 = 1f;
		float value3 = 1f;
		int value4 = 0;
		switch (prestige)
		{
		case ContractPrestige.Trivial:
		{
			gatherGoal = 0f;
			ConfigNode node3 = configNode.GetNode("Trivial");
			SystemUtilities.LoadNode(node3, "ISRUContract", "Amount", ref gatherGoal, 500f, logging: false);
			SystemUtilities.LoadNode(node3, "ISRUContract", "DeliveryChance", ref value4, 50, logging: false);
			SystemUtilities.LoadNode(node3, "ISRUContract", "FundsMultiplier", ref value, 1f, logging: false);
			SystemUtilities.LoadNode(node3, "ISRUContract", "ScienceMultiplier", ref value2, 1f, logging: false);
			SystemUtilities.LoadNode(node3, "ISRUContract", "ReputationMultiplier", ref value3, 1f, logging: false);
			break;
		}
		case ContractPrestige.Significant:
		{
			gatherGoal = 0f;
			ConfigNode node2 = configNode.GetNode("Significant");
			SystemUtilities.LoadNode(node2, "ISRUContract", "Amount", ref gatherGoal, 1000f, logging: false);
			SystemUtilities.LoadNode(node2, "ISRUContract", "DeliveryChance", ref value4, 65, logging: false);
			SystemUtilities.LoadNode(node2, "ISRUContract", "FundsMultiplier", ref value, 1.1f, logging: false);
			SystemUtilities.LoadNode(node2, "ISRUContract", "ScienceMultiplier", ref value2, 1.1f, logging: false);
			SystemUtilities.LoadNode(node2, "ISRUContract", "ReputationMultiplier", ref value3, 1.1f, logging: false);
			break;
		}
		case ContractPrestige.Exceptional:
		{
			gatherGoal = 0f;
			ConfigNode node = configNode.GetNode("Exceptional");
			SystemUtilities.LoadNode(node, "ISRUContract", "Amount", ref gatherGoal, 2500f, logging: false);
			SystemUtilities.LoadNode(node, "ISRUContract", "DeliveryChance", ref value4, 80, logging: false);
			SystemUtilities.LoadNode(node, "ISRUContract", "FundsMultiplier", ref value, 1.3f, logging: false);
			SystemUtilities.LoadNode(node, "ISRUContract", "ScienceMultiplier", ref value2, 1.3f, logging: false);
			SystemUtilities.LoadNode(node, "ISRUContract", "ReputationMultiplier", ref value3, 1.3f, logging: false);
			break;
		}
		}
		if (kSPRandom.Next(0, 100) < value4)
		{
			deliveryBody = WeightedBodyChoice(CelestialUtilities.GetNeighbors(targetBody, (CelestialBody cb) => cb != Planetarium.fetch.Sun), kSPRandom);
			isDelivering = true;
		}
		if (deliveryBody == null)
		{
			isDelivering = false;
		}
		if (isDelivering)
		{
			float value5 = 1.8f;
			SystemUtilities.LoadNode(configNode, "ISRUContract", "DeliveryMultiplier", ref value5, 1.8f, logging: false);
			value *= value5;
			value2 *= value5;
			value3 *= value5;
		}
		float num = (float)kSPRandom.NextDouble() * 0.25f;
		num = ((kSPRandom.Next(0, 100) <= 50) ? (1f - num) : (1f + num));
		value *= num;
		value2 *= num;
		value3 *= num;
		gatherGoal *= num;
		gatherGoal = Mathf.Round(gatherGoal / 50f) * 50f;
		AddParameter(new ResourceExtractionParameter(targetResource, resourceTitle, gatherGoal, targetBody, new List<string>(configNode.GetValues("Module"))));
		if (isDelivering)
		{
			deliverySituation = CelestialUtilities.ApplicableSituation(base.MissionSeed, deliveryBody, splashAllowed: false);
			AddParameter(new ResourcePossessionParameter(PartResourceLibrary.Instance.GetDefinition(targetResource).name, resourceTitle, Localizer.Format("#autoLOC_7001016"), gatherGoal));
			AddParameter(new LocationAndSituationParameter(deliveryBody, deliverySituation, PartResourceLibrary.Instance.GetDefinition(targetResource).displayName));
			AddParameter(new StabilityParameter(10f));
		}
		string value6 = "ISRU";
		SystemUtilities.LoadNode(configNode, "ISRUContract", "Keyword", ref value6, "ISRU", logging: false);
		AddKeywords(value6);
		SetExpiry(ContractDefs.ISRU.Expire.MinimumExpireDays, ContractDefs.ISRU.Expire.MaximumExpireDays);
		SetDeadlineDays(ContractDefs.ISRU.Expire.DeadlineDays, targetBody);
		SetFunds(Mathf.Round(ContractDefs.ISRU.Funds.BaseAdvance * value), Mathf.Round(ContractDefs.ISRU.Funds.BaseReward * value), Mathf.Round(ContractDefs.ISRU.Funds.BaseFailure * value), targetBody);
		SetScience(Mathf.Round(ContractDefs.ISRU.Science.BaseReward * value2));
		SetReputation(Mathf.Round(ContractDefs.ISRU.Reputation.BaseReward * value3), Mathf.Round(ContractDefs.ISRU.Reputation.BaseFailure * value3));
		if (ContractSystem.Instance.AnyCurrentContracts((ISRUContract contract) => contract.targetBody == targetBody))
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
		if (isDelivering)
		{
			return Localizer.Format("#autoLOC_279475", resourceTitle, targetBody.displayName, deliveryBody.displayName);
		}
		return Localizer.Format("#autoLOC_279477", Convert.ToDecimal(Mathf.Round(gatherGoal)).ToString("#,###"), resourceTitle, targetBody.displayName);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, Localizer.Format(resourceTitle).ToLower(), targetBody.displayName + "Srf", base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (isDelivering)
		{
			return Localizer.Format("#autoLOC_279488", resourceTitle, targetBody.displayName, Convert.ToDecimal(Mathf.Round(gatherGoal)).ToString("#,###"), deliveryBody.displayName);
		}
		return Localizer.Format("#autoLOC_279490", resourceTitle, targetBody.displayName, Convert.ToDecimal(Mathf.Round(gatherGoal)).ToString("#,###"));
	}

	public override string MessageCompleted()
	{
		if (isDelivering)
		{
			return Localizer.Format("#autoLOC_279496", Convert.ToDecimal(Mathf.Round(gatherGoal)).ToString("#,###"), resourceTitle, deliveryBody.displayName, base.Agent.Title);
		}
		return Localizer.Format("#autoLOC_279498", Convert.ToDecimal(Mathf.Round(gatherGoal)).ToString("#,###"), resourceTitle, base.Agent.Title);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("gatherGoal", gatherGoal);
		node.AddValue("targetResource", targetResource);
		node.AddValue("resourceTitle", resourceTitle);
		node.AddValue("isDelivering", isDelivering);
		if (isDelivering)
		{
			node.AddValue("deliveryBody", deliveryBody.flightGlobalsIndex);
			node.AddValue("deliverySituation", deliverySituation);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "ISRUContract", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "ISRUContract", "gatherGoal", ref gatherGoal, 1f);
		SystemUtilities.LoadNode(node, "ISRUContract", "targetResource", ref targetResource, "Matter");
		SystemUtilities.LoadNode(node, "ISRUContract", "resourceTitle", ref resourceTitle, "matter");
		SystemUtilities.LoadNode(node, "ISRUContract", "isDelivering", ref isDelivering, defaultValue: false);
		if (isDelivering)
		{
			SystemUtilities.LoadNode(node, "ISRUContract", "deliveryBody", ref deliveryBody, Planetarium.fetch.Home);
			SystemUtilities.LoadNode(node, "ISRUContract", "deliverySituation", ref deliverySituation, Vessel.Situations.ORBITING);
		}
		ResourcePossessionParameter resourcePossessionParameter = (ResourcePossessionParameter)GetParameter(typeof(ResourcePossessionParameter));
		if (resourcePossessionParameter != null && !PartResourceLibrary.Instance.resourceDefinitions.Contains(resourcePossessionParameter.resourceName))
		{
			resourcePossessionParameter.resourceName = targetResource;
		}
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
		List<CelestialBody> list = new List<CelestialBody> { targetBody };
		if (isDelivering)
		{
			list.Add(deliveryBody);
		}
		return list;
	}

	public List<ConfigNode> PossibleResources()
	{
		List<ConfigNode> list = new List<ConfigNode>();
		ConfigNode node = ContractDefs.config.GetNode("ISRU");
		if (node == null)
		{
			return list;
		}
		ConfigNode[] nodes = node.GetNodes("RESOURCE_REQUEST");
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ConfigNode configNode = nodes[i];
			if (ProgressUtilities.HaveAnyTech(null, configNode.GetValues("Module"), logging: false))
			{
				list.Add(configNode);
			}
		}
		return list;
	}

	public static bool CelestialIsForbidden(CelestialBody body, ConfigNode resourceRequest)
	{
		if (!(body == null) && resourceRequest != null)
		{
			string[] values = resourceRequest.GetValues("Forbidden");
			int num = values.Length;
			do
			{
				if (num-- <= 0)
				{
					return false;
				}
			}
			while (!(body.GetName() == values[num]));
			return true;
		}
		return true;
	}
}
