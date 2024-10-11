using System;
using System.Runtime.CompilerServices;

namespace KSP.UI;

[Serializable]
public class ButtonStateTooltip
{
	public string name;

	public string tooltipText;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ButtonStateTooltip()
	{
		throw null;
	}
}
