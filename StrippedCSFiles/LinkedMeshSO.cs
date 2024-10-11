using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class LinkedMeshSO : ScriptableObject
{
	[Serializable]
	public class LinkedTri
	{
		public int aVert;

		public int aIndex;

		public int bVert;

		public int bIndex;

		public int cVert;

		public int cIndex;

		public static int size
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinkedTri(int aVert, int aIndex, int bVert, int bIndex, int cVert, int cIndex)
		{
			throw null;
		}
	}

	[Serializable]
	public class LinkedVert
	{
		public int[] meshVerts;

		public Vector3 origVert;

		public int size
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinkedVert(int[] meshVerts, Vector3 origVert)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Contains(int vertIndex)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int IndexOf(int vertIndex)
		{
			throw null;
		}
	}

	[SerializeField]
	private string meshName;

	[SerializeField]
	private LinkedVert[] linkedVerts;

	[SerializeField]
	private LinkedTri[] linkedTris;

	[SerializeField]
	private Vector2[] uvs;

	[SerializeField]
	private int vertCount;

	[SerializeField]
	private int linkedVertCount;

	[SerializeField]
	private int triCount;

	[SerializeField]
	private int triIndexCount;

	[SerializeField]
	private bool isBuilt;

	[SerializeField]
	private int size;

	[SerializeField]
	private bool isLocked;

	private static Vector3[] meshVerts;

	private static Vector2[] meshUV;

	private static Vector3[] meshNormals;

	private static int[] meshTris;

	private static Vector4[] tangents;

	private static Vector3[] tan1;

	private static Vector3[] tan2;

	private static Vector3[] cloneVerts;

	private static Vector2[] cloneUV;

	public string MeshName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int VertCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int LinkedVertCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int TriCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int TriIndexCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsBuilt
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Size
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SizeString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsLocked
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3[] CloneVerts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector2[] CloneUV
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedMeshSO()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NormalizeMesh(Mesh baseMesh)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3[] ComputeNormals(Vector3[] verts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector4[] CalculateTangets(Vector3[] verts, Vector3[] normals, Vector2[] uv, int[] tris)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateLinkedMesh(Mesh baseMesh, bool setLock)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh GetOriginalMesh(bool calcTangents)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupClone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh GetCloneMesh(bool calcTangents)
	{
		throw null;
	}
}
