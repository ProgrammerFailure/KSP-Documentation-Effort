using UnityEngine;

namespace EdyCommonTools;

public static class SplineUtility
{
	public static float HermiteLerp(float x0, float y0, float x1, float y1, float outTangent, float inTangent, float x)
	{
		float num = (x - x0) / (x1 - x0);
		float num2 = num * num;
		float num3 = num * num2;
		return y0 * (2f * num3 - 3f * num2 + 1f) + y1 * (-2f * num3 + 3f * num2) + outTangent * (num3 - 2f * num2 + num) + inTangent * (num3 - num2);
	}

	public static Vector3 Hermite(Vector3 p0, Vector3 p1, Vector3 t0, Vector3 t1, float s)
	{
		float num = s * s;
		float num2 = num * s;
		float num3 = 2f * num2 - 3f * num + 1f;
		float num4 = -2f * num2 + 3f * num;
		float num5 = num2 - 2f * num + s;
		float num6 = num2 - num;
		return p0 * num3 + p1 * num4 + t0 * num5 + t1 * num6;
	}

	public static Vector3 HermiteTangent(Vector3 p0, Vector3 p1, Vector3 t0, Vector3 t1, float s)
	{
		float num = s * s;
		float num2 = 6f * num - 6f * s;
		float num3 = -6f * num + 6f * s;
		float num4 = 3f * num - 4f * s + 1f;
		float num5 = 3f * num - 2f * s;
		return num2 * p0 + num3 * p1 + num4 * t0 + num5 * t1;
	}

	public static Vector3 GetNormal(Vector3 tangent, Vector3 up)
	{
		Vector3 normalized = Vector3.Cross(up, tangent).normalized;
		return Vector3.Cross(tangent, normalized);
	}

	public static Vector3 Bezier(Vector3[] points, float t)
	{
		float num = 1f - t;
		float num2 = num * num;
		float num3 = t * t;
		return points[0] * (num2 * num) + points[1] * (3f * num2 * t) + points[2] * (3f * num * num3) + points[3] * (num3 * t);
	}

	public static Vector3 BezierTangent(Vector3[] points, float t)
	{
		float num = 1f - t;
		float num2 = num * num;
		float num3 = t * t;
		return (points[0] * (0f - num2) + points[1] * (3f * num2 - 2f * num) + points[2] * (-3f * num3 + 2f * t) + points[3] * num3).normalized;
	}

	public static Vector3 BezierNormal(Vector3[] points, float t, Vector3 up)
	{
		Vector3 vector = BezierTangent(points, t);
		Vector3 normalized = Vector3.Cross(up, vector).normalized;
		return Vector3.Cross(vector, normalized);
	}
}
