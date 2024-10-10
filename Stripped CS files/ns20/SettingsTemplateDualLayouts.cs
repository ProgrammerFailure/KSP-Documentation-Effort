using UnityEngine;

namespace ns20;

public class SettingsTemplateDualLayouts : SettingsTemplate
{
	public RectTransform layout2;

	public bool flop;

	public RectTransform GetLayoutFlipFlop()
	{
		if (flop)
		{
			flop = !flop;
			return layout2;
		}
		flop = !flop;
		return layout;
	}
}
