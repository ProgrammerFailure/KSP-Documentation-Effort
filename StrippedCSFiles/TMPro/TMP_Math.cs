using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public static class TMP_Math
{
	public const float FLOAT_MAX = 32767f;

	public const float FLOAT_MIN = -32767f;

	public const int INT_MAX = int.MaxValue;

	public const int INT_MIN = -2147483647;

	public const float FLOAT_UNSET = -32767f;

	public const int INT_UNSET = -32767;

	public static Vector2 MAX_16BIT;

	public static Vector2 MIN_16BIT;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_Math()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool Approximately(float a, float b)
	{
		throw null;
	}
}
