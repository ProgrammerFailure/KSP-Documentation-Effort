using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Contracts.Agents;

public class Agent
{
	[SerializeField]
	private string name;

	[SerializeField]
	private string title;

	[SerializeField]
	private string description;

	[SerializeField]
	private string logoURL;

	[SerializeField]
	private Texture2D logo;

	[SerializeField]
	private Texture2D logoScaled;

	[SerializeField]
	private List<AgentMentality> mentality;

	[SerializeField]
	private List<AgentStanding> standings;

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Title
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Description
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string DescriptionRich
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string LogoURL
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Texture2D Logo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Texture2D LogoScaled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<AgentMentality> Mentality
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<AgentStanding> Standings
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Agent(string name, string title, string logoURL, string logoScaledURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanProcessContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasExclusiveKeywords(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int ScoreAgentSuitability(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ProcessContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetMindsetString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Agent LoadAgent(ConfigNode node)
	{
		throw null;
	}
}
