using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class Competitive : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001037");
	}

	public override string GetDescription()
	{
		return null;
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
	}
}
