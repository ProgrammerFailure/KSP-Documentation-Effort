using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts.Parameters;
using FinePrint;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Contracts.Templates;

[Serializable]
public class PlantFlag : Contract
{
	[SerializeField]
	public CelestialBody targetBody;

	public CelestialBody TargetBody => targetBody;

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete(FlightGlobals.GetHomeBodyName(), "Orbit"))
		{
			return false;
		}
		if (!GameVariables.Instance.UnlockedEVAFlags(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
		{
			return false;
		}
		return true;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		return new List<CelestialBody> { targetBody };
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, targetBody.displayName, targetBody.displayName + "Srf", base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		return Localizer.Format("#autoLOC_273996", targetBody.displayName);
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_274001", targetBody.displayName);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_274006", targetBody.displayName);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("body"))
		{
			targetBody = FlightGlobals.fetch.bodies[int.Parse(node.GetValue("body"))];
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("body", targetBody.flightGlobalsIndex);
	}

	public override bool Generate()
	{
		if (ContractSystem.Instance.GetCurrentContracts<PlantFlag>().Length >= ContractDefs.Flag.MaximumExistent)
		{
			return false;
		}
		if (base.Prestige == ContractPrestige.Trivial)
		{
			List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(ProgressType.LANDING, progressComplete: true, MannedStatus.MANNED, (CelestialBody cb) => cb != Planetarium.fetch.Home && cb != Planetarium.fetch.Sun && cb.hasSolidSurface);
			Dictionary<CelestialBody, int> dictionary = ProgressUtilities.CelestialCrewCounts(new List<Vessel.Situations>
			{
				Vessel.Situations.LANDED,
				Vessel.Situations.SPLASHED,
				Vessel.Situations.FLYING,
				Vessel.Situations.SUB_ORBITAL
			});
			int count = bodiesProgress.Count;
			while (count-- > 0)
			{
				CelestialBody celestialBody = bodiesProgress[count];
				if (dictionary.ContainsKey(celestialBody) && dictionary[celestialBody] > 0 && ProgressTracking.Instance.NodeComplete(celestialBody.name, "FlagPlant"))
				{
					bodiesProgress.Remove(celestialBody);
				}
			}
			if (bodiesProgress.Count <= 0)
			{
				return false;
			}
			targetBody = WeightedBodyChoice(bodiesProgress);
			AddParameter(new Contracts.Parameters.PlantFlag(targetBody));
			AddKeywords("Commercial");
		}
		else
		{
			if (base.Prestige != ContractPrestige.Significant)
			{
				return false;
			}
			List<CelestialBody> bodiesProgress2 = ProgressUtilities.GetBodiesProgress(ProgressType.LANDING, bodyReached: true, progressComplete: false, MannedStatus.MANNED, (CelestialBody cb) => cb != Planetarium.fetch.Home && cb != Planetarium.fetch.Sun && cb.hasSolidSurface);
			if (bodiesProgress2.Count == 0)
			{
				return false;
			}
			targetBody = WeightedBodyChoice(bodiesProgress2);
			AddParameter(new Contracts.Parameters.PlantFlag(targetBody));
			AddKeywords("Commercial");
			AddKeywords("Pioneer");
		}
		SetExpiry(ContractDefs.Flag.Expire.MinimumExpireDays, ContractDefs.Flag.Expire.MaximumExpireDays);
		SetDeadlineDays(ContractDefs.Flag.Expire.DeadlineDays, targetBody);
		SetFunds(ContractDefs.Flag.Funds.BaseAdvance, ContractDefs.Flag.Funds.BaseReward, ContractDefs.Flag.Funds.BaseFailure, targetBody);
		SetReputation(ContractDefs.Flag.Reputation.BaseReward, ContractDefs.Flag.Reputation.BaseFailure);
		SetScience(ContractDefs.Flag.Science.BaseReward);
		if (ContractSystem.Instance.AnyCurrentContracts((PlantFlag contract) => contract.TargetBody == TargetBody))
		{
			return false;
		}
		return true;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(this).ToString(CultureInfo.InvariantCulture);
	}
}
