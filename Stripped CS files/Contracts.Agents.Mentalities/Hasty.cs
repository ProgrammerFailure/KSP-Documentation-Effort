using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Hasty : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001043");
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_265075");
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
		FactorDeadline(contract, positive: false);
		FactorExpiry(contract, positive: false);
		FactorReputationCompletion(contract, positive: true);
	}
}
