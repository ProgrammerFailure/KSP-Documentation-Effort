using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Label : UI_Control
{
	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
	}
}
