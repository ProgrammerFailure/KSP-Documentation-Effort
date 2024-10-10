using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_FloatRange : UI_Control
{
	public float minValue;

	public float maxValue = 1f;

	public float stepIncrement = 0.1f;

	public static string UIControlName = "FloatRange";

	public static string errorNoValue = "Requires a minValue, maxValue and stepIncrement to be defined";

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (UI_Control.ParseFloat(out minValue, node, "minValue", UIControlName, errorNoValue) && UI_Control.ParseFloat(out maxValue, node, "maxValue", UIControlName, errorNoValue))
		{
			UI_Control.ParseFloat(out stepIncrement, node, "stepIncrement", UIControlName, errorNoValue);
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.AddValue("minValue", minValue.ToString());
		node.AddValue("maxValue", maxValue.ToString());
		node.AddValue("stepIncrement", stepIncrement.ToString());
	}
}
