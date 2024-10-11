using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vertex(Vector3 position, int subdivision)
		{
			throw null;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Triangle(int a, int b, int c)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool HasVertex(int v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool LinkTriangle(Triangle t, int tIndex)
		{
			throw null;
		}
	}

	public int recursionLevel;

	public bool isCompiled;

	[HideInInspector]
	public List<Vertex> verts;

	public int vCount;

	[HideInInspector]
	public List<Triangle> tris;

	public int tCount;

	public int size;

	private Dictionary<long, int> middlePointIndexCache;

	private int index;

	public string SizeString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SphereBaseSO()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Create()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int AddVertex(Vector3 p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int AddVertex(Vector3 p, int subdivision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetMidPoint(int p1, int p2, int subdivision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LinkSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LinkTriangles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LinkVerts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LinkEdges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh CreateMesh(float[] vertexLengths, Color[] vertexColors, bool createUV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3[] CalculateNormals(Vector3[] verts)
	{
		throw null;
	}
}
