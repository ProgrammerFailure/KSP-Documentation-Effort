using System;
using ns9;

namespace Contracts.Agents.Mentalities;

[Serializable]
public class NoRandomContractMentality : AgentMentality
{
	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_7001047");
	}

	public override bool CanProcessContract(Contract contract)
	{
		return false;
	}

	public override KeywordScore ScoreKeyword(string keyword)
	{
		return KeywordScore.NegativeHigh;
	}

	public override void OnAdded()
	{
		int count = base.Agent.Mentality.Count;
		while (count-- > 0)
		{
			if (base.Agent.Mentality[count] != this)
			{
				base.Agent.Mentality.RemoveAt(count);
			}
		}
		base.OnAdded();
	}
}
