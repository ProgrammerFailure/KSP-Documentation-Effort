namespace Experience.Effects;

public class ScienceSkill : ExperienceEffect
{
	public ScienceSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public ScienceSkill(ExperienceTrait parent, float[] modifiers)
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

	public override void OnRegister(Part part)
	{
		part.PartValues.ScienceSkill.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.ScienceSkill.Remove(GetValue);
	}

	public int GetValue()
	{
		return base.Parent.CrewMemberExperienceLevel();
	}
}
