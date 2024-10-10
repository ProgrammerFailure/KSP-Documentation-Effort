using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_FieldFloatRange : UI_Control
{
	public string minValue = "minValue";

	public string maxValue = "maxValue";

	public static string UIControlName = "FieldFloatRange";

	public static string errorNoValue = "Requires a minValue and maxValue to be defined";

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (UI_Control.ParseString(out minValue, node, "minValue", UIControlName, errorNoValue))
		{
			UI_Control.ParseString(out maxValue, node, "maxValue", UIControlName, errorNoValue);
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.SetValue("minValue", minValue);
		node.SetValue("maxValue", maxValue);
	}
}
