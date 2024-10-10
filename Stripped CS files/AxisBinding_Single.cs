using System;
using System.Globalization;
using UnityEngine;

public class AxisBinding_Single : InputBinding, IConfigNode, ICloneable
{
	public string idTag = "None";

	public string name = "None";

	public string title = "None";

	public int deviceIdx = -1;

	public int axisIdx = -1;

	public bool preinvertAxis;

	public bool inverted;

	public float sensitivity = 1f;

	public float deadzone = 0.05f;

	public float scale = 1f;

	public float neutralPoint;

	public float value;

	public AxisBinding_Single()
	{
	}

	public AxisBinding_Single(ControlTypes lockMask)
	{
		inputLockMask = (ulong)lockMask;
	}

	public AxisBinding_Single(float neutral)
	{
		neutralPoint = neutral;
	}

	public AxisBinding_Single(float neutral, ControlTypes lockMask)
	{
		neutralPoint = neutral;
		inputLockMask = (ulong)lockMask;
	}

	public AxisBinding_Single(InputBindingModes useSwitch)
	{
		switchState = useSwitch;
	}

	public AxisBinding_Single(InputBindingModes useSwitch, ControlTypes lockMask)
	{
		switchState = useSwitch;
		inputLockMask = (ulong)lockMask;
	}

	public AxisBinding_Single(InputBindingModes useSwitch, float neutral)
	{
		switchState = useSwitch;
		neutralPoint = neutral;
	}

	public AxisBinding_Single(InputBindingModes useSwitch, float neutral, ControlTypes lockMask)
	{
		switchState = useSwitch;
		neutralPoint = neutral;
		inputLockMask = (ulong)lockMask;
	}

	public AxisBinding_Single(string Id, string Name, bool isInverted)
	{
		idTag = Id;
		name = Name;
		inverted = isInverted;
	}

	public AxisBinding_Single(string Id, string Name, bool isInverted, ControlTypes lockMask)
	{
		idTag = Id;
		name = Name;
		inverted = isInverted;
		inputLockMask = (ulong)lockMask;
	}

	public AxisBinding_Single(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale)
	{
		idTag = Id;
		name = Name;
		inverted = isInverted;
		sensitivity = sens;
		deadzone = dead_zone;
		scale = axisScale;
	}

	public AxisBinding_Single(string Id, string Name, bool isInverted, float sens, float dead_zone, float axisScale, ControlTypes lockMask)
	{
		idTag = Id;
		name = Name;
		inverted = isInverted;
		sensitivity = sens;
		deadzone = dead_zone;
		scale = axisScale;
		inputLockMask = (ulong)lockMask;
	}

	public override void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("axis", axisIdx);
		node.AddValue("inv", inverted);
		node.AddValue("sensitivity", sensitivity);
		node.AddValue("deadzone", deadzone);
		node.AddValue("scale", scale);
		node.AddValue("group", inputLockMask.ToString());
		base.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		name = "NULL";
		if (node.HasValue("name"))
		{
			name = node.GetValue("name");
		}
		if (node.HasValue("axis"))
		{
			deviceIdx = -1;
			if (node.HasValue("axis"))
			{
				axisIdx = int.Parse(node.GetValue("axis"));
			}
			else
			{
				axisIdx = -1;
			}
		}
		else if (node.HasValue("id"))
		{
			idTag = node.GetValue("id");
			if (idTag.StartsWith("joy"))
			{
				deviceIdx = int.Parse(idTag.Split('.')[0].Replace("joy", ""));
				axisIdx = int.Parse(idTag.Split('.')[1]);
			}
			else
			{
				deviceIdx = -1;
				axisIdx = -1;
			}
		}
		else
		{
			deviceIdx = -1;
			axisIdx = -1;
		}
		if (node.HasValue("inv"))
		{
			inverted = bool.Parse(node.GetValue("inv"));
		}
		if (node.HasValue("sensitivity"))
		{
			sensitivity = float.Parse(node.GetValue("sensitivity"), CultureInfo.InvariantCulture);
		}
		if (node.HasValue("deadzone"))
		{
			deadzone = float.Parse(node.GetValue("deadzone"), CultureInfo.InvariantCulture);
		}
		if (node.HasValue("scale"))
		{
			scale = float.Parse(node.GetValue("scale"), CultureInfo.InvariantCulture);
		}
		if (node.HasValue("group"))
		{
			inputLockMask = ulong.Parse(node.GetValue("group"));
		}
		base.Load(node);
		if (axisIdx != -1)
		{
			deviceIdx = GameSettings.INPUT_DEVICES.GetDeviceIndex(name);
			if (deviceIdx != -1)
			{
				idTag = "joy" + deviceIdx + "." + axisIdx;
				title = name + "Axis " + axisIdx;
			}
			else
			{
				idTag = "None";
				title = "Not Found";
			}
		}
		else
		{
			idTag = name;
			title = name;
		}
	}

	public float GetAxis()
	{
		if (idTag == "None")
		{
			return neutralPoint;
		}
		if (!CompareSwitchState(switchState))
		{
			return neutralPoint;
		}
		if (IsLocked())
		{
			return neutralPoint;
		}
		value = Input.GetAxis(idTag) * scale;
		if (inverted ^ preinvertAxis)
		{
			value *= -1f;
		}
		if (Mathf.Abs(value) < deadzone)
		{
			value = 0f;
		}
		else
		{
			if (value > 0f)
			{
				value = (value - deadzone) * (1f / (1f - deadzone));
			}
			if (value < 0f)
			{
				value = (value + deadzone) * (1f / (1f - deadzone));
			}
		}
		if (value > 0f)
		{
			value = Mathf.Pow(Mathf.Clamp01(value), sensitivity);
		}
		if (value < 0f)
		{
			value = 0f - Mathf.Pow(Mathf.Clamp01(Mathf.Abs(value)), sensitivity);
		}
		return value;
	}

	public override bool IsNeutral()
	{
		return GetAxis() == 0f;
	}
}
