using ns9;

namespace Experience.Effects;

public class VesselScienceReturn : ExperienceEffect
{
	public VesselScienceReturn(ExperienceTrait parent)
		: base(parent)
	{
	}

	public VesselScienceReturn(ExperienceTrait parent, float[] modifiers)
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
		return Localizer.Format("#autoLOC_18908", text);
	}

	public override void OnRegister(Part part)
	{
		part.PartValues.ScienceReturnMax.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.ScienceReturnMax.Remove(GetValue);
	}

	public float GetValue()
	{
		return base.LevelModifiers[base.Parent.CrewMemberExperienceLevel(base.LevelModifiers.Length)];
	}
}
