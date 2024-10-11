using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_ScaleEdit : UI_Control
{
	private const string UIControlName = "ScaleEdit";

	public float[] intervals;

	public float[] incrementSlide;

	public bool useSI;

	public string unit;

	public int sigFigs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_ScaleEdit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float MinValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float MaxValue()
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
