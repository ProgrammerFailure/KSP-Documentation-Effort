using System;
using System.Collections;
using System.Collections.Generic;
using KSPAchievements;
using UnityEngine;

[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER
})]
public class ScenarioDiscoverableObjects : ScenarioModule
{
	public float spawnInterval = 15f;

	public float maxUntrackedLifetime = 20f;

	public float minUntrackedLifetime = 1f;

	public int spawnOddsAgainst = 2;

	public int spawnGroupMinLimit = 3;

	public int spawnGroupMaxLimit = 10;

	[KSPField(isPersistant = true)]
	public FloatCurve sizeCurve;

	[KSPField(isPersistant = true)]
	public int lastSeed;

	public UntrackedObjectClass minAsteroidClass;

	public UntrackedObjectClass maxAsteroidClass = UntrackedObjectClass.const_4;

	public int currentSize;

	public int rndGroupSize;

	public int newGroupSize;

	public bool discoveryUnlocked;

	public List<uint> untrackedObjectIDs;

	public List<uint> discoveredObjectIDs;

	public string tmpIDs;

	public string[] tmpIDList;

	public uint tmpId;

	public static ScenarioDiscoverableObjects Instance { get; set; }

	public override void OnLoad(ConfigNode node)
	{
		LoadGameDBSettings();
		untrackedObjectIDs = loadIDList(node, "untrackedIDs");
		discoveredObjectIDs = loadIDList(node, "discoveredIDs");
	}

	public void LoadGameDBSettings()
	{
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("SPACEOBJECTS_LIMITS");
		ConfigNode configNode = null;
		if (configNodes.Length != 0)
		{
			configNode = configNodes[0];
			if (configNodes.Length > 1)
			{
				Debug.LogWarning("[ScenarioDiscoverabelObjects]: Found more than one SPACEOBJECTS_LIMITS node using the first one");
			}
		}
		if (configNode == null)
		{
			Debug.LogWarning("[ScenarioDiscoverabelObjects]: No SPACEOBJECTS_LIMITS node found. Using default values");
			return;
		}
		configNode.TryGetValue("homeBodySpawnGroupMinLimit", ref spawnGroupMinLimit);
		configNode.TryGetValue("homeBodySpawnGroupMaxLimit", ref spawnGroupMaxLimit);
	}

	public List<uint> loadIDList(ConfigNode node, string listName)
	{
		List<uint> list = new List<uint>();
		tmpIDs = "";
		node.TryGetValue(listName, ref tmpIDs);
		tmpIDList = tmpIDs.Split(',');
		for (int i = 0; i < tmpIDList.Length; i++)
		{
			tmpId = 0u;
			if (uint.TryParse(tmpIDList[i], out tmpId))
			{
				list.Add(tmpId);
			}
		}
		return list;
	}

	public override void OnSave(ConfigNode node)
	{
		SaveIDList(node, "untrackedIDs", untrackedObjectIDs);
		SaveIDList(node, "discoveredIDs", discoveredObjectIDs);
	}

	public void SaveIDList(ConfigNode node, string name, List<uint> loadList)
	{
		tmpIDs = string.Join(",", loadList);
		node.AddValue(name, tmpIDs);
	}

	public override void OnAwake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("[ScenarioDiscoverableObjects]: Instance already exists!", Instance.gameObject);
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		if (sizeCurve == null)
		{
			sizeCurve = new FloatCurve();
			sizeCurve.Add(0f, 0f);
			sizeCurve.Add(0.3f, 0.45f);
			sizeCurve.Add(0.7f, 0.55f);
			sizeCurve.Add(1f, 1f);
		}
		discoveredObjectIDs = new List<uint>();
		untrackedObjectIDs = new List<uint>();
		GameEvents.onKnowledgeChanged.Add(OnKnowledgeChanged);
		GameEvents.onVesselDestroy.Add(OnVesselDestroy);
	}

	public void OnKnowledgeChanged(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> kChg)
	{
		Vessel vessel = kChg.host as Vessel;
		if (!(vessel == null) && discoveredObjectIDs.Contains(vessel.persistentId))
		{
			if (vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
			{
				untrackedObjectIDs.Remove(vessel.persistentId);
			}
			else
			{
				untrackedObjectIDs.AddUnique(vessel.persistentId);
			}
		}
	}

	public void OnVesselDestroy(Vessel v)
	{
		discoveredObjectIDs.Remove(v.persistentId);
		untrackedObjectIDs.Remove(v.persistentId);
	}

	public void Start()
	{
		UnityEngine.Random.InitState(lastSeed);
		discoveryUnlocked = GameVariables.Instance.UnlockedSpaceObjectDiscovery(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation));
		int count = untrackedObjectIDs.Count;
		while (count-- > 0)
		{
			if (!FlightGlobals.PersistentVesselIds.Contains(untrackedObjectIDs[count]))
			{
				untrackedObjectIDs.RemoveAt(count);
			}
		}
		StartCoroutine(SpawnDaemon());
	}

	public void OnDestroy()
	{
		GameEvents.onKnowledgeChanged.Remove(OnKnowledgeChanged);
		GameEvents.onVesselDestroy.Remove(OnVesselDestroy);
		discoveredObjectIDs = null;
		untrackedObjectIDs = null;
	}

	public IEnumerator SpawnDaemon()
	{
		while ((bool)this)
		{
			if (!ExpireDiscoveredObjects(Planetarium.GetUniversalTime()))
			{
				UpdateSpaceObjects();
			}
			yield return new WaitForSeconds(Mathf.Max(0.1f, spawnInterval / TimeWarp.CurrentRate));
		}
	}

	public void UpdateSpaceObjects()
	{
		if (!discoveryUnlocked)
		{
			return;
		}
		rndGroupSize = UnityEngine.Random.Range(spawnGroupMinLimit, spawnGroupMaxLimit);
		newGroupSize = Mathf.Max(currentSize, rndGroupSize);
		if (newGroupSize > currentSize)
		{
			if (CometManager.ShouldSpawnComet())
			{
				SpawnComet();
			}
			else if (UnityEngine.Random.Range(0, spawnOddsAgainst) == 0)
			{
				SpawnAsteroid();
			}
			else
			{
				Debug.Log("[AsteroidSpawner]: No new objects this time. (Odds are 1:" + spawnOddsAgainst + ")", base.gameObject);
			}
		}
	}

	public bool ExpireDiscoveredObjects(double double_0)
	{
		bool flag = false;
		int num = 0;
		currentSize = 0;
		for (int num2 = FlightGlobals.Vessels.Count - 1; num2 >= 0; num2--)
		{
			Vessel vessel = FlightGlobals.Vessels[num2];
			if (vessel != null && !vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
			{
				if (vessel.DiscoveryInfo.GetSignalLife(double_0) == 0.0)
				{
					if (!flag)
					{
						Debug.Log("[AsteroidSpawner]: " + vessel.GetDisplayName() + " has been untracked for too long and is now lost.", base.gameObject);
						GameEvents.OnDiscoverableObjectExpired.Fire(vessel);
						vessel.Die();
						flag = true;
					}
					num++;
				}
				else if (discoveredObjectIDs.Contains(vessel.persistentId))
				{
					currentSize++;
				}
			}
		}
		return flag;
	}

	[ContextMenu("Spawn An Asteroid")]
	public void SpawnAsteroid()
	{
		int num = UnityEngine.Random.Range(0, int.MaxValue);
		UnityEngine.Random.InitState(num);
		lastSeed = num;
		if (ReachedBody("Dres") && Mathf.Abs(num % 3) == 0)
		{
			SpawnDresAsteroid(num);
		}
		else
		{
			SpawnHomeAsteroid(num);
		}
	}

	[ContextMenu("Spawn Last Asteroid")]
	public void SpawnLastAsteroid()
	{
		UnityEngine.Random.InitState(lastSeed);
		if (ReachedBody("Dres") && Mathf.Abs(lastSeed % 3) == 0)
		{
			SpawnDresAsteroid(lastSeed);
		}
		else
		{
			SpawnHomeAsteroid(lastSeed);
		}
	}

	public void SpawnHomeAsteroid(int asteroidSeed)
	{
		CelestialBody homeBody = FlightGlobals.GetHomeBody();
		if (!(homeBody == null))
		{
			double randomDuration = GetRandomDuration();
			string text = DiscoverableObjectsUtil.GenerateAsteroidName();
			UntrackedObjectClass sizeCurveBasedClass = GetSizeCurveBasedClass(0f, 1f);
			double lifeTime = (double)UnityEngine.Random.Range(minUntrackedLifetime, maxUntrackedLifetime) * 24.0 * 60.0 * 60.0;
			double lifeTimeMax = (double)maxUntrackedLifetime * 24.0 * 60.0 * 60.0;
			ProtoVessel protoVessel = DiscoverableObjectsUtil.SpawnAsteroid(text, Orbit.CreateRandomOrbitFlyBy(homeBody, randomDuration), (uint)asteroidSeed, sizeCurveBasedClass, lifeTime, lifeTimeMax);
			discoveredObjectIDs.Add(protoVessel.persistentId);
			untrackedObjectIDs.Add(protoVessel.persistentId);
			Debug.Log("[AsteroidSpawner]: New object found near " + homeBody.name + ": " + text + "!", base.gameObject);
		}
	}

	public UntrackedObjectClass GetSizeCurveBasedClass(float minRange, float maxRange)
	{
		int num = maxAsteroidClass - minAsteroidClass;
		float time = Mathf.Clamp01(UnityEngine.Random.Range(minRange, maxRange));
		float num2 = sizeCurve.Evaluate(time);
		return minAsteroidClass + Mathf.RoundToInt(num2 * (float)num);
	}

	public void SpawnDresAsteroid(int asteroidSeed)
	{
		CelestialBody bodyByName = FlightGlobals.GetBodyByName("Dres");
		if (!(bodyByName == null))
		{
			string text = DiscoverableObjectsUtil.GenerateAsteroidName();
			UntrackedObjectClass sizeCurveBasedClass = GetSizeCurveBasedClass(0.5f, 1f);
			double lifeTime = (double)UnityEngine.Random.Range(minUntrackedLifetime, maxUntrackedLifetime) * 24.0 * 60.0 * 60.0;
			double lifeTimeMax = (double)maxUntrackedLifetime * 24.0 * 60.0 * 60.0;
			double num = (bodyByName.sphereOfInfluence - bodyByName.Radius) / 2.0;
			Orbit orbit = Orbit.CreateRandomOrbitAround(bodyByName, bodyByName.Radius + num * 1.100000023841858, bodyByName.Radius + num * 1.25);
			orbit.meanAnomalyAtEpoch = (double)(UnityEngine.Random.value * 2f) * Math.PI;
			ProtoVessel protoVessel = DiscoverableObjectsUtil.SpawnAsteroid(text, orbit, (uint)asteroidSeed, sizeCurveBasedClass, lifeTime, lifeTimeMax);
			discoveredObjectIDs.Add(protoVessel.persistentId);
			untrackedObjectIDs.Add(protoVessel.persistentId);
			Debug.Log("[AsteroidSpawner]: New object found near Dres: " + text + "!", base.gameObject);
		}
	}

	[ContextMenu("Spawn A Comet")]
	public void SpawnComet()
	{
		CometOrbitType cometType = CometManager.GenerateWeightedCometType();
		SpawnComet(cometType);
	}

	public void SpawnComet(string typeName)
	{
		CometOrbitType cometOrbitType = CometManager.GetCometOrbitType(typeName);
		if (cometOrbitType == null)
		{
			Debug.LogWarning("[ScenarioDiscoverableObject] Unable to find Spawn Comet Type: " + typeName);
		}
		else
		{
			SpawnComet(cometOrbitType);
		}
	}

	public void SpawnComet(CometOrbitType cometType)
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		UnityEngine.Random.InitState(seed);
		lastSeed = seed;
		double lifeTime = (double)UnityEngine.Random.Range(minUntrackedLifetime, maxUntrackedLifetime) * 24.0 * 60.0 * 60.0;
		double lifeTimeMax = (double)maxUntrackedLifetime * 24.0 * 60.0 * 60.0;
		UntrackedObjectClass randomObjClass = cometType.GetRandomObjClass();
		CometDefinition cometDef = CometManager.GenerateDefinition(cometType, randomObjClass, seed);
		Orbit o = cometType.CalculateHomeOrbit();
		DiscoverableObjectsUtil.SpawnComet("UnknownComet", o, cometDef, (uint)seed, randomObjClass, lifeTime, lifeTimeMax, optimizedCollider: false, 0f);
		Debug.Log("[CometSpawner]: New object found near " + FlightGlobals.GetHomeBodyName());
	}

	[ContextMenu("Check Spawn Probability")]
	public void DebugSpawnProbability()
	{
		float num = 1f / (float)spawnOddsAgainst;
		float num2 = spawnInterval / 60f / 60f;
		float num3 = 1f / num2 * num;
		if (num2 > 1f)
		{
			Debug.Log("For a " + num.ToString("0.####") + " chance every " + num2.ToString("0.0##") + " hours, the average spawn rate is a spawn every " + 1f / num3 + " hours");
		}
		else
		{
			Debug.Log("For a " + num.ToString("0.####") + " chance every " + (num2 * 60f).ToString("0.0##") + " minutes, the average spawn rate is a spawn every " + 1f / num3 * 60f + " minutes");
		}
	}

	public double GetRandomDuration()
	{
		return UnityEngine.Random.Range(15f, 60f);
	}

	public bool ReachedBody(string bodyName)
	{
		if (ProgressTracking.Instance != null)
		{
			CelestialBodySubtree bodyTree = ProgressTracking.Instance.GetBodyTree(bodyName);
			if (bodyTree != null && bodyTree.IsReached)
			{
				return true;
			}
		}
		return false;
	}
}
