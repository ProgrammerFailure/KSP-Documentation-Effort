using System;
using UnityEngine;

[Serializable]
public class MinMaxFloat
{
	public float min;

	public float max;

	public float GetLerp(float t)
	{
		return Mathf.Lerp(min, max, t);
	}

	public float GetInverseLerp(float v)
	{
		return Mathf.InverseLerp(min, max, v);
	}
}
