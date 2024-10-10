using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class ScoreModule_Time : ScoreModule
{
	[MEGUI_ScoreRangeList(ContentType = MEGUI_ScoreRangeList.RangeContentType.Time, minLabel = "", maxLabel = "", valueLabel = "", guiName = "#autoLOC_8004155")]
	public List<ScoreRange> scoreRanges;

	public KSPUtil.DefaultDateTimeFormatter dateFormatter;

	public ScoreModule_Time()
	{
		dateFormatter = new KSPUtil.DefaultDateTimeFormatter();
		scoreRanges = new List<ScoreRange>();
	}

	public ScoreModule_Time(MENode node)
		: base(node)
	{
		dateFormatter = new KSPUtil.DefaultDateTimeFormatter();
		scoreRanges = new List<ScoreRange>();
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100161");
	}

	public override float AwardScore(float currentScore)
	{
		int num = 0;
		while (true)
		{
			if (num < scoreRanges.Count)
			{
				if (scoreRanges[num].isValueInRange(Planetarium.GetUniversalTime()))
				{
					break;
				}
				num++;
				continue;
			}
			return currentScore;
		}
		awardedScoreDescription = Localizer.Format("#autoLOC_8001003", scoreRanges[num].score);
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
			text += Localizer.Format("#autoLOC_8100158", dateFormatter.PrintTimeStampCompact(scoreRanges[i].minRange, days: true, years: true), dateFormatter.PrintTimeStampCompact(scoreRanges[i].maxRange, days: true, years: true), scoreRanges[i].score);
		}
		return text;
	}

	public override List<string> GetDefaultPinnedParameters()
	{
		return new List<string> { "scoreRanges" };
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ScoreModule_Time scoreModule_Time))
		{
			return false;
		}
		if (base.name.Equals(scoreModule_Time.name))
		{
			return MissionCheckpointValidator.CompareObjectLists(scoreRanges, scoreModule_Time.scoreRanges);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004156");
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
