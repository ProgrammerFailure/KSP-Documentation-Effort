using System;
using Contracts;
using ns9;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class CollectROCScienceArm : ContractParameter
{
	public CelestialBody targetBody;

	public string subjectId = string.Empty;

	public float sciencePercentage;

	public float SciencePercentage;

	public float scienceCollected;

	public float ScienceCollected;

	public string rocType = string.Empty;

	public string scienceTitle = string.Empty;

	public CelestialBody TargetBody => targetBody;

	public string SubjectId => subjectId;

	public string ScienceTitle => scienceTitle;

	public CollectROCScienceArm()
	{
	}

	public CollectROCScienceArm(CelestialBody targetBody, string subjectId, string rocType, string scienceTitle, float sciencePercentage)
	{
		this.targetBody = targetBody;
		this.subjectId = subjectId;
		this.rocType = rocType;
		this.scienceTitle = scienceTitle;
		this.sciencePercentage = sciencePercentage;
		scienceCollected = 0f;
	}

	public override string GetHashString()
	{
		return SubjectId + " " + targetBody.name;
	}

	public override string GetTitle()
	{
		if (base.Root.ContractState != Contract.State.Active && base.Root.ContractState != Contract.State.Cancelled && base.Root.ContractState != Contract.State.Completed && base.Root.ContractState != Contract.State.DeadlineExpired && base.Root.ContractState != Contract.State.Failed)
		{
			return Localizer.Format("#autoLOC_8004394", sciencePercentage.ToString("N0"), scienceTitle, targetBody.displayName);
		}
		return Localizer.Format("#autoLOC_8004395", scienceCollected.ToString("N0"), sciencePercentage.ToString("N0"), scienceTitle, targetBody.displayName);
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
		node.TryGetValue("scienceCollected", ref scienceCollected);
		node.TryGetValue("scienceTitle", ref scienceTitle);
		node.TryGetValue("rocType", ref rocType);
		int num = 0;
		ROCDefinition rOCDefinition;
		while (true)
		{
			if (num < ROCManager.Instance.rocDefinitions.Count)
			{
				rOCDefinition = ROCManager.Instance.rocDefinitions[num];
				if (rocType == rOCDefinition.type)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		scienceTitle = rOCDefinition.displayName;
	}

	public override void OnSave(ConfigNode node)
	{
		if (targetBody != null)
		{
			node.AddValue("body", targetBody.name);
		}
		node.AddValue("subjectId", subjectId);
		node.AddValue("sciencePercentage", sciencePercentage);
		node.AddValue("scienceCollected", scienceCollected);
		node.AddValue("rocType", rocType);
		node.AddValue("scienceTitle", scienceTitle);
	}

	public override void OnRegister()
	{
		GameEvents.OnScienceRecieved.Add(OnScience);
	}

	public override void OnUnregister()
	{
		GameEvents.OnScienceRecieved.Remove(OnScience);
	}

	public void OnScience(float science, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
	{
		if (subject.id == subjectId)
		{
			scienceCollected = subject.science / subject.scienceCap * 100f;
			if (subject.IsFromBody(targetBody) && subject.IsFromSituation(ExperimentSituations.SrfLanded) && subject.science / subject.scienceCap >= sciencePercentage * 0.95f / 100f)
			{
				SetComplete();
			}
			else
			{
				GameEvents.Contract.onParameterChange.Fire(base.Root, this);
			}
		}
	}
}
