using System;
using System.Collections.Generic;
using Contracts;
using FinePrint;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class ROCScienceArmContract : Contract
{
	public class PotentialScienceCombo
	{
		public ROCDefinition rocDef;

		public string body;

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

	[SerializeField]
	public float sciencePercentage;

	public string rocType = string.Empty;

	public string scienceTitle = string.Empty;

	public string scannerArmName = string.Empty;

	public string scannerArmTitle = string.Empty;

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
		string contractType = "ROCAdvancedArmScience";
		if (sciencePercentage < ContractDefs.ROCScienceArm.SimpleArmPercentage)
		{
			contractType = "ROCSimpleArmScience";
		}
		else if (sciencePercentage < ContractDefs.ROCScienceArm.ComplexArmPercentage)
		{
			contractType = "ROCComplexArmScience";
		}
		return TextGen.GenerateBackStories(contractType, base.Agent.Title, scienceTitle, scienceTitle, base.MissionSeed, allowGenericIntroduction: false, allowGenericProblem: false, allowGenericConclusion: false);
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
		return Localizer.Format("#autoLOC_8004414", scienceTitle, scannerArmTitle) + text;
	}

	public override string GetTitle()
	{
		if (sciencePercentage < ContractDefs.ROCScienceArm.SimpleArmPercentage)
		{
			return Localizer.Format("#autoLOC_8004391", scienceTitle);
		}
		if (sciencePercentage < ContractDefs.ROCScienceArm.ComplexArmPercentage)
		{
			return Localizer.Format("#autoLOC_8004392", scienceTitle);
		}
		return Localizer.Format("#autoLOC_8004393", scienceTitle);
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
		node.TryGetValue("sciencePercentage", ref sciencePercentage);
		node.TryGetValue("scannerArmName", ref scannerArmName);
		node.TryGetValue("scannerArmTitle", ref scannerArmTitle);
		AvailablePart partInfoByName = PartLoader.getPartInfoByName(scannerArmName);
		if (partInfoByName != null)
		{
			scannerArmTitle = partInfoByName.title;
		}
		node.TryGetValue("rocType", ref rocType);
		node.TryGetValue("scienceTitle", ref scienceTitle);
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
		node.AddValue("scannerArmName", scannerArmName);
		node.AddValue("scannerArmTitle", scannerArmTitle);
		node.AddValue("sciencePercentage", sciencePercentage);
		for (int i = 0; i < biomes.Count; i++)
		{
			node.AddValue("biomes", biomes[i]);
		}
	}

	public override bool Generate()
	{
		if (ContractSystem.Instance.GetCurrentContracts<ROCScienceArmContract>().Length >= ContractDefs.ROCScienceArm.MaximumExistent)
		{
			return false;
		}
		List<CelestialBody> bodiesProgress = ProgressUtilities.GetBodiesProgress(bodyReached: true, (CelestialBody cb) => cb != Planetarium.fetch.Sun && cb.hasSolidSurface);
		if (bodiesProgress.Count == 0)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		List<AvailablePart> availableRobotArmScannerParts = PartLoader.Instance.GetAvailableRobotArmScannerParts();
		AvailablePart availablePart = null;
		sciencePercentage = -1f;
		for (int i = 0; i < availableRobotArmScannerParts.Count; i++)
		{
			ModuleScienceExperiment moduleScienceExperiment = availableRobotArmScannerParts[i].partPrefab.FindModuleImplementing<ModuleScienceExperiment>();
			if (moduleScienceExperiment != null && moduleScienceExperiment.scienceValueRatio > sciencePercentage)
			{
				availablePart = availableRobotArmScannerParts[i];
				sciencePercentage = moduleScienceExperiment.scienceValueRatio;
			}
		}
		if (availablePart == null)
		{
			return false;
		}
		float num7 = sciencePercentage * 0.95f;
		sciencePercentage *= 100f;
		scannerArmName = availablePart.name;
		scannerArmTitle = availablePart.title;
		if (sciencePercentage < ContractDefs.ROCScienceArm.SimpleArmPercentage)
		{
			num = ContractDefs.ROCScienceArm.Funds.SimpleArmAdvance;
			num2 = ContractDefs.ROCScienceArm.Funds.SimpleArmReward;
			num3 = ContractDefs.ROCScienceArm.Funds.SimpleArmFailure;
			num4 = ContractDefs.ROCScienceArm.Reputation.SimpleArmReward;
			num5 = ContractDefs.ROCScienceArm.Reputation.SimpleArmFailure;
			num6 = ContractDefs.ROCScienceArm.Science.SimpleArmReward;
		}
		else if (sciencePercentage < ContractDefs.ROCScienceArm.ComplexArmPercentage)
		{
			num = ContractDefs.ROCScienceArm.Funds.ComplexArmAdvance;
			num2 = ContractDefs.ROCScienceArm.Funds.ComplexArmReward;
			num3 = ContractDefs.ROCScienceArm.Funds.ComplexArmFailure;
			num4 = ContractDefs.ROCScienceArm.Reputation.ComplexArmReward;
			num5 = ContractDefs.ROCScienceArm.Reputation.ComplexArmFailure;
			num6 = ContractDefs.ROCScienceArm.Science.ComplexArmReward;
		}
		else
		{
			num = ContractDefs.ROCScienceArm.Funds.AdvancedArmAdvance;
			num2 = ContractDefs.ROCScienceArm.Funds.AdvancedArmReward;
			num3 = ContractDefs.ROCScienceArm.Funds.AdvancedArmFailure;
			num4 = ContractDefs.ROCScienceArm.Reputation.AdvancedArmReward;
			num5 = ContractDefs.ROCScienceArm.Reputation.AdvancedArmFailure;
			num6 = ContractDefs.ROCScienceArm.Science.AdvancedArmReward;
		}
		List<PotentialScienceCombo> list = new List<PotentialScienceCombo>();
		for (int j = 0; j < bodiesProgress.Count; j++)
		{
			for (int k = 0; k < ROCManager.Instance.rocDefinitions.Count; k++)
			{
				ROCDefinition rOCDefinition = ROCManager.Instance.rocDefinitions[k];
				for (int l = 0; l < rOCDefinition.myCelestialBodies.Count; l++)
				{
					if (bodiesProgress[j].name == rOCDefinition.myCelestialBodies[l].name)
					{
						string text = "ROCScience_" + rOCDefinition.type + "@" + rOCDefinition.myCelestialBodies[l].name + "SrfLanded";
						ScienceSubject subjectByID = ResearchAndDevelopment.GetSubjectByID(text);
						if ((subjectByID != null && subjectByID.science / subjectByID.scienceCap < num7) || subjectByID == null)
						{
							list.Add(new PotentialScienceCombo(bodiesProgress[j].name, rOCDefinition, text));
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
		targetBody = FlightGlobals.GetBodyByName(potentialScienceCombo.body);
		rocType = potentialScienceCombo.rocDef.type;
		scienceTitle = potentialScienceCombo.rocDef.displayName;
		RocCBDefinition rocCBDefinition = null;
		for (int m = 0; m < potentialScienceCombo.rocDef.myCelestialBodies.Count; m++)
		{
			if (potentialScienceCombo.rocDef.myCelestialBodies[m].name == potentialScienceCombo.body)
			{
				rocCBDefinition = potentialScienceCombo.rocDef.myCelestialBodies[m];
				break;
			}
		}
		if (rocCBDefinition == null)
		{
			return false;
		}
		int num8 = 1;
		if (rocCBDefinition.biomes.Count >= 5)
		{
			num8 = 3;
		}
		else if (rocCBDefinition.biomes.Count >= 3)
		{
			num8 = 2;
		}
		for (int n = 0; n < 10; n++)
		{
			if (biomes.Count >= num8)
			{
				break;
			}
			int index2 = kSPRandom.Next(0, rocCBDefinition.biomes.Count - 1);
			if (!biomes.Contains(rocCBDefinition.biomes[index2]))
			{
				biomes.Add(rocCBDefinition.biomes[index2]);
			}
		}
		for (int num9 = 0; num9 < biomes.Count; num9++)
		{
			biomes[num9] = ScienceUtil.GetBiomedisplayName(targetBody, biomes[num9], formatted: false);
		}
		AddParameter(new CollectROCScienceArm(targetBody, subjectId, rocType, scienceTitle, sciencePercentage));
		AddKeywords("Scientific", "Commercial");
		SetExpiry(ContractDefs.ROCScienceArm.Expire.MinimumExpireDays, ContractDefs.ROCScienceArm.Expire.MaximumExpireDays);
		SetDeadlineDays(ContractDefs.ROCScienceArm.Expire.DeadlineDays, targetBody);
		SetFunds(num, num2, num3, targetBody);
		SetReputation(num4, num5);
		SetScience(num6);
		return true;
	}
}
