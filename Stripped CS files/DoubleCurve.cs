using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DoubleCurve : IConfigNode
{
	public List<DoubleKeyframe> keys;

	public double minTime;

	public double maxTime;

	public static char[] delimiters = new char[4] { ' ', ',', ';', '\t' };

	public static int findCurveMinMaxInterations = 100;

	public DoubleCurve()
	{
		minTime = double.MaxValue;
		maxTime = double.MinValue;
		keys = new List<DoubleKeyframe>();
	}

	public DoubleCurve(DoubleKeyframe[] keyframes)
	{
		keys = new List<DoubleKeyframe>(keyframes);
		minTime = ((keys.Count > 0) ? keys[0].time : double.MaxValue);
		maxTime = ((keys.Count > 0) ? keys[keys.Count - 1].time : double.MinValue);
	}

	public DoubleCurve(List<DoubleKeyframe> keyframes)
	{
		keys = new List<DoubleKeyframe>(keyframes);
		minTime = ((keys.Count > 0) ? keys[0].time : double.MaxValue);
		maxTime = ((keys.Count > 0) ? keys[keys.Count - 1].time : double.MinValue);
	}

	public int GetInsertionIndex(double time)
	{
		int result = 0;
		int count = keys.Count;
		while (count-- > 0)
		{
			if (!(keys[count].time >= time))
			{
				result = count + 1;
				break;
			}
		}
		return result;
	}

	public void Add(double time, double value)
	{
		keys.Insert(GetInsertionIndex(time), new DoubleKeyframe(time, value));
		minTime = Math.Min(minTime, time);
		maxTime = Math.Max(maxTime, time);
		RecomputeTangents();
	}

	public void Add(double time, double value, double inTangent, double outTangent)
	{
		keys.Insert(GetInsertionIndex(time), new DoubleKeyframe(time, value, inTangent, outTangent));
		minTime = Math.Min(minTime, time);
		maxTime = Math.Max(maxTime, time);
		RecomputeTangents();
	}

	public double Evaluate(double t)
	{
		if (keys.Count == 0)
		{
			return 0.0;
		}
		int num = keys.Count - 1;
		int num2 = 0;
		if (t < minTime)
		{
			return keys[num2].value;
		}
		if (t > maxTime)
		{
			return keys[num].value;
		}
		int num3;
		while (true)
		{
			if (num - num2 > 1)
			{
				num3 = (num + num2) / 2;
				double time = keys[num3].time;
				if (time == t)
				{
					break;
				}
				if (time < t)
				{
					num2 = num3;
				}
				else
				{
					num = num3;
				}
				continue;
			}
			DoubleKeyframe doubleKeyframe = keys[num2];
			DoubleKeyframe doubleKeyframe2 = keys[num];
			double num4 = doubleKeyframe2.time - doubleKeyframe.time;
			double num5 = doubleKeyframe.outTangent * num4;
			double num6 = doubleKeyframe2.inTangent * num4;
			t = (t - doubleKeyframe.time) / num4;
			double num7 = t * t;
			double num8 = num7 * t;
			double num9 = 2.0 * num8 - 3.0 * num7 + 1.0;
			double num10 = num8 - 2.0 * num7 + t;
			double num11 = num8 - num7;
			double num12 = -2.0 * num8 + 3.0 * num7;
			return num9 * doubleKeyframe.value + num10 * num5 + num11 * num6 + num12 * doubleKeyframe2.value;
		}
		return keys[num3].value;
	}

	public void RecomputeTangents()
	{
		int count = keys.Count;
		DoubleKeyframe value;
		if (count == 1)
		{
			value = keys[0];
			double outTangent = 0.0;
			value.inTangent = 0.0;
			value.outTangent = outTangent;
			keys[0] = value;
			return;
		}
		value = keys[0];
		if (value.autoTangent)
		{
			value.inTangent = 0.0;
			value.outTangent = (keys[1].value - value.value) / (keys[1].time - value.time);
			keys[0] = value;
		}
		int num = count - 1;
		value = keys[num];
		if (value.autoTangent)
		{
			value.inTangent = (value.value - keys[num - 1].value) / (value.time - keys[num - 1].value);
			value.outTangent = 0.0;
			keys[num] = value;
		}
		if (count <= 2)
		{
			return;
		}
		for (int i = 1; i < num; i++)
		{
			value = keys[i];
			if (value.autoTangent)
			{
				double num2 = (value.value - keys[i - 1].value) / (value.time - keys[i - 1].value);
				double num3 = (keys[i + 1].value - value.value) / (keys[i + 1].time - value.time);
				value.inTangent = (value.outTangent = (num2 + num3) * 0.5);
			}
		}
	}

	public void Load(ConfigNode node)
	{
		keys.Clear();
		minTime = double.MaxValue;
		maxTime = double.MinValue;
		string[] values = node.GetValues("key");
		int num = values.Length;
		for (int i = 0; i < num; i++)
		{
			string[] array = values[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 2)
			{
				Debug.LogError("DoubleCurve: Invalid line. Requires two values, 'time' and 'value', or four values, 'time', 'value', 'inTangent', and 'outTangent'.");
			}
			if (array.Length == 4)
			{
				Add(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]), double.Parse(array[3]));
			}
			else
			{
				Add(double.Parse(array[0]), double.Parse(array[1]));
			}
		}
	}

	public void Save(ConfigNode node)
	{
		for (int i = 0; i < keys.Count; i++)
		{
			DoubleKeyframe doubleKeyframe = keys[i];
			if (doubleKeyframe.autoTangent)
			{
				node.AddValue("key", doubleKeyframe.time + " " + doubleKeyframe.value);
				continue;
			}
			node.AddValue("key", doubleKeyframe.time + " " + doubleKeyframe.value + " " + doubleKeyframe.inTangent + " " + doubleKeyframe.outTangent);
		}
	}

	public void FindMinMaxValue(out double min, out double max)
	{
		min = 0.0;
		max = 0.0;
		min = double.MaxValue;
		max = double.MinValue;
		double num = double.MaxValue;
		double num2 = double.MinValue;
		for (int i = 0; i < keys.Count; i++)
		{
			if (keys[i].time < num)
			{
				num = keys[i].time;
			}
			if (keys[i].time > num2)
			{
				num2 = keys[i].time;
			}
		}
		double num3 = (num2 - num) / (double)findCurveMinMaxInterations;
		for (int j = 0; j < findCurveMinMaxInterations; j++)
		{
			double num4 = Evaluate(num + (double)j * num3);
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

	public void FindMinMaxValue(out double min, out double max, out double tMin, out double tMax)
	{
		min = 0.0;
		max = 0.0;
		tMin = 0.0;
		tMax = 0.0;
		min = double.MaxValue;
		max = double.MinValue;
		double num = double.MaxValue;
		double num2 = double.MinValue;
		for (int i = 0; i < keys.Count; i++)
		{
			if (keys[i].time < num)
			{
				num = keys[i].time;
			}
			if (keys[i].time > num2)
			{
				num2 = keys[i].time;
			}
		}
		double num3 = (num2 - num) / (double)findCurveMinMaxInterations;
		for (int j = 0; j < findCurveMinMaxInterations; j++)
		{
			double num4 = num + (double)j * num3;
			double num5 = Evaluate(num4);
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
