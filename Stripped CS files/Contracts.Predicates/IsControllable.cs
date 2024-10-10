using System;

namespace Contracts.Predicates;

[Serializable]
public class IsControllable : ContractPredicate
{
	public IsControllable(IContractParameterHost parent)
		: base(parent)
	{
	}

	public override bool Test(Vessel vessel)
	{
		return vessel.IsControllable;
	}

	public override bool Test(ProtoVessel protoVessel)
	{
		return protoVessel.wasControllable;
	}

	public override string GetDescription()
	{
		return null;
	}
}
