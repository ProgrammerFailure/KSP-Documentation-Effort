using ns9;
using UnityEngine;

namespace Experience.Effects;

public class AutopilotSkill : ExperienceEffect
{
	public enum Skills
	{
		StabilityAssist = 0,
		Prograde = 1,
		Retrograde = 1,
		Normal = 2,
		Antinormal = 2,
		RadialIn = 2,
		RadialOut = 2,
		Target = 3,
		AntiTarget = 3,
		Maneuver = 3
	}

	public static string[] SkillsReadable = new string[4]
	{
		Localizer.Format("#autoLOC_18284"),
		Localizer.Format("#autoLOC_18285"),
		Localizer.Format("#autoLOC_18286"),
		Localizer.Format("#autoLOC_18287")
	};

	public AutopilotSkill(ExperienceTrait parent)
		: base(parent)
	{
	}

	public AutopilotSkill(ExperienceTrait parent, float[] modifiers)
		: base(parent, modifiers)
	{
	}

	public override float GetDefaultValue()
	{
		return 0f;
	}

	public override string GetDescription()
	{
		int num = base.Parent.CrewMemberExperienceLevel();
		string text = "";
		for (int i = 0; i < Mathf.Min(num + 1, SkillsReadable.Length); i++)
		{
			if (i != 0)
			{
				text += "\n";
			}
			text += Localizer.Format(SkillsReadable[i]);
		}
		return text;
	}

	public override void OnRegister(Part part)
	{
		part.PartValues.AutopilotSkill.Add(GetValue);
		part.PartValues.AutopilotKerbalSkill.Add(GetValue);
	}

	public override void OnUnregister(Part part)
	{
		part.PartValues.AutopilotSkill.Remove(GetValue);
		part.PartValues.AutopilotKerbalSkill.Remove(GetValue);
	}

	public int GetValue()
	{
		return base.Parent.CrewMemberExperienceLevel();
	}
}
