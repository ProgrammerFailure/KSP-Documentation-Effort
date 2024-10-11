using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Vector2 : UI_Control
{
	public float minValueX;

	public float maxValueX;

	public float minValueY;

	public float maxValueY;

	private static string UIControlName;

	private static string errorNoValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_Vector2()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UI_Vector2()
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
