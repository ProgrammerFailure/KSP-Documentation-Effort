using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Stern : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001054");
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_265434");
	}

	public override KeywordScore ScoreKeyword(string keyword)
	{
		if (keyword == "Commercial")
		{
			return KeywordScore.Positive;
		}
		return KeywordScore.None;
	}

	public override bool CanProcessContract(Contract contract)
	{
		return true;
	}

	public override void ProcessContract(Contract contract)
	{
		FactorReputationFailure(contract, positive: true);
		FactorReputationCompletion(contract, positive: true);
	}
}
