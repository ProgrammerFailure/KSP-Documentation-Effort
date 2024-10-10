using ns2;

namespace ns12;

public class TooltipController_TitleAndText : TooltipController
{
	public Tooltip_TitleAndText prefab;

	public string titleString = "No text";

	public string textString = "No text";

	public bool continuousUpdate;

	public override bool OnTooltipAboutToSpawn()
	{
		if (string.IsNullOrEmpty(titleString))
		{
			return !string.IsNullOrEmpty(textString);
		}
		return true;
	}

	public override void OnTooltipSpawned(Tooltip instance)
	{
		((Tooltip_TitleAndText)instance).title.text = titleString;
		((Tooltip_TitleAndText)instance).label.text = textString;
	}

	public override bool OnTooltipUpdate(Tooltip instance)
	{
		if (continuousUpdate)
		{
			((Tooltip_TitleAndText)instance).title.text = titleString;
			((Tooltip_TitleAndText)instance).label.text = textString;
		}
		return OnTooltipAboutToSpawn();
	}
}
