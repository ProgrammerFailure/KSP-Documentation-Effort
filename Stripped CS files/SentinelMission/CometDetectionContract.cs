using System.Globalization;
using Contracts;
using Contracts.Parameters;
using FinePrint;
using FinePrint.Contracts.Parameters;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace SentinelMission;

public class CometDetectionContract : Contract, IUpdateWaypoints
{
	public int cometCount = 3;

	public override bool Generate()
	{
		if (!ProgressUtilities.HavePartTech(SentinelUtilities.SentinelPartName, logging: false))
		{
			return false;
		}
		if (prestige == ContractPrestige.Trivial)
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		CometDetectionContract[] currentContracts = ContractSystem.Instance.GetCurrentContracts<CometDetectionContract>();
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
		if (num < ContractDefs.CometDetection.MaximumAvailable && num2 < ContractDefs.CometDetection.MaximumActive)
		{
			switch (prestige)
			{
			case ContractPrestige.Exceptional:
				cometCount = 3;
				break;
			case ContractPrestige.Significant:
				cometCount = 2;
				break;
			}
			ConfigNode configNode = new ConfigNode("PART_REQUEST");
			configNode.AddValue("Article", "a");
			configNode.AddValue("PartDescription", SentinelUtilities.SentinelPartTitle);
			configNode.AddValue("VesselDescription", Localizer.Format("#autoLOC_6002407"));
			configNode.AddValue("Part", SentinelUtilities.SentinelPartName);
			AddParameter(new PartRequestParameter(configNode));
			AddParameter(new EnterOrbit(Planetarium.fetch.Sun));
			AddParameter(new CometDetectionParameter(cometCount, SentinelUtilities.SentinelPartTitle));
			AddKeywords("Scientific");
			SetExpiry(ContractDefs.CometDetection.Expire.MinimumExpireDays, ContractDefs.CometDetection.Expire.MaximumExpireDays);
			SetDeadlineDays(Mathf.RoundToInt(ContractDefs.CometDetection.Expire.DeadlineDays));
			switch (prestige)
			{
			case ContractPrestige.Exceptional:
				SetScience(Mathf.RoundToInt(ContractDefs.CometDetection.Science.BaseReward * ContractDefs.CometDetection.Exceptional.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.CometDetection.Reputation.BaseReward * ContractDefs.CometDetection.Exceptional.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.CometDetection.Reputation.BaseFailure * ContractDefs.CometDetection.Exceptional.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.CometDetection.Funds.BaseAdvance * ContractDefs.CometDetection.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometDetection.Funds.BaseReward * ContractDefs.CometDetection.Exceptional.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometDetection.Funds.BaseFailure * ContractDefs.CometDetection.Exceptional.FundsMultiplier));
				break;
			case ContractPrestige.Significant:
				SetScience(Mathf.RoundToInt(ContractDefs.CometDetection.Science.BaseReward * ContractDefs.CometDetection.Significant.ScienceMultiplier));
				SetReputation(Mathf.RoundToInt(ContractDefs.CometDetection.Reputation.BaseReward * ContractDefs.CometDetection.Significant.ReputationMultiplier), Mathf.RoundToInt(ContractDefs.CometDetection.Reputation.BaseFailure * ContractDefs.CometDetection.Significant.ReputationMultiplier));
				SetFunds(Mathf.RoundToInt(ContractDefs.CometDetection.Funds.BaseAdvance * ContractDefs.CometDetection.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometDetection.Funds.BaseReward * ContractDefs.CometDetection.Significant.FundsMultiplier), Mathf.RoundToInt(ContractDefs.CometDetection.Funds.BaseFailure * ContractDefs.CometDetection.Significant.FundsMultiplier));
				break;
			}
			return true;
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
		return Localizer.Format("#autoLOC_6011148", cometCount.ToString(), SentinelUtilities.SentinelPartTitle);
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories("CometDetection", base.Agent.Title, Localizer.Format("#autoLOC_6006050"), Planetarium.fetch.Sun.displayName, base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
	}

	public override string GetSynopsys()
	{
		return Localizer.Format("#autoLOC_6011146", cometCount.ToString(), SentinelUtilities.SentinelPartTitle);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_6011145", SentinelUtilities.SentinelPartTitle);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "CometContract", "cometCount", ref cometCount, 3);
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("cometCount", cometCount);
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
}
