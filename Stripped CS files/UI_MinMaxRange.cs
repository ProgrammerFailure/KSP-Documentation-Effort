using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_MinMaxRange : UI_Control
{
	public float minValueX;

	public float maxValueX = 1f;

	public float minValueY;

	public float maxValueY = 1f;

	public float stepIncrement = 0.1f;

	public static string UIControlName = "MinMaxRange";

	public static string errorNoValue = "Requires a minValueX, maxValueX, minValueY, maxValueY and stepIncrement to be defined";

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (UI_Control.ParseFloat(out minValueX, node, "minValueX", UIControlName, errorNoValue) && UI_Control.ParseFloat(out maxValueX, node, "maxValueX", UIControlName, errorNoValue) && UI_Control.ParseFloat(out minValueY, node, "minValueY", UIControlName, errorNoValue) && UI_Control.ParseFloat(out maxValueY, node, "maxValueY", UIControlName, errorNoValue))
		{
			UI_Control.ParseFloat(out stepIncrement, node, "stepIncrement", UIControlName, errorNoValue);
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.AddValue("minValueX", minValueX.ToString());
		node.AddValue("maxValueX", maxValueX.ToString());
		node.AddValue("minValueY", minValueY.ToString());
		node.AddValue("maxValueY", maxValueY.ToString());
		node.AddValue("stepIncrement", stepIncrement.ToString());
	}
}
