using System;
using UniLinq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Cycle : UI_Control
{
	public string[] stateNames = new string[1] { "Default" };

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (node.HasValue("stateNames"))
		{
			stateNames = (from s in node.GetValue("stateNames").Split(',')
				select s.Trim()).ToArray();
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.AddValue("stateNames", KSPUtil.PrintCollection(stateNames));
	}
}
