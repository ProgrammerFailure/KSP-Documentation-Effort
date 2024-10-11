using System;
using System.Runtime.CompilerServices;

namespace Contracts.Predicates;

[Serializable]
public class IsActiveVessel : ContractPredicate
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public IsActiveVessel(IContractParameterHost parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test(ProtoVessel vessel)
	{
		throw null;
	}
}
