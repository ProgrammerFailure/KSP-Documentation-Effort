using System;
using UnityEngine;

[Serializable]
public class Vector4Curve
{
	[SerializeField]
	public AnimationCurve x;

	[SerializeField]
	public AnimationCurve y;

	[SerializeField]
	public AnimationCurve z;

	[SerializeField]
	public AnimationCurve w;

	public float minTime { get; set; }

	public float maxTime { get; set; }

	public Vector4Curve()
	{
		x = new AnimationCurve();
		y = new AnimationCurve();
		z = new AnimationCurve();
		w = new AnimationCurve();
		minTime = float.MaxValue;
		maxTime = float.MinValue;
	}

	public void Add(float time, Vector4 value)
	{
		x.AddKey(time, value.x);
		y.AddKey(time, value.y);
		z.AddKey(time, value.z);
		w.AddKey(time, value.w);
		minTime = Mathf.Min(minTime, time);
		maxTime = Mathf.Max(maxTime, time);
	}

	public void Add(float time, Color value)
	{
		x.AddKey(time, value.r);
		y.AddKey(time, value.g);
		z.AddKey(time, value.b);
		w.AddKey(time, value.a);
		minTime = Mathf.Min(minTime, time);
		maxTime = Mathf.Max(maxTime, time);
	}

	public Vector4 EvaluateVector(float time)
	{
		return new Vector4(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time), w.Evaluate(time));
	}

	public Color EvaluateColor(float time)
	{
		return new Color(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time), w.Evaluate(time));
	}
}
