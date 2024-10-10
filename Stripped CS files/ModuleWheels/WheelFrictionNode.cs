using System;
using UnityEngine;

namespace ModuleWheels;

[Serializable]
public class WheelFrictionNode
{
	public float stiffness;

	public float extremumSlip;

	public float extremumValue;

	public float asymptoteSlip;

	public float asymptoteValue;

	public WheelFrictionNode From(WheelFrictionCurve fCurve)
	{
		stiffness = fCurve.stiffness;
		extremumSlip = fCurve.extremumSlip;
		extremumValue = fCurve.extremumValue;
		asymptoteSlip = fCurve.asymptoteSlip;
		asymptoteValue = fCurve.asymptoteValue;
		return this;
	}

	public WheelFrictionNode Load(ConfigNode node)
	{
		if (node.HasValue("stiffness"))
		{
			stiffness = float.Parse(node.GetValue("stiffness"));
		}
		if (node.HasValue("extremumSlip"))
		{
			extremumSlip = float.Parse(node.GetValue("extremumSlip"));
		}
		if (node.HasValue("extremumValue"))
		{
			extremumValue = float.Parse(node.GetValue("extremumValue"));
		}
		if (node.HasValue("asymptoteSlip"))
		{
			asymptoteSlip = float.Parse(node.GetValue("asymptoteSlip"));
		}
		if (node.HasValue("asymptoteValue"))
		{
			asymptoteValue = float.Parse(node.GetValue("asymptoteValue"));
		}
		return this;
	}

	public WheelFrictionCurve GetCurve()
	{
		WheelFrictionCurve result = default(WheelFrictionCurve);
		result.asymptoteSlip = asymptoteSlip;
		result.asymptoteValue = asymptoteValue;
		result.extremumSlip = extremumSlip;
		result.extremumValue = extremumValue;
		result.stiffness = stiffness;
		return result;
	}
}
