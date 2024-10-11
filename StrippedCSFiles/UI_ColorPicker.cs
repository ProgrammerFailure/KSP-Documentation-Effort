using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_ColorPicker : UI_Control
{
	public UIPartActionColorPicker UI_PartActionColorPicker;

	public bool useFieldNameForColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_ColorPicker()
	{
		throw null;
	}
}
