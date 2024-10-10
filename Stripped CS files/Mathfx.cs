using System;
using UnityEngine;

public class Mathfx
{
	public static float Hermite(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value * value * (3f - 2f * value));
	}

	public static float HermiteIntegral()
	{
		return 0.5f;
	}

	public static float SinerpIntegral()
	{
		return 2f / (float)Math.PI;
	}

	public static float CubicHermiteSpline(float start, float end, float v, float easeIn, float easeOut)
	{
		float num = Mathf.Pow(v, 2f);
		float num2 = Mathf.Pow(v, 3f);
		return Mathf.Lerp(start, end, (num2 - 2f * num + v) * easeIn + (-2f * num2 + 3f * num) + (num2 - num) * easeOut);
	}

	public static float Easerp(float start, float end, float v)
	{
		return Mathf.Lerp(start, end, Mathf.Pow(Mathf.Sin(v * 1.5707f), 2f));
	}

	public static float Easerp(float start, float end, float v, float power)
	{
		return Mathf.Lerp(start, end, (Mathf.Pow(Mathf.Sin(v * 1.5707f), power) + (1f - Mathf.Pow(Mathf.Cos(v * 1.5707f), power))) / 2f);
	}

	public static float Ease(float v, float power)
	{
		return (Mathf.Pow(Mathf.Sin(v * 1.5707f), power) + (1f - Mathf.Pow(Mathf.Cos(v * 1.5707f), power))) / 2f;
	}

	public static float Sinerp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(value * (float)Math.PI * 0.5f));
	}

	public static float Coserp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(value * (float)Math.PI * 0.5f));
	}

	public static float XLerp(float start, float end, float value, float power)
	{
		return Mathf.Lerp(start, end, Mathf.Pow(Mathf.Clamp01(value), power));
	}

	public static float Berp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * (float)Math.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float num = (x - min) / (max - min);
		float num2 = (x - min) / (max - min);
		return -2f * num * num * num + 3f * num2 * num2;
	}

	public static float Lerp(float start, float end, float value)
	{
		return (1f - value) * start + value * end;
	}

	public static double Lerp(double start, double end, double value)
	{
		return (1.0 - value) * start + value * end;
	}

	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
		float num = Vector3.Dot(point - lineStart, vector) / Vector3.Dot(vector, vector);
		return lineStart + num * vector;
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = lineEnd - lineStart;
		Vector3 vector2 = Vector3.Normalize(vector);
		float value = Vector3.Dot(point - lineStart, vector2) / Vector3.Dot(vector2, vector2);
		return lineStart + Mathf.Clamp(value, 0f, Vector3.Magnitude(vector)) * vector2;
	}

	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	public static bool Approx(float val, float about, float range)
	{
		return Mathf.Abs(val - about) < range;
	}

	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		return (val - about).sqrMagnitude < range * range;
	}

	public static float Clerp(float start, float end, float value)
	{
		float num = 360f;
		float num2 = Mathf.Abs(180f);
		float num3 = 0f;
		float num4 = 0f;
		if (end - start < 0f - num2)
		{
			num4 = (num - start + end) * value;
			return start + num4;
		}
		if (end - start > num2)
		{
			num4 = (0f - (num - end + start)) * value;
			return start + num4;
		}
		return start + (end - start) * value;
	}

	public static Vector3 GetWeightedAvgVector3(int count, Func<int, Vector3> getVector, Func<int, float> getWeight)
	{
		Vector3 zero = Vector3.zero;
		float num = 0f;
		int arg = count;
		while (arg-- > 0)
		{
			float num2 = getWeight(arg);
			zero += getVector(arg) * num2;
			num += num2;
		}
		if (num != 0f)
		{
			return zero / num;
		}
		return zero;
	}
}
