using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/PQuadSphere")]
public class PQS : MonoBehaviour
{
	private enum QI
	{
		xPos,
		xNeg,
		yPos,
		yNeg,
		zPos,
		zNeg
	}

	public enum QuadEdge
	{
		North = 0,
		South = 1,
		East = 2,
		West = 3,
		Null = -1
	}

	public enum QuadChild
	{
		SouthWest = 0,
		SouthEast = 1,
		NorthWest = 2,
		NorthEast = 3,
		Null = -1
	}

	[Flags]
	public enum EdgeState
	{
		Reset = -1,
		NoLerps = 0,
		NorthLerp = 1,
		SouthLerp = 2,
		EastLerp = 4,
		WestLerp = 8
	}

	public enum QuadPlane
	{
		XP,
		XN,
		YP,
		YN,
		ZP,
		ZN
	}

	[Flags]
	public enum ModiferRequirements
	{
		Default = 0,
		UniqueMaterialInstances = 1,
		VertexMapCoords = 2,
		VertexGnomonicMapCoords = 4,
		UVSphereCoords = 8,
		UVQuadCoords = 0x10,
		MeshColorChannel = 0x20,
		MeshCustomNormals = 0x40,
		MeshBuildTangents = 0x80,
		MeshUV2 = 0x100,
		MeshUV3 = 0x200,
		MeshUV4 = 0x400,
		MeshAssignTangents = 0x800
	}

	public class VertexBuildData
	{
		public PQ buildQuad;

		public Vector3d globalV;

		public Vector3d directionFromCenter;

		public Vector3d directionD;

		public Vector3d directionXZ;

		public int vertIndex;

		public double vertHeight;

		public Color vertColor;

		public double u;

		public double v;

		public double u2;

		public double v2;

		public double u3;

		public double v3;

		public double u4;

		public double v4;

		public double gnomonicU;

		public double gnomonicV;

		public bool allowScatter;

		public QuadPlane gnomonicPlane;

		public double latitude;

		public double longitude;

		public GnomonicUV[] gnomonicUVs;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VertexBuildData()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Reset()
		{
			throw null;
		}
	}

	public struct GnomonicUV
	{
		public bool acceptable;

		public double gnomonicU;

		public double gnomonicV;
	}

	[CompilerGenerated]
	private sealed class _003CResetAndWaitCoroutine_003Ed__134 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PQS _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CResetAndWaitCoroutine_003Ed__134(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CUpdateSphere_003Ed__148 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PQS _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CUpdateSphere_003Ed__148(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public static bool Global_ForceShaderModel;

	public static bool Global_AllowScatter;

	public static double Global_ScatterFactor;

	public static PQS_GameBindings GameBindings;

	private EventData<PQS> onReady;

	public PQS parentSphere;

	public GameObject LocalSpacePQStorage;

	private List<PQ> LocalSpacePQList;

	public int seed;

	public double radius;

	public bool DEBUG_ShowGUI;

	public bool DEBUG_ShowGUIRebuild;

	public bool DEBUG_UseSharedMaterial;

	[HideInInspector]
	public bool defaultInspector;

	public Material lowQualitySurfaceMaterial;

	public Material mediumQualitySurfaceMaterial;

	public Material highQualitySurfaceMaterial;

	public Material ultraQualitySurfaceMaterial;

	public Material surfaceMaterial;

	public Material fallbackMaterial;

	public List<Material> materialsForUpdates;

	public float frameTimeDelta;

	public float maxFrameTime;

	public bool meshCastShadows;

	public bool meshRecieveShadows;

	public int minLevel;

	public int maxLevel;

	public float maxQuadLenghtsPerFrame;

	public int maxLevelAtCurrentTgtSpeed;

	private float angularTargetSpeed;

	public double subdivisionThreshold;

	public double collapseSeaLevelValue;

	public double collapseAltitudeValue;

	public double collapseAltitudeMax;

	[HideInInspector]
	public double collapseDelta;

	public double collapseThreshold;

	public double visRadSeaLevelValue;

	public double visRadAltitudeValue;

	public double visRadAltitudeMax;

	[HideInInspector]
	public double visRadDelta;

	public double visRad;

	public double detailSeaLevelQuads;

	public double detailAltitudeQuads;

	public double detailAltitudeMax;

	[HideInInspector]
	public double detailDelta;

	public double detailRad;

	public string mapFilename;

	public int mapFilesize;

	public double mapMaxHeight;

	public bool mapOcean;

	public double mapOceanHeight;

	public Color mapOceanColor;

	public bool surfaceRelativeQuads;

	public bool buildTangents;

	public bool isDisabled;

	public bool isAlive;

	public bool isActive;

	public bool isThinking;

	public bool isStarted;

	public bool isBuildingMaps;

	public Transform target;

	private Vector3d targetPosition;

	private Vector3d targetPositionPrev;

	public Vector3d relativeTargetPosition;

	public Vector3d relativeTargetPositionNormalized;

	private Vector3d lastRelTgtPosNormalized;

	private double targetDistance;

	public double targetAltitude;

	public double targetHeight;

	public Vector3d targetVelocity;

	public double targetSpeed;

	public Transform secondaryTarget;

	public Vector3d relativeSecondaryTargetPosition;

	public double targetSecondaryAltitude;

	public double visibleAltitude;

	public double visibleRadius;

	public double horizonDistance;

	public double horizonAngle;

	public QuaternionD transformRotation;

	private QuaternionD transformRotationInverse;

	public Vector3d transformPosition;

	public double minDetailDistance;

	public double maxDetailDistance;

	public bool isSubdivisionEnabled;

	public bool useSharedMaterial;

	public double radiusSquared;

	public double circumference;

	public List<PQ> normalUpdateList;

	public bool quadAllowBuild;

	public float maxFrameEnd;

	public double radiusMax;

	public double radiusMin;

	public double radiusDelta;

	public double halfChord;

	public double meshVertMin;

	public double meshVertMax;

	public int quadCount;

	public int[] quadCounts;

	public bool cancelUpdate;

	public double[] subdivisionThresholds;

	public double[] collapseThresholds;

	private int pqID;

	public PQ[] quads;

	private static int itr;

	private static int tempInt;

	private int maxLevel1;

	private int fixedUpdateFrame;

	private float fixedFrameTime;

	private float prevFixedFrameTime;

	private PQSLandControl PQSLandControl;

	private PQSMod_AerialPerspectiveMaterial PQSAerialPerspectiveMaterial;

	private float savemaxFrameTime;

	public PQSMod_CelestialBodyTransform PQSModCBTransform;

	private Vector3d precisePosition;

	private PQSCache cache;

	private PQS[] _childSpheres;

	private bool externalRenderScatter;

	private const float HalfPI = 1.5707963f;

	private bool hasCache;

	public static int cacheSideVertCount;

	public static float cacheMeshSize;

	public static float cacheQuadSize;

	public static float cacheQuadSizeDiv2;

	public static Mesh cacheMesh;

	public static int cacheVertCount;

	public static int cacheRes;

	public static int cacheResDiv2;

	public static int cacheResDiv2Plus1;

	public static int cacheTriIndexCount;

	public static int cacheTriCount;

	public static int[][] cacheIndices;

	public static Vector3[] cacheVerts;

	public static Vector2[] cacheUVs;

	public static Vector3[] cacheNormals;

	public static Vector4[] cacheTangents;

	public static Vector3[] tan1;

	public static Vector3[] tan2;

	public static Color[] cacheColors;

	public static Vector2[] cacheUV2s;

	public static Vector2[] cacheUV3s;

	public static Vector2[] cacheUV4s;

	public static Vector2d[] cacheUVQuad;

	public static double cacheVertCountReciprocal;

	private PQSMod[] mods;

	private int modCount;

	private ModiferRequirements modRequirements;

	public bool reqVertexMapCoods;

	public bool reqGnomonicCoords;

	public bool reqCustomNormals;

	public bool reqColorChannel;

	public bool reqBuildTangents;

	public bool reqAssignTangents;

	public bool reqUV2;

	public bool reqUV3;

	public bool reqUV4;

	public bool reqSphereUV;

	public bool reqUVQuad;

	public bool isFakeBuild;

	private PQ buildQuad;

	private int vertexIndex;

	private static VertexBuildData vbData;

	private double latitude;

	private double longitude;

	public double sy;

	public double sx;

	private Vector3d directionXZ;

	private int uMin;

	private int vMin;

	private int uMax;

	private int vMax;

	private double uDiff;

	private double vDiff;

	private Color c1;

	private Color c2;

	private Color c3;

	private Color c4;

	private double f1;

	private double f2;

	private double f3;

	private double f4;

	private Bounds meshBounds;

	public static Vector3d[] verts;

	private static Vector2[] uvs;

	private static Vector3[] normals;

	private static Vector3[] triNormals;

	private static Vector3d vertRel;

	private static Vector3d planetRel;

	public EventData<PQS> OnReady
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3d PrecisePosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public PQS[] ChildSpheres
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PQS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
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
	public virtual void ApplyTerrainShaderSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StartSphere(bool force)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reset Sphere")]
	public void ResetSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Rebuild Sphere")]
	public void RebuildSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForceStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartUpSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetAndWait()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CResetAndWaitCoroutine_003Ed__134))]
	private IEnumerator ResetAndWaitCoroutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupExternalRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetupLaunchsitePlacementRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetLaunchsitePlacementRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseExternalRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture2D[] CreateMaps(int width, double maxHeight, bool hasOcean, double oceanHeight, Color oceanColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQ AssignQuad(int subdiv)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void QuadCreated(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyQuad(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PQ Q(QI initQuad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateQuads()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupQuad(PQ quad, string name, QuadPlane plane, PQ north, PQ south, PQ east, PQ west, Vector3d pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateSphere_003Ed__148))]
	private IEnumerator UpdateSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void FastUpdateQuadsPosition(Vector3d movement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void PreciseUpdateQuadsPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddPQToLocalSpaceStorage(PQ newLocalSpaceQuad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RemovePQFromLocalSpaceStorage(PQ quadToRemove)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeactivateSphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetVisible(bool visible)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTarget(Transform target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSecondaryTarget(Transform secondaryTarget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVisual()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableSubdivision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableSubdivision()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateQuadsInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateQuads()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateEdges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadCacheSettings(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CreateCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void BuildTangents(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CreateIndexCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CreateIndexState(EdgeState state, int[] tris, int[] trisBck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int ti(int x, int z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int vi(int x, int z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CreateCacheNormals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CreateCacheColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CreateCacheUVExtras()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void BuildNormals(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void BuildNormalsNoClear(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnPostSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnSphereReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnSphereStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnSphereStarted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnSphereTransformUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double Mod_GetVertexMaxHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double Mod_GetVertexMinHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnPreUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnUpdateFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnVertexBuildHeight(VertexBuildData data, bool overrideQuadBuildCheck = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnVertexBuild(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnMeshBuild()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnQuadCreate(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnQuadDestroy(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnQuadPreBuild(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnQuadBuilt(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnQuadUpdate(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnQuadUpdateNormals(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupMods()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetModList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetChildMods(GameObject obj, List<PQSMod> mods)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupBuildDelegates(bool fakeBuild)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSphereStartDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSphereTransformUpdateDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPreUpdateDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool BuildQuad(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexMapCoords(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexHeight(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexSurfaceRelative(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexColor(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexUV2(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexUV3(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexUV4(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexSphereNormal(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexQuadUV(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildVertexSphereUV(VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshSphereNormals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshCustomNormals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshTangents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AssignMeshTangents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshColorChannel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshUVChannel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshUV2Channel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshUV3Channel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildMeshUV4Channel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Color BilinearInterpColorMap(Texture2D texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Color BilinearInterpColorMap(Vector3 planetPos, Texture2D texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double BilinearInterpFloatMap(Texture2D texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double BilinearInterpFloatMap(Vector3 planetPos, Texture2D texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BackupEdgeNormals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BackupEdgeNormals(Vector3[] edge, int vCount, int localStart, int localDelta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateEdgeNormals(PQ q)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CombineEdgeNormals(PQ q, int vCount, int vertStart, int vertStep, Vector3[] bkpLocal, int localBackupStart, int localBackupStep, Vector3[] bkpRemote, int remoteBackupStart, int remoteBackupStep)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetRightmostCornerNormal(PQ caller, PQ nextQuad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuadEdge GetEdgeRotatedClockwise(QuadEdge edge)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuadEdge GetEdgeRotatedCounterclockwise(QuadEdge edge)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelativePosition(Vector3d worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelativeDirection(Vector3d worldDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetWorldPosition(Vector3d localPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetRelativeDistance(Vector3 worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetRelativeAltitude(Vector3 worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetRelativeDistanceSqr(Vector3 worldDirection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetClampedWorldSurfaceHeight(Vector3 worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetClampedWorldAltitude(Vector3 worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSurfaceHeight(Vector3d radialVector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal double GetSurfaceHeight(Vector3d radialVector, bool overrideQuadBuildCheck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetAltitude(Vector3d worldPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RayIntersection(Vector3 worldStart, Vector3 worldDirection, out double intersectionDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RayIntersection(Vector3 worldStart, Vector3 worldDirection, out Vector3d intersection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool LineSphereIntersection(Vector3d relPos, Vector3d relVel, double radius, out Vector3d relIntersection)
	{
		throw null;
	}
}
