using System;
using System.Runtime.CompilerServices;

namespace Contracts.Predicates;

[Serializable]
public class IsControllable : ContractPredicate
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public IsControllable(IContractParameterHost parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Test(ProtoVessel protoVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDescription()
	{
		throw null;
	}
}
