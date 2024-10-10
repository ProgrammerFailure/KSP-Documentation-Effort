using ns9;

namespace Experience.Effects;

public class PartScienceReturn : ExperienceEffect
{
	public PartScienceReturn(ExperienceTrait parent)
		: base(parent)
	{
	}

	public PartScienceReturn(ExperienceTrait parent, float[] modifiers)
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
		return Localizer.Format("#autoLOC_18706", text);
	}

	public override void OnRegister(Part part)
	{
		part.PartValues.ScienceReturnSum.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.ScienceReturnSum.Remove(GetValue);
	}

	public float GetValue()
	{
		return base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
	}
}