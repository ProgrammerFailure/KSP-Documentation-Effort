using System.Runtime.CompilerServices;
using UnityEngine;

public static class Drawing
{
	private static Texture2D aaLineTex;

	private static Texture2D lineTex;

	private static Material blitMaterial;

	private static Material blendMaterial;

	private static Rect lineRect;

	private static bool lineAntiAlias;

	private static float lineWidth;

	private static float boxBorder;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Drawing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DrawGraph(Vector2[] points, float[] xGrads, Color axisColor, Color lineColor, float height, float width = 0f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DrawGraphLine(Vector2 start, Vector2 end, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float GetMaxValueX(Vector2[] points)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float GetMaxValueY(Vector2[] points)
	{
		throw null;
	}
}
