using System;

public class AxisKeyBindingList : IConfigNode, ICloneable
{
	public AxisKeyBinding[] axisKeyBindings;

	public AxisKeyBinding this[int index]
	{
		get
		{
			return axisKeyBindings[index];
		}
		set
		{
			axisKeyBindings[index] = value;
		}
	}

	public int Length => axisKeyBindings.Length;

	public AxisKeyBindingList()
	{
	}

	public AxisKeyBindingList(int count)
	{
		axisKeyBindings = new AxisKeyBinding[count];
		for (int i = 0; i < count; i++)
		{
			axisKeyBindings[i] = new AxisKeyBinding();
		}
	}

	public void Save(ConfigNode node)
	{
		for (int i = 0; i < axisKeyBindings.Length; i++)
		{
			ConfigNode node2 = node.AddNode("AXIS_KEY_BINDING");
			axisKeyBindings[i].Save(node2);
		}
	}

	public void Load(ConfigNode node)
	{
		ConfigNode[] nodes = node.GetNodes("AXIS_KEY_BINDING");
		int num = nodes.Length;
		if (num > axisKeyBindings.Length)
		{
			num = axisKeyBindings.Length;
		}
		for (int i = 0; i < num; i++)
		{
			axisKeyBindings[i].Load(nodes[i]);
		}
	}

	public object Clone()
	{
		AxisKeyBindingList axisKeyBindingList = new AxisKeyBindingList();
		axisKeyBindingList.axisKeyBindings = new AxisKeyBinding[axisKeyBindings.Length];
		for (int i = 0; i < axisKeyBindings.Length; i++)
		{
			axisKeyBindingList.axisKeyBindings[i] = (AxisKeyBinding)axisKeyBindings[i].Clone();
		}
		return axisKeyBindingList;
	}
}
