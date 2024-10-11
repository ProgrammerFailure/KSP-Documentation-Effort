using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_MinMaxRange : UI_Control
{
	public float minValueX;

	public float maxValueX;

	public float minValueY;

	public float maxValueY;

	public float stepIncrement;

	private static string UIControlName;

	private static string errorNoValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_MinMaxRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UI_MinMaxRange()
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
