using UnityEngine;

namespace Contracts.Agents;

public class AgentStanding
{
	[SerializeField]
	public string agentName;

	[SerializeField]
	public Agent parentAgent;

	[SerializeField]
	public Agent agent;

	[SerializeField]
	public float standing;

	public string AgentName => agentName;

	public Agent ParentAgent => parentAgent;

	public Agent Agent => agent;

	public float Standing => standing;

	public AgentStanding(Agent parentAgent, string agentName, float standing)
	{
		this.parentAgent = parentAgent;
		this.agentName = agentName;
		this.standing = standing;
	}

	public bool Link()
	{
		agent = AgentList.Instance.GetAgent(agentName);
		if (agent == parentAgent)
		{
			agent = null;
		}
		return agent != null;
	}
}
