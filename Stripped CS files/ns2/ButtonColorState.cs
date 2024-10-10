using System;
using UnityEngine;

namespace ns2;

[Serializable]
public class ButtonColorState
{
	public Color normalColor = Color.white;

	public Color highlightColor = Color.white;

	public Color pressedColor = Color.white;

	public Color disabledColor = Color.white;
}
