using ns9;

namespace Experience.Effects;

public class ScienceResetSkill : ExperienceEffect
{
	public ScienceResetSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public ScienceResetSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 0f;
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_18818");
	}
}
