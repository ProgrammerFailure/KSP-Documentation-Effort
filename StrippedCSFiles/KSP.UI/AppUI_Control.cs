using System;
using System.Runtime.CompilerServices;

namespace KSP.UI;

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

	public HorizontalAlignment guiNameHorizAlignment;

	public VerticalAlignment guiNameVertAlignment;

	public int order;

	public bool showGuiName;

	public string hoverText;

	public bool hideOnError;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AppUI_Control()
	{
		throw null;
	}
}
