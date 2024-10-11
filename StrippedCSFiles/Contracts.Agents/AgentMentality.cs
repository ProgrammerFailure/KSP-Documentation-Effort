using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Contracts.Agents;

public class AgentMentality
{
	private static List<Type> mentalityTypes;

	[SerializeField]
	private Agent agent;

	[SerializeField]
	private float factor;

	public Agent Agent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float Factor
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

	public string DisplayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AgentMentality()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AgentMentality()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void GenerateMentalityTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Type GetMentalityType(string typeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual KeywordScore ScoreKeyword(string keyword)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CanProcessContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ProcessContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnAdded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AgentMentality AddMentality(Agent agent, string mentalityName, float factor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorFundsAdvance(Contract contract, bool positive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorFundsCompletion(Contract contract, bool positive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorFundsFailure(Contract contract, bool positive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorScienceCompletion(Contract contract, bool positive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorReputationCompletion(Contract contract, bool positive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorReputationFailure(Contract contract, bool positive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorDeadline(Contract contract, bool positive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FactorExpiry(Contract contract, bool positive)
	{
		throw null;
	}
}
