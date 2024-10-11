using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class LinkedMesh
{
	[Flags]
	public enum MeshOption
	{
		None = 0,
		CalculateNormals = 1,
		CalculateBounds = 2,
		CalculateTangents = 4,
		Debug = 8
	}

	[Serializable]
	public class LinkedTri
	{
		public int a;

		public int b;

		public int c;

		public int[] links;

		public Vector3 normal;

		public Vector3 edgeAB;

		public Vector3 edgeAC;

		public Vector3 midpoint;

		public bool isInSelection;

		public float selectionDelta;

		public LinkedVert aVert
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public LinkedVert bVert
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public LinkedVert cVert
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public LinkedTri[] linkTris
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinkedTri(int a, int b, int c, Vector3 midpoint)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetVerts(LinkedVert aVert, LinkedVert bVert, LinkedVert cVert)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ClearVerts()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetLinks(int[] links)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetLinkTris(LinkedTri[] links)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool IsVertInTri(int vert)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool HasVertsInTri(LinkedTri tri)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool IsNeighbourOf(LinkedTri tri)
		{
			throw null;
		}
	}

	[Serializable]
	public class LinkedVert
	{
		public int meshVert;

		public int[] meshVerts;

		public int meshVertCount;

		public int triCount;

		public bool isInSelection;

		public float selectionDelta;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinkedVert(int meshVert)
		{
			throw null;
		}
	}

	[HideInInspector]
	public Vector3[] verts;

	[HideInInspector]
	public int[] tris;

	[HideInInspector]
	public Vector2[] uv;

	[HideInInspector]
	public Vector3[] normals;

	[HideInInspector]
	public LinkedVert[] linkedVerts;

	[HideInInspector]
	public LinkedTri[] linkedTris;

	public int vertCount;

	[HideInInspector]
	public int triIndexCount;

	public int triCount;

	private Vector4[] tangents;

	private Vector3[] tan1;

	private Vector3[] tan2;

	public bool meshBuilt;

	private Mesh mesh;

	[HideInInspector]
	public List<LinkedTri> selection;

	[HideInInspector]
	public int selectionCount;

	[HideInInspector]
	public List<LinkedVert> selectionVerts;

	[HideInInspector]
	public int selectionVertsCount;

	private Vector3 selectionCenter;

	private float selectionRadiusSqr;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedMesh(Mesh baseMesh)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateTangets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh GetMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh GetMesh(MeshOption meshOption)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NormalizeMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateLinks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ComputeNormals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ComputeSelectionNormals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Raycast(Vector3 rayPosition, Vector3 rayDirection, float rayDistance, out LinkedTri hitTri, out float hitDistance, out Vector3 hitNormal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool RayIntersectMesh(Vector3 rayPosition, Vector3 rayDirection, float rayDistance, out LinkedTri hitTri, out float hitDistance, out Vector3 hitNormal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool RayIntersectTriangle(Vector3 rayPosition, Vector3 rayDirection, float rayDistance, LinkedTri t, ref float pickDistance, ref float barycentricU, ref float barycentricV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectMesh(Vector3 point, int triIndex, float fallOffRadius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectMesh(Vector3 point, LinkedTri tri, float fallOffRadius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectByLinks(LinkedTri tri, List<LinkedTri> selection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSelection(bool selectionNormals)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearSelection()
	{
		throw null;
	}
}
