using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Economic : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001042");
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_265039");
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
		FactorFundsAdvance(contract, positive: true);
		FactorFundsFailure(contract, positive: true);
		FactorFundsCompletion(contract, positive: true);
		FactorReputationCompletion(contract, positive: false);
	}
}
