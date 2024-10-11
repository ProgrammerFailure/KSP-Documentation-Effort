using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Scatter")]
public class PQSMod_MeshScatter : PQSMod
{
	public string scatterName;

	public int seed;

	public int maxCache;

	public int maxScatter;

	public Material material;

	public Texture2D scatterMap;

	public Color baseColor;

	public Mesh baseMesh;

	public int minSubdivision;

	public float countPerSqM;

	public float verticalOffset;

	public Vector3 minScale;

	public Vector3 maxScale;

	public bool castShadows;

	public bool recieveShadows;

	[HideInInspector]
	public int vertStride;

	[HideInInspector]
	public int triStride;

	[HideInInspector]
	public int maxCacheCount;

	[HideInInspector]
	public GameObject scatterParent;

	[HideInInspector]
	public Vector3[] vertsUntrans;

	[HideInInspector]
	public Vector3[] normUntrans;

	[HideInInspector]
	public Vector2[] uvUntrans;

	[HideInInspector]
	public int[] trisUntrans;

	[HideInInspector]
	public Vector3[] verts;

	[HideInInspector]
	public Vector3[] normals;

	[HideInInspector]
	public List<MeshFilter> cacheUnassigned;

	[HideInInspector]
	public int cacheUnassignedCount;

	[HideInInspector]
	public List<MeshFilter> cacheAssigned;

	[HideInInspector]
	public int cacheAssignedCount;

	private Vector3 scatterPos;

	private int rndCount;

	private PQSMod_MeshScatter_QuadControl qc;

	private double countFactor;

	private Vector3 scatterUp;

	private Vector3 scatterScale;

	private Quaternion scatterRot;

	private float scatterAngle;

	private int scatterLoop;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_MeshScatter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddScatterMeshController(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildCacheMesh(Vector3[] vertBase, Vector2[] uvBase, Vector3[] normBase, int[] triBase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildCacheQuad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MeshFilter AssignScatterMesh(PQ quad, int seed, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnassignScatterMesh(MeshFilter mf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 RandomRange(Vector3 min, Vector3 max)
	{
		throw null;
	}
}
