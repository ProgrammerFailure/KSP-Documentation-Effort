using System;

namespace ns2;

public abstract class AppUI_Control : Attribute
{
	public enum HorizontalAlignment
	{
		None,
		Left,
		Middle,
		Right
	}

	public enum VerticalAlignment
	{
		Top,
		Bottom,
		Midline,
		Capline
	}

	public string guiName;

	public HorizontalAlignment guiNameHorizAlignment = HorizontalAlignment.Left;

	public VerticalAlignment guiNameVertAlignment = VerticalAlignment.Midline;

	public int order;

	public bool showGuiName = true;

	public string hoverText;

	public bool hideOnError;

	public AppUI_Control()
	{
		order = -1;
	}
}
