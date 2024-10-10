using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Commercial : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001036");
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_264842");
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
		FactorFundsCompletion(contract, positive: true);
		FactorFundsFailure(contract, positive: true);
		FactorReputationCompletion(contract, positive: false);
	}
}
