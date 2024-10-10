using ns9;

namespace Experience.Effects;

public class FuelUsage : ExperienceEffect
{
	public FuelUsage(ExperienceTrait parent)
		: base(parent)
	{
	}

	public FuelUsage(ExperienceTrait parent, float[] modifiers)
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
		return Localizer.Format("#autoLOC_18516", text);
	}

	public override void OnRegister(Part part)
	{
		part.PartValues.FuelUsage.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.FuelUsage.Remove(GetValue);
	}

	public float GetValue()
	{
		return base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
	}
}
