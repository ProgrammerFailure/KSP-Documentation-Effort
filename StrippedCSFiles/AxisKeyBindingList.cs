using System;
using System.Runtime.CompilerServices;

public class AxisKeyBindingList : IConfigNode, ICloneable
{
	private AxisKeyBinding[] axisKeyBindings;

	public AxisKeyBinding this[int index]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public int Length
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AxisKeyBindingList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisKeyBindingList(int count)
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
