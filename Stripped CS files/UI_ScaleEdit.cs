using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_ScaleEdit : UI_Control
{
	public const string UIControlName = "ScaleEdit";

	public float[] intervals = new float[3] { 1f, 2f, 4f };

	public float[] incrementSlide = new float[2] { 0.02f, 0.04f };

	public bool useSI;

	public string unit = "";

	public int sigFigs;

	public float MinValue()
	{
		return intervals[0];
	}

	public float MaxValue()
	{
		return intervals[intervals.Length - 1];
	}

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
	}
}
