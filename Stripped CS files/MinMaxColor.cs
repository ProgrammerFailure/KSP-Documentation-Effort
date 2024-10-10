using System;
using UnityEngine;

[Serializable]
public class MinMaxColor
{
	public Color min;

	public Color max;

	public Color GetLerp(float t)
	{
		return Color.Lerp(min, max, t);
	}
}
