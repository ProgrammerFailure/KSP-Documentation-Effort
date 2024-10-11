using System.Runtime.CompilerServices;

namespace Experience.Effects;

public class FailureRepairSkill : ExperienceEffect
{
	public enum Skills
	{
		Failures = 1
	}

	public static string[] SkillsReadable;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureRepairSkill(ExperienceTrait parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FailureRepairSkill(ExperienceTrait parent, float[] modifiers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FailureRepairSkill()
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
