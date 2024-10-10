using ns9;

namespace Experience.Effects;

public class MaxThrottle : ExperienceEffect
{
	public MaxThrottle(ExperienceTrait parent)
		: base(parent)
	{
	}

	public MaxThrottle(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 1f;
	}

	public override string GetDescription()
	{
		string text = (GetValue() * 100f).ToString("F2");
		return Localizer.Format("#autoLOC_7001012", text);
	}

	public override void OnRegister(Part part)
	{
		part.PartValues.MaxThrottle.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.MaxThrottle.Remove(GetValue);
	}

	public float GetValue()
	{
		return base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
	}
}
