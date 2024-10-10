namespace Experience.Effects;

public class SpecialExperimentSkill : ExperienceEffect
{
	public SpecialExperimentSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public SpecialExperimentSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 0f;
	}

	public override string GetDescription()
	{
		return "";
	}
}
