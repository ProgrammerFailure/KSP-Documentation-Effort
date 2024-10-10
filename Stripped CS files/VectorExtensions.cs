using System;
using UnityEngine;

public static class VectorExtensions
{
	public static void Zero(this Vector3 v)
	{
		v.x = 0f;
		v.y = 0f;
		v.z = 0f;
	}

	public static void Zero(this Vector2 v)
	{
		v.x = 0f;
		v.y = 0f;
	}

	public static bool IsZero(this Vector3 v)
	{
		if (v.x == 0f && v.y == 0f)
		{
			return v.z == 0f;
		}
		return false;
	}

	public static bool IsZero(this Vector2 v)
	{
		if (v.x == 0f)
		{
			return v.y == 0f;
		}
		return false;
	}

	public static bool IsInvalid(this Vector3 v)
	{
		float f = v.x + v.y + v.z;
		if (!float.IsNaN(f))
		{
			return float.IsInfinity(f);
		}
		return true;
	}

	public static bool IsInvalid(this Vector2 v)
	{
		float f = v.x + v.y;
		if (!float.IsNaN(f))
		{
			return float.IsInfinity(f);
		}
		return true;
	}

	public static bool IsInvalid(this Vector3d v)
	{
		double d = v.x + v.y + v.z;
		if (!double.IsNaN(d))
		{
			return double.IsInfinity(d);
		}
		return true;
	}

	public static bool IsSmallerThan(this Vector3 v, float length = 0.1f)
	{
		return v.x * v.x + v.y * v.y + v.z * v.z < length * length;
	}

	public static bool IsSmallerThan(this Vector2 v, float length = 0.1f)
	{
		return v.x * v.x + v.y * v.y < length * length;
	}

	public static float HeadingRadians(this Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return UtilMath.WrapAround(Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis)), 0f, (float)Math.PI * 2f);
	}

	public static float HeadingDegrees(this Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return UtilMath.WrapAround(Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis)) * 57.29578f, 0f, 360f);
	}

	public static float BearingRadians(this Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis));
	}

	public static float BearingDegrees(this Vector3 v1, Vector3 v2, Vector3 upAxis)
	{
		if (v1 == v2)
		{
			return 0f;
		}
		return Mathf.Acos(Vector3.Dot(v1, v2)) * Mathf.Sign(Vector3.Dot(Vector3.Cross(v1, v2), upAxis)) * 57.29578f;
	}
}
