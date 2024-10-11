using System.Runtime.CompilerServices;

namespace Experience.Effects;

public class RepairSkill : ExperienceEffect
{
	public enum Skills
	{
		Parachutes = 1,
		Wheels = 3,
		LandingLegs = 2
	}

	public static string[] SkillsReadable;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RepairSkill(ExperienceTrait parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RepairSkill(ExperienceTrait parent, float[] modifiers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static RepairSkill()
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
