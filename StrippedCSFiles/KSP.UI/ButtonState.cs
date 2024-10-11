using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[Serializable]
public class ButtonState
{
	public string name;

	public Sprite normal;

	public Sprite highlight;

	public Sprite pressed;

	public Sprite disabled;

	public Color textColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ButtonState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(Button button, Image image, TextMeshProUGUI text = null)
	{
		throw null;
	}
}
