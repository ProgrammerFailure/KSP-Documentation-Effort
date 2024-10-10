using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Record : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001051");
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_265328");
	}

	public override bool CanProcessContract(Contract contract)
	{
		int count = contract.KeywordsRequired.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(contract.KeywordsRequired[count] == "Record"));
		return true;
	}

	public override KeywordScore ScoreKeyword(string keyword)
	{
		if (keyword == "Record")
		{
			return KeywordScore.Positive;
		}
		return KeywordScore.NegativeHigh;
	}

	public override void ProcessContract(Contract contract)
	{
	}
}
