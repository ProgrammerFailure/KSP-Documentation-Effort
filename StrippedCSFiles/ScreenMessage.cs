using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ScreenMessage
{
	public ScreenMessagesText textInstance;

	public string message;

	public float duration;

	public float startTime;

	public Color color;

	public ScreenMessageStyle style;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenMessage(string message, float duration, ScreenMessageStyle style)
	{
		throw null;
	}
}
