using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Land Control")]
public class PQSLandControl : PQSMod
{
	[Serializable]
	public class LerpRange
	{
		public double startStart;

		public double startEnd;

		public double endStart;

		public double endEnd;

		[HideInInspector]
		public double startDelta;

		[HideInInspector]
		public double endDelta;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LerpRange()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Lerp(double point)
		{
			throw null;
		}
	}

	[Serializable]
	public class LandClass
	{
		public string landClassName;

		public Color color;

		public Color noiseColor;

		public LerpRange altitudeRange;

		public LerpRange latitudeRange;

		public bool latitudeDouble;

		[HideInInspector]
		public LerpRange latitudeDoubleRange;

		public LerpRange longitudeRange;

		public float coverageBlend;

		public int coverageSeed;

		public int coverageOctaves;

		public float coveragePersistance;

		public float coverageFrequency;

		[HideInInspector]
		public Simplex coverageSimplex;

		public float noiseBlend;

		public int noiseSeed;

		public int noiseOctaves;

		public float noisePersistance;

		public float noiseFrequency;

		[HideInInspector]
		public Simplex noiseSimplex;

		public double minimumRealHeight;

		public double alterRealHeight;

		public float alterApparentHeight;

		[HideInInspector]
		public double altDelta;

		[HideInInspector]
		public double latDelta;

		[HideInInspector]
		public double lonDelta;

		[HideInInspector]
		public double delta;

		public LandClassScatterAmount[] scatter;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LandClass()
		{
			throw null;
		}
	}

	[Serializable]
	public class LandClassScatterAmount
	{
		public string scatterName;

		public double density;

		public LandClassScatter scatter;

		public int scatterIndex;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LandClassScatterAmount()
		{
			throw null;
		}
	}

	[Serializable]
	public class LandClassScatterInstance
	{
		public double density;

		public LandClassScatter scatter;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LandClassScatterInstance()
		{
			throw null;
		}
	}

	[Serializable]
	public class LandClassScatter
	{
		public string scatterName;

		public int seed;

		public int maxCache;

		public int maxCacheDelta;

		public int maxScatter;

		public double densityFactor;

		public Material material;

		public Mesh baseMesh;

		public int maxLevelOffset;

		public float verticalOffset;

		public float minScale;

		public float maxScale;

		public bool castShadows;

		public bool recieveShadows;

		private int vertStride;

		private int triStride;

		private GameObject scatterParent;

		[HideInInspector]
		private Vector3[] vertsUntrans;

		[HideInInspector]
		private Vector3[] normUntrans;

		[HideInInspector]
		private Vector2[] uvUntrans;

		[HideInInspector]
		private int[] trisUntrans;

		[HideInInspector]
		private Vector3[] vertsEmpty;

		[HideInInspector]
		private Vector3[] normalsEmpty;

		[HideInInspector]
		private Vector3[] vertsTransformed;

		[HideInInspector]
		private Vector3[] normalsTransformed;

		[HideInInspector]
		private Vector2[] uvs;

		[HideInInspector]
		private int[] tris;

		private Stack<PQSMod_LandClassScatterQuad> cacheUnassigned;

		private List<PQSMod_LandClassScatterQuad> cacheAssigned;

		private int cacheUnassignedCount;

		private int cacheAssignedCount;

		private int cacheTotalCount;

		private PQS sphere;

		private Vector3 scatterPos;

		private int rndCount;

		private PQSMod_LandClassScatterQuad qc;

		private double countFactor;

		private Vector3 scatterUp;

		private Quaternion scatterRot;

		private float scatterAngle;

		private int scatterLoop;

		private int scatterN;

		private bool cacheCreated;

		private float scatterScale;

		public double maxSpeed;

		public int minLevel
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
		public LandClassScatter()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(PQS sphere)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SphereActive()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SphereInactive()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddScatterMeshController(PQ quad, double density)
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
		private void BuildCache(int countToAdd)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DestroyQuad(PQSMod_LandClassScatterQuad q)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void CreateScatterMesh(PQSMod_LandClassScatterQuad q)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector3 RandomRange(Vector3 min, Vector3 max)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ClearAll()
		{
			throw null;
		}
	}

	public LandClass[] landClasses;

	public LandClassScatter[] scatters;

	public bool useHeightMap;

	public MapSO heightMap;

	public float vHeightMax;

	public bool createColors;

	public bool createScatter;

	public float altitudeBlend;

	public int altitudeSeed;

	public int altitudeOctaves;

	public float altitudePersistance;

	public float altitudeFrequency;

	[HideInInspector]
	public Simplex altitudeSimplex;

	public float latitudeBlend;

	public int latitudeSeed;

	public int latitudeOctaves;

	public float latitudePersistance;

	public float latitudeFrequency;

	[HideInInspector]
	public Simplex latitudeSimplex;

	public float longitudeBlend;

	public int longitudeSeed;

	public int longitudeOctaves;

	public float longitudePersistance;

	public float longitudeFrequency;

	[HideInInspector]
	public Simplex longitudeSimplex;

	private int lcCount;

	private int itr;

	private LandClass lcSelected;

	private int lcSelectedIndex;

	private double vHeight;

	private double ct2;

	private double ct3;

	private List<LandClass> lcList;

	private double[] lcScatterList;

	private int lcListCount;

	private LandClass lc;

	private bool scatterActive;

	private int scatterMinSubdiv;

	private int scatterCount;

	private int scatterInstCount;

	private double totalDelta;

	private double vLat;

	private double vLon;

	private double vHeightAltered;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSLandControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyCastShadowsSetting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Clear All Buffers")]
	public void ClearAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadPreBuild(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuildHeight(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuild(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereStarted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Lerp(double v1, double v2, double dt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Clamp(double v, double low, double high)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double DoubleLerp(double startStart, double startEnd, double endStart, double endEnd, double point)
	{
		throw null;
	}
}
