using System.Reflection;
using UnityEngine;

public static class Drawing
{
	public static Texture2D aaLineTex;

	public static Texture2D lineTex;

	public static Material blitMaterial;

	public static Material blendMaterial;

	public static Rect lineRect;

	public static bool lineAntiAlias;

	public static float lineWidth;

	public static float boxBorder;

	static Drawing()
	{
		aaLineTex = null;
		lineTex = null;
		blitMaterial = null;
		blendMaterial = null;
		lineRect = new Rect(0f, 0f, 1f, 1f);
		lineAntiAlias = true;
		lineWidth = 1f;
		boxBorder = 4f;
		Initialize();
	}

	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
	{
		float num = pointB.x - pointA.x;
		float num2 = pointB.y - pointA.y;
		float num3 = Mathf.Sqrt(num * num + num2 * num2);
		if (!(num3 < 0.001f))
		{
			Texture2D texture;
			Material mat;
			if (antiAlias)
			{
				width *= 3f;
				texture = aaLineTex;
				mat = blendMaterial;
			}
			else
			{
				texture = lineTex;
				mat = blitMaterial;
			}
			float num4 = width * num2 / num3;
			float num5 = width * num / num3;
			Matrix4x4 identity = Matrix4x4.identity;
			identity.m00 = num;
			identity.m01 = 0f - num4;
			identity.m03 = pointA.x + 0.5f * num4;
			identity.m10 = num2;
			identity.m11 = num5;
			identity.m13 = pointA.y - 0.5f * num5;
			GL.PushMatrix();
			GL.MultMatrix(identity);
			Graphics.DrawTexture(lineRect, texture, lineRect, 0, 0, 0, 0, color, mat);
			GL.PopMatrix();
		}
	}

	public static void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
	{
		Vector2 pointA = CubeBezier(start, startTangent, end, endTangent, 0f);
		for (int i = 1; i < segments; i++)
		{
			Vector2 vector = CubeBezier(start, startTangent, end, endTangent, (float)i / (float)segments);
			DrawLine(pointA, vector, color, width, antiAlias);
			pointA = vector;
		}
	}

	public static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
	{
		float num = 1f - t;
		return num * num * num * s + 3f * num * num * t * st + 3f * num * t * t * et + t * t * t * e;
	}

	public static void Initialize()
	{
		if (lineTex == null)
		{
			lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, mipChain: false);
			lineTex.SetPixel(0, 1, Color.white);
			lineTex.Apply();
		}
		if (aaLineTex == null)
		{
			aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, mipChain: false);
			aaLineTex.SetPixel(0, 0, new Color(1f, 1f, 1f, 0f));
			aaLineTex.SetPixel(0, 1, Color.white);
			aaLineTex.SetPixel(0, 2, new Color(1f, 1f, 1f, 0f));
			aaLineTex.Apply();
		}
		blitMaterial = (Material)typeof(GUI).GetMethod("get_blitMaterial", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
		blendMaterial = (Material)typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
	}

	public static void DrawGraph(Vector2[] points, float[] xGrads, Color axisColor, Color lineColor, float height, float width = 0f)
	{
		if (width == 0f)
		{
			GUILayout.Box("", GUILayout.ExpandWidth(expand: true), GUILayout.Height(height));
		}
		else
		{
			GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(height));
		}
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		Rect lastRect = GUILayoutUtility.GetLastRect();
		lastRect = new Rect(lastRect.xMin + boxBorder, lastRect.yMin + boxBorder, lastRect.width - boxBorder * 2f, lastRect.height - boxBorder * 2f);
		float maxValueX = GetMaxValueX(points);
		float maxValueY = GetMaxValueY(points);
		float num = lastRect.width / maxValueX;
		float num2 = lastRect.height / maxValueY;
		Vector2 start = new Vector2(lastRect.xMin, lastRect.yMax);
		DrawGraphLine(start, new Vector2(start.x, start.y - lastRect.height), lineColor);
		DrawGraphLine(start, new Vector2(start.x + lastRect.width, start.y), lineColor);
		GUI.Label(new Rect(lastRect.xMax - 30f, lastRect.yMax - 20f, 30f, 20f), maxValueX.ToString("F2"));
		GUI.Label(new Rect(lastRect.xMin + 2f, lastRect.yMin - 4f, 30f, 20f), maxValueY.ToString("F2"));
		if (xGrads != null)
		{
			for (int i = 0; i < xGrads.Length; i++)
			{
				DrawGraphLine(new Vector2(start.x + xGrads[i] * num, start.y), new Vector2(start.x + xGrads[i] * num, start.y - 4f), lineColor);
				GUI.Label(new Rect(start.x + xGrads[i] * num - 15f, lastRect.yMax - 20f, 24f, 20f), xGrads[i].ToString("F2"));
			}
		}
		Vector2 start2 = new Vector2(start.x + points[0].x * num, start.y - points[0].y * num2);
		for (int j = 1; j < points.Length; j++)
		{
			Vector2 vector = new Vector2(start.x + points[j].x * num, start.y - points[j].y * num2);
			DrawGraphLine(start2, vector, Color.green);
			start2 = vector;
		}
	}

	public static void DrawGraphLine(Vector2 start, Vector2 end, Color color)
	{
		DrawLine(start, end, color, lineWidth, lineAntiAlias);
	}

	public static float GetMaxValueX(Vector2[] points)
	{
		float num = 0f;
		int num2 = points.Length;
		while (num2-- > 0)
		{
			if (points[num2].x > num)
			{
				num = points[num2].x;
			}
		}
		return num;
	}

	public static float GetMaxValueY(Vector2[] points)
	{
		float num = 0f;
		int num2 = points.Length;
		while (num2-- > 0)
		{
			if (points[num2].y > num)
			{
				num = points[num2].y;
			}
		}
		return num;
	}
}
