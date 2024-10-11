using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public struct TMP_MeshInfo
{
	private static readonly Color32 s_DefaultColor;

	private static readonly Vector3 s_DefaultNormal;

	private static readonly Vector4 s_DefaultTangent;

	public Mesh mesh;

	public int vertexCount;

	public Vector3[] vertices;

	public Vector3[] normals;

	public Vector4[] tangents;

	public Vector2[] uvs0;

	public Vector2[] uvs2;

	public Color32[] colors32;

	public int[] triangles;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_MeshInfo(Mesh mesh, int size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_MeshInfo(Mesh mesh, int size, bool isVolumetric)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_MeshInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResizeMeshInfo(int size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResizeMeshInfo(int size, bool isVolumetric)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear(bool uploadChanges)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearUnusedVertices()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearUnusedVertices(int startIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearUnusedVertices(int startIndex, bool updateMesh)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortGeometry(VertexSortingOrder order)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SortGeometry(IList<int> sortingOrder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SwapVertexData(int src, int dst)
	{
		throw null;
	}
}
