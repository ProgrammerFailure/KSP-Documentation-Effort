using System;
using System.Runtime.CompilerServices;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class NoRandomContractMentality : AgentMentality
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public NoRandomContractMentality()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanProcessContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override KeywordScore ScoreKeyword(string keyword)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAdded()
	{
		throw null;
	}
}
