using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("PQuadSphere/PQuadSphere")]
public class GClass4 : MonoBehaviour
{
	public enum Enum0
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
		const_0,
		const_1,
		const_2,
		const_3,
		const_4,
		const_5
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
		public GClass3 buildQuad;

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

		public VertexBuildData()
		{
			gnomonicUVs = new GnomonicUV[6];
		}

		public void Reset()
		{
			buildQuad = null;
		}
	}

	public struct GnomonicUV
	{
		public bool acceptable;

		public double gnomonicU;

		public double gnomonicV;
	}

	public static bool Global_ForceShaderModel = true;

	public static bool Global_AllowScatter = true;

	public static double Global_ScatterFactor = 1.0;

	public static PQS_GameBindings GameBindings = new PQS_GameBindings();

	public EventData<GClass4> onReady = new EventData<GClass4>("OnReady");

	public GClass4 parentSphere;

	public GameObject LocalSpacePQStorage;

	public List<GClass3> LocalSpacePQList;

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

	public List<Material> materialsForUpdates = new List<Material>();

	public float frameTimeDelta;

	public float maxFrameTime;

	public bool meshCastShadows;

	public bool meshRecieveShadows;

	public int minLevel;

	public int maxLevel;

	public float maxQuadLenghtsPerFrame = 0.03f;

	public int maxLevelAtCurrentTgtSpeed;

	public float angularTargetSpeed;

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

	public Vector3d targetPosition;

	public Vector3d targetPositionPrev;

	public Vector3d relativeTargetPosition;

	public Vector3d relativeTargetPositionNormalized;

	public Vector3d lastRelTgtPosNormalized;

	public double targetDistance;

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

	public QuaternionD transformRotationInverse;

	public Vector3d transformPosition;

	public double minDetailDistance;

	public double maxDetailDistance;

	public bool isSubdivisionEnabled;

	public bool useSharedMaterial;

	public double radiusSquared;

	public double circumference;

	public List<GClass3> normalUpdateList;

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

	public int pqID;

	public GClass3[] quads;

	public static int itr;

	public static int tempInt;

	public int maxLevel1;

	public int fixedUpdateFrame;

	public float fixedFrameTime;

	public float prevFixedFrameTime;

	public PQSLandControl PQSLandControl;

	public PQSMod_AerialPerspectiveMaterial PQSAerialPerspectiveMaterial;

	public float savemaxFrameTime;

	public PQSMod_CelestialBodyTransform PQSModCBTransform;

	public Vector3d precisePosition;

	public PQSCache cache;

	public GClass4[] _childSpheres;

	public bool externalRenderScatter;

	public const float HalfPI = 1.5707963f;

	public bool hasCache;

	public static int cacheSideVertCount = 15;

	public static float cacheMeshSize = 1f;

	public static float cacheQuadSize = 1f;

	public static float cacheQuadSizeDiv2 = cacheQuadSize / 2f;

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

	public static double cacheVertCountReciprocal = 1.0 / (double)(cacheSideVertCount * cacheSideVertCount);

	public PQSMod[] mods;

	public int modCount;

	public ModiferRequirements modRequirements;

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

	public GClass3 buildQuad;

	public int vertexIndex;

	public static VertexBuildData vbData = new VertexBuildData();

	public double latitude;

	public double longitude;

	public double sy;

	public double sx;

	public Vector3d directionXZ;

	public int uMin;

	public int vMin;

	public int uMax;

	public int vMax;

	public double uDiff;

	public double vDiff;

	public Color c1;

	public Color c2;

	public Color c3;

	public Color c4;

	public double f1;

	public double f2;

	public double f3;

	public double f4;

	public Bounds meshBounds;

	public static Vector3d[] verts;

	public static Vector2[] uvs;

	public static Vector3[] normals;

	public static Vector3[] triNormals;

	public static Vector3d vertRel;

	public static Vector3d planetRel;

	public EventData<GClass4> OnReady => onReady;

	public Vector3d PrecisePosition
	{
		get
		{
			return precisePosition;
		}
		set
		{
			Vector3d vector3d = value - precisePosition;
			precisePosition = value;
			if (vector3d != Vector3d.zero)
			{
				FastUpdateQuadsPosition(vector3d);
			}
			int num;
			if (_childSpheres != null && (num = _childSpheres.Length) > 0)
			{
				int num2 = num;
				while (num2-- > 0)
				{
					_childSpheres[num2].PrecisePosition = value;
				}
			}
		}
	}

	public GClass4[] ChildSpheres
	{
		get
		{
			if (_childSpheres == null)
			{
				List<GClass4> list = new List<GClass4>(GetComponentsInChildren<GClass4>());
				list.Remove(this);
				_childSpheres = list.ToArray();
			}
			return _childSpheres;
		}
	}

	public void Reset()
	{
		radius = 1000.0;
		seed = 1;
		frameTimeDelta = 0f;
		maxFrameTime = 0.1f;
		minLevel = 3;
		maxLevel = 10;
		maxQuadLenghtsPerFrame = 0.03f;
		maxLevelAtCurrentTgtSpeed = maxLevel;
		minDetailDistance = 7.0;
		maxDetailDistance = 20.0;
		subdivisionThreshold = 1.0;
		visRadSeaLevelValue = 4.0;
		visRadAltitudeMax = 100000.0;
		visRadAltitudeValue = 1.5;
		collapseSeaLevelValue = 2.0;
		collapseAltitudeValue = 16.0;
		collapseAltitudeMax = 10000000.0;
		detailSeaLevelQuads = 3000.0;
		detailAltitudeQuads = 3000.0;
		detailAltitudeMax = 100000.0;
		mapFilename = "pqsMap";
		mapFilesize = 4096;
	}

	public void Start()
	{
		if (cacheMesh == null)
		{
			CreateCache();
		}
		if (PQSCache.Instance == null)
		{
			cache = (PQSCache)UnityEngine.Object.FindObjectOfType(typeof(PQSCache));
			if (cache == null)
			{
				Debug.Log("No PQS cache found");
				return;
			}
			PQSCache.Instance = cache;
		}
		else
		{
			cache = PQSCache.Instance;
		}
		if (!cache.cacheReady)
		{
			cache.CreateCache();
		}
		if (GameBindings.SettingsReady)
		{
			Global_ForceShaderModel = GameBindings.PLANET_FORCE_SHADER_MODEL_2_0;
			Global_AllowScatter = GameBindings.PLANET_SCATTER;
			Global_ScatterFactor = Mathf.Clamp01(GameBindings.PLANET_SCATTER_FACTOR);
		}
		ApplyTerrainShaderSettings();
		GameEvents.OnPQSStarting.Fire(this);
		if (!isStarted)
		{
			StartSphere(force: false);
		}
		else
		{
			Debug.LogError("[PQS]: PQS " + base.name + " already marked as started during Start() method. This is probably wrong!", base.gameObject);
		}
		LocalSpace componentInParent = GetComponentInParent<LocalSpace>();
		LocalSpacePQList = new List<GClass3>();
		string childName = "PQStorage";
		if (componentInParent != null)
		{
			LocalSpacePQStorage = componentInParent.gameObject.GetChild(childName);
		}
		if (LocalSpacePQStorage == null)
		{
			LocalSpacePQStorage = new GameObject(childName);
			LocalSpacePQStorage.transform.parent = ((componentInParent != null) ? componentInParent.gameObject.transform : base.gameObject.transform);
		}
		savemaxFrameTime = maxFrameTime;
		GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
	}

	public void OnDestroy()
	{
		GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
		if (PQSCache.Instance != null)
		{
			ResetSphere();
		}
	}

	public virtual void ApplyTerrainShaderSettings()
	{
		if (SystemInfo.graphicsShaderLevel >= 30 && (!GameBindings.SettingsReady || !GameBindings.PLANET_FORCE_SHADER_MODEL_2_0))
		{
			switch (GameSettings.TERRAIN_SHADER_QUALITY)
			{
			case 3:
				if (ultraQualitySurfaceMaterial != null)
				{
					surfaceMaterial = ultraQualitySurfaceMaterial;
					break;
				}
				goto case 2;
			case 2:
				if (highQualitySurfaceMaterial != null)
				{
					surfaceMaterial = highQualitySurfaceMaterial;
					break;
				}
				goto case 1;
			case 1:
				if (mediumQualitySurfaceMaterial != null)
				{
					surfaceMaterial = mediumQualitySurfaceMaterial;
					break;
				}
				goto default;
			default:
				if (lowQualitySurfaceMaterial != null)
				{
					surfaceMaterial = lowQualitySurfaceMaterial;
				}
				else if (surfaceMaterial == null)
				{
					surfaceMaterial = fallbackMaterial;
					Debug.Log("PQS - Graphics shader in fallback mode");
				}
				break;
			}
		}
		else
		{
			surfaceMaterial = fallbackMaterial;
			Debug.Log("PQS - Graphics shader in fallback mode");
		}
		if (!DEBUG_UseSharedMaterial)
		{
			Material m = new Material(surfaceMaterial);
			m.CopyKeywordsFrom(surfaceMaterial);
			surfaceMaterial = m;
		}
	}

	public void OnGameSettingsApplied()
	{
		if (HighLogic.LoadedScene == GameScenes.SETTINGS)
		{
			ApplyTerrainShaderSettings();
		}
	}

	public void StartSphere(bool force)
	{
		if (isStarted && (!force || !GameBindings.LoadedSceneIsGame))
		{
			return;
		}
		cache = (PQSCache)UnityEngine.Object.FindObjectOfType(typeof(PQSCache));
		parentSphere = base.gameObject.GetComponentOnParent<GClass4>();
		if (parentSphere != null && (!parentSphere.isStarted || !parentSphere.isAlive))
		{
			return;
		}
		normalUpdateList = new List<GClass3>();
		vbData.Reset();
		targetPositionPrev = Vector3d.down;
		pqID = 0;
		fixedUpdateFrame = 0;
		if (PQSCache.PresetList != null)
		{
			PQSCache.PQSSpherePreset preset = PQSCache.PresetList.GetPreset(base.gameObject.name);
			if (preset != null)
			{
				minDetailDistance = preset.minDistance;
				minLevel = preset.minSubdivision;
				maxLevel = preset.maxSubdivision;
			}
		}
		minLevel = Mathf.Min(minLevel, maxLevel - 1);
		minLevel = Mathf.Max(minLevel, 2);
		maxLevel = Mathf.Max(maxLevel, minLevel + 1);
		maxLevel1 = maxLevel + 1;
		maxLevelAtCurrentTgtSpeed = maxLevel;
		subdivisionThresholds = new double[maxLevel1];
		collapseThresholds = new double[maxLevel1];
		quadCounts = new int[maxLevel1];
		for (itr = 0; itr < maxLevel1; itr++)
		{
			subdivisionThresholds[itr] = radius * minDetailDistance / (double)Mathf.Pow(2f, itr) * subdivisionThreshold;
		}
		collapseDelta = (collapseAltitudeValue - collapseSeaLevelValue) / collapseAltitudeMax;
		visRadDelta = (visRadAltitudeValue - visRadSeaLevelValue) / visRadAltitudeMax;
		circumference = 6.2831854820251465 * radius;
		radiusSquared = radius * radius;
		SetupMods();
		SetupBuildDelegates(fakeBuild: false);
		isActive = false;
		isAlive = false;
		isThinking = false;
		isStarted = true;
		isDisabled = false;
		if (force)
		{
			isAlive = true;
		}
		else
		{
			Mod_OnSphereStart();
		}
		UpdateVisual();
		if (!isAlive || !(target != null))
		{
			return;
		}
		UpdateQuadsInit();
		Mod_OnPostSetup();
		Mod_OnSphereStarted();
		int num = ChildSpheres.Length;
		while (num-- > 0)
		{
			if (!ChildSpheres[num].isStarted || force)
			{
				ChildSpheres[num].StartSphere(force);
			}
		}
		OnReady.Fire(this);
		StartCoroutine(UpdateSphere());
	}

	[ContextMenu("Reset Sphere")]
	public void ResetSphere()
	{
		if (isAlive || isStarted)
		{
			if (quads != null && quads.Length == 6)
			{
				for (int i = 0; i < 6; i++)
				{
					if (quads[i] != null)
					{
						quads[i].Destroy();
						quads[i] = null;
					}
				}
				quads = null;
			}
			Mod_OnSphereReset();
			if (normalUpdateList != null)
			{
				normalUpdateList.Clear();
			}
			quadCount = 0;
			isAlive = false;
			isStarted = false;
			int num = ChildSpheres.Length;
			while (num-- > 0)
			{
				ChildSpheres[num].ResetSphere();
			}
		}
		ResetLaunchsitePlacementRender();
		StopCoroutine("UpdateSphere");
		StopCoroutine("ResetAndWaitCoroutine");
	}

	[ContextMenu("Rebuild Sphere")]
	public void RebuildSphere()
	{
		ResetSphere();
		StartSphere(force: false);
	}

	public void ForceStart()
	{
		if (parentSphere == null)
		{
			StopCoroutine("ResetAndWaitCoroutine");
			StartSphere(force: true);
		}
	}

	public void StartUpSphere()
	{
		StartSphere(force: true);
	}

	public void ResetAndWait()
	{
		if (parentSphere == null)
		{
			ResetSphere();
			StartCoroutine(ResetAndWaitCoroutine());
		}
	}

	public IEnumerator ResetAndWaitCoroutine()
	{
		Mod_OnSphereStart();
		while (!isAlive && !isDisabled)
		{
			yield return null;
			Mod_OnSphereStart();
		}
		StartSphere(force: false);
	}

	public void SetupExternalRender()
	{
		externalRenderScatter = Global_AllowScatter;
		Global_AllowScatter = false;
		isBuildingMaps = true;
		SetupMods();
		SetupBuildDelegates(fakeBuild: true);
	}

	public void SetupLaunchsitePlacementRender()
	{
		SetupMods();
		if (PQSLandControl != null)
		{
			PQSLandControl.createColors = false;
			PQSLandControl.createScatter = false;
		}
		if (PQSAerialPerspectiveMaterial != null)
		{
			PQSAerialPerspectiveMaterial.modEnabled = false;
		}
		savemaxFrameTime = maxFrameTime;
		maxFrameTime = 240f;
		SetupBuildDelegates(fakeBuild: false);
	}

	public void ResetLaunchsitePlacementRender()
	{
		if (PQSLandControl != null)
		{
			PQSLandControl.createColors = true;
			PQSLandControl.createScatter = true;
		}
		if (PQSAerialPerspectiveMaterial != null)
		{
			PQSAerialPerspectiveMaterial.modEnabled = true;
		}
		maxFrameTime = savemaxFrameTime;
		SetupBuildDelegates(fakeBuild: false);
	}

	public void CloseExternalRender()
	{
		isBuildingMaps = false;
		Global_AllowScatter = externalRenderScatter;
	}

	public Texture2D[] CreateMaps(int width, double maxHeight, bool hasOcean, double oceanHeight, Color oceanColor)
	{
		ProfileTimer.Push("PQSMap: SaveMap");
		int num = width / 2;
		double num2 = 360.0 / (double)width;
		double num3 = 180.0 / (double)num;
		VertexBuildData vertexBuildData = new VertexBuildData();
		Texture2D texture2D = null;
		Texture2D texture2D2 = null;
		bool global_AllowScatter = Global_AllowScatter;
		Global_AllowScatter = false;
		isBuildingMaps = true;
		SetupMods();
		SetupBuildDelegates(fakeBuild: true);
		List<Texture2D> list = new List<Texture2D>();
		texture2D = new Texture2D(width, num, TextureFormat.ARGB32, mipChain: true);
		list.Add(texture2D);
		texture2D2 = new Texture2D(width, num, TextureFormat.RGB24, mipChain: true);
		list.Add(texture2D2);
		double num4 = (float)(1.0 / maxHeight);
		for (int i = 0; i < num; i++)
		{
			QuaternionD quaternionD = QuaternionD.AngleAxis(90.0 - num3 * (double)i, Vector3d.right);
			for (int j = 0; j < width; j++)
			{
				QuaternionD quaternionD2 = QuaternionD.AngleAxis(num2 * (double)j, Vector3d.up) * quaternionD;
				vertexBuildData.directionFromCenter = quaternionD2 * Vector3d.forward;
				vertexBuildData.vertHeight = radius;
				Mod_OnVertexBuildHeight(vertexBuildData);
				Mod_OnVertexBuild(vertexBuildData);
				double num5 = (vertexBuildData.vertHeight - radius) * num4;
				if (num5 < 0.0)
				{
					num5 = 0.0;
				}
				if (num5 > 1.0)
				{
					num5 = 1.0;
				}
				if (hasOcean)
				{
					if (num5 <= oceanHeight)
					{
						oceanColor.a = 1f;
						texture2D.SetPixel(j, i, oceanColor);
					}
					else
					{
						vertexBuildData.vertColor.a = 0f;
						texture2D.SetPixel(j, i, vertexBuildData.vertColor);
					}
				}
				else
				{
					vertexBuildData.vertColor.a = 1f;
					texture2D.SetPixel(j, i, vertexBuildData.vertColor);
				}
				float num6 = (float)num5;
				texture2D2.SetPixel(j, i, new Color(num6, num6, num6));
			}
		}
		foreach (Texture2D item in list)
		{
			item.Apply();
		}
		isFakeBuild = false;
		isBuildingMaps = false;
		Global_AllowScatter = global_AllowScatter;
		ProfileTimer.Pop();
		return list.ToArray();
	}

	public GClass3 AssignQuad(int subdiv)
	{
		GClass3 quad = cache.GetQuad();
		quad.transform.parent = base.transform;
		if (useSharedMaterial)
		{
			quad.meshRenderer.sharedMaterial = surfaceMaterial;
		}
		else
		{
			quad.meshRenderer.material = surfaceMaterial;
		}
		if (GameSettings.CELESTIAL_BODIES_CAST_SHADOWS)
		{
			quad.meshRenderer.shadowCastingMode = (meshCastShadows ? ShadowCastingMode.On : ShadowCastingMode.Off);
			quad.meshRenderer.receiveShadows = meshRecieveShadows;
		}
		else
		{
			quad.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			quad.meshRenderer.receiveShadows = meshRecieveShadows;
		}
		quad.meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		quad.sphereRoot = this;
		quad.gameObject.layer = base.gameObject.layer;
		quad.subdivision = subdiv;
		quad.subdivideThresholdFactor = 1.0;
		quad.quadRoot = null;
		quad.id = pqID++;
		quad.gameObject.SetActive(value: true);
		quad.isActive = true;
		quad.isForcedInvisible = false;
		return quad;
	}

	public void QuadCreated(GClass3 quad)
	{
		Mod_OnQuadCreate(quad);
	}

	public void DestroyQuad(GClass3 quad)
	{
		Mod_OnQuadDestroy(quad);
		if (quad.isBuilt)
		{
			if (useSharedMaterial)
			{
				quad.meshRenderer.sharedMaterial = null;
				quad.meshRenderer.material = null;
			}
			else
			{
				UnityEngine.Object.Destroy(quad.meshRenderer.material);
				quad.meshRenderer.material = null;
			}
		}
		cache.DestroyQuad(quad);
	}

	public GClass3 method_0(Enum0 initQuad)
	{
		return quads[(int)initQuad];
	}

	public void CreateQuads()
	{
		if (quads != null && quads.Length == 6)
		{
			for (int i = 0; i < 6; i++)
			{
				if (quads[i] != null)
				{
					quads[i].Destroy();
				}
			}
		}
		quads = new GClass3[6];
		for (int num = 5; num >= 0; num--)
		{
			quads[num] = AssignQuad(0);
		}
		SetupQuad(method_0(Enum0.xPos), base.gameObject.name + " Xp", QuadPlane.const_0, method_0(Enum0.zPos), method_0(Enum0.zNeg), method_0(Enum0.yNeg), method_0(Enum0.yPos), Vector3d.right);
		SetupQuad(method_0(Enum0.xNeg), base.gameObject.name + " Xn", QuadPlane.const_1, method_0(Enum0.zPos), method_0(Enum0.zNeg), method_0(Enum0.yPos), method_0(Enum0.yNeg), Vector3d.left);
		SetupQuad(method_0(Enum0.yPos), base.gameObject.name + " Yp", QuadPlane.const_2, method_0(Enum0.zPos), method_0(Enum0.zNeg), method_0(Enum0.xPos), method_0(Enum0.xNeg), Vector3d.up);
		SetupQuad(method_0(Enum0.yNeg), base.gameObject.name + " Yn", QuadPlane.const_3, method_0(Enum0.zNeg), method_0(Enum0.zPos), method_0(Enum0.xPos), method_0(Enum0.xNeg), Vector3d.down);
		SetupQuad(method_0(Enum0.zPos), base.gameObject.name + " Zp", QuadPlane.const_4, method_0(Enum0.yNeg), method_0(Enum0.yPos), method_0(Enum0.xPos), method_0(Enum0.xNeg), Vector3d.forward);
		SetupQuad(method_0(Enum0.zNeg), base.gameObject.name + " Zn", QuadPlane.const_5, method_0(Enum0.yPos), method_0(Enum0.yNeg), method_0(Enum0.xPos), method_0(Enum0.xNeg), Vector3d.back);
		quadCount += 6;
		for (int num2 = 5; num2 >= 0; num2--)
		{
			quads[num2].SetupQuad(null, GClass3.QuadChild.Null);
			QuadCreated(quads[num2]);
		}
	}

	public void SetupQuad(GClass3 quad, string name, QuadPlane plane, GClass3 north, GClass3 south, GClass3 east, GClass3 west, Vector3d pos)
	{
		quad.scalePlaneRelative = 1.0;
		quad.scalePlanetRelative = radius;
		quad.positionPlanePosition = pos.normalized;
		quad.positionParentRelative.Zero();
		quad.positionPlanetRelative = quad.positionPlanePosition;
		quad.positionPlanet = quad.positionPlanetRelative * radius;
		quad.plane = plane;
		quad.gameObject.name = name;
		quad.north = north;
		quad.south = south;
		quad.east = east;
		quad.west = west;
		quad.uvSW = Vector2.zero;
		quad.uvDelta = Vector2.one;
	}

	public IEnumerator UpdateSphere()
	{
		while (isAlive)
		{
			if (!isDisabled)
			{
				Mod_OnPreUpdate();
				UpdateVisual();
				if (isAlive && isActive && target != null)
				{
					UpdateQuads();
					Mod_OnUpdateFinished();
				}
			}
			if (frameTimeDelta > 0f)
			{
				yield return new WaitForSeconds(frameTimeDelta);
			}
			else
			{
				yield return null;
			}
		}
	}

	public void FastUpdateQuadsPosition(Vector3d movement)
	{
		if (LocalSpacePQList == null)
		{
			return;
		}
		for (int num = LocalSpacePQList.Count - 1; num >= 0; num--)
		{
			if (LocalSpacePQList[num] != null)
			{
				LocalSpacePQList[num].FastUpdateSubQuadsPosition(movement);
			}
			else
			{
				LocalSpacePQList.RemoveAt(num);
			}
		}
	}

	public void PreciseUpdateQuadsPosition()
	{
		if (LocalSpacePQList == null)
		{
			return;
		}
		for (int num = LocalSpacePQList.Count - 1; num >= 0; num--)
		{
			if (LocalSpacePQList[num] != null)
			{
				LocalSpacePQList[num].PreciseUpdateSubQuadsPosition();
			}
			else
			{
				LocalSpacePQList.RemoveAt(num);
			}
		}
	}

	public void AddPQToLocalSpaceStorage(GClass3 newLocalSpaceQuad)
	{
		LocalSpacePQList.AddUnique(newLocalSpaceQuad);
	}

	public void RemovePQFromLocalSpaceStorage(GClass3 quadToRemove)
	{
		LocalSpacePQList.Remove(quadToRemove);
	}

	public void EnableSphere()
	{
		isDisabled = false;
		ResetAndWait();
	}

	public void ActivateSphere()
	{
		if (isActive)
		{
			return;
		}
		if (!isAlive)
		{
			if (!isStarted)
			{
				StartSphere(force: false);
				if (!isAlive)
				{
					UpdateQuadsInit();
					isAlive = true;
				}
			}
			else
			{
				UpdateQuadsInit();
				isAlive = true;
			}
		}
		isActive = true;
		int num = ChildSpheres.Length;
		while (num-- > 0)
		{
			ChildSpheres[num].ActivateSphere();
		}
		Mod_OnActive();
		SetVisible(visible: true);
	}

	public void DisableSphere()
	{
		isDisabled = true;
		Mod_OnInactive();
		ResetSphere();
	}

	public void DeactivateSphere()
	{
		if (isActive)
		{
			isActive = false;
			Mod_OnInactive();
			int num = ChildSpheres.Length;
			while (num-- > 0)
			{
				ChildSpheres[num].DeactivateSphere();
			}
			SetVisible(visible: false);
		}
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			for (int num = 5; num >= 0; num--)
			{
				quads[num].SetMasterVisible();
			}
		}
		else
		{
			for (int num2 = 5; num2 >= 0; num2--)
			{
				quads[num2].SetMasterInvisible();
			}
		}
		int num3 = ChildSpheres.Length;
		while (num3-- > 0)
		{
			ChildSpheres[num3].SetVisible(visible);
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
		int num = ChildSpheres.Length;
		while (num-- > 0)
		{
			ChildSpheres[num].SetTarget(target);
		}
	}

	public void SetSecondaryTarget(Transform secondaryTarget)
	{
		this.secondaryTarget = secondaryTarget;
		int num = ChildSpheres.Length;
		while (num-- > 0)
		{
			ChildSpheres[num].SetSecondaryTarget(secondaryTarget);
		}
	}

	public void UpdateVisual()
	{
		if (target == null)
		{
			return;
		}
		targetPosition = target.position;
		relativeTargetPosition = base.transform.InverseTransformPoint(targetPosition);
		relativeTargetPositionNormalized = relativeTargetPosition.normalized;
		if (fixedUpdateFrame != 0)
		{
			fixedFrameTime = Time.realtimeSinceStartup - prevFixedFrameTime;
			prevFixedFrameTime = Time.realtimeSinceStartup;
			fixedUpdateFrame = 0;
			targetVelocity = (relativeTargetPosition - targetPositionPrev) / fixedFrameTime;
			targetSpeed = targetVelocity.magnitude;
			targetPositionPrev = relativeTargetPosition;
			angularTargetSpeed = (float)Math.Acos(Vector3d.Dot(relativeTargetPositionNormalized, lastRelTgtPosNormalized));
			lastRelTgtPosNormalized = relativeTargetPositionNormalized;
			if (float.IsNaN(angularTargetSpeed))
			{
				angularTargetSpeed = 0f;
			}
		}
		if (maxQuadLenghtsPerFrame > 0f)
		{
			maxLevelAtCurrentTgtSpeed = minLevel;
			itr = maxLevel;
			while (itr >= minLevel)
			{
				if (angularTargetSpeed >= 1.5707963f / Mathf.Pow(2f, itr) * maxQuadLenghtsPerFrame)
				{
					itr--;
					continue;
				}
				maxLevelAtCurrentTgtSpeed = itr;
				break;
			}
		}
		else
		{
			maxLevelAtCurrentTgtSpeed = maxLevel;
		}
		targetDistance = relativeTargetPosition.magnitude;
		targetAltitude = targetDistance - radius;
		visibleAltitude = targetAltitude;
		relativeTargetPosition = relativeTargetPositionNormalized * radius;
		targetHeight = targetDistance - GetSurfaceHeight(relativeTargetPositionNormalized);
		if (secondaryTarget != null)
		{
			relativeSecondaryTargetPosition = base.transform.InverseTransformPoint(secondaryTarget.position);
			targetSecondaryAltitude = relativeSecondaryTargetPosition.magnitude - radius;
			visibleAltitude = Math.Max(targetAltitude, targetSecondaryAltitude);
		}
		double num = radius + visibleAltitude;
		horizonDistance = Math.Sqrt(num * num - radiusMin * radiusMin) + halfChord;
		horizonAngle = Math.Pow(Math.Cos(radius / (radius + visibleAltitude)), -1.0);
		visRad = visRadSeaLevelValue + visibleAltitude * visRadDelta;
		if (visRad < visRadAltitudeValue)
		{
			visRad = visRadAltitudeValue;
		}
		else if (visRad > visRadSeaLevelValue)
		{
			visRad = visRadSeaLevelValue;
		}
		visibleRadius = horizonDistance * Math.Sin(horizonAngle) * visRad;
		for (itr = 0; itr < maxLevel1; itr++)
		{
			collapseThreshold = collapseSeaLevelValue + targetAltitude * collapseDelta;
			if (collapseThreshold < collapseSeaLevelValue)
			{
				collapseThreshold = collapseSeaLevelValue;
			}
			else if (collapseThreshold > collapseAltitudeValue)
			{
				collapseThreshold = collapseAltitudeValue;
			}
			collapseThresholds[itr] = subdivisionThresholds[itr] * collapseThreshold;
		}
		if (targetAltitude > maxDetailDistance * radius)
		{
			DisableSubdivision();
		}
		else
		{
			EnableSubdivision();
		}
	}

	public void DisableSubdivision()
	{
		if (isSubdivisionEnabled)
		{
			UpdateQuads();
			isSubdivisionEnabled = false;
		}
	}

	public void EnableSubdivision()
	{
		if (!isSubdivisionEnabled)
		{
			isSubdivisionEnabled = true;
		}
	}

	public void UpdateQuadsInit()
	{
		CreateQuads();
		isThinking = true;
		int num = -1;
		quadAllowBuild = false;
		int num2 = 10;
		for (int num3 = 0; num3 < num2; num3 = ((num >= quadCount) ? (num3 + 1) : 0))
		{
			num = quadCount;
			for (int num4 = 5; num4 >= 0; num4--)
			{
				quads[num4].UpdateSubdivisionInit();
			}
		}
		quadAllowBuild = true;
		for (int num5 = 5; num5 >= 0; num5--)
		{
			quads[num5].UpdateSubdivision();
		}
		isThinking = false;
	}

	public void UpdateQuads()
	{
		if (quads == null)
		{
			return;
		}
		isThinking = true;
		maxFrameEnd = Time.realtimeSinceStartup + maxFrameTime;
		int num = quads.Length;
		for (int i = 1; i < num; i++)
		{
			GClass3 gClass = quads[i];
			int num2 = i - 1;
			while (num2 >= 0 && !((relativeTargetPosition - quads[num2].positionPlanetRelative).sqrMagnitude <= (relativeTargetPosition - gClass.positionPlanetRelative).sqrMagnitude))
			{
				quads[num2 + 1] = quads[num2];
				num2--;
			}
			quads[num2 + 1] = gClass;
		}
		for (int j = 0; j < Mathf.Min(quads.Length, 5); j++)
		{
			quads[j].UpdateSubdivision();
		}
		if (reqCustomNormals)
		{
			UpdateEdges();
		}
		isThinking = false;
	}

	public void UpdateEdges()
	{
		int count;
		while ((count = normalUpdateList.Count) > 0)
		{
			count--;
			GClass3 gClass = normalUpdateList[count];
			normalUpdateList.RemoveAt(count);
			if (gClass != null)
			{
				if (!gClass.isSubdivided && gClass.isVisible)
				{
					UpdateEdgeNormals(gClass);
				}
				gClass.isQueuedForNormalUpdate = false;
				gClass.isQueuedOnlyForCornerNormalUpdate = false;
			}
		}
	}

	public void FixedUpdate()
	{
		fixedUpdateFrame++;
		Mod_OnSphereTransformUpdate();
		transformRotationInverse = QuaternionD.Inverse(transformRotation);
	}

	public void Update()
	{
		if (isAlive && isStarted && isThinking)
		{
			RebuildSphere();
			Debug.LogError("PQS " + base.gameObject.name + ": Restarted");
		}
	}

	public void SetCache()
	{
		if (hasCache)
		{
			ClearCache();
		}
		for (int i = 0; i < 6; i++)
		{
			quads[i].SetCache();
		}
		hasCache = true;
	}

	public void ClearCache()
	{
		for (int i = 0; i < 6; i++)
		{
			quads[i].ClearCache();
		}
		hasCache = false;
	}

	public static void LoadCacheSettings(ConfigNode node)
	{
		if (node.HasValue("vertCount"))
		{
			cacheSideVertCount = int.Parse(node.GetValue("vertCount"));
		}
		if (node.HasValue("cacheMeshSize"))
		{
			cacheMeshSize = float.Parse(node.GetValue("cacheMeshSize"));
		}
		if (node.HasValue("cacheQuadSize"))
		{
			cacheQuadSize = float.Parse(node.GetValue("cacheQuadSize"));
			cacheQuadSizeDiv2 = cacheQuadSize / 2f;
		}
	}

	public static void CreateCache()
	{
		cacheRes = cacheSideVertCount - 1;
		cacheResDiv2 = cacheRes / 2;
		cacheResDiv2Plus1 = cacheResDiv2 + 1;
		cacheVertCount = cacheSideVertCount * cacheSideVertCount;
		cacheTriIndexCount = cacheRes * cacheRes * 6;
		cacheTriCount = cacheTriIndexCount / 3;
		cacheVerts = new Vector3[cacheVertCount];
		Vector3[] array = new Vector3[cacheVertCount];
		cacheUVs = new Vector2[cacheVertCount];
		cacheUVQuad = new Vector2d[cacheVertCount];
		CreateCacheNormals();
		CreateCacheColors();
		CreateCacheUVExtras();
		float num = cacheMeshSize / (float)cacheRes;
		float num2 = cacheMeshSize / (float)cacheRes;
		float num3 = 1f / (float)cacheSideVertCount;
		float num4 = 1f / (float)cacheSideVertCount;
		float num5 = cacheMeshSize / 2f;
		float num6 = cacheMeshSize / 2f;
		Vector3 vector = new Vector3(0f, 1f, 0f);
		int i = 0;
		int num7 = 0;
		for (; i < cacheSideVertCount; i++)
		{
			int num8 = 0;
			while (num8 < cacheSideVertCount)
			{
				cacheVerts[num7] = new Vector3(num5 - (float)num8 * num, 0f, num6 - (float)i * num2);
				cacheUVs[num7] = new Vector2((float)num8 * num3, (float)i * num4);
				cacheUVQuad[num7] = new Vector2d(1.0 - (double)((float)num8 * num3), 1.0 - (double)((float)i * num4));
				array[num7] = vector;
				num8++;
				num7++;
			}
		}
		CreateIndexCache();
		cacheTangents = new Vector4[cacheVertCount];
		tan1 = new Vector3[cacheVertCount];
		tan2 = new Vector3[cacheVertCount];
		int num9 = 0;
		int num10 = 0;
		while (num9 < cacheTriCount)
		{
			int num11 = cacheIndices[0][num10];
			int num12 = cacheIndices[0][num10 + 1];
			int num13 = cacheIndices[0][num10 + 2];
			Vector3 vector2 = cacheVerts[num11];
			Vector3 vector3 = cacheVerts[num12];
			Vector3 vector4 = cacheVerts[num13];
			Vector3 vector5 = cacheUVs[num11];
			Vector3 vector6 = cacheUVs[num12];
			Vector3 vector7 = cacheUVs[num13];
			float num14 = vector3.x - vector2.x;
			float num15 = vector4.x - vector2.x;
			float num16 = vector3.y - vector2.y;
			float num17 = vector4.y - vector2.y;
			float num18 = vector3.z - vector2.z;
			float num19 = vector4.z - vector2.z;
			float num20 = vector6.x - vector5.x;
			float num21 = vector7.x - vector5.x;
			float num22 = vector6.y - vector5.y;
			float num23 = vector7.y - vector5.y;
			float num24 = 1f / (num20 * num23 - num21 * num22);
			Vector3 vector8 = new Vector3((num23 * num14 - num22 * num15) * num24, (num23 * num16 - num22 * num17) * num24, (num23 * num18 - num22 * num19) * num24);
			Vector3 vector9 = new Vector3((num20 * num15 - num21 * num14) * num24, (num20 * num17 - num21 * num16) * num24, (num20 * num19 - num21 * num18) * num24);
			tan1[num11] += vector8;
			tan1[num12] += vector8;
			tan1[num13] += vector8;
			tan2[num11] += vector9;
			tan2[num12] += vector9;
			tan2[num13] += vector9;
			num9++;
			num10 += 3;
		}
		for (num9 = 0; num9 < cacheVertCount; num9++)
		{
			vector = array[num9];
			Vector3 tangent = tan1[num9];
			Vector3.OrthoNormalize(ref vector, ref tangent);
			cacheTangents[num9].x = tangent.x;
			cacheTangents[num9].y = tangent.y;
			cacheTangents[num9].z = tangent.z;
			cacheTangents[num9].w = ((Vector3.Dot(Vector3.Cross(vector, tangent), tan2[num9]) < 0f) ? (-1f) : 1f);
		}
		cacheMesh = new Mesh();
		cacheMesh.vertices = cacheVerts;
		cacheMesh.uv = cacheUVs;
		cacheMesh.normals = array;
		cacheMesh.tangents = cacheTangents;
		cacheMesh.colors = cacheColors;
		cacheMesh.uv2 = cacheUV2s;
		cacheMesh.uv3 = cacheUV3s;
		cacheMesh.uv4 = cacheUV4s;
		cacheMesh.triangles = cacheIndices[0];
		verts = new Vector3d[cacheVertCount];
		uvs = new Vector2[cacheVertCount];
		normals = new Vector3[cacheVertCount];
		triNormals = new Vector3[cacheTriCount];
	}

	public static void BuildTangents(GClass3 quad)
	{
		for (int i = 0; i < cacheVertCount; i++)
		{
			Vector3 normal = quad.mesh.normals[i];
			Vector3 tangent = Vector3.zero;
			Vector3.OrthoNormalize(ref normal, ref tangent);
			cacheTangents[i].x = tangent.x;
			cacheTangents[i].y = tangent.y;
			cacheTangents[i].z = tangent.z;
			cacheTangents[i].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), tan2[i]) < 0f) ? (-1f) : 1f);
		}
		quad.mesh.tangents = cacheTangents;
	}

	public static void CreateIndexCache()
	{
		cacheIndices = new int[17][];
		cacheIndices[0] = new int[cacheTriIndexCount];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		while (num < cacheRes)
		{
			int num4 = 0;
			while (num4 < cacheRes)
			{
				cacheIndices[0][num3++] = num2;
				cacheIndices[0][num3++] = num2 + cacheSideVertCount;
				cacheIndices[0][num3++] = num2 + 1;
				cacheIndices[0][num3++] = num2 + 1;
				cacheIndices[0][num3++] = num2 + cacheSideVertCount;
				cacheIndices[0][num3++] = num2 + cacheSideVertCount + 1;
				num4++;
				num2++;
			}
			num++;
			num2++;
		}
		for (int i = 1; i < 17; i++)
		{
			cacheIndices[i] = new int[cacheTriIndexCount];
			cacheIndices[0].CopyTo(cacheIndices[i], 0);
			CreateIndexState((EdgeState)i, cacheIndices[i], cacheIndices[0]);
		}
	}

	public static void CreateIndexState(EdgeState state, int[] tris, int[] trisBck)
	{
		int num = 0;
		if ((state & EdgeState.NorthLerp) != 0)
		{
			for (int i = 0; i < cacheRes; i += 2)
			{
				int num2 = ti(i, 0);
				int num3 = ti(i + 1, 0);
				tris[num3] = (tris[num2 + 3] = trisBck[num2]);
				tris[num2] = 0;
				tris[num2 + 1] = 0;
				tris[num2 + 2] = 0;
			}
		}
		if ((state & EdgeState.SouthLerp) != 0)
		{
			for (int i = 0; i < cacheRes; i += 2)
			{
				int num2 = ti(i, cacheRes - 1);
				int num3 = ti(i + 1, cacheRes - 1);
				tris[num3 + 1] = (tris[num2 + 5] = trisBck[num3 + 5]);
				tris[num3 + 3] = 0;
				tris[num3 + 4] = 0;
				tris[num3 + 5] = 0;
			}
		}
		if ((state & EdgeState.EastLerp) != 0)
		{
			for (int i = 0; i < cacheRes; i += 2)
			{
				int num2 = ti(0, i);
				int num3 = ti(0, i + 1);
				tris[num3] = (tris[num2 + 4] = trisBck[num2]);
				tris[num2] = 0;
				tris[num2 + 1] = 0;
				tris[num2 + 2] = 0;
			}
		}
		if ((state & EdgeState.WestLerp) != 0)
		{
			for (int i = 0; i < cacheRes; i += 2)
			{
				int num2 = ti(cacheRes - 1, i);
				int num3 = ti(cacheRes - 1, i + 1);
				tris[num3 + 2] = (tris[num2 + 5] = trisBck[num3 + 5]);
				tris[num3 + 3] = 0;
				tris[num3 + 4] = 0;
				tris[num3 + 5] = 0;
			}
		}
	}

	public static int ti(int x, int z)
	{
		return x * 6 + z * cacheRes * 6;
	}

	public static int vi(int x, int z)
	{
		return z * cacheSideVertCount + x;
	}

	public static void CreateCacheNormals()
	{
		cacheNormals = new Vector3[cacheVertCount];
		for (int i = 0; i < cacheVertCount; i++)
		{
			cacheNormals[i] = Vector3.zero;
		}
	}

	public static void CreateCacheColors()
	{
		cacheColors = new Color[cacheVertCount];
		for (int i = 0; i < cacheVertCount; i++)
		{
			cacheColors[i] = Color.white;
		}
	}

	public static void CreateCacheUVExtras()
	{
		cacheUV2s = new Vector2[cacheVertCount];
		cacheUV3s = new Vector2[cacheVertCount];
		cacheUV4s = new Vector2[cacheVertCount];
		for (int i = 0; i < cacheVertCount; i++)
		{
			cacheUV2s[i] = Vector2.zero;
			cacheUV3s[i] = Vector2.zero;
			cacheUV4s[i] = Vector2.zero;
		}
	}

	public static void BuildNormals(GClass3 quad)
	{
		int num = 0;
		int num2 = 0;
		Vector3 vector3 = default(Vector3);
		while (num < cacheTriCount)
		{
			Vector3 vector = verts[cacheIndices[0][num2 + 1]] - verts[cacheIndices[0][num2]];
			Vector3 vector2 = verts[cacheIndices[0][num2 + 2]] - verts[cacheIndices[0][num2]];
			vector3.x = vector.y * vector2.z - vector.z * vector2.y;
			vector3.y = vector.z * vector2.x - vector.x * vector2.z;
			vector3.z = vector.x * vector2.y - vector.y * vector2.x;
			triNormals[num] = vector3.normalized;
			num++;
			num2 += 3;
		}
		for (num2 = 0; num2 < cacheVertCount; num2++)
		{
			quad.vertNormals[num2].x = 0f;
			quad.vertNormals[num2].y = 0f;
			quad.vertNormals[num2].z = 0f;
		}
		num = 0;
		num2 = 0;
		while (num < cacheTriCount)
		{
			quad.vertNormals[cacheIndices[0][num2]] = quad.vertNormals[cacheIndices[0][num2]] + triNormals[num];
			quad.vertNormals[cacheIndices[0][num2 + 1]] = quad.vertNormals[cacheIndices[0][num2 + 1]] + triNormals[num];
			quad.vertNormals[cacheIndices[0][num2 + 2]] = quad.vertNormals[cacheIndices[0][num2 + 2]] + triNormals[num];
			num++;
			num2 += 3;
		}
		for (num2 = 0; num2 < quad.vertNormals.Length; num2++)
		{
			quad.vertNormals[num2] = quad.vertNormals[num2].normalized;
		}
	}

	public static void BuildNormalsNoClear(GClass3 quad)
	{
		int[] array = cacheIndices[0];
		int num = 0;
		int num2 = 0;
		Vector3 vector3 = default(Vector3);
		while (num < cacheTriCount)
		{
			Vector3 vector = verts[array[num2 + 1]] - verts[array[num2]];
			Vector3 vector2 = verts[array[num2 + 2]] - verts[array[num2]];
			vector3.x = vector.y * vector2.z - vector.z * vector2.y;
			vector3.y = vector.z * vector2.x - vector.x * vector2.z;
			vector3.z = vector.x * vector2.y - vector.y * vector2.x;
			triNormals[num] = vector3.normalized;
			num++;
			num2 += 3;
		}
		num = 0;
		num2 = 0;
		while (num < cacheTriCount)
		{
			quad.vertNormals[array[num2]] = (quad.vertNormals[array[num2]] + triNormals[num]).normalized;
			quad.vertNormals[array[num2 + 1]] = (quad.vertNormals[array[num2 + 1]] + triNormals[num]).normalized;
			quad.vertNormals[array[num2 + 2]] = (quad.vertNormals[array[num2 + 2]] + triNormals[num]).normalized;
			num++;
			num2 += 3;
		}
	}

	public void Mod_OnSetup()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnSetup();
		}
	}

	public void Mod_OnPostSetup()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnPostSetup();
		}
	}

	public void Mod_OnSphereReset()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnSphereReset();
		}
	}

	public void Mod_OnSphereStart()
	{
		int num = 0;
		for (int i = 0; i < modCount; i++)
		{
			if (mods[i].OnSphereStart())
			{
				num++;
			}
		}
		if (num == 0)
		{
			isAlive = true;
			if (target == null && parentSphere != null && parentSphere.target != null)
			{
				SetTarget(parentSphere.target);
				SetSecondaryTarget(parentSphere.secondaryTarget);
			}
			if (target == null && Camera.main != null)
			{
				SetTarget(Camera.main.transform);
			}
		}
	}

	public void Mod_OnSphereStarted()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnSphereStarted();
		}
	}

	public void Mod_OnSphereTransformUpdate()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnSphereTransformUpdate();
		}
	}

	public double Mod_GetVertexMaxHeight()
	{
		double num = 0.0;
		for (int i = 0; i < modCount; i++)
		{
			num += mods[i].GetVertexMaxHeight();
		}
		return num;
	}

	public double Mod_GetVertexMinHeight()
	{
		double num = 0.0;
		for (int i = 0; i < modCount; i++)
		{
			num += mods[i].GetVertexMinHeight();
		}
		return num;
	}

	public void Mod_OnPreUpdate()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnPreUpdate();
		}
	}

	public void Mod_OnUpdateFinished()
	{
		bool flag = false;
		for (int i = 0; i < modCount; i++)
		{
			if (mods[i] != null)
			{
				mods[i].OnUpdateFinished();
			}
			else
			{
				flag = true;
			}
		}
		if (flag)
		{
			ResetModList();
		}
	}

	public void Mod_OnActive()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnSphereActive();
		}
	}

	public void Mod_OnInactive()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnSphereInactive();
		}
	}

	public void Mod_OnVertexBuildHeight(VertexBuildData data, bool overrideQuadBuildCheck = false)
	{
		if (reqVertexMapCoods)
		{
			BuildVertexMapCoords(data);
		}
		for (int i = 0; i < modCount; i++)
		{
			mods[i].overrideQuadBuildCheck = overrideQuadBuildCheck;
			mods[i].OnVertexBuildHeight(data);
			mods[i].overrideQuadBuildCheck = false;
		}
	}

	public void Mod_OnVertexBuild(VertexBuildData data)
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnVertexBuild(data);
		}
		if (!isFakeBuild)
		{
			if (surfaceRelativeQuads)
			{
				BuildVertexSurfaceRelative(data);
			}
			else
			{
				BuildVertexHeight(data);
			}
			if (!reqCustomNormals)
			{
				BuildVertexSphereNormal(data);
			}
			if (reqColorChannel)
			{
				BuildVertexColor(data);
			}
			if (reqSphereUV)
			{
				BuildVertexSphereUV(data);
			}
			if (reqUVQuad)
			{
				BuildVertexQuadUV(data);
			}
			if (reqUV2)
			{
				BuildVertexUV2(data);
			}
			if (reqUV3)
			{
				BuildVertexUV3(data);
			}
			if (reqUV4)
			{
				BuildVertexUV4(data);
			}
		}
	}

	public void Mod_OnMeshBuild()
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnMeshBuild();
		}
		if (reqCustomNormals)
		{
			BuildMeshCustomNormals();
		}
		else
		{
			BuildMeshSphereNormals();
		}
		if (reqSphereUV || reqUVQuad)
		{
			BuildMeshUVChannel();
		}
		if (reqUV2)
		{
			BuildMeshUV2Channel();
		}
		if (reqUV3)
		{
			BuildMeshUV3Channel();
		}
		if (reqUV4)
		{
			BuildMeshUV4Channel();
		}
		if (reqColorChannel)
		{
			BuildMeshColorChannel();
		}
		if (reqBuildTangents)
		{
			BuildMeshTangents();
		}
		if (reqAssignTangents)
		{
			AssignMeshTangents();
		}
	}

	public void Mod_OnQuadCreate(GClass3 quad)
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnQuadCreate(quad);
		}
	}

	public void Mod_OnQuadDestroy(GClass3 quad)
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnQuadDestroy(quad);
		}
	}

	public void Mod_OnQuadPreBuild(GClass3 quad)
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnQuadPreBuild(quad);
		}
	}

	public void Mod_OnQuadBuilt(GClass3 quad)
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnQuadBuilt(quad);
		}
	}

	public void Mod_OnQuadUpdate(GClass3 quad)
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnQuadUpdate(quad);
		}
	}

	public void Mod_OnQuadUpdateNormals(GClass3 quad)
	{
		for (int i = 0; i < modCount; i++)
		{
			mods[i].OnQuadUpdateNormals(quad);
		}
	}

	public void SetupMods()
	{
		List<PQSMod> list = new List<PQSMod>();
		GetChildMods(base.gameObject, list);
		list.Sort((PQSMod a, PQSMod b) => a.order.CompareTo(b.order));
		if (list.Count > 0)
		{
			int count = list.Count;
			while (count-- > 0)
			{
				list[count].sphere = this;
				PQSLandControl pQSLandControl = list[count] as PQSLandControl;
				if (pQSLandControl != null)
				{
					PQSLandControl = pQSLandControl;
				}
				PQSMod_AerialPerspectiveMaterial pQSMod_AerialPerspectiveMaterial = list[count] as PQSMod_AerialPerspectiveMaterial;
				if (pQSMod_AerialPerspectiveMaterial != null)
				{
					PQSAerialPerspectiveMaterial = pQSMod_AerialPerspectiveMaterial;
				}
				PQSMod_CelestialBodyTransform pQSMod_CelestialBodyTransform = list[count] as PQSMod_CelestialBodyTransform;
				if (pQSMod_CelestialBodyTransform != null)
				{
					PQSModCBTransform = pQSMod_CelestialBodyTransform;
				}
				if (!list[count].modEnabled || !list[count].gameObject.activeSelf || list[count].modExpansionDisabled)
				{
					list.RemoveAt(count);
				}
			}
			mods = list.ToArray();
		}
		else
		{
			mods = new PQSMod[0];
		}
		modCount = mods.Length;
		Mod_OnSetup();
		modRequirements = ModiferRequirements.Default;
		PQSMod[] array = mods;
		foreach (PQSMod pQSMod in array)
		{
			modRequirements |= pQSMod.requirements;
		}
	}

	public void ResetModList()
	{
		SetupMods();
	}

	public void GetChildMods(GameObject obj, List<PQSMod> mods)
	{
		foreach (Transform item in obj.transform)
		{
			if (!(item == base.transform) && !(item.GetComponent<GClass4>() != null))
			{
				PQSMod[] components = item.GetComponents<PQSMod>();
				if (components != null)
				{
					mods.AddRange(components);
					GetChildMods(item.gameObject, mods);
				}
			}
		}
	}

	public void SetupBuildDelegates(bool fakeBuild)
	{
		isFakeBuild = fakeBuild;
		reqVertexMapCoods = (modRequirements & ModiferRequirements.VertexMapCoords) != 0;
		reqGnomonicCoords = (modRequirements & ModiferRequirements.VertexGnomonicMapCoords) != 0;
		reqCustomNormals = (modRequirements & ModiferRequirements.MeshCustomNormals) != 0;
		reqColorChannel = (modRequirements & ModiferRequirements.MeshColorChannel) != 0;
		reqBuildTangents = (modRequirements & ModiferRequirements.MeshBuildTangents) != 0 || buildTangents;
		reqAssignTangents = (modRequirements & ModiferRequirements.MeshAssignTangents) != 0 || reqBuildTangents;
		reqUV2 = (modRequirements & ModiferRequirements.MeshUV2) != 0;
		reqUV3 = (modRequirements & ModiferRequirements.MeshUV3) != 0;
		reqUV4 = (modRequirements & ModiferRequirements.MeshUV4) != 0;
		reqSphereUV = (modRequirements & ModiferRequirements.UVSphereCoords) != 0;
		reqUVQuad = (modRequirements & ModiferRequirements.UVQuadCoords) != 0;
		if (reqSphereUV)
		{
			reqVertexMapCoods = true;
			if (reqUVQuad)
			{
				Debug.LogWarning("Cannot have both Sphere UV and Quad UV mods enabled. Defaulting to UV");
				reqUVQuad = false;
			}
		}
		useSharedMaterial = (modRequirements & ModiferRequirements.UniqueMaterialInstances) == 0;
		meshVertMin = double.MaxValue;
		meshVertMax = double.MinValue;
		radiusMax = radius + Mod_GetVertexMaxHeight();
		radiusMin = radius + Mod_GetVertexMinHeight();
		radiusDelta = radiusMax - radiusMin;
		halfChord = Math.Sqrt(radiusMax * radiusMax - radiusMin * radiusMin);
	}

	public void OnSphereStartDefault()
	{
		isAlive = true;
		if (target == null && parentSphere != null && parentSphere.target != null)
		{
			SetTarget(parentSphere.target);
			SetSecondaryTarget(parentSphere.secondaryTarget);
		}
		if (target == null && Camera.main != null)
		{
			SetTarget(Camera.main.transform);
		}
	}

	public void OnSphereTransformUpdateDefault()
	{
		transformRotation = base.transform.rotation;
		transformPosition = base.transform.position;
	}

	public void OnPreUpdateDefault()
	{
		if (parentSphere == null)
		{
			ActivateSphere();
		}
	}

	public bool BuildQuad(GClass3 quad)
	{
		if (quad.isBuilt)
		{
			return false;
		}
		if (quad.isSubdivided)
		{
			return false;
		}
		if (!(quad == null) && !(quad.gameObject == null))
		{
			buildQuad = quad;
			Mod_OnQuadPreBuild(quad);
			vbData.buildQuad = buildQuad;
			vbData.allowScatter = true;
			vbData.gnomonicPlane = buildQuad.plane;
			buildQuad.meshVertMin = double.MaxValue;
			buildQuad.meshVertMax = double.MinValue;
			for (vertexIndex = 0; vertexIndex < cacheVertCount; vertexIndex++)
			{
				vbData.globalV = buildQuad.quadMatrix.MultiplyPoint3x4(cacheVerts[vertexIndex]);
				vbData.directionFromCenter = vbData.globalV.normalized;
				vbData.vertHeight = radius;
				vbData.vertIndex = vertexIndex;
				Mod_OnVertexBuildHeight(vbData);
				Mod_OnVertexBuild(vbData);
				vbData.vertHeight -= radius;
				if (vbData.vertHeight > buildQuad.meshVertMax)
				{
					buildQuad.meshVertMax = vbData.vertHeight;
				}
				if (vbData.vertHeight < buildQuad.meshVertMin)
				{
					buildQuad.meshVertMin = vbData.vertHeight;
				}
				if (vbData.vertHeight > meshVertMax)
				{
					meshVertMax = vbData.vertHeight;
				}
				if (vbData.vertHeight < meshVertMin)
				{
					meshVertMin = vbData.vertHeight;
				}
			}
			buildQuad.mesh.vertices = buildQuad.verts;
			buildQuad.mesh.triangles = cacheIndices[0];
			buildQuad.mesh.RecalculateBounds();
			buildQuad.edgeState = EdgeState.Reset;
			Mod_OnMeshBuild();
			Mod_OnQuadBuilt(quad);
			buildQuad = null;
			return true;
		}
		return false;
	}

	public void BuildVertexMapCoords(VertexBuildData data)
	{
		latitude = Math.Asin(data.directionFromCenter.y);
		if (double.IsNaN(latitude))
		{
			latitude = Math.PI / 2.0;
		}
		directionXZ = new Vector3d(data.directionFromCenter.x, 0.0, data.directionFromCenter.z).normalized;
		if (directionXZ.magnitude > 0.0)
		{
			longitude = ((directionXZ.z < 0.0) ? (Math.PI - Math.Asin(directionXZ.x / directionXZ.magnitude)) : Math.Asin(directionXZ.x / directionXZ.magnitude));
		}
		else
		{
			longitude = 0.0;
		}
		data.latitude = latitude;
		data.longitude = longitude;
		sy = latitude / Math.PI + 0.5;
		sx = longitude / Math.PI * 0.5;
		data.u = sx;
		data.v = sy;
	}

	public void BuildVertexHeight(VertexBuildData data)
	{
		verts[vertexIndex] = vbData.directionFromCenter * vbData.vertHeight;
		buildQuad.verts[vertexIndex] = verts[vertexIndex];
	}

	public void BuildVertexSurfaceRelative(VertexBuildData data)
	{
		vertRel = vbData.directionFromCenter * vbData.vertHeight;
		planetRel = base.transform.TransformPoint(vertRel);
		verts[vertexIndex] = vertRel;
		buildQuad.verts[vertexIndex] = buildQuad.transform.InverseTransformPoint(planetRel);
	}

	public void BuildVertexColor(VertexBuildData data)
	{
		cacheColors[vertexIndex] = vbData.vertColor;
	}

	public void BuildVertexUV2(VertexBuildData data)
	{
		cacheUV2s[vertexIndex].x = (float)vbData.u2;
		cacheUV2s[vertexIndex].y = (float)vbData.v2;
	}

	public void BuildVertexUV3(VertexBuildData data)
	{
		cacheUV3s[vertexIndex].x = (float)vbData.u3;
		cacheUV3s[vertexIndex].y = (float)vbData.v3;
	}

	public void BuildVertexUV4(VertexBuildData data)
	{
		cacheUV4s[vertexIndex].x = (float)vbData.u4;
		cacheUV4s[vertexIndex].y = (float)vbData.v4;
	}

	public void BuildVertexSphereNormal(VertexBuildData data)
	{
		normals[vertexIndex] = vbData.directionFromCenter;
	}

	public void BuildVertexQuadUV(VertexBuildData data)
	{
		uvs[vertexIndex].x = data.buildQuad.uvSW.x + cacheUVs[vertexIndex].x * data.buildQuad.uvDelta.x;
		uvs[vertexIndex].y = data.buildQuad.uvSW.y + cacheUVs[vertexIndex].y * data.buildQuad.uvDelta.y;
	}

	public void BuildVertexSphereUV(VertexBuildData data)
	{
		uvs[vertexIndex].x = (float)sx;
		uvs[vertexIndex].y = (float)sy;
	}

	public void BuildMeshSphereNormals()
	{
		buildQuad.mesh.normals = normals;
	}

	public void BuildMeshCustomNormals()
	{
		BuildNormals(buildQuad);
		BackupEdgeNormals();
		buildQuad.mesh.normals = buildQuad.vertNormals;
	}

	public void BuildMeshTangents()
	{
		BuildTangents(buildQuad);
	}

	public void AssignMeshTangents()
	{
		buildQuad.mesh.tangents = cacheTangents;
	}

	public void BuildMeshColorChannel()
	{
		buildQuad.mesh.colors = cacheColors;
	}

	public void BuildMeshUVChannel()
	{
		buildQuad.mesh.uv = cacheUVs;
	}

	public void BuildMeshUV2Channel()
	{
		buildQuad.mesh.uv2 = cacheUV2s;
	}

	public void BuildMeshUV3Channel()
	{
		buildQuad.mesh.uv3 = cacheUV3s;
	}

	public void BuildMeshUV4Channel()
	{
		buildQuad.mesh.uv4 = cacheUV4s;
	}

	public Color BilinearInterpColorMap(Texture2D texture)
	{
		return texture.GetPixelBilinear((float)sx, (float)sy);
	}

	public Color BilinearInterpColorMap(Vector3 planetPos, Texture2D texture)
	{
		latitude = Math.Asin(planetPos.y);
		if (double.IsNaN(latitude))
		{
			latitude = Math.PI / 2.0;
		}
		directionXZ = new Vector3d(planetPos.x, 0.0, planetPos.z).normalized;
		if (directionXZ.magnitude > 0.0)
		{
			longitude = ((directionXZ.z < 0.0) ? (Math.PI - Math.Asin(directionXZ.x / directionXZ.magnitude)) : Math.Asin(directionXZ.x / directionXZ.magnitude));
		}
		else
		{
			longitude = 0.0;
		}
		sy = latitude / Math.PI + 0.5;
		sx = longitude / Math.PI * 0.5;
		return texture.GetPixelBilinear((float)sx, (float)sy);
	}

	public double BilinearInterpFloatMap(Texture2D texture)
	{
		return texture.GetPixelBilinear((float)sx, (float)sy).grayscale;
	}

	public double BilinearInterpFloatMap(Vector3 planetPos, Texture2D texture)
	{
		latitude = Math.Asin(planetPos.y);
		if (double.IsNaN(latitude))
		{
			latitude = Math.PI / 2.0;
		}
		directionXZ = new Vector3d(planetPos.x, 0.0, planetPos.z).normalized;
		if (directionXZ.magnitude > 0.0)
		{
			longitude = ((directionXZ.z < 0.0) ? (Math.PI - Math.Asin(directionXZ.x / directionXZ.magnitude)) : Math.Asin(directionXZ.x / directionXZ.magnitude));
		}
		else
		{
			longitude = 0.0;
		}
		sy = latitude / Math.PI + 0.5;
		sx = longitude / Math.PI * 0.5;
		return texture.GetPixelBilinear((float)sx, (float)sy).grayscale;
	}

	public void BackupEdgeNormals()
	{
		for (int i = 0; i < 4; i++)
		{
			switch ((QuadEdge)i)
			{
			case QuadEdge.North:
				BackupEdgeNormals(buildQuad.edgeNormals[i], cacheSideVertCount, vi(0, 0), 1);
				break;
			case QuadEdge.South:
				BackupEdgeNormals(buildQuad.edgeNormals[i], cacheSideVertCount, vi(cacheRes, cacheRes), -1);
				break;
			case QuadEdge.East:
				BackupEdgeNormals(buildQuad.edgeNormals[i], cacheSideVertCount, vi(0, cacheRes), -cacheSideVertCount);
				break;
			case QuadEdge.West:
				BackupEdgeNormals(buildQuad.edgeNormals[i], cacheSideVertCount, vi(cacheRes, 0), cacheSideVertCount);
				break;
			}
		}
	}

	public void BackupEdgeNormals(Vector3[] edge, int vCount, int localStart, int localDelta)
	{
		int num = localStart;
		for (int i = 0; i < vCount; i++)
		{
			edge[i] = buildQuad.vertNormals[num];
			num += localDelta;
		}
	}

	public void UpdateEdgeNormals(GClass3 q)
	{
		if (q == null || !q.isActive || (q.parent != null && !q.parent.isSubdivided) || q.parent == null || !q.isBuilt)
		{
			return;
		}
		bool flag = !q.isQueuedOnlyForCornerNormalUpdate;
		Vector3 vector = q.edgeNormals[0][0];
		Vector3 vector2 = q.edgeNormals[0][cacheRes];
		Vector3 vector3 = q.edgeNormals[1][0];
		Vector3 vector4 = q.edgeNormals[1][cacheRes];
		Vector3 zero = Vector3.zero;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		GClass3 left = null;
		GClass3 right = null;
		QuadEdge quadEdge = QuadEdge.Null;
		QuadEdge quadEdge2 = QuadEdge.Null;
		if (q.north.subdivision == q.subdivision)
		{
			if (q.north.isSubdivided)
			{
				q.north.GetEdgeQuads(q, out left, out right);
				if (left != null && right != null)
				{
					quadEdge = left.GetEdge(q);
					quadEdge2 = right.GetEdge(q);
					if (quadEdge == QuadEdge.Null || quadEdge2 == QuadEdge.Null)
					{
						return;
					}
					if (!left.isBuilt)
					{
						left.Build();
					}
					if (!right.isBuilt)
					{
						right.Build();
					}
					zero = q.edgeNormals[0][cacheResDiv2];
					if (right.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(0, 0), 1, q.edgeNormals[0], 0, 1, right.edgeNormals[(int)quadEdge2], cacheRes, -2);
						}
						vector += right.edgeNormals[(int)quadEdge2][cacheRes];
						zero += right.edgeNormals[(int)quadEdge2][0];
					}
					if (left.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheResDiv2, 0), 1, q.edgeNormals[0], cacheResDiv2, 1, left.edgeNormals[(int)quadEdge], cacheRes, -2);
						}
						vector2 += left.edgeNormals[(int)quadEdge][0];
						zero += left.edgeNormals[(int)quadEdge][cacheRes];
					}
					q.vertNormals[vi(cacheResDiv2, 0)] = zero.normalized;
				}
			}
			else
			{
				quadEdge = q.north.GetEdge(q);
				if (quadEdge == QuadEdge.Null)
				{
					return;
				}
				if (!q.north.isBuilt)
				{
					q.north.Build();
				}
				if (q.north.isBuilt)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheSideVertCount, vi(0, 0), 1, q.edgeNormals[0], 0, 1, q.north.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector += q.north.edgeNormals[(int)quadEdge][cacheRes];
					vector2 += q.north.edgeNormals[(int)quadEdge][0];
				}
			}
		}
		else if (q.north.subdivision < q.subdivision)
		{
			q.parent.GetEdgeQuads(q.north, out left, out right);
			quadEdge = q.north.GetEdge(q.parent);
			if (quadEdge == QuadEdge.Null)
			{
				return;
			}
			if (!q.north.isBuilt)
			{
				q.north.Build();
			}
			if (q.north.isBuilt)
			{
				if (left == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(0, 0), 2, q.edgeNormals[0], 0, 2, q.north.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector += q.north.edgeNormals[(int)quadEdge][cacheRes];
					vector2 += q.north.edgeNormals[(int)quadEdge][cacheResDiv2];
					flag3 = true;
				}
				else if (right == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(0, 0), 2, q.edgeNormals[0], 0, 2, q.north.edgeNormals[(int)quadEdge], cacheResDiv2, -1);
					}
					vector += q.north.edgeNormals[(int)quadEdge][cacheResDiv2];
					vector2 += q.north.edgeNormals[(int)quadEdge][0];
					flag2 = true;
				}
			}
		}
		if (q.south.subdivision == q.subdivision)
		{
			if (q.south.isSubdivided)
			{
				q.south.GetEdgeQuads(q, out left, out right);
				if (left != null && right != null)
				{
					quadEdge = left.GetEdge(q);
					quadEdge2 = right.GetEdge(q);
					if (quadEdge == QuadEdge.Null || quadEdge2 == QuadEdge.Null)
					{
						return;
					}
					if (!left.isBuilt)
					{
						left.Build();
					}
					if (!right.isBuilt)
					{
						right.Build();
					}
					zero = q.edgeNormals[1][cacheResDiv2];
					if (right.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheRes, cacheRes), -1, q.edgeNormals[1], 0, 1, right.edgeNormals[(int)quadEdge2], cacheRes, -2);
						}
						vector3 += right.edgeNormals[(int)quadEdge2][cacheRes];
						zero += right.edgeNormals[(int)quadEdge2][0];
					}
					if (left.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheResDiv2, cacheRes), -1, q.edgeNormals[1], cacheResDiv2, 1, left.edgeNormals[(int)quadEdge], cacheRes, -2);
						}
						vector4 += left.edgeNormals[(int)quadEdge][0];
						zero += left.edgeNormals[(int)quadEdge][cacheRes];
					}
					q.vertNormals[vi(cacheResDiv2, cacheRes)] = zero.normalized;
				}
			}
			else
			{
				quadEdge = q.south.GetEdge(q);
				if (quadEdge == QuadEdge.Null)
				{
					return;
				}
				if (!q.south.isBuilt)
				{
					q.south.Build();
				}
				if (q.south.isBuilt)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheSideVertCount, vi(cacheRes, cacheRes), -1, q.edgeNormals[1], 0, 1, q.south.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector3 += q.south.edgeNormals[(int)quadEdge][cacheRes];
					vector4 += q.south.edgeNormals[(int)quadEdge][0];
				}
			}
		}
		else if (q.south.subdivision < q.subdivision)
		{
			q.parent.GetEdgeQuads(q.south, out left, out right);
			quadEdge = q.south.GetEdge(q.parent);
			if (quadEdge == QuadEdge.Null)
			{
				return;
			}
			if (!q.south.isBuilt)
			{
				q.south.Build();
			}
			if (q.south.isBuilt)
			{
				if (left == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheRes, cacheRes), -2, q.edgeNormals[1], 0, 2, q.south.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector3 += q.south.edgeNormals[(int)quadEdge][cacheRes];
					vector4 += q.south.edgeNormals[(int)quadEdge][cacheResDiv2];
					flag5 = true;
				}
				else if (right == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheRes, cacheRes), -2, q.edgeNormals[1], 0, 2, q.south.edgeNormals[(int)quadEdge], cacheResDiv2, -1);
					}
					vector3 += q.south.edgeNormals[(int)quadEdge][cacheResDiv2];
					vector4 += q.south.edgeNormals[(int)quadEdge][0];
					flag4 = true;
				}
			}
		}
		if (q.east.subdivision == q.subdivision)
		{
			if (q.east.isSubdivided)
			{
				q.east.GetEdgeQuads(q, out left, out right);
				if (left != null && right != null)
				{
					quadEdge = left.GetEdge(q);
					quadEdge2 = right.GetEdge(q);
					if (quadEdge == QuadEdge.Null || quadEdge2 == QuadEdge.Null)
					{
						return;
					}
					if (!left.isBuilt)
					{
						left.Build();
					}
					if (!right.isBuilt)
					{
						right.Build();
					}
					zero = q.edgeNormals[2][cacheResDiv2];
					if (right.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(0, cacheRes), -cacheSideVertCount, q.edgeNormals[2], 0, 1, right.edgeNormals[(int)quadEdge2], cacheRes, -2);
						}
						vector4 += right.edgeNormals[(int)quadEdge2][cacheRes];
						zero += right.edgeNormals[(int)quadEdge2][0];
					}
					if (left.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(0, cacheResDiv2), -cacheSideVertCount, q.edgeNormals[2], cacheResDiv2, 1, left.edgeNormals[(int)quadEdge], cacheRes, -2);
						}
						vector += left.edgeNormals[(int)quadEdge][0];
						zero += left.edgeNormals[(int)quadEdge][cacheRes];
					}
					q.vertNormals[vi(0, cacheResDiv2)] = zero.normalized;
				}
			}
			else
			{
				quadEdge = q.east.GetEdge(q);
				if (quadEdge == QuadEdge.Null)
				{
					return;
				}
				if (!q.east.isBuilt)
				{
					q.east.Build();
				}
				if (q.east.isBuilt)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheSideVertCount, vi(0, cacheRes), -cacheSideVertCount, q.edgeNormals[2], 0, 1, q.east.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector += q.east.edgeNormals[(int)quadEdge][0];
					vector4 += q.east.edgeNormals[(int)quadEdge][cacheRes];
				}
			}
		}
		else if (q.east.subdivision < q.subdivision)
		{
			q.parent.GetEdgeQuads(q.east, out left, out right);
			quadEdge = q.east.GetEdge(q.parent);
			if (quadEdge == QuadEdge.Null)
			{
				return;
			}
			if (!q.east.isBuilt)
			{
				q.east.Build();
			}
			if (q.east.isBuilt)
			{
				if (right == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(0, cacheRes), -(cacheSideVertCount * 2), q.edgeNormals[2], 0, 2, q.east.edgeNormals[(int)quadEdge], cacheResDiv2, -1);
					}
					vector += q.east.edgeNormals[(int)quadEdge][0];
					vector4 += q.east.edgeNormals[(int)quadEdge][cacheResDiv2];
					flag5 = true;
				}
				else if (left == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(0, cacheRes), -(cacheSideVertCount * 2), q.edgeNormals[2], 0, 2, q.east.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector += q.east.edgeNormals[(int)quadEdge][cacheResDiv2];
					vector4 += q.east.edgeNormals[(int)quadEdge][cacheRes];
					flag2 = true;
				}
			}
		}
		if (q.west.subdivision == q.subdivision)
		{
			if (q.west.isSubdivided)
			{
				q.west.GetEdgeQuads(q, out left, out right);
				if (left != null && right != null)
				{
					quadEdge = left.GetEdge(q);
					quadEdge2 = right.GetEdge(q);
					if (quadEdge == QuadEdge.Null || quadEdge2 == QuadEdge.Null)
					{
						return;
					}
					if (!left.isBuilt)
					{
						left.Build();
					}
					if (!right.isBuilt)
					{
						right.Build();
					}
					zero = q.edgeNormals[3][cacheResDiv2];
					if (right.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheRes, 0), cacheSideVertCount, q.edgeNormals[3], 0, 1, right.edgeNormals[(int)quadEdge2], cacheRes, -2);
						}
						vector2 += right.edgeNormals[(int)quadEdge2][cacheRes];
						zero += right.edgeNormals[(int)quadEdge2][0];
					}
					if (left.isBuilt)
					{
						if (flag)
						{
							CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheRes, cacheResDiv2), cacheSideVertCount, q.edgeNormals[3], cacheResDiv2, 1, left.edgeNormals[(int)quadEdge], cacheRes, -2);
						}
						vector3 += left.edgeNormals[(int)quadEdge][0];
						zero += left.edgeNormals[(int)quadEdge][cacheRes];
					}
					q.vertNormals[vi(cacheRes, cacheResDiv2)] = zero.normalized;
				}
			}
			else
			{
				quadEdge = q.west.GetEdge(q);
				if (quadEdge == QuadEdge.Null)
				{
					return;
				}
				if (!q.west.isBuilt)
				{
					q.west.Build();
				}
				if (q.west.isBuilt)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheSideVertCount, vi(cacheRes, 0), cacheSideVertCount, q.edgeNormals[3], 0, 1, q.west.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector2 += q.west.edgeNormals[(int)quadEdge][cacheRes];
					vector3 += q.west.edgeNormals[(int)quadEdge][0];
				}
			}
		}
		else if (q.west.subdivision < q.subdivision)
		{
			q.parent.GetEdgeQuads(q.west, out left, out right);
			quadEdge = q.west.GetEdge(q.parent);
			if (!q.west.isBuilt)
			{
				q.west.Build();
			}
			if (q.west.isBuilt)
			{
				if (left == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheRes, 0), cacheSideVertCount * 2, q.edgeNormals[3], 0, 2, q.west.edgeNormals[(int)quadEdge], cacheRes, -1);
					}
					vector2 += q.west.edgeNormals[(int)quadEdge][cacheRes];
					vector3 += q.west.edgeNormals[(int)quadEdge][cacheResDiv2];
					flag4 = true;
				}
				else if (right == q)
				{
					if (flag)
					{
						CombineEdgeNormals(q, cacheResDiv2Plus1, vi(cacheRes, 0), cacheSideVertCount * 2, q.edgeNormals[3], 0, 2, q.west.edgeNormals[(int)quadEdge], cacheResDiv2, -1);
					}
					vector2 += q.west.edgeNormals[(int)quadEdge][cacheResDiv2];
					vector3 += q.west.edgeNormals[(int)quadEdge][0];
					flag3 = true;
				}
			}
		}
		if (!flag2)
		{
			vector += GetRightmostCornerNormal(q, q.north);
		}
		if (!flag3)
		{
			vector2 += GetRightmostCornerNormal(q, q.west);
		}
		if (!flag4)
		{
			vector3 += GetRightmostCornerNormal(q, q.south);
		}
		if (!flag5)
		{
			vector4 += GetRightmostCornerNormal(q, q.east);
		}
		q.vertNormals[vi(0, 0)] = vector.normalized;
		q.vertNormals[vi(cacheRes, 0)] = vector2.normalized;
		q.vertNormals[vi(cacheRes, cacheRes)] = vector3.normalized;
		q.vertNormals[vi(0, cacheRes)] = vector4.normalized;
		q.mesh.normals = q.vertNormals;
		Mod_OnQuadUpdateNormals(q);
		if (reqBuildTangents)
		{
			BuildTangents(q);
		}
	}

	public void CombineEdgeNormals(GClass3 q, int vCount, int vertStart, int vertStep, Vector3[] bkpLocal, int localBackupStart, int localBackupStep, Vector3[] bkpRemote, int remoteBackupStart, int remoteBackupStep)
	{
		int num = 0;
		int num2 = vertStart;
		int num3 = localBackupStart;
		int num4 = remoteBackupStart;
		while (num < vCount)
		{
			q.vertNormals[num2] = (bkpLocal[num3] + bkpRemote[num4]) * 0.5f;
			num++;
			num2 += vertStep;
			num3 += localBackupStep;
			num4 += remoteBackupStep;
		}
	}

	public Vector3 GetRightmostCornerNormal(GClass3 caller, GClass3 nextQuad)
	{
		if (!(caller == null) && !(nextQuad == null))
		{
			GClass3 rightmostCornerPQ = caller.GetRightmostCornerPQ(nextQuad);
			if (rightmostCornerPQ == null)
			{
				if (!caller.isPendingCollapse && !caller.parent.isPendingCollapse && !nextQuad.isPendingCollapse && !nextQuad.parent.isPendingCollapse)
				{
					Debug.LogFormat(string.Concat("[PQS] cornerQuad in GetRightmostCornerNormal is null! caller: ", caller, " nextQuad: ", nextQuad));
				}
				return Vector3.zero;
			}
			QuadEdge quadEdge = QuadEdge.Null;
			quadEdge = ((nextQuad.subdivision < rightmostCornerPQ.subdivision) ? rightmostCornerPQ.parent.GetEdge(nextQuad) : ((nextQuad.subdivision <= rightmostCornerPQ.subdivision) ? rightmostCornerPQ.GetEdge(nextQuad) : rightmostCornerPQ.GetEdge(nextQuad.parent)));
			if (!rightmostCornerPQ.isBuilt)
			{
				rightmostCornerPQ.Build();
			}
			if (quadEdge == QuadEdge.Null)
			{
				if (!caller.isPendingCollapse && !caller.parent.isPendingCollapse && !nextQuad.isPendingCollapse && !nextQuad.parent.isPendingCollapse)
				{
					Debug.LogFormat(string.Concat("[PQS] Last edge accessor in GetRightmostCornerNormal is null! Caller: ", caller, " nextQuad: ", nextQuad, " cornerQuad ", rightmostCornerPQ));
				}
				return Vector3.zero;
			}
			if (rightmostCornerPQ.edgeNormals.Length >= (int)quadEdge && rightmostCornerPQ.edgeNormals[(int)quadEdge].Length >= cacheRes && cacheRes >= 0)
			{
				return rightmostCornerPQ.edgeNormals[(int)quadEdge][cacheRes];
			}
			if (!caller.isPendingCollapse && !caller.parent.isPendingCollapse && !nextQuad.isPendingCollapse && !nextQuad.parent.isPendingCollapse)
			{
				Debug.LogFormat(string.Concat("[PQS] Unable to Calculate CornerQuad Normal. Caller: ", caller, " nextQuad: ", nextQuad, " cornerQuad ", rightmostCornerPQ));
			}
			return Vector3.zero;
		}
		Debug.LogFormat(string.Concat("[PQS] Null parameter in GetRightmostCornerNormal! caller: ", caller, " nextQuad: ", nextQuad));
		return Vector3.zero;
	}

	public static QuadEdge GetEdgeRotatedClockwise(QuadEdge edge)
	{
		return edge switch
		{
			QuadEdge.North => QuadEdge.East, 
			QuadEdge.East => QuadEdge.South, 
			QuadEdge.South => QuadEdge.West, 
			QuadEdge.West => QuadEdge.North, 
			_ => QuadEdge.Null, 
		};
	}

	public static QuadEdge GetEdgeRotatedCounterclockwise(QuadEdge edge)
	{
		return edge switch
		{
			QuadEdge.North => QuadEdge.West, 
			QuadEdge.West => QuadEdge.South, 
			QuadEdge.South => QuadEdge.East, 
			QuadEdge.East => QuadEdge.North, 
			_ => QuadEdge.Null, 
		};
	}

	public Vector3d GetRelativePosition(Vector3d worldPosition)
	{
		return transformRotation * (worldPosition - transformPosition);
	}

	public Vector3d GetRelativeDirection(Vector3d worldDirection)
	{
		return transformRotationInverse * worldDirection;
	}

	public Vector3d GetWorldPosition(Vector3d localPosition)
	{
		return transformRotationInverse * localPosition + transformPosition;
	}

	public double GetRelativeDistance(Vector3 worldPosition)
	{
		return GetRelativePosition(worldPosition).magnitude;
	}

	public double GetRelativeAltitude(Vector3 worldPosition)
	{
		return GetRelativePosition(worldPosition).magnitude - radius;
	}

	public double GetRelativeDistanceSqr(Vector3 worldDirection)
	{
		return GetRelativePosition(worldDirection).sqrMagnitude;
	}

	public double GetClampedWorldSurfaceHeight(Vector3 worldPosition)
	{
		double surfaceHeight = GetSurfaceHeight(GetRelativePosition(worldPosition));
		if (surfaceHeight < radius)
		{
			surfaceHeight = radius;
		}
		return surfaceHeight;
	}

	public double GetClampedWorldAltitude(Vector3 worldPosition)
	{
		Vector3d relativePosition = GetRelativePosition(worldPosition);
		return relativePosition.magnitude - GetClampedWorldSurfaceHeight(relativePosition);
	}

	public double GetSurfaceHeight(Vector3d radialVector)
	{
		return GetSurfaceHeight(radialVector, overrideQuadBuildCheck: false);
	}

	public double GetSurfaceHeight(Vector3d radialVector, bool overrideQuadBuildCheck)
	{
		vbData.Reset();
		vbData.directionFromCenter = radialVector.normalized;
		vbData.vertHeight = radius;
		vbData.vertIndex = vertexIndex;
		Mod_OnVertexBuildHeight(vbData, overrideQuadBuildCheck);
		return vbData.vertHeight;
	}

	public double GetAltitude(Vector3d worldPosition)
	{
		return GetRelativeDistance(worldPosition) - radius;
	}

	public bool RayIntersection(Vector3 worldStart, Vector3 worldDirection, out double intersectionDistance)
	{
		Vector3d relIntersection = Vector3d.zero;
		Vector3d relativePosition = GetRelativePosition(worldStart);
		if (LineSphereIntersection(relativePosition, GetRelativeDirection(worldDirection), radius, out relIntersection))
		{
			intersectionDistance = Vector3d.Distance(relativePosition, relIntersection);
			return true;
		}
		intersectionDistance = double.PositiveInfinity;
		return false;
	}

	public bool RayIntersection(Vector3 worldStart, Vector3 worldDirection, out Vector3d intersection)
	{
		Vector3d relIntersection = Vector3d.zero;
		if (LineSphereIntersection(GetRelativePosition(worldStart), GetRelativeDirection(worldDirection), radius, out relIntersection))
		{
			intersection = GetWorldPosition(relIntersection);
			return true;
		}
		intersection = Vector3d.zero;
		return false;
	}

	public static bool LineSphereIntersection(Vector3d relPos, Vector3d relVel, double radius, out Vector3d relIntersection)
	{
		double num = radius * radius;
		if (relPos.sqrMagnitude <= num)
		{
			relIntersection = relPos;
			return true;
		}
		Vector3d normalized = relVel.normalized;
		double sqrMagnitude = normalized.sqrMagnitude;
		double num2 = 2.0 * Vector3d.Dot(normalized, relPos);
		double num3 = relPos.sqrMagnitude - radius * radius;
		double num4 = num2 * num2 - 4.0 * sqrMagnitude * num3;
		if (num4 < 0.0)
		{
			relIntersection = Vector3d.zero;
			return false;
		}
		if (num4 < 1.401298464324817E-45)
		{
			double num5 = (0.0 - num2) / (2.0 * sqrMagnitude);
			relIntersection = relPos + num5 * normalized;
			return true;
		}
		double num6 = Math.Sqrt(num4);
		double num7 = (0.0 - num2 + num6) / (2.0 * sqrMagnitude);
		double num8 = (0.0 - num2 - num6) / (2.0 * sqrMagnitude);
		if (num7 < num8)
		{
			relIntersection = relPos + num7 * normalized;
			return true;
		}
		relIntersection = relPos + num8 * normalized;
		return true;
	}
}
