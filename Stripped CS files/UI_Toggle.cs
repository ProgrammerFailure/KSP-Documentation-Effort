using System;
using ns9;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Toggle : UI_Control
{
	public string enabledText = "#autoLOC_6001072";

	public string disabledText = "#autoLOC_6001071";

	public bool invertButton;

	public string tipText = "";

	public string displayEnabledText => Localizer.Format(enabledText);

	public string displayDisabledText => Localizer.Format(disabledText);

	public override void Load(ConfigNode node, object host)
	{
		base.Load(node, host);
		if (node.HasValue("enabledText"))
		{
			enabledText = node.GetValue("enabledText");
		}
		if (node.HasValue("disabledText"))
		{
			disabledText = node.GetValue("disabledText");
		}
		if (node.HasValue("invertButton"))
		{
			invertButton = bool.Parse(node.GetValue("invertButton"));
		}
		if (node.HasValue("tipText"))
		{
			tipText = node.GetValue("topText");
		}
	}

	public override void Save(ConfigNode node, object host)
	{
		base.Save(node, host);
		node.SetValue("enabledText", enabledText);
		node.SetValue("disabledText", disabledText);
		node.SetValue("invertButton", invertButton.ToString());
		node.SetValue("tipText", tipText);
	}
}
