using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class ScoreModule_Modifier : ScoreModule
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "Addition", guiName = "#autoLOC_8100123")]
	public ScoreModifierType modifierType;

	[MEGUI_InputField(CharacterLimit = 7, ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8100124")]
	public float modifierScore;

	public ScoreModule_Modifier()
	{
		modifierType = ScoreModifierType.Multiply;
		modifierScore = 0f;
	}

	public ScoreModule_Modifier(MENode node)
		: base(node)
	{
		modifierType = ScoreModifierType.Multiply;
		modifierScore = 0f;
	}

	public override string GetDisplayName()
	{
		return "#autoLOC_8100125";
	}

	public override string GetDisplayToolTip()
	{
		return "#autoLOC_8001024";
	}

	public override float AwardScore(float currentScore)
	{
		switch (modifierType)
		{
		default:
			return currentScore;
		case ScoreModifierType.Multiply:
			awardedScoreDescription = Localizer.Format("#autoLOC_8006052", modifierScore);
			return currentScore * modifierScore;
		case ScoreModifierType.Divide:
			awardedScoreDescription = Localizer.Format("#autoLOC_8006053", modifierScore);
			return currentScore / modifierScore;
		case ScoreModifierType.Substract:
			awardedScoreDescription = Localizer.Format("#autoLOC_8006054", modifierScore);
			return currentScore - modifierScore;
		case ScoreModifierType.Set:
			awardedScoreDescription = Localizer.Format("#autoLOC_8006055", modifierScore);
			return modifierScore;
		}
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "modifierScore")
		{
			return ScoreDescription();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string ScoreDescription()
	{
		if (modifierType != 0 && modifierType != ScoreModifierType.Divide)
		{
			if (modifierType == ScoreModifierType.Substract)
			{
				string text = Localizer.Format("#autoLOC_8005414") + " " + modifierScore + " " + Localizer.Format("#autoLOC_8100147");
				return Localizer.Format("#autoLOC_8004190", GetDisplayName(), text);
			}
			return Localizer.Format("#autoLOC_8006050", GetDisplayName(), modifierScore);
		}
		return Localizer.Format("#autoLOC_8100154", GetDisplayName(), modifierType.Description(), modifierScore);
	}

	public override List<string> GetDefaultPinnedParameters()
	{
		return new List<string> { "modifierScore" };
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ScoreModule_Modifier scoreModule_Modifier))
		{
			return false;
		}
		if (base.name.Equals(scoreModule_Modifier.name) && modifierType.Equals(scoreModule_Modifier.modifierType))
		{
			return modifierScore.Equals(scoreModule_Modifier.modifierScore);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004158");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetEnum("type", ref modifierType, ScoreModifierType.Multiply);
		node.TryGetValue("modifier", ref modifierScore);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("type", modifierType);
		node.AddValue("modifier", modifierScore);
	}
}
