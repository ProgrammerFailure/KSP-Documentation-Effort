using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProceduralFairings;

public class ColliderChunk
{
	public MeshCollider collider;

	private Mesh mesh;

	private List<int> tris;

	private MeshPoint[] pts;

	private int lodStep;

	private int nSides;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ColliderChunk(MeshArc bottom, MeshArc top, int nSidesTotal, int nArcs, int nColliders, int subdivIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MeshCollider CreateColliderGO(GameObject parent, string name, int layer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh GenerateColliderMesh(bool triangulate, Vector3 offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void TriangulateColliderMesh(MeshPoint[] pts, List<int> tris)
	{
		throw null;
	}
}
