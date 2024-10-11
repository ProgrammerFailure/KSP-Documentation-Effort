using System;
using System.Runtime.CompilerServices;

public class AxisBinding_Single : InputBinding, IConfigNode, ICloneable
{
	public string idTag;

	public string name;

	public string title;

	public int deviceIdx;

	public int axisIdx;

	public bool preinvertAxis;

	public bool inverted;

	public float sensitivity;

	public float deadzone;

	public float scale;

	public float neutralPoint;

	private float value;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(float neutral)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(float neutral, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(InputBindingModes useSwitch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(InputBindingModes useSwitch, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(InputBindingModes useSwitch, float neutral)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(InputBindingModes useSwitch, float neutral, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(string Id, string Name, bool isInverted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(string Id, string Name, bool isInverted, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisBinding_Single(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsNeutral()
	{
		throw null;
	}
}
