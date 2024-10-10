using System;
using UnityEngine;

[Serializable]
public class FloatCurve : IConfigNode
{
	[SerializeField]
	public AnimationCurve fCurve;

	[SerializeField]
	public float _minTime;

	[SerializeField]
	public float _maxTime;

	public static char[] delimiters = new char[4] { ' ', ',', ';', '\t' };

	public static int findCurveMinMaxInterations = 100;

	public AnimationCurve Curve
	{
		get
		{
			return fCurve;
		}
		set
		{
			fCurve = value;
		}
	}

	public float minTime => _minTime;

	public float maxTime => _maxTime;

	public FloatCurve()
	{
		fCurve = new AnimationCurve();
		_minTime = float.MaxValue;
		_maxTime = float.MinValue;
		fCurve.postWrapMode = WrapMode.ClampForever;
		fCurve.preWrapMode = WrapMode.ClampForever;
	}

	public FloatCurve(Keyframe[] keyframes)
		: this()
	{
		for (int i = 0; i < keyframes.Length; i++)
		{
			Add(keyframes[i].time, keyframes[i].value, keyframes[i].inTangent, keyframes[i].outTangent);
		}
	}

	public void Add(float time, float value)
	{
		fCurve.AddKey(time, value);
		_minTime = Mathf.Min(minTime, time);
		_maxTime = Mathf.Max(maxTime, time);
	}

	public void Add(float time, float value, float inTangent, float outTangent)
	{
		Keyframe key = default(Keyframe);
		key.inTangent = inTangent;
		key.outTangent = outTangent;
		key.time = time;
		key.value = value;
		fCurve.AddKey(key);
		_minTime = Mathf.Min(minTime, time);
		_maxTime = Mathf.Max(maxTime, time);
	}

	public float Evaluate(float time)
	{
		return fCurve.Evaluate(time);
	}

	public void Load(ConfigNode node)
	{
		fCurve = new AnimationCurve();
		_minTime = float.MaxValue;
		_maxTime = float.MinValue;
		string[] values = node.GetValues("key");
		int num = values.Length;
		for (int i = 0; i < num; i++)
		{
			string[] array = values[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 2)
			{
				Debug.LogError("FloatCurve: Invalid line. Requires two values, 'time' and 'value'");
			}
			if (array.Length == 4)
			{
				Add(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
			}
			else
			{
				Add(float.Parse(array[0]), float.Parse(array[1]));
			}
		}
	}

	public void Save(ConfigNode node)
	{
		for (int i = 0; i < fCurve.keys.Length; i++)
		{
			node.AddValue("key", fCurve.keys[i].time + " " + fCurve.keys[i].value + " " + fCurve.keys[i].inTangent + " " + fCurve.keys[i].outTangent);
		}
	}

	public void FindMinMaxValue(out float min, out float max)
	{
		min = 0f;
		max = 0f;
		if (fCurve == null)
		{
			return;
		}
		min = float.MaxValue;
		max = float.MinValue;
		float num = float.MaxValue;
		float num2 = float.MinValue;
		for (int i = 0; i < fCurve.keys.Length; i++)
		{
			if (fCurve.keys[i].time < num)
			{
				num = fCurve.keys[i].time;
			}
			if (fCurve.keys[i].time > num2)
			{
				num2 = fCurve.keys[i].time;
			}
		}
		float num3 = (num2 - num) / (float)findCurveMinMaxInterations;
		for (int j = 0; j < findCurveMinMaxInterations; j++)
		{
			float num4 = fCurve.Evaluate(num + (float)j * num3);
			if (num4 < min)
			{
				min = num4;
			}
			if (num4 > max)
			{
				max = num4;
			}
		}
	}

	public void FindMinMaxValue(out float min, out float max, out float tMin, out float tMax)
	{
		min = 0f;
		max = 0f;
		tMin = 0f;
		tMax = 0f;
		if (fCurve == null)
		{
			return;
		}
		min = float.MaxValue;
		max = float.MinValue;
		float num = float.MaxValue;
		float num2 = float.MinValue;
		for (int i = 0; i < fCurve.keys.Length; i++)
		{
			if (fCurve.keys[i].time < num)
			{
				num = fCurve.keys[i].time;
			}
			if (fCurve.keys[i].time > num2)
			{
				num2 = fCurve.keys[i].time;
			}
		}
		float num3 = (num2 - num) / (float)findCurveMinMaxInterations;
		for (int j = 0; j < findCurveMinMaxInterations; j++)
		{
			float num4 = num + (float)j * num3;
			float num5 = fCurve.Evaluate(num4);
			if (num5 < min)
			{
				min = num5;
				tMin = num4;
			}
			if (num5 > max)
			{
				max = num5;
				tMax = num4;
			}
		}
	}
}
