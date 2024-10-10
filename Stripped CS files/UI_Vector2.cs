using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Vector2 : UI_Control
{
	public float minValueX;

	public float maxValueX = 1f;

	public float minValueY;

	public float maxValueY = 1f;

	public static string UIControlName = "UI_Vector2";

	public static string errorNoValue = "Requires a minValueX, maxValueX, minValueY and maxValueY to be defined";

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (UI_Control.ParseFloat(out minValueX, node, "minValueX", UIControlName, errorNoValue) && UI_Control.ParseFloat(out maxValueX, node, "maxValueX", UIControlName, errorNoValue) && UI_Control.ParseFloat(out minValueX, node, "minValueY", UIControlName, errorNoValue))
		{
			UI_Control.ParseFloat(out maxValueX, node, "maxValueY", UIControlName, errorNoValue);
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.SetValue("minValueX", minValueX.ToString());
		node.SetValue("maxValueX", maxValueX.ToString());
		node.SetValue("minValueY", minValueY.ToString());
		node.SetValue("maxValueY", maxValueY.ToString());
	}
}
