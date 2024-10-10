using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_FloatEdit : UI_Control
{
	public const string UIControlName = "FloatEdit";

	public float minValue = float.NegativeInfinity;

	public float maxValue = float.PositiveInfinity;

	public float incrementLarge;

	public float incrementSmall;

	public float incrementSlide;

	public bool useSI;

	public string unit = "";

	public int sigFigs;

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
	}
}
