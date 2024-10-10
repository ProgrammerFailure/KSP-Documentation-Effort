using System;
using UnityEngine;

[Serializable]
public class ScreenMessage
{
	public ScreenMessagesText textInstance;

	public string message = "";

	public float duration = 3f;

	public float startTime;

	public Color color = Color.white;

	public ScreenMessageStyle style = ScreenMessageStyle.LOWER_CENTER;

	public ScreenMessage(string message, float duration, ScreenMessageStyle style)
	{
		this.message = message;
		this.duration = duration;
		this.style = style;
		startTime = 0f;
		if ((bool)ScreenMessages.Instance)
		{
			color = ScreenMessages.Instance.defaultColor;
		}
	}
}
