using UnityEngine;

public class DialogGUIVerticalLayout : DialogGUILayoutBase
{
	public DialogGUIVerticalLayout(params DialogGUIBase[] list)
		: base(list)
	{
	}

	public DialogGUIVerticalLayout(float minWidth, float minHeight, params DialogGUIBase[] list)
		: base(list)
	{
		base.minHeight = minHeight;
		base.minWidth = minWidth;
	}

	public DialogGUIVerticalLayout(bool sw = false, bool sh = false)
	{
		stretchWidth = sw;
		stretchHeight = sh;
	}

	public DialogGUIVerticalLayout(bool sw, bool sh, float sp, RectOffset pad, TextAnchor achr, params DialogGUIBase[] list)
		: base(list)
	{
		stretchWidth = sw;
		stretchHeight = sh;
		spacing = sp;
		padding = pad;
		anchor = achr;
	}

	public DialogGUIVerticalLayout(float minWidth, float minHeight, float sp, RectOffset pad, TextAnchor achr, params DialogGUIBase[] list)
		: base(list)
	{
		base.minHeight = minHeight;
		base.minWidth = minWidth;
		spacing = sp;
		padding = pad;
		anchor = achr;
	}

	public override void Update()
	{
		base.Update();
	}
}
