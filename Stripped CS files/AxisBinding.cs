using System;
using UnityEngine;

public class AxisBinding : IConfigNode, ICloneable
{
	public AxisBinding_Single primary;

	public AxisBinding_Single secondary;

	public float pAxis;

	public float sAxis;

	public float deadzone => Mathf.Max(primary.deadzone, secondary.deadzone);

	public float scale => primary.scale;

	public AxisBinding()
	{
		primary = new AxisBinding_Single();
		secondary = new AxisBinding_Single();
	}

	public AxisBinding(ControlTypes lockMask)
	{
		primary = new AxisBinding_Single(lockMask);
		secondary = new AxisBinding_Single(lockMask);
	}

	public AxisBinding(float neutral)
	{
		primary = new AxisBinding_Single(neutral);
		secondary = new AxisBinding_Single(neutral);
	}

	public AxisBinding(float neutral, ControlTypes lockMask)
	{
		primary = new AxisBinding_Single(neutral, lockMask);
		secondary = new AxisBinding_Single(neutral, lockMask);
	}

	public AxisBinding(InputBindingModes useSwitch)
	{
		primary = new AxisBinding_Single(useSwitch);
		secondary = new AxisBinding_Single(useSwitch);
	}

	public AxisBinding(InputBindingModes useSwitch, ControlTypes lockMask)
	{
		primary = new AxisBinding_Single(useSwitch, lockMask);
		secondary = new AxisBinding_Single(useSwitch, lockMask);
	}

	public AxisBinding(InputBindingModes useSwitch, float neutral)
	{
		primary = new AxisBinding_Single(useSwitch, neutral);
		secondary = new AxisBinding_Single(useSwitch, neutral);
	}

	public AxisBinding(InputBindingModes useSwitch, float neutral, ControlTypes lockMask)
	{
		primary = new AxisBinding_Single(useSwitch, neutral, lockMask);
		secondary = new AxisBinding_Single(useSwitch, neutral, lockMask);
	}

	public AxisBinding(string Id, string Name, bool isInverted)
	{
		primary = new AxisBinding_Single(Id, Name, isInverted);
		secondary = new AxisBinding_Single();
	}

	public AxisBinding(string Id, string Name, bool isInverted, ControlTypes lockMask)
	{
		primary = new AxisBinding_Single(Id, Name, isInverted, lockMask);
		secondary = new AxisBinding_Single(lockMask);
	}

	public AxisBinding(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale)
	{
		primary = new AxisBinding_Single(Id, Name, isInverted, sens, dead_zone, axisScale);
		secondary = new AxisBinding_Single();
	}

	public AxisBinding(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale, ControlTypes lockMask)
	{
		primary = new AxisBinding_Single(Id, Name, isInverted, sens, dead_zone, axisScale, lockMask);
		secondary = new AxisBinding_Single(lockMask);
	}

	public void Load(ConfigNode node)
	{
		if (node.HasNode("PRIMARY"))
		{
			primary.Load(node.GetNode("PRIMARY"));
		}
		if (node.HasNode("SECONDARY"))
		{
			secondary.Load(node.GetNode("SECONDARY"));
		}
	}

	public void Save(ConfigNode node)
	{
		primary.Save(node.AddNode("PRIMARY"));
		secondary.Save(node.AddNode("SECONDARY"));
	}

	public float GetAxis()
	{
		pAxis = primary.GetAxis();
		sAxis = secondary.GetAxis();
		if (Mathf.Abs(pAxis - primary.neutralPoint) >= Mathf.Abs(sAxis - secondary.neutralPoint))
		{
			return pAxis;
		}
		return sAxis;
	}

	public bool IsNeutral()
	{
		if (primary.IsNeutral())
		{
			return secondary.IsNeutral();
		}
		return false;
	}

	public object Clone()
	{
		AxisBinding obj = (AxisBinding)MemberwiseClone();
		obj.primary = (AxisBinding_Single)primary.Clone();
		obj.secondary = (AxisBinding_Single)secondary.Clone();
		return obj;
	}
}
