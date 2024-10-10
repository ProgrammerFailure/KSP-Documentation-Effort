using Expansions;
using ns9;

namespace Experience.Effects;

public class DeployedSciencePowerSkill : ExperienceEffect
{
	public DeployedSciencePowerSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public DeployedSciencePowerSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return 1f;
		}
		return 0f;
	}

	public override string GetDescription()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return Localizer.Format("#autoLOC_8002229", GetValue());
		}
		return "";
	}

	public float GetValue()
	{
		if (ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			if (base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length) == 0)
			{
				return GetDefaultValue();
			}
			return GetDefaultValue() + base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
		}
		return 0f;
	}

	public float GetTraitBonus(int level)
	{
		if (level < base.LevelModifiers.Length)
		{
			return 1f + base.LevelModifiers[level];
		}
		return 1f;
	}
}
