using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_FloatRange : UI_Control
{
	public float minValue;

	public float maxValue;

	public float stepIncrement;

	private static string UIControlName;

	private static string errorNoValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_FloatRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UI_FloatRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node, object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node, object host)
	{
		throw null;
	}
}