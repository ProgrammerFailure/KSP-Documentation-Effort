using ns9;

namespace Experience.Effects;

public class EnginePower : ExperienceEffect
{
	public EnginePower(ExperienceTrait parent)
		: base(parent)
	{
	}

	public EnginePower(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 1f;
	}

	public override string GetDescription()
	{
		string text = (GetValue() * 100f).ToString("F2") + "%";
		return Localizer.Format("#autoLOC_18451", text);
	}

	public override void OnRegister(Part part)
	{
		part.PartValues.EnginePower.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.EnginePower.Remove(GetValue);
	}

	public float GetValue()
	{
		return base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
	}
}
