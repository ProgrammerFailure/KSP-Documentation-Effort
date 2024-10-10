using System;
using UniLinq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_ChooseOption : UI_Control
{
	public string[] options = new string[1] { "Default" };

	public string[] display;

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (node.HasValue("options"))
		{
			options = (from s in node.GetValue("options").Split(',')
				select s.Trim()).ToArray();
		}
		if (node.HasValue("display"))
		{
			display = (from s in node.GetValue("display").Split(',')
				select s.Trim()).ToArray();
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.AddValue("options", KSPUtil.PrintCollection(options));
		if (display != null)
		{
			node.AddValue("display", KSPUtil.PrintCollection(display));
		}
	}
}
