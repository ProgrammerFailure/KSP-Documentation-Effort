using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Conglomerate : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001038");
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_264908");
	}

	public override KeywordScore ScoreKeyword(string keyword)
	{
		if (keyword == "Commercial")
		{
			return KeywordScore.PositiveHigh;
		}
		return KeywordScore.None;
	}

	public override bool CanProcessContract(Contract contract)
	{
		return true;
	}

	public override void ProcessContract(Contract contract)
	{
		FactorExpiry(contract, positive: false);
		FactorDeadline(contract, positive: false);
		FactorFundsAdvance(contract, positive: true);
		FactorFundsCompletion(contract, positive: true);
	}
}
