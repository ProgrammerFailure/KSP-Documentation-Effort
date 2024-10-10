using ns9;

namespace Experience.Effects;

public class FullVesselControlSkill : ExperienceEffect
{
	public FullVesselControlSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public FullVesselControlSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 0f;
	}

	public override string GetDescription()
	{
		string result = Localizer.Format("#autoLOC_18556");
		if (!HighLogic.CurrentGame.Parameters.Difficulty.EnableCommNet)
		{
			return "";
		}
		return result;
	}
}
