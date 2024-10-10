using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

		public void Setup()
		{
			startDelta = 1.0 / (startEnd - startStart);
			endDelta = 1.0 / (endEnd - endStart);
		}

		public double Lerp(double point)
		{
			if (!(point <= startStart) && point < endEnd)
			{
				if (point < startEnd)
				{
					return (point - startStart) * startDelta;
				}
				if (point <= endStart)
				{
					return 1.0;
				}
				if (point < endEnd)
				{
					return 1.0 - (point - endStart) * endDelta;
				}
				return 0.0;
			}
			return 0.0;
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
	}

	[Serializable]
	public class LandClassScatterAmount
	{
		public string scatterName;

		public double density;

		public LandClassScatter scatter;

		public int scatterIndex;
	}

	[Serializable]
	public class LandClassScatterInstance
	{
		public double density;

		public LandClassScatter scatter;
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

		public int vertStride;

		public int triStride;

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
		public Vector3[] vertsEmpty;

		[HideInInspector]
		public Vector3[] normalsEmpty;

		[HideInInspector]
		public Vector3[] vertsTransformed;

		[HideInInspector]
		public Vector3[] normalsTransformed;

		[HideInInspector]
		public Vector2[] uvs;

		[HideInInspector]
		public int[] tris;

		public Stack<PQSMod_LandClassScatterQuad> cacheUnassigned;

		public List<PQSMod_LandClassScatterQuad> cacheAssigned;

		public int cacheUnassignedCount;

		public int cacheAssignedCount;

		public int cacheTotalCount;

		public GClass4 sphere;

		public Vector3 scatterPos;

		public int rndCount;

		public PQSMod_LandClassScatterQuad qc;

		public double countFactor;

		public Vector3 scatterUp;

		public Quaternion scatterRot;

		public float scatterAngle;

		public int scatterLoop;

		public int scatterN;

		public bool cacheCreated;

		public float scatterScale;

		public double maxSpeed = 200.0;

		public int minLevel { get; set; }

		public void Setup(GClass4 sphere)
		{
			this.sphere = sphere;
			minLevel = Mathf.Abs(maxLevelOffset) + sphere.maxLevel;
			if (!cacheCreated)
			{
				if (baseMesh == null)
				{
					BuildCacheQuad();
				}
				else
				{
					BuildCacheMesh(baseMesh.vertices, baseMesh.uv, baseMesh.normals, baseMesh.triangles);
				}
				cacheAssigned = new List<PQSMod_LandClassScatterQuad>(maxCache);
				cacheAssignedCount = 0;
				cacheUnassigned = new Stack<PQSMod_LandClassScatterQuad>(maxCache);
				cacheUnassignedCount = 0;
				cacheTotalCount = 0;
				cacheCreated = true;
			}
		}

		public void SphereActive()
		{
			if (GClass4.Global_AllowScatter && cacheUnassignedCount == 0)
			{
				BuildCache(maxCacheDelta);
			}
		}

		public void SphereInactive()
		{
			while (cacheUnassignedCount > 0)
			{
				UnityEngine.Object.Destroy(cacheUnassigned.Pop().gameObject);
				cacheUnassignedCount--;
			}
			int index = cacheAssignedCount;
			while (index-- > 0)
			{
				UnityEngine.Object.Destroy(cacheAssigned[index].gameObject);
			}
			cacheAssignedCount = 0;
		}

		public void AddScatterMeshController(GClass3 quad, double density)
		{
			if (cacheUnassignedCount == 0)
			{
				BuildCache(maxCacheDelta);
			}
			scatterN = (int)(density * densityFactor * (quad.quadArea / sphere.radius / 1000.0) * (double)maxScatter);
			if (scatterN >= 1)
			{
				if (scatterN > maxScatter)
				{
					scatterN = maxScatter;
				}
				qc = cacheUnassigned.Pop();
				cacheAssigned.Add(qc);
				cacheUnassignedCount--;
				cacheAssignedCount++;
				qc.Setup(quad, seed + quad.gameObject.name.GetHashCode_Net35(), this, scatterN);
			}
		}

		public void BuildCacheMesh(Vector3[] vertBase, Vector2[] uvBase, Vector3[] normBase, int[] triBase)
		{
			vertStride = vertBase.Length;
			triStride = triBase.Length;
			vertsUntrans = new Vector3[vertStride];
			vertBase.CopyTo(vertsUntrans, 0);
			normUntrans = new Vector3[vertStride];
			normBase.CopyTo(normUntrans, 0);
			uvUntrans = new Vector2[vertStride];
			uvBase.CopyTo(uvUntrans, 0);
			trisUntrans = new int[triStride];
			triBase.CopyTo(trisUntrans, 0);
			List<Vector2> list = new List<Vector2>();
			List<int> list2 = new List<int>();
			int num = 0;
			for (int i = 0; i < maxScatter; i++)
			{
				list.AddRange(uvUntrans);
				for (int j = 0; j < triStride; j++)
				{
					list2.Add(trisUntrans[j] + num);
				}
				num += vertStride;
			}
			uvs = list.ToArray();
			tris = list2.ToArray();
			vertsEmpty = new Vector3[vertStride * maxScatter];
			vertsTransformed = new Vector3[vertStride * maxScatter];
			normalsEmpty = new Vector3[vertStride * maxScatter];
			normalsTransformed = new Vector3[vertStride * maxScatter];
		}

		public void BuildCacheQuad()
		{
			Vector3[] vertBase = new Vector3[8]
			{
				Vector3.zero,
				Vector3.right,
				Vector3.up,
				new Vector3(1f, 1f, 0f),
				Vector3.zero,
				Vector3.right,
				Vector3.up,
				new Vector3(1f, 1f, 0f)
			};
			Vector2[] uvBase = new Vector2[8]
			{
				Vector2.zero,
				Vector2.right,
				Vector2.up,
				Vector2.one,
				Vector2.zero,
				Vector2.right,
				Vector2.up,
				Vector2.one
			};
			Vector3[] array = new Vector3[8];
			array[0] = Vector3.forward;
			array[1] = Vector3.forward;
			array[2] = Vector3.forward;
			array[3] = Vector3.forward;
			array[4] = Vector3.back;
			array[5] = Vector3.back;
			array[6] = Vector3.back;
			array[7] = Vector3.back;
			BuildCacheMesh(vertBase, uvBase, array, new int[12]
			{
				0, 1, 2, 2, 1, 3, 4, 6, 5, 6,
				7, 5
			});
		}

		public void BuildCache(int countToAdd)
		{
			if (scatterParent == null)
			{
				scatterParent = new GameObject();
				scatterParent.name = "Scatter " + scatterName;
				scatterParent.transform.parent = sphere.transform;
				scatterParent.transform.localPosition = Vector3.zero;
				scatterParent.transform.localRotation = Quaternion.identity;
				scatterParent.transform.localScale = Vector3.one;
			}
			for (int i = 0; i < countToAdd; i++)
			{
				GameObject gameObject = new GameObject();
				PQSMod_LandClassScatterQuad pQSMod_LandClassScatterQuad = gameObject.AddComponent<PQSMod_LandClassScatterQuad>();
				pQSMod_LandClassScatterQuad.obj = gameObject;
				gameObject.layer = 15;
				gameObject.name = "Unass";
				gameObject.transform.parent = scatterParent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				pQSMod_LandClassScatterQuad.mesh = new Mesh();
				pQSMod_LandClassScatterQuad.mesh.vertices = vertsEmpty;
				pQSMod_LandClassScatterQuad.mesh.normals = normalsEmpty;
				pQSMod_LandClassScatterQuad.mesh.uv = uvs;
				pQSMod_LandClassScatterQuad.mesh.triangles = tris;
				pQSMod_LandClassScatterQuad.mf = gameObject.AddComponent<MeshFilter>();
				pQSMod_LandClassScatterQuad.mf.sharedMesh = pQSMod_LandClassScatterQuad.mesh;
				pQSMod_LandClassScatterQuad.mr = gameObject.AddComponent<MeshRenderer>();
				pQSMod_LandClassScatterQuad.mr.sharedMaterial = material;
				if (castShadows)
				{
					pQSMod_LandClassScatterQuad.mr.shadowCastingMode = ShadowCastingMode.On;
				}
				else
				{
					pQSMod_LandClassScatterQuad.mr.shadowCastingMode = ShadowCastingMode.Off;
				}
				pQSMod_LandClassScatterQuad.mr.receiveShadows = recieveShadows;
				pQSMod_LandClassScatterQuad.obj.SetActive(value: false);
				cacheUnassigned.Push(pQSMod_LandClassScatterQuad);
			}
			cacheTotalCount += countToAdd;
			cacheUnassignedCount += countToAdd;
		}

		public void DestroyQuad(PQSMod_LandClassScatterQuad q)
		{
			q.Destroy();
			cacheAssigned.Remove(q);
			cacheUnassigned.Push(q);
			q.gameObject.name = "Unass";
			cacheAssignedCount--;
			cacheUnassignedCount++;
		}

		public void CreateScatterMesh(PQSMod_LandClassScatterQuad q)
		{
			int num = 0;
			UnityEngine.Random.InitState(q.seed);
			for (scatterLoop = 0; scatterLoop < q.count; scatterLoop++)
			{
				int num2 = -1;
				int num3 = -1;
				while (num3 == num2)
				{
					int num4 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
					int num5 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
					int x = num4 + UnityEngine.Random.Range(-1, 1);
					int z = num5 + UnityEngine.Random.Range(-1, 1);
					num3 = GClass4.vi(num4, num5);
					num2 = GClass4.vi(x, z);
				}
				scatterPos = Vector3.Lerp(q.quad.verts[num3], q.quad.verts[num2], UnityEngine.Random.value);
				if (sphere.surfaceRelativeQuads)
				{
					scatterUp = (scatterPos + q.quad.positionPlanet).normalized;
				}
				else
				{
					scatterUp = scatterPos.normalized;
				}
				scatterPos += scatterUp * verticalOffset;
				scatterAngle = UnityEngine.Random.value * 360f;
				scatterRot = Quaternion.AngleAxis(scatterAngle, scatterUp) * (Quaternion)q.quad.quadRotation;
				scatterScale = UnityEngine.Random.Range(minScale, maxScale);
				int num6 = 0;
				while (num6 < vertStride)
				{
					vertsTransformed[num] = scatterPos + scatterRot * (vertsUntrans[num6] * scatterScale);
					normalsTransformed[num] = scatterRot * normUntrans[num6];
					num6++;
					num++;
				}
			}
			while (scatterLoop < maxScatter)
			{
				int num6 = 0;
				while (num6 < vertStride)
				{
					vertsTransformed[num] = Vector3.zero;
					num6++;
					num++;
				}
				scatterLoop++;
			}
			q.mesh.vertices = vertsTransformed;
			q.mesh.normals = normalsTransformed;
			q.mesh.RecalculateBounds();
			q.obj.SetActive(value: true);
			q.isBuilt = true;
		}

		public static Vector3 RandomRange(Vector3 min, Vector3 max)
		{
			return new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
		}

		public void ClearAll()
		{
			vertsTransformed = null;
			vertsEmpty = null;
			normalsTransformed = null;
			normalsEmpty = null;
			vertsUntrans = null;
			uvUntrans = null;
			trisUntrans = null;
			normUntrans = null;
			if (cacheUnassigned != null)
			{
				cacheUnassigned.Clear();
			}
			if (scatterParent != null)
			{
				UnityEngine.Object.Destroy(scatterParent);
				scatterParent = null;
			}
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

	public int lcCount;

	public int itr;

	public LandClass lcSelected;

	public int lcSelectedIndex;

	public double vHeight;

	public double ct2;

	public double ct3;

	public List<LandClass> lcList;

	public double[] lcScatterList;

	public int lcListCount;

	public LandClass lc;

	public bool scatterActive;

	public int scatterMinSubdiv;

	public int scatterCount;

	public int scatterInstCount;

	public double totalDelta;

	public double vLat;

	public double vLon;

	public double vHeightAltered;

	public void Start()
	{
		GameEvents.OnGameSettingsApplied.Add(ApplyCastShadowsSetting);
	}

	public void OnDestroy()
	{
		GameEvents.OnGameSettingsApplied.Remove(ApplyCastShadowsSetting);
	}

	public void ApplyCastShadowsSetting()
	{
		if (scatters.Length == 0)
		{
			return;
		}
		for (int i = 0; i < scatters.Length; i++)
		{
			if (GameSettings.CELESTIAL_BODIES_CAST_SHADOWS)
			{
				scatters[i].castShadows = true;
				scatters[i].recieveShadows = true;
			}
			else
			{
				scatters[i].castShadows = false;
				scatters[i].recieveShadows = false;
			}
		}
	}

	public void Reset()
	{
	}

	[ContextMenu("Clear All Buffers")]
	public void ClearAll()
	{
		LandClassScatter[] array = scatters;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ClearAll();
		}
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.VertexMapCoords | GClass4.ModiferRequirements.MeshColorChannel;
		lcCount = landClasses.Length;
		lcList = new List<LandClass>(lcCount);
		LandClass[] array = landClasses;
		foreach (LandClass landClass in array)
		{
			landClass.altitudeRange.Setup();
			landClass.latitudeRange.Setup();
			landClass.latitudeDoubleRange.startStart = 1.0 - landClass.latitudeRange.endEnd;
			landClass.latitudeDoubleRange.startEnd = 1.0 - landClass.latitudeRange.endStart;
			landClass.latitudeDoubleRange.endStart = 1.0 - landClass.latitudeRange.startEnd;
			landClass.latitudeDoubleRange.endEnd = 1.0 - landClass.latitudeRange.startStart;
			landClass.latitudeDoubleRange.Setup();
			landClass.longitudeRange.Setup();
			landClass.coverageSimplex = new Simplex(landClass.coverageSeed, landClass.coverageOctaves, landClass.coveragePersistance, landClass.coverageFrequency);
			landClass.noiseSimplex = new Simplex(landClass.noiseSeed, landClass.noiseOctaves, landClass.noisePersistance, landClass.noiseFrequency);
			for (int j = 0; j < landClass.scatter.Length; j++)
			{
				LandClassScatterAmount landClassScatterAmount = landClass.scatter[j];
				int k = 0;
				for (int num = scatters.Length; k < num; k++)
				{
					if (landClassScatterAmount.scatterName == scatters[k].scatterName)
					{
						landClassScatterAmount.scatter = scatters[k];
						if (GameSettings.CELESTIAL_BODIES_CAST_SHADOWS)
						{
							landClassScatterAmount.scatter.castShadows = true;
							landClassScatterAmount.scatter.recieveShadows = true;
						}
						else
						{
							landClassScatterAmount.scatter.castShadows = false;
							landClassScatterAmount.scatter.recieveShadows = false;
						}
						landClassScatterAmount.scatterIndex = k;
						break;
					}
				}
			}
		}
		if (GClass4.Global_AllowScatter)
		{
			scatterCount = scatters.Length;
			lcScatterList = new double[scatterCount];
			scatterMinSubdiv = int.MaxValue;
			int l = 0;
			for (int num2 = scatters.Length; l < num2; l++)
			{
				LandClassScatter landClassScatter = scatters[l];
				landClassScatter.Setup(sphere);
				if (landClassScatter.minLevel < scatterMinSubdiv)
				{
					scatterMinSubdiv = landClassScatter.minLevel;
				}
			}
		}
		altitudeSimplex = new Simplex(altitudeSeed, altitudeOctaves, altitudePersistance, altitudeFrequency);
		latitudeSimplex = new Simplex(latitudeSeed, latitudeOctaves, latitudePersistance, latitudeFrequency);
		longitudeSimplex = new Simplex(longitudeSeed, longitudeOctaves, longitudePersistance, longitudeFrequency);
	}

	public override void OnQuadPreBuild(GClass3 quad)
	{
		if (quad.subdivision >= scatterMinSubdiv && createScatter && GClass4.Global_AllowScatter)
		{
			scatterActive = true;
		}
		else
		{
			scatterActive = false;
		}
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		lcList.Clear();
		lcListCount = 0;
		totalDelta = 0.0;
		data.vertColor = Color.black;
		if (useHeightMap)
		{
			vHeight = heightMap.GetPixelFloat(data.u, data.v);
		}
		else
		{
			vHeight = (data.vertHeight - sphere.radius) / (double)vHeightMax;
		}
		vHeight += (double)altitudeBlend * altitudeSimplex.noise(data.directionFromCenter);
		if (vHeight > 1.0)
		{
			vHeight = 1.0;
		}
		vLat = sphere.sy + (double)latitudeBlend * latitudeSimplex.noise(data.directionFromCenter);
		if (vLat > 1.0)
		{
			vLat = 1.0;
		}
		else if (vLat < 0.0)
		{
			vLat = 0.0;
		}
		vLon = sphere.sx + (double)longitudeBlend * longitudeSimplex.noise(data.directionFromCenter);
		if (vLon > 1.0)
		{
			vLon = 1.0;
		}
		else if (vLon < 0.0)
		{
			vLon = 0.0;
		}
		for (itr = 0; itr < lcCount; itr++)
		{
			lc = landClasses[itr];
			lc.altDelta = lc.altitudeRange.Lerp(vHeight);
			lc.latDelta = lc.latitudeRange.Lerp(vLat);
			if (lc.latitudeDouble)
			{
				lc.latDelta = Math.Max(lc.latitudeDoubleRange.Lerp(vLat), lc.latDelta);
			}
			lc.lonDelta = lc.longitudeRange.Lerp(vLon);
			lc.delta = lc.altDelta * lc.latDelta * lc.lonDelta;
			lc.delta = Lerp(lc.delta, lc.delta * lc.coverageSimplex.noiseNormalized(data.directionFromCenter), lc.coverageBlend);
			if (lc.delta != 0.0)
			{
				totalDelta += lc.delta;
				lcList.Add(landClasses[itr]);
				lcListCount++;
			}
		}
		for (itr = 0; itr < lcListCount; itr++)
		{
			lc = lcList[itr];
			lc.delta /= totalDelta;
			if (lc.delta > 0.0)
			{
				if (lc.minimumRealHeight != 0.0 && data.vertHeight - sphere.radius < lc.minimumRealHeight)
				{
					data.vertHeight = sphere.radius + lc.delta * lc.minimumRealHeight;
				}
				data.vertHeight += lc.delta * lc.alterRealHeight;
			}
		}
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		vHeightAltered = vHeight;
		for (itr = 0; itr < lcListCount; itr++)
		{
			lc = lcList[itr];
			if (lc.delta > 0.0)
			{
				if (createColors)
				{
					data.vertColor += Color.Lerp(lc.color, lc.noiseColor, (float)((double)lc.noiseBlend * lc.noiseSimplex.noiseNormalized(data.directionFromCenter))) * (float)lc.delta;
					vHeightAltered += lc.delta * (double)lc.alterApparentHeight;
				}
				if (data.allowScatter && scatterActive && lc.delta > 0.05)
				{
					LandClassScatterAmount[] scatter = lc.scatter;
					foreach (LandClassScatterAmount landClassScatterAmount in scatter)
					{
						if (data.buildQuad.subdivision >= scatterMinSubdiv)
						{
							lcScatterList[landClassScatterAmount.scatterIndex] += landClassScatterAmount.density * lc.delta * GClass4.cacheVertCountReciprocal * GClass4.Global_ScatterFactor;
							scatterInstCount++;
						}
					}
				}
			}
		}
		if (createColors)
		{
			if (vHeightAltered > 1.0)
			{
				vHeightAltered = 1.0;
			}
			else if (vHeightAltered < 0.0)
			{
				vHeightAltered = 0.0;
			}
			data.vertColor.a = (float)vHeightAltered;
		}
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		if (!scatterActive || scatterInstCount <= 0)
		{
			return;
		}
		for (itr = 0; itr < scatterCount; itr++)
		{
			if (lcScatterList[itr] > 0.0)
			{
				scatters[itr].AddScatterMeshController(quad, lcScatterList[itr]);
				lcScatterList[itr] = 0.0;
			}
		}
		scatterActive = false;
	}

	public override void OnSphereStarted()
	{
		int i = 0;
		for (int num = scatters.Length; i < num; i++)
		{
			scatters[i].SphereActive();
		}
	}

	public override void OnSphereReset()
	{
		int i = 0;
		for (int num = scatters.Length; i < num; i++)
		{
			scatters[i].SphereInactive();
		}
	}

	public static double Lerp(double v1, double v2, double dt)
	{
		return v2 * dt + v1 * (1.0 - dt);
	}

	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		ct2 = t * t;
		ct3 = ct2 * t;
		return start * (2.0 * ct3 - 3.0 * ct2 + 1.0) + startTangent * (ct3 - 2.0 * ct2 + t) + end * (-2.0 * ct3 + 3.0 * ct2) + endTangent * (ct3 - ct2);
	}

	public static double Clamp(double v, double low, double high)
	{
		if (v < low)
		{
			return low;
		}
		if (v > high)
		{
			return high;
		}
		return v;
	}

	public static double DoubleLerp(double startStart, double startEnd, double endStart, double endEnd, double point)
	{
		if (!(point <= startStart) && point < endEnd)
		{
			if (point < startEnd)
			{
				return (point - startStart) / (startEnd - startStart);
			}
			if (point <= endStart)
			{
				return 1.0;
			}
			if (point < endEnd)
			{
				return (point - endStart) / (endEnd - endStart);
			}
			return 0.0;
		}
		return 0.0;
	}
}
