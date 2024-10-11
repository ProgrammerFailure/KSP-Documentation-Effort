using System;
using System.Runtime.CompilerServices;

public class AxisBinding : IConfigNode, ICloneable
{
	public AxisBinding_Single primary;

	public AxisBinding_Single secondary;

	private float pAxis;

	private float sAxis;

	public float deadzone
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float scale
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(float neutral)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(float neutral, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(InputBindingModes useSwitch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(InputBindingModes useSwitch, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(InputBindingModes useSwitch, float neutral)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(InputBindingModes useSwitch, float neutral, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(string Id, string Name, bool isInverted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(string Id, string Name, bool isInverted, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsNeutral()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object Clone()
	{
		throw null;
	}
}
