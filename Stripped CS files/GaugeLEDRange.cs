using System;
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

	public GClass5.colorIndices color;

	public float blinkInterval;

	public AudioClip soundClip;
}
