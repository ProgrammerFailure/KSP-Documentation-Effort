using System;
using System.Collections.Generic;
using Contracts;
using FinePrint;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class DeployedScienceContract : Contract
{
	public class PotentialScienceCombo
	{
		public string experimentId;

		public CelestialBody body;

		public PotentialScienceCombo(string id, CelestialBody body)
		{
			experimentId = id;
			this.body = body;
		}
	}

	[SerializeField]
	public CelestialBody targetBody;

	[SerializeField]
	public string subjectId;

	[SerializeField]
	public float sciencePercentage;

	public float SciencePercentage;

	public string scienceTitle = "";

	public CelestialBody TargetBody => targetBody;

	public string SubjectId => subjectId;

	public override bool MeetRequirements()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return false;
		}
		if (!GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
		{
			return false;
		}
		if (!ProgressTracking.Instance.NodeComplete(FlightGlobals.GetHomeBodyName(), "Orbit"))
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
		return TextGen.GenerateBackStories("DeployedScience" + subjectId, base.Agent.Title, targetBody.displayName, targetBody.displayName + "Srf", base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
	}

	public override string GetSynopsys()
	{
		return Localizer.Format("#autoLOC_272436", targetBody.displayName);
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_8002258", scienceTitle, targetBody.displayName);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_8002259", sciencePercentage.ToString("F2"), scienceTitle, targetBody.displayName);
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("body", ref value))
		{
			targetBody = FlightGlobals.GetBodyByName(value);
		}
		node.TryGetValue("subjectId", ref subjectId);
		if (!string.IsNullOrEmpty(subjectId))
		{
			ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(subjectId);
			if (experiment != null)
			{
				scienceTitle = experiment.experimentTitle;
			}
		}
		node.TryGetValue("sciencePercentage", ref sciencePercentage);
	}

	public override void OnSave(ConfigNode node)
	{
		if (targetBody != null)
		{
			node.AddValue("body", targetBody.name);
		}
		node.AddValue("subjectId", subjectId);
		node.AddValue("sciencePercentage", sciencePercentage);
	}

	public override bool Generate()
	{
		if (ContractSystem.Instance.GetCurrentContracts<DeployedScienceContract>().Length >= ContractDefs.DeployedScience.MaximumExistent)
		{
			return false;
		}
		return GenerateContract_Surface();
	}

	public bool GenerateContract_Surface()
	{
		if (base.Prestige == ContractPrestige.Trivial)
		{
			return false;
		}
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(bodyReached: true, (CelestialBody cb) => cb != Planetarium.fetch.Home && cb != Planetarium.fetch.Sun && cb.hasSolidSurface);
		if (bodiesProgress.Count == 0)
		{
			return false;
		}
		List<AvailablePart> availableDeployedScienceExpParts = PartLoader.Instance.GetAvailableDeployedScienceExpParts();
		if (availableDeployedScienceExpParts.Count == 0)
		{
			return false;
		}
		List<ModuleGroundExperiment> list = new List<ModuleGroundExperiment>();
		for (int i = 0; i < availableDeployedScienceExpParts.Count; i++)
		{
			ModuleGroundExperiment moduleGroundExperiment = availableDeployedScienceExpParts[i].partPrefab.FindModuleImplementing<ModuleGroundExperiment>();
			if (moduleGroundExperiment != null)
			{
				list.Add(moduleGroundExperiment);
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		sciencePercentage = ContractDefs.DeployedScience.SciencePercentage;
		List<PotentialScienceCombo> list2 = new List<PotentialScienceCombo>();
		for (int j = 0; j < list.Count; j++)
		{
			for (int k = 0; k < bodiesProgress.Count; k++)
			{
				ScienceSubject subjectByID = ResearchAndDevelopment.GetSubjectByID(list[j].experimentId + "@" + bodiesProgress[k].name + "SrfLanded");
				ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(list[j].experimentId);
				if ((!experiment.requireAtmosphere || bodiesProgress[k].atmosphere) && (!experiment.requireNoAtmosphere || !bodiesProgress[k].atmosphere))
				{
					if (subjectByID == null)
					{
						list2.Add(new PotentialScienceCombo(list[j].experimentId, bodiesProgress[k]));
					}
					else if (subjectByID.science / subjectByID.scienceCap * 100f < sciencePercentage)
					{
						list2.Add(new PotentialScienceCombo(list[j].experimentId, bodiesProgress[k]));
					}
				}
			}
		}
		if (list2.Count == 0)
		{
			return false;
		}
		int index = new KSPRandom(base.MissionSeed).Next(0, list2.Count - 1);
		subjectId = list2[index].experimentId;
		for (int num = bodiesProgress.Count - 1; num >= 0; num--)
		{
			if (!PotentialsContainBody(bodiesProgress[num], list2, subjectId))
			{
				bodiesProgress.RemoveAt(num);
			}
		}
		targetBody = WeightedBodyChoice(bodiesProgress);
		ScienceExperiment experiment2 = ResearchAndDevelopment.GetExperiment(subjectId);
		if (experiment2 != null)
		{
			scienceTitle = experiment2.experimentTitle;
		}
		AddParameter(new CollectDeployedScience(targetBody, subjectId, scienceTitle, sciencePercentage));
		AddKeywords("Scientific", "Commercial");
		SetExpiry(ContractDefs.DeployedScience.Expire.MinimumExpireDays, ContractDefs.DeployedScience.Expire.MaximumExpireDays);
		SetDeadlineDays(ContractDefs.DeployedScience.Expire.DeadlineDays, targetBody);
		SetFunds(ContractDefs.DeployedScience.Funds.BaseAdvance, ContractDefs.DeployedScience.Funds.BaseReward, ContractDefs.DeployedScience.Funds.BaseFailure, targetBody);
		SetReputation(ContractDefs.DeployedScience.Reputation.BaseReward, ContractDefs.DeployedScience.Reputation.BaseFailure);
		SetScience(ContractDefs.DeployedScience.Science.BaseReward);
		return true;
	}

	public bool PotentialsContainBody(CelestialBody body, List<PotentialScienceCombo> potentialCombos, string id)
	{
		int num = 0;
		while (true)
		{
			if (num < potentialCombos.Count)
			{
				if (potentialCombos[num].experimentId == id && potentialCombos[num].body.name == body.name)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
