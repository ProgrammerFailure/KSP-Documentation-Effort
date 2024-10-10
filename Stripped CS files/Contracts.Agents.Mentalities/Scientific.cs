using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Scientific : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001052");
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_265367");
	}

	public override KeywordScore ScoreKeyword(string keyword)
	{
		if (keyword == "Scientific")
		{
			return KeywordScore.Positive;
		}
		return KeywordScore.None;
	}

	public override bool CanProcessContract(Contract contract)
	{
		return !contract.Keywords.Contains("Record");
	}

	public override void ProcessContract(Contract contract)
	{
	}
}
