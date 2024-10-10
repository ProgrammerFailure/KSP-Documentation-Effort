using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class ScoreModule_Accuracy : ScoreModule
{
	[MEGUI_ScoreRangeList(ContentType = MEGUI_ScoreRangeList.RangeContentType.Percentage, guiName = "#autoLOC_8004155")]
	public List<ScoreRange> scoreRanges;

	public ScoreModule_Accuracy()
	{
		scoreRanges = new List<ScoreRange>();
	}

	public ScoreModule_Accuracy(MENode node)
		: base(node)
	{
		scoreRanges = new List<ScoreRange>();
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8001002");
	}

	public override float AwardScore(float currentScore)
	{
		double value = (double)base.scoreableObjective.GetScoreModifier(GetType()) * 100.0;
		int num = 0;
		while (true)
		{
			if (num < scoreRanges.Count)
			{
				if (scoreRanges[num].isValueInRange(value))
				{
					break;
				}
				num++;
				continue;
			}
			return currentScore;
		}
		awardedScoreDescription = Localizer.Format("#autoLOC_8006056", value.ToString("00"), scoreRanges[num].score);
		return currentScore += scoreRanges[num].score;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "scoreRanges")
		{
			return ScoreDescription();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string ScoreDescription()
	{
		string text = Localizer.Format("#autoLOC_8100157", GetDisplayName());
		int i = 0;
		for (int count = scoreRanges.Count; i < count; i++)
		{
			text += Localizer.Format("#autoLOC_8100156", scoreRanges[i].minRange, scoreRanges[i].maxRange, scoreRanges[i].score);
		}
		return text;
	}

	public override List<string> GetDefaultPinnedParameters()
	{
		return new List<string> { "scoreRanges" };
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ScoreModule_Accuracy scoreModule_Accuracy))
		{
			return false;
		}
		if (base.name.Equals(scoreModule_Accuracy.name))
		{
			return MissionCheckpointValidator.CompareObjectLists(scoreRanges, scoreModule_Accuracy.scoreRanges);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004159");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (node.HasNode("SCORERANGES"))
		{
			ConfigNode[] nodes = node.GetNode("SCORERANGES").GetNodes("SCORERANGE");
			foreach (ConfigNode configNode in nodes)
			{
				ScoreRange scoreRange = new ScoreRange();
				scoreRange.Load(configNode);
				scoreRanges.Add(scoreRange);
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		ConfigNode configNode = node.AddNode("SCORERANGES");
		int i = 0;
		for (int count = scoreRanges.Count; i < count; i++)
		{
			scoreRanges[i].Save(configNode.AddNode("SCORERANGE"));
		}
	}
}
