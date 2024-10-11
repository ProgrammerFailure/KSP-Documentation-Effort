using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class GaugeLEDRange
{
	public enum LedAction
	{
		off,
		on,
		blink
	}

	public float minValue;

	public float maxValue;

	public LedAction ledAction;

	public LED.colorIndices color;

	public float blinkInterval;

	public AudioClip soundClip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GaugeLEDRange()
	{
		throw null;
	}
}
