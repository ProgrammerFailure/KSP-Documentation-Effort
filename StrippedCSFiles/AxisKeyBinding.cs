using System;
using System.Runtime.CompilerServices;

public class AxisKeyBinding : IConfigNode, ICloneable
{
	public AxisBinding axisBinding;

	public KeyBinding plusKeyBinding;

	public KeyBinding minusKeyBinding;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisKeyBinding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object Clone()
	{
		throw null;
	}
}
