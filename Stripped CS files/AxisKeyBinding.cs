using System;

public class AxisKeyBinding : IConfigNode, ICloneable
{
	public AxisBinding axisBinding;

	public KeyBinding plusKeyBinding;

	public KeyBinding minusKeyBinding;

	public AxisKeyBinding()
	{
		axisBinding = new AxisBinding();
		plusKeyBinding = new KeyBinding();
		minusKeyBinding = new KeyBinding();
	}

	public void Save(ConfigNode node)
	{
		axisBinding.Save(node.AddNode("AXIS_BINDING"));
		plusKeyBinding.Save(node.AddNode("PLUS_KEY_BINDING"));
		minusKeyBinding.Save(node.AddNode("MINUS_KEY_BINDING"));
	}

	public void Load(ConfigNode node)
	{
		ConfigNode node2 = node.GetNode("AXIS_BINDING");
		if (node2 != null)
		{
			axisBinding.Load(node2);
		}
		node2 = node.GetNode("PLUS_KEY_BINDING");
		if (node2 != null)
		{
			plusKeyBinding.Load(node2);
		}
		node2 = node.GetNode("MINUS_KEY_BINDING");
		if (node2 != null)
		{
			minusKeyBinding.Load(node2);
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
