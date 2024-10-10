using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_ProgressBar : UI_Control
{
	public float minValue;

	public float maxValue = 1f;

	public static string UIControlName = "ProgressBar";

	public static string errorNoValue = "Requires a minValue and maxValue to be defined";

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (UI_Control.ParseFloat(out minValue, node, "minValue", UIControlName, errorNoValue))
		{
			UI_Control.ParseFloat(out maxValue, node, "maxValue", UIControlName, errorNoValue);
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.SetValue("minValue", minValue.ToString());
		node.SetValue("maxValue", maxValue.ToString());
	}
}
