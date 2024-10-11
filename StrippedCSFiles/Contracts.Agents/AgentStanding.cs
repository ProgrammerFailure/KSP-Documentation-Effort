using System.Runtime.CompilerServices;
using UnityEngine;

namespace Contracts.Agents;

public class AgentStanding
{
	[SerializeField]
	private string agentName;

	[SerializeField]
	private Agent parentAgent;

	[SerializeField]
	private Agent agent;

	[SerializeField]
	private float standing;

	public string AgentName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Agent ParentAgent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Agent Agent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float Standing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AgentStanding(Agent parentAgent, string agentName, float standing)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Link()
	{
		throw null;
	}
}
