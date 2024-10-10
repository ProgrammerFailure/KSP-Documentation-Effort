using System;

namespace Contracts.Predicates;

[Serializable]
public class IsActiveVessel : ContractPredicate
{
	public IsActiveVessel(IContractParameterHost parent)
		: base(parent)
	{
	}

	public override bool Test(Vessel vessel)
	{
		return vessel == FlightGlobals.ActiveVessel;
	}

	public override bool Test(ProtoVessel vessel)
	{
		return false;
	}
}
