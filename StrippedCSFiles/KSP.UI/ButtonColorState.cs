using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

[Serializable]
public class ButtonColorState
{
	public Color normalColor;

	public Color highlightColor;

	public Color pressedColor;

	public Color disabledColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ButtonColorState()
	{
		throw null;
	}
}
