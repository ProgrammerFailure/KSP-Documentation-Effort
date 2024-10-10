using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("PQuadSphere/PQuadSphere_Cache")]
public class PQSCache : MonoBehaviour
{
	public class PQSGlobalPresetList
	{
		public List<PQSPreset> presets;

		public string preset;

		public int presetIndex;

		public string version;

		public PQSGlobalPresetList()
		{
			presets = new List<PQSPreset>();
			version = VersioningBase.GetVersionString();
		}

		public void Load(ConfigNode node)
		{
			preset = node.GetValue("preset");
			if (preset == null)
			{
				Debug.LogWarning("No preset name found");
				preset = "";
				return;
			}
			version = node.GetValue("version");
			if (version == null)
			{
				version = "0.0.0";
			}
			foreach (ConfigNode node2 in node.nodes)
			{
				if (node2.name == "PRESET")
				{
					PQSPreset pQSPreset = new PQSPreset();
					pQSPreset.Load(node2);
					presets.Add(pQSPreset);
				}
			}
			int i = 0;
			for (int count = presets.Count; i < count; i++)
			{
				if (presets[i].name == preset)
				{
					presetIndex = i;
				}
			}
		}

		public void SetPreset(int presetIndex)
		{
			preset = presets[presetIndex].name;
			this.presetIndex = presetIndex;
		}

		public void SetPreset(string presetName)
		{
			int i = 0;
			for (int count = presets.Count; i < count; i++)
			{
				if (presets[i].name == preset)
				{
					preset = presetName;
					presetIndex = i;
				}
			}
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("preset", preset);
			node.AddValue("version", version);
			foreach (PQSPreset preset in presets)
			{
				preset.Save(node.AddNode("PRESET"));
			}
		}

		public PQSSpherePreset GetPreset(string pqsName)
		{
			foreach (PQSPreset preset in presets)
			{
				if (!(preset.name == this.preset))
				{
					continue;
				}
				foreach (PQSSpherePreset spherePreset in preset.spherePresets)
				{
					if (spherePreset.name == pqsName)
					{
						return spherePreset;
					}
				}
			}
			Debug.LogWarning("Cannot find preset '" + this.preset + "' for pqs '" + pqsName + "'");
			return null;
		}
	}

	public class PQSPreset
	{
		public string name;

		public List<PQSSpherePreset> spherePresets;

		public PQSPreset()
		{
			name = "";
			spherePresets = new List<PQSSpherePreset>();
		}

		public void Load(ConfigNode node)
		{
			name = node.GetValue("name");
			if (name == null)
			{
				Debug.LogWarning("No preset name found");
				name = "";
				return;
			}
			foreach (ConfigNode node2 in node.nodes)
			{
				if (node2.name == "PLANET")
				{
					PQSSpherePreset pQSSpherePreset = new PQSSpherePreset();
					pQSSpherePreset.Load(node2);
					spherePresets.Add(pQSSpherePreset);
				}
			}
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("name", name);
			foreach (PQSSpherePreset spherePreset in spherePresets)
			{
				spherePreset.Save(node.AddNode("PLANET"));
			}
		}
	}

	public class PQSSpherePreset
	{
		public string name;

		public double minDistance;

		public int minSubdivision;

		public int maxSubdivision;

		public PQSSpherePreset()
		{
			name = "";
		}

		public PQSSpherePreset(string name, double minDistance, int minSubdivision, int maxSubdivision)
		{
			this.name = name;
			this.minDistance = minDistance;
			this.minSubdivision = minSubdivision;
			this.maxSubdivision = maxSubdivision;
		}

		public void Load(ConfigNode node)
		{
			name = node.GetValue("name");
			if (name == null)
			{
				Debug.LogWarning("No preset name found");
				name = "";
				return;
			}
			if (node.HasValue("minDistance"))
			{
				minDistance = double.Parse(node.GetValue("minDistance"));
			}
			if (node.HasValue("minSubdivision"))
			{
				minSubdivision = int.Parse(node.GetValue("minSubdivision"));
			}
			if (node.HasValue("maxSubdivision"))
			{
				maxSubdivision = int.Parse(node.GetValue("maxSubdivision"));
			}
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("name", name);
			node.AddValue("minDistance", minDistance);
			node.AddValue("minSubdivision", minSubdivision);
			node.AddValue("maxSubdivision", maxSubdivision);
		}
	}

	public delegate void OnEvent();

	public static PQSCache Instance;

	public static PQSGlobalPresetList PresetList;

	public static int lastCompatibleMajor = 1;

	public static int lastCompatibleMinor = 4;

	public static int lastCompatibleRev = 3;

	public int cachePQInitialCount;

	public int cachePQIncreasePerFrame;

	public bool cachePQCoroutine;

	public bool cacheBeingCreated;

	[HideInInspector]
	public bool cacheReady;

	[HideInInspector]
	public Stack<GClass3> cachePQUnassigned;

	public int cachePQAssignedCount;

	public int cachePQUnassignedCount;

	public int cachePQTotalCount;

	public OnEvent onCacheReady;

	public static void LoadPresetList(ConfigNode node)
	{
		if (node.HasNode("TERRAIN") && node.GetNode("TERRAIN").nodes.Count != 0)
		{
			ConfigNode node2 = node.GetNode("TERRAIN");
			PresetList = new PQSGlobalPresetList();
			PresetList.Load(node2);
			if (node2.HasNode("PQS"))
			{
				GClass4.LoadCacheSettings(node2.GetNode("PQS"));
			}
			if (!GClass4.GameBindings.TerrainPresetListIsCompatible(PresetList.version))
			{
				Debug.LogWarning("Terrain Preset list is incompatible. Reverting to Defaults.");
				CreateDefaultPresetList();
			}
		}
		else
		{
			Debug.LogWarning("Creating terrain preset list");
			CreateDefaultPresetList();
		}
	}

	public static void SavePresetList(ConfigNode node)
	{
		if (PresetList == null)
		{
			Debug.LogWarning("Creating terrain preset list");
			CreateDefaultPresetList();
		}
		PresetList.Save(node.AddNode("TERRAIN"));
	}

	public static void CreateDefaultPresetList()
	{
		PresetList = new PQSGlobalPresetList();
		PQSPreset pQSPreset = new PQSPreset();
		pQSPreset.name = "Low";
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Kerbin", 4.0, 1, 8));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("KerbinOcean", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Mun", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Minmus", 4.0, 1, 6));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Bop", 4.0, 1, 6));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Duna", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Eve", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("EveOcean", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Gilly", 4.0, 1, 6));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Ike", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Laythe", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("LaytheOcean", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Moho", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Tylo", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Vall", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Dres", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Pol", 4.0, 1, 7));
		pQSPreset.spherePresets.Add(new PQSSpherePreset("Eeloo", 4.0, 1, 7));
		PQSPreset pQSPreset2 = new PQSPreset();
		pQSPreset2.name = "Default";
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Kerbin", 6.0, 1, 9));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("KerbinOcean", 6.0, 1, 7));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Mun", 6.0, 1, 8));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Minmus", 6.0, 1, 6));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Bop", 6.0, 1, 6));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Duna", 6.0, 1, 8));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Eve", 6.0, 1, 9));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("EveOcean", 6.0, 1, 7));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Gilly", 6.0, 1, 6));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Ike", 6.0, 1, 6));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Laythe", 6.0, 1, 9));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("LaytheOcean", 6.0, 1, 7));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Moho", 6.0, 1, 8));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Tylo", 6.0, 1, 8));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Vall", 6.0, 1, 8));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Dres", 6.0, 1, 8));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Pol", 6.0, 1, 8));
		pQSPreset2.spherePresets.Add(new PQSSpherePreset("Eeloo", 6.0, 1, 8));
		PQSPreset pQSPreset3 = new PQSPreset();
		pQSPreset3.name = "High";
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Kerbin", 8.0, 1, 10));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("KerbinOcean", 8.0, 1, 7));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Mun", 8.0, 1, 9));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Minmus", 8.0, 1, 7));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Bop", 8.0, 1, 6));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Duna", 8.0, 1, 9));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Eve", 8.0, 1, 10));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("EveOcean", 8.0, 1, 7));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Gilly", 8.0, 1, 7));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Ike", 8.0, 1, 7));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Laythe", 8.0, 1, 10));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("LaytheOcean", 8.0, 1, 7));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Moho", 8.0, 1, 9));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Tylo", 8.0, 1, 9));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Vall", 8.0, 1, 9));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Dres", 8.0, 1, 9));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Pol", 8.0, 1, 9));
		pQSPreset3.spherePresets.Add(new PQSSpherePreset("Eeloo", 8.0, 1, 9));
		PresetList.presets.Add(pQSPreset);
		PresetList.presets.Add(pQSPreset2);
		PresetList.presets.Add(pQSPreset3);
		PresetList.preset = "Default";
		PresetList.presetIndex = 1;
	}

	public void Reset()
	{
		cachePQInitialCount = 9216;
		cachePQCoroutine = true;
		cachePQIncreasePerFrame = 256;
		cacheReady = false;
	}

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogWarning("PQSCache already exists. Removing");
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (base.transform == base.transform.root)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		if (!cacheReady && !cacheBeingCreated)
		{
			CreateCache();
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void CreateCache()
	{
		if (GClass4.cacheMesh == null)
		{
			GClass4.CreateCache();
		}
		cachePQUnassigned = new Stack<GClass3>(cachePQInitialCount);
		cachePQAssignedCount = 0;
		if (cachePQCoroutine)
		{
			StartCoroutine(CoroutineCreatePQCache());
			return;
		}
		IncreasePQCache(cachePQInitialCount);
		cacheReady = true;
		if (onCacheReady != null)
		{
			onCacheReady();
		}
		onCacheReady = null;
	}

	public void IncreasePQCache(int addCount)
	{
		for (int i = 0; i < addCount; i++)
		{
			GameObject gameObject = new GameObject();
			GClass3 gClass = gameObject.AddComponent<GClass3>();
			gClass.meshFilter = gameObject.AddComponent<MeshFilter>();
			gClass.meshRenderer = gameObject.AddComponent<MeshRenderer>();
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gClass.mesh = new Mesh();
			gClass.mesh.vertices = GClass4.cacheVerts;
			gClass.mesh.uv = GClass4.cacheUVs;
			gClass.mesh.normals = GClass4.cacheNormals;
			gClass.mesh.tangents = GClass4.cacheTangents;
			gClass.mesh.triangles = GClass4.cacheIndices[0];
			gClass.mesh.colors = GClass4.cacheColors;
			gClass.meshFilter.sharedMesh = gClass.mesh;
			gClass.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			gClass.meshRenderer.receiveShadows = false;
			gClass.verts = (Vector3[])GClass4.cacheVerts.Clone();
			gClass.vertNormals = (Vector3[])GClass4.cacheNormals.Clone();
			gClass.edgeNormals = new Vector3[4][];
			for (int j = 0; j < 4; j++)
			{
				gClass.edgeNormals[j] = new Vector3[GClass4.cacheSideVertCount];
			}
			gameObject.SetActive(value: false);
			cachePQUnassigned.Push(gClass);
		}
		cachePQUnassignedCount += addCount;
		cachePQTotalCount += addCount;
	}

	public GClass3 GetQuad()
	{
		if (cachePQUnassignedCount == 0)
		{
			IncreasePQCache(cachePQIncreasePerFrame);
			MonoBehaviour.print("PQSCache: Increased PQ Cache by " + cachePQIncreasePerFrame + " to " + cachePQTotalCount);
		}
		cachePQAssignedCount++;
		cachePQUnassignedCount--;
		return cachePQUnassigned.Pop();
	}

	public void DestroyQuad(GClass3 quad)
	{
		quad.north = null;
		quad.south = null;
		quad.east = null;
		quad.west = null;
		quad.parent = null;
		quad.quadRoot = null;
		quad.sphereRoot = null;
		quad.subdivideThresholdFactor = 1.0;
		quad.isBuilt = false;
		quad.isSubdivided = false;
		quad.isActive = false;
		quad.isVisible = false;
		quad.transform.parent = base.transform;
		quad.meshRenderer.enabled = false;
		quad.gameObject.SetActive(value: false);
		cachePQAssignedCount--;
		cachePQUnassignedCount++;
		cachePQUnassigned.Push(quad);
	}

	public void ResetCache()
	{
		GClass4[] array = (GClass4[])Object.FindObjectsOfType(typeof(GClass4));
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetSphere();
		}
	}

	public IEnumerator CoroutineCreatePQCache()
	{
		cacheBeingCreated = true;
		while (cachePQUnassignedCount < cachePQInitialCount)
		{
			IncreasePQCache(cachePQIncreasePerFrame);
			yield return null;
		}
		cacheBeingCreated = false;
		cacheReady = true;
		if (onCacheReady != null)
		{
			onCacheReady();
		}
		onCacheReady = null;
	}
}
