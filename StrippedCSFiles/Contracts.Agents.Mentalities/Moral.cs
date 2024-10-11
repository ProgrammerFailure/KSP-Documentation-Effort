using System;
using System.Runtime.CompilerServices;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Moral : AgentMentality
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public Moral()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CanProcessContract(Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ProcessContract(Contract contract)
	{
		throw null;
	}
}
