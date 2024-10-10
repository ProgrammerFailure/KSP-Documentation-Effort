using ns9;

namespace Experience.Effects;

public class GeeForceTolerance : ExperienceEffect
{
	public GeeForceTolerance(ExperienceTrait parent)
		: base(parent)
	{
	}

	public GeeForceTolerance(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 1f;
	}

	public override string GetDescription()
	{
		if (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits)
		{
			string text = GeeTolerance().ToString("F2");
			return Localizer.Format("#autoLOC_18586", text);
		}
		return null;
	}

	public virtual float GeeTolerance()
	{
		if (base.Parent.CrewMember.experienceLevel == 0)
		{
			return base.LevelModifiers[1] - (base.LevelModifiers[2] - base.LevelModifiers[1]);
		}
		return base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
	}
}
