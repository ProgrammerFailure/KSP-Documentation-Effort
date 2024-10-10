using UnityEngine;

namespace EdyCommonTools;

public static class MathUtility
{
	public static float ClampAngle(float angle)
	{
		angle %= 360f;
		if (angle > 180f)
		{
			angle -= 360f;
		}
		else if (angle < -180f)
		{
			angle += 360f;
		}
		return angle;
	}

	public static float ClampAngle360(float angle)
	{
		angle %= 360f;
		if (angle < 0f)
		{
			angle += 360f;
		}
		return angle;
	}

	public static float FastAbs(float x)
	{
		if (!(x < 0f))
		{
			return x;
		}
		return 0f - x;
	}

	public static float MaxAbs(float a, float b)
	{
		if (!(FastAbs(a) >= FastAbs(b)))
		{
			return b;
		}
		return a;
	}

	public static float MinAbs(float a, float b)
	{
		if (!(FastAbs(a) < FastAbs(b)))
		{
			return b;
		}
		return a;
	}

	public static bool Vector3Equals(Vector3 a, Vector3 b)
	{
		if (a.x == b.x && a.y == b.y)
		{
			return a.z == b.z;
		}
		return false;
	}

	public static float ClampMagnitude(float a, float min, float max)
	{
		if (!(a >= 0f))
		{
			return 0f - Mathf.Clamp(0f - a, min, max);
		}
		return Mathf.Clamp(a, min, max);
	}

	public static bool IsSimilarOrSmaller(float a, float b, float threshold = 0.0001f)
	{
		return a - b < threshold;
	}

	public static bool IsSimilarOrGreater(float a, float b, float threshold = 0.0001f)
	{
		return a - b > 0f - threshold;
	}

	public static bool IsSimilar(float a, float b, float threshold = 0.0001f)
	{
		return FastAbs(a - b) < threshold;
	}

	public static float Lin2Log(float val)
	{
		return Mathf.Log(FastAbs(val) + 1f) * Mathf.Sign(val);
	}

	public static Vector3 Lin2Log(Vector3 val)
	{
		return Vector3.ClampMagnitude(val, Lin2Log(val.magnitude));
	}

	public static Vector3 ClosestPointOnPlane(Vector3 planePoint, Vector3 planeNormal, Vector3 point)
	{
		Vector3 vector = Vector3.Project(point - planePoint, planeNormal);
		return point - vector;
	}

	public static float UnclampedLerp(float from, float to, float t)
	{
		return from + (to - from) * t;
	}

	public static float LinearLerp(float x0, float y0, float x1, float y1, float x)
	{
		return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
	}

	public static float LinearLerp(Vector2 from, Vector2 to, float t)
	{
		return LinearLerp(from.x, from.y, to.x, to.y, t);
	}

	public static float CubicLerp(float x0, float y0, float x1, float y1, float x)
	{
		float num = (x - x0) / (x1 - x0);
		float num2 = num * num;
		float num3 = num * num2;
		return y0 * (2f * num3 - 3f * num2 + 1f) + y1 * (-2f * num3 + 3f * num2);
	}

	public static float CubicLerp(Vector2 from, Vector2 to, float t)
	{
		return CubicLerp(from.x, from.y, to.x, to.y, t);
	}

	public static float TangentLerp(float x0, float y0, float x1, float y1, float a, float b, float x)
	{
		float num = y1 - y0;
		float num2 = 3f * num * a;
		float num3 = 3f * num * b;
		float num4 = (x - x0) / (x1 - x0);
		float num5 = num4 * num4;
		float num6 = num4 * num5;
		return y0 * (2f * num6 - 3f * num5 + 1f) + y1 * (-2f * num6 + 3f * num5) + num2 * (num6 - 2f * num5 + num4) + num3 * (num6 - num5);
	}

	public static float TangentLerp(Vector2 from, Vector2 to, float a, float b, float t)
	{
		return TangentLerp(from.x, from.y, to.x, to.y, a, b, t);
	}

	public static float SmoothStep(float from, float to, float t)
	{
		t = Mathf.Clamp01(t);
		t = t * t * (3f - 2f * t);
		return from + (to - from) * t;
	}

	public static float SmootherStep(float from, float to, float t)
	{
		t = Mathf.Clamp01(t);
		t = t * t * t * (t * (6f * t - 15f) + 10f);
		return from + (to - from) * t;
	}

	public static float GetMultiplier(int decimals)
	{
		float result = 1f;
		switch (decimals)
		{
		default:
			result = ((decimals > 0) ? Mathf.Pow(10f, decimals) : 1f);
			break;
		case 1:
			result = 10f;
			break;
		case 2:
			result = 100f;
			break;
		case 3:
			result = 1000f;
			break;
		case 4:
			result = 10000f;
			break;
		case 0:
			break;
		}
		return result;
	}

	public static float FloorDecimals(float value, int decimals)
	{
		float multiplier = GetMultiplier(decimals);
		return Mathf.Floor(value * multiplier) / multiplier;
	}

	public static Vector3 FloorDecimals(Vector3 value, int decimals)
	{
		float multiplier = GetMultiplier(decimals);
		return new Vector3(Mathf.Floor(value.x * multiplier) / multiplier, Mathf.Floor(value.y * multiplier) / multiplier, Mathf.Floor(value.z * multiplier) / multiplier);
	}

	public static float RoundDecimals(float value, int decimals)
	{
		float multiplier = GetMultiplier(decimals);
		return Mathf.Round(value * multiplier) / multiplier;
	}

	public static Vector3 RoundDecimals(Vector3 value, int decimals)
	{
		float multiplier = GetMultiplier(decimals);
		return new Vector3(Mathf.Round(value.x * multiplier) / multiplier, Mathf.Round(value.y * multiplier) / multiplier, Mathf.Round(value.z * multiplier) / multiplier);
	}
}
