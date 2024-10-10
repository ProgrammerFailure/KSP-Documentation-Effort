using UnityEngine;

namespace ProceduralFairings;

public class MeshPoint
{
	public Vector3 vert;

	public Vector3 normal;

	public Vector2 uv1;

	public Vector2 uv2;

	public Color color;

	public int index;

	public void Clear()
	{
		vert = Vector3.zero;
		normal = Vector3.zero;
		uv1 = Vector2.zero;
		uv2 = Vector2.zero;
		color = Color.clear;
		index = 0;
	}

	public static Vector3[] GetVerts(MeshPoint[] meshPoints, Vector3 offset)
	{
		Vector3[] array = new Vector3[meshPoints.Length];
		int num = array.Length;
		while (num-- > 0)
		{
			array[num] = meshPoints[num].vert + offset;
		}
		return array;
	}

	public static Vector3[] GetNormals(MeshPoint[] meshPoints)
	{
		Vector3[] array = new Vector3[meshPoints.Length];
		int num = array.Length;
		while (num-- > 0)
		{
			array[num] = meshPoints[num].normal;
		}
		return array;
	}

	public static Vector2[] GetUV1(MeshPoint[] meshPoints)
	{
		Vector2[] array = new Vector2[meshPoints.Length];
		int num = array.Length;
		while (num-- > 0)
		{
			array[num] = meshPoints[num].uv1;
		}
		return array;
	}

	public static Vector2[] GetUV2(MeshPoint[] meshPoints)
	{
		Vector2[] array = new Vector2[meshPoints.Length];
		int num = array.Length;
		while (num-- > 0)
		{
			array[num] = meshPoints[num].uv2;
		}
		return array;
	}

	public static Color[] GetColors(MeshPoint[] meshPoints)
	{
		Color[] array = new Color[meshPoints.Length];
		int num = array.Length;
		while (num-- > 0)
		{
			array[num] = meshPoints[num].color;
		}
		return array;
	}

	public static MeshPoint CopyFrom(MeshPoint p)
	{
		return new MeshPoint
		{
			vert = p.vert,
			normal = p.normal,
			uv1 = p.uv1,
			uv2 = p.uv2,
			color = p.color,
			index = p.index
		};
	}
}
