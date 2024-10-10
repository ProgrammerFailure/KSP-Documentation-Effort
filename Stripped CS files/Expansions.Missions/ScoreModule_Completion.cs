using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class ScoreModule_Completion : ScoreModule
{
	[MEGUI_InputField(CharacterLimit = 7, ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8100147")]
	public float bonusScore;

	public ScoreModule_Completion()
	{
		bonusScore = 0f;
	}

	public ScoreModule_Completion(MENode node)
		: base(node)
	{
		bonusScore = 0f;
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100148");
	}

	public override string GetDisplayToolTip()
	{
		return "#autoLOC_8001025";
	}

	public override float AwardScore(float currentScore)
	{
		awardedScoreDescription = Localizer.Format("#autoLOC_8006057", bonusScore);
		currentScore += bonusScore;
		return currentScore;
	}

	public override string ScoreDescription()
	{
		return Localizer.Format("#autoLOC_8100149", GetDisplayName(), bonusScore);
	}

	public override List<string> GetDefaultPinnedParameters()
	{
		return new List<string> { "bonusScore" };
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ScoreModule_Completion scoreModule_Completion))
		{
			return false;
		}
		if (base.name.Equals(scoreModule_Completion.name))
		{
			return bonusScore.Equals(scoreModule_Completion.bonusScore);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004160");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("bonusScore", ref bonusScore);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("bonusScore", bonusScore);
	}
}
