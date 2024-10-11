using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public static class TMP_TextUtilities
{
	private struct LineSegment
	{
		public Vector3 Point1;

		public Vector3 Point2;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LineSegment(Vector3 p1, Vector3 p2)
		{
			throw null;
		}
	}

	private static Vector3[] m_rectWorldCorners;

	private const string k_lookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

	private const string k_lookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_TextUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera, out CaretPosition cursor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindNearestLine(TMP_Text text, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindNearestCharacterOnLine(TMP_Text text, Vector3 position, int line, Camera camera, bool visibleOnly)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindIntersectingCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindNearestCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindIntersectingWord(TMP_Text text, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindNearestWord(TMP_Text text, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindIntersectingLine(TMP_Text text, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindIntersectingLink(TMP_Text text, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int FindNearestLink(TMP_Text text, Vector3 position, Camera camera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IntersectLinePlane(LineSegment line, Vector3 point, Vector3 normal, out Vector3 intersectingPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static char ToLowerFast(char c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static char ToUpperFast(char c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetSimpleHashCode(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint GetSimpleHashCodeLowercase(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int HexToInt(char hex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int StringToInt(string s)
	{
		throw null;
	}
}
