using System;
using System.Collections.Generic;
using Contracts;
using FinePrint;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class ROCScienceRetrievalContract : Contract
{
	public class PotentialScienceCombo
	{
		public string body;

		public ROCDefinition rocDef;

		public string id;

		public PotentialScienceCombo(string body, ROCDefinition roc, string id)
		{
			this.body = body;
			rocDef = roc;
			this.id = id;
		}
	}

	[SerializeField]
	public CelestialBody targetBody;

	[SerializeField]
	public string subjectId;

	public string rocType = string.Empty;

	public string scienceTitle = string.Empty;

	public List<string> biomes = new List<string>();

	public CelestialBody TargetBody => targetBody;

	public string SubjectId => subjectId;

	public override bool MeetRequirements()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return false;
		}
		if (!(ROCManager.Instance == null) && ROCManager.Instance.RocsEnabledInCurrentGame)
		{
			if (!GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		return new List<CelestialBody> { targetBody };
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories("ROCScienceRetrieval", base.Agent.Title, scienceTitle, scienceTitle, base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
	}

	public override string GetSynopsys()
	{
		string text = string.Empty;
		if (biomes.Count == 1)
		{
			text = Localizer.Format("#autoLOC_8004435", biomes[0]);
		}
		else if (biomes.Count == 2)
		{
			text = Localizer.Format("#autoLOC_8004436", biomes[0], biomes[1]);
		}
		else if (biomes.Count >= 3)
		{
			text = Localizer.Format("#autoLOC_8004437", biomes[0], biomes[1], biomes[2]);
		}
		return Localizer.Format("#autoLOC_8004421", scienceTitle) + text;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_8004396", scienceTitle);
	}

	public override string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_8004424", scienceTitle, targetBody.displayName);
	}

	public override void OnLoad(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("body", ref value))
		{
			targetBody = FlightGlobals.GetBodyByName(value);
		}
		node.TryGetValue("subjectId", ref subjectId);
		node.TryGetValue("scienceTitle", ref scienceTitle);
		node.TryGetValue("rocType", ref rocType);
		for (int i = 0; i < ROCManager.Instance.rocDefinitions.Count; i++)
		{
			ROCDefinition rOCDefinition = ROCManager.Instance.rocDefinitions[i];
			if (rocType == rOCDefinition.type)
			{
				scienceTitle = rOCDefinition.displayName;
				break;
			}
		}
		biomes = node.GetValuesList("biomes");
		if (biomes == null)
		{
			biomes = new List<string>();
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (targetBody != null)
		{
			node.AddValue("body", targetBody.name);
		}
		node.AddValue("subjectId", subjectId);
		node.AddValue("scienceTitle", scienceTitle);
		node.AddValue("rocType", rocType);
		for (int i = 0; i < biomes.Count; i++)
		{
			node.AddValue("biomes", biomes[i]);
		}
	}

	public override bool Generate()
	{
		if (ContractSystem.Instance.GetCurrentContracts<ROCScienceRetrievalContract>().Length >= ContractDefs.ROCScienceRetrieval.MaximumExistent)
		{
			return false;
		}
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(bodyReached: true, (CelestialBody cb) => cb != Planetarium.fetch.Sun && cb.hasSolidSurface);
		if (bodiesProgress.Count == 0)
		{
			return false;
		}
		List<PotentialScienceCombo> list = new List<PotentialScienceCombo>();
		for (int i = 0; i < bodiesProgress.Count; i++)
		{
			for (int j = 0; j < ROCManager.Instance.rocDefinitions.Count; j++)
			{
				ROCDefinition rOCDefinition = ROCManager.Instance.rocDefinitions[j];
				for (int k = 0; k < rOCDefinition.myCelestialBodies.Count; k++)
				{
					if (bodiesProgress[i].name == rOCDefinition.myCelestialBodies[k].name && rOCDefinition.canBeTaken)
					{
						string text = "ROCScience_" + rOCDefinition.type + "@" + rOCDefinition.myCelestialBodies[k].name + "SrfLanded";
						ScienceSubject subjectByID = ResearchAndDevelopment.GetSubjectByID(text);
						if ((subjectByID != null && subjectByID.science / subjectByID.scienceCap < 1f) || subjectByID == null)
						{
							list.Add(new PotentialScienceCombo(bodiesProgress[i].name, rOCDefinition, text));
						}
					}
				}
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
		int index = kSPRandom.Next(0, list.Count - 1);
		PotentialScienceCombo potentialScienceCombo = list[index];
		subjectId = potentialScienceCombo.id;
		rocType = potentialScienceCombo.rocDef.type;
		targetBody = FlightGlobals.GetBodyByName(potentialScienceCombo.body);
		scienceTitle = potentialScienceCombo.rocDef.displayName;
		RocCBDefinition rocCBDefinition = null;
		for (int l = 0; l < potentialScienceCombo.rocDef.myCelestialBodies.Count; l++)
		{
			if (potentialScienceCombo.rocDef.myCelestialBodies[l].name == potentialScienceCombo.body)
			{
				rocCBDefinition = potentialScienceCombo.rocDef.myCelestialBodies[l];
				break;
			}
		}
		if (rocCBDefinition == null)
		{
			return false;
		}
		int num = 1;
		if (rocCBDefinition.biomes.Count >= 5)
		{
			num = 3;
		}
		else if (rocCBDefinition.biomes.Count >= 3)
		{
			num = 2;
		}
		for (int m = 0; m < 10; m++)
		{
			if (biomes.Count >= num)
			{
				break;
			}
			int index2 = kSPRandom.Next(0, rocCBDefinition.biomes.Count - 1);
			if (!biomes.Contains(rocCBDefinition.biomes[index2]))
			{
				biomes.Add(rocCBDefinition.biomes[index2]);
			}
		}
		for (int n = 0; n < biomes.Count; n++)
		{
			biomes[n] = ScienceUtil.GetBiomedisplayName(targetBody, biomes[n], formatted: false);
		}
		AddParameter(new CollectROCScienceRetrieval(targetBody, subjectId, rocType, scienceTitle));
		AddKeywords("Scientific", "Commercial");
		SetExpiry(ContractDefs.ROCScienceRetrieval.Expire.MinimumExpireDays, ContractDefs.ROCScienceRetrieval.Expire.MaximumExpireDays);
		SetDeadlineDays(ContractDefs.ROCScienceRetrieval.Expire.DeadlineDays, targetBody);
		SetFunds(ContractDefs.ROCScienceRetrieval.Funds.Advance, ContractDefs.ROCScienceRetrieval.Funds.Reward, ContractDefs.ROCScienceRetrieval.Funds.Failure, targetBody);
		SetReputation(ContractDefs.ROCScienceRetrieval.Reputation.Reward, ContractDefs.ROCScienceRetrieval.Reputation.Failure);
		SetScience(ContractDefs.ROCScienceRetrieval.Science.Reward);
		return true;
	}
}
