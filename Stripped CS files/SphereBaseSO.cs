using System;
using System.Collections.Generic;
using UnityEngine;

public class SphereBaseSO : ScriptableObject
{
	[Serializable]
	public class Vertex
	{
		public Vector3 position;

		public int subdivision;

		public List<int> triangles;

		public List<int> edges;

		public List<float> edgeLengths;

		public Vertex(Vector3 position, int subdivision)
		{
			this.position = position;
			this.subdivision = subdivision;
			triangles = new List<int>();
			edges = new List<int>();
			edgeLengths = new List<float>();
		}
	}

	[Serializable]
	public class Triangle
	{
		public int a;

		public int b;

		public int c;

		public int ab;

		public int bc;

		public int ca;

		public Vector3 midpoint;

		public Triangle(int a, int b, int c)
		{
			this.a = a;
			this.b = b;
			this.c = c;
		}

		public bool HasVertex(int v)
		{
			if (a != v && b != v)
			{
				return c == v;
			}
			return true;
		}

		public bool LinkTriangle(Triangle t, int tIndex)
		{
			if (t.HasVertex(a) && t.HasVertex(b))
			{
				ab = tIndex;
				return true;
			}
			if (t.HasVertex(b) && t.HasVertex(c))
			{
				bc = tIndex;
				return true;
			}
			if (t.HasVertex(c) && t.HasVertex(a))
			{
				ca = tIndex;
				return true;
			}
			return false;
		}
	}

	public int recursionLevel = 4;

	public bool isCompiled;

	[HideInInspector]
	public List<Vertex> verts;

	public int vCount;

	[HideInInspector]
	public List<Triangle> tris;

	public int tCount;

	public int size;

	public Dictionary<long, int> middlePointIndexCache;

	public int index;

	public string SizeString
	{
		get
		{
			if (size > 1048576)
			{
				return KSPUtil.LocalizeNumber(9.536743E-07f * (float)size, "F2") + "MB";
			}
			if (size > 1024)
			{
				return KSPUtil.LocalizeNumber(0.0009765625f * (float)size, "F1") + "kB";
			}
			return size + "B";
		}
	}

	public void Create()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		CreateSphere();
		LinkSphere();
		CalculateSize();
		isCompiled = true;
		Debug.Log("SphereBaseSO (level " + recursionLevel + ", " + verts.Count + " verts, " + tris.Count + " tris) : Creation time = " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F4") + "s");
	}

	public void CreateSphere()
	{
		verts = new List<Vertex>();
		tris = new List<Triangle>();
		middlePointIndexCache = new Dictionary<long, int>();
		index = 0;
		float num = (1f + Mathf.Sqrt(5f)) / 2f;
		AddVertex(new Vector3(-1f, num, 0f));
		AddVertex(new Vector3(1f, num, 0f));
		AddVertex(new Vector3(-1f, 0f - num, 0f));
		AddVertex(new Vector3(1f, 0f - num, 0f));
		AddVertex(new Vector3(0f, -1f, num));
		AddVertex(new Vector3(0f, 1f, num));
		AddVertex(new Vector3(0f, -1f, 0f - num));
		AddVertex(new Vector3(0f, 1f, 0f - num));
		AddVertex(new Vector3(num, 0f, -1f));
		AddVertex(new Vector3(num, 0f, 1f));
		AddVertex(new Vector3(0f - num, 0f, -1f));
		AddVertex(new Vector3(0f - num, 0f, 1f));
		tris.Add(new Triangle(0, 11, 5));
		tris.Add(new Triangle(0, 5, 1));
		tris.Add(new Triangle(0, 1, 7));
		tris.Add(new Triangle(0, 7, 10));
		tris.Add(new Triangle(0, 10, 11));
		tris.Add(new Triangle(1, 5, 9));
		tris.Add(new Triangle(5, 11, 4));
		tris.Add(new Triangle(11, 10, 2));
		tris.Add(new Triangle(10, 7, 6));
		tris.Add(new Triangle(7, 1, 8));
		tris.Add(new Triangle(3, 9, 4));
		tris.Add(new Triangle(3, 4, 2));
		tris.Add(new Triangle(3, 2, 6));
		tris.Add(new Triangle(3, 6, 8));
		tris.Add(new Triangle(3, 8, 9));
		tris.Add(new Triangle(4, 9, 5));
		tris.Add(new Triangle(2, 4, 11));
		tris.Add(new Triangle(6, 2, 10));
		tris.Add(new Triangle(8, 6, 7));
		tris.Add(new Triangle(9, 8, 1));
		for (int i = 1; i < recursionLevel; i++)
		{
			List<Triangle> list = new List<Triangle>();
			int j = 0;
			for (int count = tris.Count; j < count; j++)
			{
				Triangle triangle = tris[j];
				int midPoint = GetMidPoint(triangle.a, triangle.b, i);
				int midPoint2 = GetMidPoint(triangle.b, triangle.c, i);
				int midPoint3 = GetMidPoint(triangle.c, triangle.a, i);
				list.Add(new Triangle(triangle.a, midPoint, midPoint3));
				list.Add(new Triangle(triangle.b, midPoint2, midPoint));
				list.Add(new Triangle(triangle.c, midPoint3, midPoint2));
				list.Add(new Triangle(midPoint, midPoint2, midPoint3));
			}
			tris = list;
		}
		middlePointIndexCache = null;
		vCount = verts.Count;
		tCount = tris.Count;
	}

	public int AddVertex(Vector3 p)
	{
		verts.Add(new Vertex(p.normalized, 0));
		return index++;
	}

	public int AddVertex(Vector3 p, int subdivision)
	{
		verts.Add(new Vertex(p.normalized, subdivision));
		return index++;
	}

	public int GetMidPoint(int p1, int p2, int subdivision)
	{
		bool num = p1 < p2;
		long num2 = (num ? p1 : p2);
		long num3 = (num ? p2 : p1);
		long key = (num2 << 32) + num3;
		if (middlePointIndexCache.TryGetValue(key, out var value))
		{
			return value;
		}
		Vector3 p3 = (verts[p1].position + verts[p2].position) * 0.5f;
		int num4 = AddVertex(p3, subdivision);
		middlePointIndexCache.Add(key, num4);
		return num4;
	}

	public void CalculateSize()
	{
		size = 0;
		for (int i = 0; i < vCount; i++)
		{
			Vertex vertex = verts[i];
			size += 4;
			size += vertex.edgeLengths.Count;
			size += vertex.edges.Count;
			size += vertex.triangles.Count;
		}
		size += 9 * tCount;
		size *= 4;
	}

	public void LinkSphere()
	{
		LinkTriangles();
		LinkVerts();
		LinkEdges();
	}

	public void LinkTriangles()
	{
		for (int i = 0; i < tCount; i++)
		{
			Triangle triangle = tris[i];
			int num = 0;
			for (int j = 0; j < tCount; j++)
			{
				if (j != i && triangle.LinkTriangle(tris[j], j))
				{
					num++;
					if (num == 3)
					{
						break;
					}
				}
			}
			triangle.midpoint = (verts[triangle.a].position + verts[triangle.b].position + verts[triangle.c].position) / 3f;
		}
	}

	public void LinkVerts()
	{
		for (int i = 0; i < tCount; i++)
		{
			Triangle triangle = tris[i];
			verts[triangle.a].triangles.Add(i);
			verts[triangle.b].triangles.Add(i);
			verts[triangle.c].triangles.Add(i);
		}
	}

	public void LinkEdges()
	{
		for (int i = 0; i < vCount; i++)
		{
			for (int j = 0; j < verts[i].triangles.Count; j++)
			{
				Triangle triangle = tris[verts[i].triangles[j]];
				if (triangle.a == i)
				{
					verts[i].edges.Add(triangle.b);
					verts[i].edgeLengths.Add(Vector3.Distance(verts[triangle.a].position, verts[triangle.b].position));
					verts[i].edges.Add(triangle.c);
					verts[i].edgeLengths.Add(Vector3.Distance(verts[triangle.a].position, verts[triangle.b].position));
				}
				else if (triangle.b == i)
				{
					verts[i].edges.Add(triangle.a);
					verts[i].edgeLengths.Add(Vector3.Distance(verts[triangle.b].position, verts[triangle.a].position));
					verts[i].edges.Add(triangle.c);
					verts[i].edgeLengths.Add(Vector3.Distance(verts[triangle.b].position, verts[triangle.c].position));
				}
				else if (triangle.c == i)
				{
					verts[i].edges.Add(triangle.a);
					verts[i].edgeLengths.Add(Vector3.Distance(verts[triangle.c].position, verts[triangle.a].position));
					verts[i].edges.Add(triangle.b);
					verts[i].edgeLengths.Add(Vector3.Distance(verts[triangle.c].position, verts[triangle.b].position));
				}
			}
		}
	}

	public Mesh CreateMesh(float[] vertexLengths, Color[] vertexColors, bool createUV)
	{
		Vector3[] array = new Vector3[vCount];
		Vector2[] array2 = null;
		Vector3[] array3 = null;
		float num = float.MaxValue;
		float num2 = float.MinValue;
		int num3 = vCount;
		while (num3-- > 0)
		{
			float num4 = vertexLengths[num3];
			array[num3] = verts[num3].position * num4;
			if (num4 < num)
			{
				num = num4;
			}
			if (num4 > num2)
			{
				num2 = num4;
			}
		}
		array3 = CalculateNormals(array);
		if (createUV)
		{
			float num5 = num2 - num;
			if (num5 == 0f)
			{
				num5 = 1f;
			}
			array2 = new Vector2[vCount];
			int num6 = vCount;
			while (num6-- > 0)
			{
				array2[num6].x = 1f - Vector3.Dot(verts[num6].position, array3[num6]);
				array2[num6].y = (vertexLengths[num6] - num) / num5;
			}
		}
		int[] array4 = new int[tCount * 3];
		int num7 = 0;
		int num8 = tCount;
		while (num8-- > 0)
		{
			array4[num7] = tris[num8].a;
			num7++;
			array4[num7] = tris[num8].b;
			num7++;
			array4[num7] = tris[num8].c;
			num7++;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.triangles = array4;
		mesh.normals = array3;
		if (createUV)
		{
			mesh.uv = array2;
		}
		if (vertexColors != null)
		{
			mesh.colors = vertexColors;
		}
		mesh.bounds = new Bounds(Vector3.zero, Vector3.one * num2);
		return mesh;
	}

	public Vector3[] CalculateNormals(Vector3[] verts)
	{
		Vector3[] array = new Vector3[tCount];
		Vector3[] array2 = new Vector3[vCount];
		int num = tCount;
		Vector3 vector3 = default(Vector3);
		while (num-- > 0)
		{
			Triangle triangle = tris[num];
			Vector3 vector = verts[triangle.b] - verts[triangle.a];
			Vector3 vector2 = verts[triangle.c] - verts[triangle.a];
			vector3.x = vector.y * vector2.z - vector.z * vector2.y;
			vector3.y = vector.z * vector2.x - vector.x * vector2.z;
			vector3.z = vector.x * vector2.y - vector.y * vector2.x;
			array[num] = vector3.normalized;
		}
		num = tCount;
		while (num-- > 0)
		{
			Triangle triangle = tris[num];
			array2[triangle.a] = (array2[triangle.a] + array[num]).normalized;
			array2[triangle.b] = (array2[triangle.b] + array[num]).normalized;
			array2[triangle.c] = (array2[triangle.c] + array[num]).normalized;
		}
		return array2;
	}
}
