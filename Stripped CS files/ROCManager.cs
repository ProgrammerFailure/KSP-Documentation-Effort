using System;
using System.Collections.Generic;
using Expansions;
using Expansions.Missions;
using UnityEngine;

public class ROCManager : MonoBehaviour
{
	[Serializable]
	public class ROCStats
	{
		public string rocType;

		public int activeQuads;

		public float activeQuadArea;

		public int activeRocCount;

		public float rocTypeFrequency;

		public float rocCoverage
		{
			get
			{
				if (activeQuadArea <= 0f)
				{
					return 0f;
				}
				return (float)activeRocCount / activeQuadArea;
			}
		}

		public ROCStats(string rocType)
		{
			this.rocType = rocType;
			activeQuads = 0;
			activeQuadArea = 0f;
			activeRocCount = 0;
			rocTypeFrequency = 0f;
		}
	}

	public ConfigNode[] rocDefs;

	public ConfigNode[] cbDefs;

	public ConfigNode[] vfxForceDefs;

	[SerializeField]
	public DictionaryValueList<string, GameObject> rocTypeObjects;

	[SerializeField]
	public List<ROCDefinition> _rocDefinitions;

	[SerializeField]
	public List<PQSROCControl> _pqsRocControls;

	[SerializeField]
	public List<int> removedROCs;

	[SerializeField]
	public bool rocsEnabledInCurrentGame;

	public bool debugROCFinder;

	public bool debugROCScanPoints;

	public bool debugROCStats;

	public DictionaryValueList<string, ROCStats> rocStats;

	public Callback OnStatsChanged;

	public static ROCManager Instance { get; set; }

	public DictionaryValueList<string, GameObject> RocTypeObjects => rocTypeObjects;

	public List<ROCDefinition> rocDefinitions => _rocDefinitions;

	public List<PQSROCControl> pqsRocControls => _pqsRocControls;

	public bool RocsEnabledInCurrentGame => rocsEnabledInCurrentGame;

	public void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			Instance = null;
			return;
		}
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			Instance = null;
			return;
		}
		Instance = this;
		debugROCStats = GameSettings.COLLECT_ROC_STATS;
		if (base.transform == base.transform.root)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		_pqsRocControls = new List<PQSROCControl>();
		_rocDefinitions = new List<ROCDefinition>();
		rocTypeObjects = new DictionaryValueList<string, GameObject>();
		rocStats = new DictionaryValueList<string, ROCStats>();
	}

	public void Start()
	{
		rocDefs = GameDatabase.Instance.GetConfigNodes("ROC_DEFINITION");
		for (int i = 0; i < rocDefs.Length; i++)
		{
			ROCDefinition rOCDefinition = new ROCDefinition();
			rOCDefinition.Load(rocDefs[i]);
			cbDefs = rocDefs[i].GetNodes("CELESTIALBODY");
			List<RocCBDefinition> list = new List<RocCBDefinition>();
			for (int j = 0; j < cbDefs.Length; j++)
			{
				RocCBDefinition rocCBDefinition = new RocCBDefinition();
				rocCBDefinition.Load(cbDefs[j]);
				list.Add(rocCBDefinition);
			}
			rOCDefinition.myCelestialBodies = list;
			_rocDefinitions.Add(rOCDefinition);
		}
		for (int num = _rocDefinitions.Count - 1; num >= 0; num--)
		{
			GameObject gameObject = null;
			GameObject gameObject2 = null;
			if (!string.IsNullOrEmpty(rocDefinitions[num].prefabName))
			{
				gameObject = AssetBase.GetPrefab(_rocDefinitions[num].prefabName);
				if (gameObject != null)
				{
					gameObject2 = UnityEngine.Object.Instantiate(gameObject);
				}
			}
			if (gameObject2 == null)
			{
				Debug.LogWarning("[ROCManager]: Unable to instantiate prefab for ROC type " + _rocDefinitions[num].type + " with prefab name " + _rocDefinitions[num].prefabName);
			}
			else
			{
				GClass0 component = gameObject2.GetComponent<GClass0>();
				gameObject2.transform.SetParent(base.transform);
				gameObject2.SetActive(value: false);
				if (component != null)
				{
					component.SetStats(_rocDefinitions[num].type, _rocDefinitions[num].displayName, _rocDefinitions[num].prefabName, _rocDefinitions[num].modelName, _rocDefinitions[num].orientateUp, _rocDefinitions[num].depth, _rocDefinitions[num].canBeTaken, _rocDefinitions[num].frequency, _rocDefinitions[num].myCelestialBodies, _rocDefinitions[num].castShadows, _rocDefinitions[num].receiveShadows, _rocDefinitions[num].collisionThreshold, _rocDefinitions[num].smallRoc, _rocDefinitions[num].randomDepth, _rocDefinitions[num].randomOrientation, _rocDefinitions[num].localSpaceScanPoints, _rocDefinitions[num].burstEmitterMinWait, _rocDefinitions[num].burstEmitterMaxWait, _rocDefinitions[num].randomRotation, _rocDefinitions[num].scale, _rocDefinitions[num].sfxVolume, _rocDefinitions[num].idleClipPath, _rocDefinitions[num].burstClipPath, _rocDefinitions[num].vfxCurveForce, _rocDefinitions[num].vfxBaseForce, _rocDefinitions[num].applyForces, _rocDefinitions[num].vfxForceRadius, _rocDefinitions[num].forceDirection, _rocDefinitions[num].radiusCenter);
					rocTypeObjects.Add(_rocDefinitions[num].type, gameObject2);
				}
				else
				{
					_rocDefinitions.RemoveAt(num);
				}
			}
		}
		PSystemManager.Instance.OnPSystemReady.Add(GetROCControlFromCB);
		GameEvents.onGameStateLoad.Add(OnGameStateLoad);
		GameEvents.onGameNewStart.Add(OnGameNewStart);
		GameEvents.Mission.onStarted.Add(onMissionStarted);
	}

	public void OnGameStateLoad(ConfigNode node)
	{
		if (HighLogic.CurrentGame.ROCSeed > -1)
		{
			SetROCsStateForGame(active: true);
		}
		else
		{
			SetROCsStateForGame(active: false);
		}
	}

	public void OnGameNewStart()
	{
		SetROCsStateForGame(active: true);
	}

	public void onMissionStarted(Mission mission)
	{
		SetROCsStateForGame(active: true);
	}

	public void SetROCsStateForGame(bool active)
	{
		if (active)
		{
			EnableAllROCs();
			rocsEnabledInCurrentGame = true;
		}
		else
		{
			DisableAllROCs();
			rocsEnabledInCurrentGame = false;
		}
	}

	public void GetROCControlFromCB()
	{
		ValidateCBBiomeCombos();
		List<CelestialBody> localBodies = PSystemManager.Instance.localBodies;
		for (int i = 0; i < localBodies.Count; i++)
		{
			PQSROCControl componentInChildren = localBodies[i].gameObject.GetComponentInChildren<PQSROCControl>();
			if (componentInChildren != null && PopulateROCControlFromConfig(componentInChildren))
			{
				_pqsRocControls.Add(componentInChildren);
			}
		}
	}

	public bool PopulateROCControlFromConfig(PQSROCControl ROCControl)
	{
		bool result = true;
		for (int i = 0; i < _rocDefinitions.Count; i++)
		{
			ROCDefinition rOCDefinition = _rocDefinitions[i];
			for (int j = 0; j < rOCDefinition.myCelestialBodies.Count; j++)
			{
				if (!(rOCDefinition.myCelestialBodies[j].name == ROCControl.currentCBName))
				{
					continue;
				}
				LandClassROC landClassROC = new LandClassROC(rOCDefinition, ROCControl);
				GClass4 componentInParent = ROCControl.gameObject.GetComponentInParent<GClass4>();
				if ((bool)componentInParent)
				{
					landClassROC.Setup(componentInParent);
					if (componentInParent.isActive)
					{
						landClassROC.SphereActive();
					}
					if (landClassROC.celestialBody == null)
					{
						Debug.LogWarningFormat("[ROCManager]: Invalid CelestialBody Name Defined for ROC {0} BodyName: {1}", _rocDefinitions[i], ROCControl.currentCBName);
						result = false;
					}
					else
					{
						ROCControl.rocs.Add(landClassROC);
					}
				}
				else
				{
					Debug.LogWarningFormat("[ROCManager]: Unable to find PQS in parent for ROC {0} on {1}", _rocDefinitions[i], ROCControl.currentCBName);
					result = false;
				}
			}
		}
		return result;
	}

	public void ValidateCBBiomeCombos()
	{
		for (int num = _rocDefinitions.Count - 1; num >= 0; num--)
		{
			for (int num2 = _rocDefinitions[num].myCelestialBodies.Count - 1; num2 >= 0; num2--)
			{
				CelestialBody celestialBody = ValidCelestialBody(_rocDefinitions[num].myCelestialBodies[num2].name);
				if (celestialBody == null)
				{
					Debug.LogWarningFormat("[ROCManager]: Invalid CelestialBody Name {0} on ROC Definition {1}. Removed entry.", _rocDefinitions[num].myCelestialBodies[num2].name, _rocDefinitions[num].type);
					_rocDefinitions[num].myCelestialBodies.RemoveAt(num2);
				}
				else
				{
					for (int num3 = _rocDefinitions[num].myCelestialBodies[num2].biomes.Count - 1; num3 >= 0; num3--)
					{
						if (!ValidCBBiome(celestialBody, _rocDefinitions[num].myCelestialBodies[num2].biomes[num3]))
						{
							Debug.LogWarningFormat("[ROCManager]: Invalid Biome Name {0} for Celestial Body {1} on ROC Definition {2}. Removed entry.", _rocDefinitions[num].myCelestialBodies[num2].biomes[num3], _rocDefinitions[num].myCelestialBodies[num2].name, _rocDefinitions[num].type);
							_rocDefinitions[num].myCelestialBodies[num2].biomes.RemoveAt(num3);
						}
					}
				}
				if (rocDefinitions[num].myCelestialBodies[num2].biomes.Count == 0)
				{
					Debug.LogWarningFormat("[ROCManager]: No Valid Biomes for Celestial Body {0} on ROC Definition {1}. Removed entry.", _rocDefinitions[num].myCelestialBodies[num2].name, _rocDefinitions[num].type);
					_rocDefinitions[num].myCelestialBodies.RemoveAt(num2);
				}
			}
			if (rocDefinitions[num].myCelestialBodies.Count == 0)
			{
				Debug.LogWarningFormat("[ROCManager]: No Valid Celestial Bodies on ROC Definition {0}. Removed entry.", _rocDefinitions[num].type);
				_rocDefinitions.RemoveAt(num);
			}
		}
	}

	public CelestialBody ValidCelestialBody(string bodyName)
	{
		int num = 0;
		while (true)
		{
			if (num < PSystemManager.Instance.localBodies.Count)
			{
				if (PSystemManager.Instance.localBodies[num].name == bodyName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return PSystemManager.Instance.localBodies[num];
	}

	public bool ValidCBBiome(CelestialBody body, string biomeName)
	{
		int num = 0;
		while (true)
		{
			if (num < body.BiomeMap.Attributes.Length)
			{
				if (body.BiomeMap.Attributes[num].name == biomeName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void DisableAllROCs()
	{
		for (int i = 0; i < _pqsRocControls.Count; i++)
		{
			_pqsRocControls[i].modEnabled = false;
			if (_pqsRocControls[i].sphere != null)
			{
				_pqsRocControls[i].sphere.ResetModList();
			}
		}
	}

	public void EnableAllROCs()
	{
		for (int i = 0; i < _pqsRocControls.Count; i++)
		{
			_pqsRocControls[i].modEnabled = true;
			if (_pqsRocControls[i].sphere != null && _pqsRocControls[i].sphere.isStarted)
			{
				_pqsRocControls[i].sphere.ResetModList();
			}
		}
	}

	public void SaveRemoveROCs(ConfigNode node)
	{
		if (removedROCs != null)
		{
			for (int i = 0; i < removedROCs.Count; i++)
			{
				node.AddValue("ROCId", removedROCs[i]);
			}
		}
	}

	public void LoadRemovedROCs(ConfigNode node)
	{
		removedROCs = new List<int>();
		List<string> valuesList = node.GetValuesList("ROCId");
		for (int i = 0; i < valuesList.Count; i++)
		{
			int result = -1;
			if (int.TryParse(valuesList[i], out result))
			{
				removedROCs.AddUnique(result);
			}
		}
	}

	public bool ROCRemoved(int rocId)
	{
		if (removedROCs == null)
		{
			removedROCs = new List<int>();
		}
		int num = 0;
		while (true)
		{
			if (num < removedROCs.Count)
			{
				if (removedROCs[num] == rocId)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void RemoveROC(int rocId)
	{
		if (removedROCs == null)
		{
			removedROCs = new List<int>();
		}
		removedROCs.AddUnique(rocId);
	}

	public void ClearROCsCache()
	{
		for (int i = 0; i < _pqsRocControls.Count; i++)
		{
			for (int j = 0; j < _pqsRocControls[i].rocs.Count; j++)
			{
				_pqsRocControls[i].rocs[j].ClearCache();
			}
		}
	}

	public string GetTerrainTag(GameObject obj, out GameObject terrainObj)
	{
		string result = "";
		terrainObj = obj;
		PQSMod_ROCScatterQuad componentInParent = obj.GetComponentInParent<PQSMod_ROCScatterQuad>();
		if (componentInParent != null && componentInParent.quad != null)
		{
			result = (componentInParent.quad.gameObject.CompareTag("Untagged") ? "" : componentInParent.quad.tag);
			terrainObj = componentInParent.quad.gameObject;
		}
		return result;
	}

	public void AddROCStats_Quad(PQSROCControl control, ROCDefinition r, GClass3 quad)
	{
		if (!rocStats.ContainsKey(r.type))
		{
			rocStats.Add(r.type, new ROCStats(r.type));
			rocStats[r.type].rocTypeFrequency = r.frequency;
		}
		rocStats[r.type].activeQuads++;
		rocStats[r.type].activeQuadArea += control.quadArea;
		if (OnStatsChanged != null)
		{
			OnStatsChanged();
		}
	}

	public void AddROCStats_ROCs(string rType, int count, float frequency)
	{
		if (!rocStats.ContainsKey(rType))
		{
			rocStats.Add(rType, new ROCStats(rType));
			rocStats[rType].rocTypeFrequency = frequency;
		}
		rocStats[rType].activeRocCount += count;
		if (OnStatsChanged != null)
		{
			OnStatsChanged();
		}
	}

	public void ClearStats()
	{
		rocStats.Clear();
	}

	public void SubtractROCStats_Quad(PQSROCControl control, ROCDefinition r, GClass3 quad)
	{
		if (rocStats.ContainsKey(r.type))
		{
			rocStats[r.type].activeQuads--;
			rocStats[r.type].activeQuadArea -= control.quadArea;
			if (OnStatsChanged != null)
			{
				OnStatsChanged();
			}
		}
	}

	public void SubtractROCStats_ROCs(string rType, int count)
	{
		if (rocStats.ContainsKey(rType))
		{
			rocStats[rType].activeRocCount -= count;
			if (OnStatsChanged != null)
			{
				OnStatsChanged();
			}
		}
	}
}
