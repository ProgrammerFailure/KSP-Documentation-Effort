using UnityEngine;

public class DialogGUIHorizontalLayout : DialogGUILayoutBase
{
	public DialogGUIHorizontalLayout(params DialogGUIBase[] list)
		: base(list)
	{
	}

	public DialogGUIHorizontalLayout(TextAnchor achr, params DialogGUIBase[] list)
		: base(list)
	{
		anchor = achr;
	}

	public DialogGUIHorizontalLayout(float minWidth, float minHeight, params DialogGUIBase[] list)
		: base(list)
	{
		base.minHeight = minHeight;
		base.minWidth = minWidth;
	}

	public DialogGUIHorizontalLayout(bool sw = false, bool sh = false, params DialogGUIBase[] list)
		: base(list)
	{
		stretchWidth = sw;
		stretchHeight = sh;
	}

	public DialogGUIHorizontalLayout(bool sw, bool sh, float sp, RectOffset pad, TextAnchor achr, params DialogGUIBase[] list)
		: base(list)
	{
		stretchWidth = sw;
		stretchHeight = sh;
		spacing = sp;
		padding = pad;
		anchor = achr;
	}

	public DialogGUIHorizontalLayout(float minWidth, float minHeight, float sp, RectOffset pad, TextAnchor achr, params DialogGUIBase[] list)
		: base(list)
	{
		base.minHeight = minHeight;
		base.minWidth = minWidth;
		spacing = sp;
		padding = pad;
		anchor = achr;
	}
}
