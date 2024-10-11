using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_FloatEdit : UI_Control
{
	private const string UIControlName = "FloatEdit";

	public float minValue;

	public float maxValue;

	public float incrementLarge;

	public float incrementSmall;

	public float incrementSlide;

	public bool useSI;

	public string unit;

	public int sigFigs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_FloatEdit()
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
