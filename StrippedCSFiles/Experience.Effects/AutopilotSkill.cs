using System.Runtime.CompilerServices;

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

	public static string[] SkillsReadable;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AutopilotSkill(ExperienceTrait parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AutopilotSkill(ExperienceTrait parent, float[] modifiers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AutopilotSkill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override float GetDefaultValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRegister(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUnregister(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetValue()
	{
		throw null;
	}
}
