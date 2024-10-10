using System;
using System.Collections.Generic;
using Contracts;
using ns11;
using ns9;

namespace Expansions.Serenity.Contracts;

[Serializable]
public class CollectROCScienceRetrieval : ContractParameter
{
	public CelestialBody targetBody;

	public string subjectId = string.Empty;

	public string rocType = string.Empty;

	public string scienceTitle = string.Empty;

	public CelestialBody TargetBody => targetBody;

	public string SubjectId => subjectId;

	public string ScienceTitle => scienceTitle;

	public CollectROCScienceRetrieval()
	{
	}

	public CollectROCScienceRetrieval(CelestialBody targetBody, string subjectId, string rocType, string scienceTitle)
	{
		this.targetBody = targetBody;
		this.subjectId = subjectId;
		this.rocType = rocType;
		this.scienceTitle = scienceTitle;
	}

	public override string GetHashString()
	{
		return SubjectId + " " + targetBody.name;
	}

	public override string GetTitle()
	{
		if (base.Root.ContractState == Contract.State.Completed)
		{
			return Localizer.Format("#autoLOC_8004398", scienceTitle);
		}
		if (base.Root.ContractState != Contract.State.Cancelled && base.Root.ContractState != Contract.State.DeadlineExpired && base.Root.ContractState != Contract.State.Failed)
		{
			return Localizer.Format("#autoLOC_8004397", scienceTitle, targetBody.displayName);
		}
		return Localizer.Format("#autoLOC_8004399", scienceTitle);
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
		node.AddValue("scienceTitle", scienceTitle);
		node.AddValue("rocType", rocType);
	}

	public override void OnRegister()
	{
		GameEvents.onVesselRecoveryProcessing.Add(OnVesselRecoveryProcessing);
	}

	public override void OnUnregister()
	{
		GameEvents.onVesselRecoveryProcessing.Remove(OnVesselRecoveryProcessing);
	}

	public void OnVesselRecoveryProcessing(ProtoVessel pv, MissionRecoveryDialog mrDialog, float recoveryScore)
	{
		if (pv == null)
		{
			return;
		}
		List<ProtoPartSnapshot> allProtoPartsIncludingCargo = pv.GetAllProtoPartsIncludingCargo();
		for (int i = 0; i < allProtoPartsIncludingCargo.Count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = allProtoPartsIncludingCargo[i];
			for (int j = 0; j < protoPartSnapshot.modules.Count; j++)
			{
				ProtoPartModuleSnapshot protoPartModuleSnapshot = protoPartSnapshot.modules[j];
				ConfigNode moduleValues = protoPartModuleSnapshot.moduleValues;
				if ((!(protoPartModuleSnapshot.moduleName == "ModuleScienceContainer") && (!(protoPartSnapshot.partName == "kerbalEVA") || !(protoPartModuleSnapshot.moduleName == "ModuleScienceExperiment"))) || !moduleValues.HasNode("ScienceData"))
				{
					continue;
				}
				ConfigNode[] nodes = moduleValues.GetNodes("ScienceData");
				int k = 0;
				for (int num = nodes.Length; k < num; k++)
				{
					string value = nodes[k].GetValue("subjectID");
					if (subjectId == value)
					{
						SetComplete();
					}
				}
			}
		}
	}
}
