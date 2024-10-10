using System;
using Contracts.Agents;
using ns9;
using UnityEngine;

namespace Contracts.Predicates;

[Serializable]
public class AgentParts : ContractPredicate
{
	public Agent agent;

	public int partCount;

	public Agent Agent => agent;

	public int PartCount => partCount;

	public AgentParts(IContractParameterHost parent)
		: base(parent)
	{
	}

	public AgentParts(IContractParameterHost parent, Agent agent, int partCount)
		: base(parent)
	{
		if (partCount < 0)
		{
			Debug.LogError("Contract predicate: partCount must be >= 0");
		}
		this.agent = agent;
		this.partCount = partCount;
	}

	public override bool Test(Vessel vessel)
	{
		int num = 0;
		while (true)
		{
			if (num < vessel.parts.Count)
			{
				if (vessel.parts[num].partInfo.manufacturer == agent.Name)
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

	public override bool Test(ProtoVessel vessel)
	{
		int num = 0;
		while (true)
		{
			if (num < vessel.protoPartSnapshots.Count)
			{
				if (vessel.protoPartSnapshots[num].partInfo.manufacturer == agent.Name)
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

	public override string GetDescription()
	{
		if (partCount == 0)
		{
			return Localizer.Format("#autoLOC_271045", agent.Title);
		}
		if (partCount == 1)
		{
			return Localizer.Format("#autoLOC_271049", agent.Title);
		}
		return Localizer.Format("Vessel must contain at least <<1>> parts made by <<2>>", partCount.ToString(), agent.Title);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("agent") && node.HasValue("count"))
		{
			agent = AgentList.Instance.GetAgent(node.GetValue("agent"));
			if (agent == null)
			{
				Debug.LogError("Contract Predicate " + GetType().Name + " has an invalid agent name.");
			}
			else
			{
				partCount = int.Parse(node.GetValue("count"));
			}
		}
		else
		{
			Debug.LogError("Contract Predicate " + GetType().Name + " has invalid config.");
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("agent", agent.Name);
		node.AddValue("count", partCount);
	}
}
