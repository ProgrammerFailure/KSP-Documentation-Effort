using ns9;

namespace Experience.Effects;

public class ExternalExperimentSkill : ExperienceEffect
{
	public ExternalExperimentSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public ExternalExperimentSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 0f;
	}

	public override string GetDescription()
	{
		return Localizer.Format("#autoLOC_18491");
	}
}
