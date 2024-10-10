using Expansions;
using ns9;

namespace Experience.Effects;

public class DeployedScienceExpSkill : ExperienceEffect
{
	public DeployedScienceExpSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public DeployedScienceExpSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return 0.25f;
		}
		return 0f;
	}

	public override string GetDescription()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			string text = (GetValue() * 100f).ToString("F2");
			return Localizer.Format("#autoLOC_8002228", text);
		}
		return "";
	}

	public float GetValue()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
		}
		return 0f;
	}
}
