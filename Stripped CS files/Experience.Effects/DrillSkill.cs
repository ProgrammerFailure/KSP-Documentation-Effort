using ns9;

namespace Experience.Effects;

public class DrillSkill : ExperienceEffect
{
	public DrillSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public DrillSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 0f;
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_18426");
	}
}
