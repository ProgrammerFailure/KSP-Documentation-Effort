using System;
using System.Collections.Generic;
using FinePrint.Utilities;
using SentinelMission;
using UnityEngine;

public class CometManager : MonoBehaviour
{
	[Serializable]
	public class CometTailColor : IConfigNode
	{
		public Color emitterColor = new Color(0.04313725f, 0.2906635f, 13f / 15f, 23f / 51f);

		[SerializeField]
		public int chanceWeight = 5;

		public void Load(ConfigNode node)
		{
			node.TryGetValue("emitterColor", ref emitterColor);
			node.TryGetValue("chanceWeight", ref chanceWeight);
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("emitterColor", emitterColor);
			node.AddValue("chanceWeight", chanceWeight);
		}
	}

	public List<Vessel> comets;

	[SerializeField]
	public float sentinelAngle = 70f;

	public ConfigNode[] cometDefs;

	public ConfigNode managerDef;

	public List<CometOrbitType> cometDefinitions;

	public float maxSpawnDistance = 2f;

	public float minSpawnDistance = 1.5f;

	public float maxSentinelSpawnDistance = 2f;

	public float minSentinelSpawnDistance = 1.1f;

	public float explosionScaleMultiplier = 0.2f;

	public FloatCurve maxCometsPerUntrackedObjects = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(10f, 1f),
		new Keyframe(250f, 5f)
	});

	public FloatCurve spawnChance = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 0.1f),
		new Keyframe(1f, 0f)
	});

	public Transform dustTailParticleFarEffect;

	public Transform ionTailParticleFarEffect;

	public Transform comaNearEffect;

	public double maxVFXStartDistance = 2.0;

	public double minVFXStartDistance = 0.8;

	public FloatCurve vfxStartDistribution = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public double maxComaSizeRatio = 1000000.0;

	public double minComaSizeRatio = 10000.0;

	public FloatCurve comaSizeRatioDistribution = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public double maxTailWidthRatio = 1.0;

	public double maxIonTailWidthRatio = 0.3;

	public FloatCurve tailWidthDistribution = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public double maxTailLengthRatio = 100000.0;

	public double minTailLengthRatio = 1000.0;

	public FloatCurve tailLengthRatioDistribution = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public FloatCurve vfxSizeFromDistance = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 0f)
	});

	public FloatCurve nearDustVFXSizeFromDistance = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 0f)
	});

	public FloatCurve geyserVFXSizeFromDistance = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 0f)
	});

	[SerializeField]
	public double vFXUpdateFrequency = 30.0;

	[SerializeField]
	public double vFXUpdateFrequencyInAtmosphere = 0.029999999329447746;

	[SerializeField]
	public float atmospherePressureForVFXFade = 0.05f;

	[SerializeField]
	public FloatCurve atmospherePressureFadeDistributionForVFXFade = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	[SerializeField]
	public float gravityAdditionForVFXFadeStart = 300f;

	[SerializeField]
	public float gravityMultiplierForVFXFadeEnd = 100f;

	[SerializeField]
	public float gravitySubtractionForVFXFadeEnd = 20f;

	[SerializeField]
	public double minGeyserRadiusRatio = 0.04;

	[SerializeField]
	public double maxGeyserRadiusRatio = 0.04;

	[SerializeField]
	public FloatCurve geysersRatioDistribution = new FloatCurve(new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	[SerializeField]
	public float geyserVisibleDistance = 500f;

	public GameObject geyserEmitter;

	[SerializeField]
	public float geyserSizeRadiusMultiplier = 0.05f;

	[SerializeField]
	public float nearDustVisibleRadiusMultiplier = 2f;

	[SerializeField]
	public double minNearDustEmitterRadiusRatio = 0.1;

	[SerializeField]
	public double maxNearDustEmitterRadiusRatio = 0.1;

	public GameObject nearDustEmitter;

	[SerializeField]
	public float vesselRangeOrbitLoadOverride = 150000f;

	[SerializeField]
	public float vesselRangeOrbitUnloadOverride = 151000f;

	[SerializeField]
	public float vesselRangeOrbitPackOverride = 1100f;

	[SerializeField]
	public float vesselRangeOrbitUnpackOverride = 1000f;

	public float fragmentdynamicPressurekPa = 10f;

	public int minFragments = 3;

	public int maxFragments = 5;

	public int minFragmentClassStep = 1;

	public int maxFragmentClassStep = 3;

	public float fragmentMaxPressureModifier = 1f;

	public float fragmentMinPressureModifier = 6f;

	public float fragmentMinSafeTime = 3f;

	public float fragmentMaxSafeTime = 20f;

	public float fragmentInstanceOffset = 2f;

	public float fragmentAngleVariation = 22.5f;

	public int fragmentIgnoreCollisions = 20;

	public KSPRandom generator;

	[SerializeField]
	public float currentComets;

	[SerializeField]
	public float cometCap;

	[SerializeField]
	public float cometChance;

	public static double spawnSMADefault = 13600000000.0;

	public static float minSpawnDefault = 1.5f;

	public static float maxSpawnDefault = 2f;

	public static float minSentinelSpawnDefault = 1.1f;

	public static float maxSentinelSpawnDefault = 1.5f;

	[Tooltip("A list of colors with weightings that will be used to pick an ion tail tint color. Higher the weight the more chance it is chosen.")]
	public List<CometTailColor> ionTailColors;

	[Tooltip("A list of colors with weightings that will be used to pick an dust tail tint color. Higher the weight the more chance it is chosen.")]
	public List<CometTailColor> dustTailColors;

	[SerializeField]
	public Color ionTailColorP67 = new Color(0.04313725f, 0.2906635f, 13f / 15f, 23f / 51f);

	public static CometManager Instance { get; set; }

	public List<Vessel> Comets => comets;

	public List<Vessel> DiscoveredComets
	{
		get
		{
			List<Vessel> list = new List<Vessel>();
			for (int i = 0; i < comets.Count; i++)
			{
				if (comets[i].DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
				{
					list.Add(comets[i]);
				}
			}
			return list;
		}
	}

	public static float SentinelDiscoveryArc => Instance.sentinelAngle;

	public static double VFXUpdateFrequency
	{
		get
		{
			if (!(Instance == null))
			{
				return Instance.vFXUpdateFrequency;
			}
			return 10.0;
		}
	}

	public static double VFXUpdateFrequencyInAtmosphere
	{
		get
		{
			if (!(Instance == null))
			{
				return Instance.vFXUpdateFrequencyInAtmosphere;
			}
			return 0.029999999329447746;
		}
	}

	public void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			Instance = null;
			return;
		}
		Instance = this;
		generator = new KSPRandom();
		cometDefinitions = new List<CometOrbitType>();
		comets = new List<Vessel>();
		GameEvents.OnGameDatabaseLoaded.Add(OnGameDatabaseLoaded);
		GameEvents.onFlightGlobalsAddVessel.Add(OnVesselAdded);
		GameEvents.onFlightGlobalsRemoveVessel.Add(OnVesselRemoved);
	}

	public void Start()
	{
		LoadManagerDefinitions();
		LoadCometTypes();
		GameEvents.OnGameDatabaseLoaded.Add(OnGameDatabaseLoaded);
	}

	public void OnDestroy()
	{
		GameEvents.OnGameDatabaseLoaded.Remove(OnGameDatabaseLoaded);
	}

	public void OnGameDatabaseLoaded()
	{
		LoadManagerDefinitions();
		LoadCometTypes();
	}

	public void OnVesselAdded(Vessel v)
	{
		if (v.vesselType == VesselType.SpaceObject && VesselUtilities.VesselHasPartName("PotatoComet", v))
		{
			comets.AddUnique(v);
		}
	}

	public void OnVesselRemoved(Vessel v)
	{
		int count = comets.Count;
		do
		{
			if (count-- <= 0)
			{
				return;
			}
		}
		while (comets[count].persistentId != v.persistentId);
		comets.RemoveAt(count);
	}

	public float GetSpawnChance()
	{
		int num = 0;
		if (ScenarioDiscoverableObjects.Instance != null)
		{
			num += ScenarioDiscoverableObjects.Instance.spawnGroupMaxLimit;
		}
		if (SentinelScenario.Instance != null)
		{
			num += SentinelScenario.Instance.MaxSentinelUntrackedSpaceObjects;
		}
		cometCap = maxCometsPerUntrackedObjects.Evaluate(num);
		currentComets = comets.Count;
		cometChance = spawnChance.Evaluate(currentComets / cometCap);
		return cometChance;
	}

	public static bool ShouldSpawnComet()
	{
		if (Instance == null)
		{
			return false;
		}
		return UnityEngine.Random.Range(0f, 1f) < Instance.GetSpawnChance();
	}

	public static CometOrbitType GenerateWeightedCometType()
	{
		if (Instance == null)
		{
			return null;
		}
		int num = 0;
		int count = Instance.cometDefinitions.Count;
		while (count-- > 0)
		{
			num += Instance.cometDefinitions[count].chanceWeight;
		}
		int num2 = UnityEngine.Random.Range(0, num);
		int count2 = Instance.cometDefinitions.Count;
		while (count2-- > 0)
		{
			int chanceWeight = Instance.cometDefinitions[count2].chanceWeight;
			if (num2 >= chanceWeight)
			{
				num2 -= chanceWeight;
				continue;
			}
			num2 = count2;
			break;
		}
		Debug.Log("[Comet Manager]:Generated Comet Type:" + Instance.cometDefinitions[num2].name);
		return Instance.cometDefinitions[num2];
	}

	public static double GenerateHomeSpawnDistance()
	{
		CelestialBody homeBody = FlightGlobals.GetHomeBody();
		if (Instance == null)
		{
			if (FlightGlobals.ready && homeBody != null)
			{
				return homeBody.orbit.semiMajorAxis * (double)UnityEngine.Random.Range(minSpawnDefault, maxSpawnDefault);
			}
			return spawnSMADefault * (double)UnityEngine.Random.Range(minSpawnDefault, maxSpawnDefault);
		}
		return homeBody.orbit.semiMajorAxis * (double)UnityEngine.Random.Range(Instance.minSpawnDistance, Instance.maxSpawnDistance);
	}

	public static double GenerateSentinelSpawnDistance(double distance)
	{
		if (Instance == null)
		{
			return distance * (double)UnityEngine.Random.Range(minSentinelSpawnDefault, maxSentinelSpawnDefault);
		}
		return distance * (double)UnityEngine.Random.Range(Instance.minSentinelSpawnDistance, Instance.maxSentinelSpawnDistance);
	}

	public static CometOrbitType GetCometOrbitType(string name)
	{
		if (Instance == null)
		{
			return null;
		}
		int num = 0;
		while (true)
		{
			if (num < Instance.cometDefinitions.Count)
			{
				if (Instance.cometDefinitions[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return Instance.cometDefinitions[num];
	}

	public void LoadManagerDefinitions()
	{
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("COMET_MANAGER");
		managerDef = null;
		if (configNodes.Length != 0)
		{
			managerDef = configNodes[0];
			if (configNodes.Length > 1)
			{
				Debug.LogWarning("[CometManager]: Found more than one COMET_MANAGER node using the first one");
			}
		}
		if (managerDef == null)
		{
			Debug.LogWarning("[CometManager]: No COMET_MANAGER node found. Using default values");
			return;
		}
		ConfigNode node = null;
		managerDef.TryGetValue("maxSpawnDistance", ref maxSpawnDistance);
		managerDef.TryGetValue("minSpawnDistance", ref minSpawnDistance);
		if (managerDef.TryGetNode("maxCometsPerUntrackedObjects", ref node) && node.values.Count > 0)
		{
			maxCometsPerUntrackedObjects = new FloatCurve();
			maxCometsPerUntrackedObjects.Load(node);
		}
		if (managerDef.TryGetNode("spawnChance", ref node) && node.values.Count > 0)
		{
			spawnChance = new FloatCurve();
			spawnChance.Load(node);
		}
		managerDef.TryGetValue("maxVFXStartDistance", ref maxVFXStartDistance);
		managerDef.TryGetValue("minVFXStartDistance", ref minVFXStartDistance);
		if (managerDef.TryGetNode("vfxStartDistribution", ref node) && node.values.Count > 0)
		{
			vfxStartDistribution = new FloatCurve();
			vfxStartDistribution.Load(node);
		}
		managerDef.TryGetValue("maxComaSizeRatio", ref maxComaSizeRatio);
		managerDef.TryGetValue("minComaSizeRatio", ref minComaSizeRatio);
		if (managerDef.TryGetNode("comaSizeRatioDistribution", ref node) && node.values.Count > 0)
		{
			comaSizeRatioDistribution = new FloatCurve();
			comaSizeRatioDistribution.Load(node);
		}
		managerDef.TryGetValue("maxTailWidthRatio", ref maxTailWidthRatio);
		managerDef.TryGetValue("maxIonTailWidthRatio", ref maxIonTailWidthRatio);
		if (managerDef.TryGetNode("tailWidthDistribution", ref node) && node.values.Count > 0)
		{
			tailWidthDistribution = new FloatCurve();
			tailWidthDistribution.Load(node);
		}
		managerDef.TryGetValue("maxTailLengthRatio", ref maxTailLengthRatio);
		managerDef.TryGetValue("minTailLengthRatio", ref minTailLengthRatio);
		if (managerDef.TryGetNode("tailLengthRatioDistribution", ref node) && node.values.Count > 0)
		{
			tailLengthRatioDistribution = new FloatCurve();
			tailLengthRatioDistribution.Load(node);
		}
		if (managerDef.TryGetNode("vfxSizeFromDistance", ref node) && node.values.Count > 0)
		{
			vfxSizeFromDistance = new FloatCurve();
			vfxSizeFromDistance.Load(node);
		}
		if (managerDef.TryGetNode("nearDustVFXSizeFromDistance", ref node) && node.values.Count > 0)
		{
			nearDustVFXSizeFromDistance = new FloatCurve();
			nearDustVFXSizeFromDistance.Load(node);
		}
		if (managerDef.TryGetNode("geyserVFXSizeFromDistance", ref node) && node.values.Count > 0)
		{
			geyserVFXSizeFromDistance = new FloatCurve();
			geyserVFXSizeFromDistance.Load(node);
		}
		managerDef.TryGetValue("minGeyserRadiusRatio", ref minGeyserRadiusRatio);
		managerDef.TryGetValue("maxGeyserRadiusRatio", ref maxGeyserRadiusRatio);
		if (managerDef.TryGetNode("geysersRatioDistribution", ref node) && node.values.Count > 0)
		{
			geysersRatioDistribution = new FloatCurve();
			geysersRatioDistribution.Load(node);
		}
		managerDef.TryGetValue("geyserVisibleDistance", ref geyserVisibleDistance);
		managerDef.TryGetValue("geyserSizeRadiusMultiplier", ref geyserSizeRadiusMultiplier);
		managerDef.TryGetValue("nearDustVisibleRadiusMultiplier", ref nearDustVisibleRadiusMultiplier);
		managerDef.TryGetValue("minNearDustEmitterRadiusRatio", ref minNearDustEmitterRadiusRatio);
		managerDef.TryGetValue("maxNearDustEmitterRadiusRatio", ref maxNearDustEmitterRadiusRatio);
		managerDef.TryGetValue("atmospherePressureForVFXFade", ref atmospherePressureForVFXFade);
		if (managerDef.TryGetNode("atmospherePressureFadeDistributionForVFXFade", ref node) && node.values.Count > 0)
		{
			atmospherePressureFadeDistributionForVFXFade = new FloatCurve();
			atmospherePressureFadeDistributionForVFXFade.Load(node);
		}
		managerDef.TryGetValue("gravityAdditionForVFXFadeStart", ref gravityAdditionForVFXFadeStart);
		managerDef.TryGetValue("gravityMultiplierForVFXFadeEnd", ref gravityMultiplierForVFXFadeEnd);
		managerDef.TryGetValue("gravitySubtractionForVFXFadeEnd", ref gravitySubtractionForVFXFadeEnd);
		managerDef.TryGetValue("vesselRangeOrbitLoadOverride", ref vesselRangeOrbitLoadOverride);
		managerDef.TryGetValue("vesselRangeOrbitUnloadOverride", ref vesselRangeOrbitUnloadOverride);
		managerDef.TryGetValue("fragmentdynamicPressurekPa", ref fragmentdynamicPressurekPa);
		managerDef.TryGetValue("minFragments", ref minFragments);
		managerDef.TryGetValue("maxFragments", ref maxFragments);
		managerDef.TryGetValue("minFragmentClassStep", ref minFragmentClassStep);
		managerDef.TryGetValue("maxFragmentClassStep", ref maxFragmentClassStep);
		managerDef.TryGetValue("fragmentInstanceOffset", ref fragmentInstanceOffset);
		managerDef.TryGetValue("fragmentAngleVariation", ref fragmentAngleVariation);
		managerDef.TryGetValue("fragmentIgnoreCollisions", ref fragmentIgnoreCollisions);
		managerDef.TryGetValue("fragmentMinPressureModifier", ref fragmentMinPressureModifier);
		managerDef.TryGetValue("fragmentMaxPressureModifier", ref fragmentMaxPressureModifier);
		managerDef.TryGetValue("fragmentMinSafeTime", ref fragmentMinSafeTime);
		managerDef.TryGetValue("fragmentMaxSafeTime", ref fragmentMaxSafeTime);
		LoadTailColorLists();
	}

	public void LoadCometTypes()
	{
		cometDefinitions.Clear();
		cometDefs = GameDatabase.Instance.GetConfigNodes("COMET_ORBIT_TYPE");
		for (int i = 0; i < cometDefs.Length; i++)
		{
			CometOrbitType cometOrbitType = new CometOrbitType();
			cometOrbitType.Load(cometDefs[i]);
			cometDefinitions.Add(cometOrbitType);
		}
	}

	public static CometDefinition GenerateDefinition(CometOrbitType cometType, UntrackedObjectClass objClass, int seed)
	{
		CometDefinition cometDefinition = new CometDefinition();
		cometDefinition.typeName = cometType.name;
		cometDefinition.cometType = cometType;
		cometDefinition.seed = seed;
		cometDefinition.radius = DiscoveryInfo.GetClassRadius(objClass, seed);
		Instance.generator = new KSPRandom(seed);
		if (Instance == null)
		{
			Debug.LogError("[CometManager]: Unable to generate definition specifics as no instance of CometManager");
			return cometDefinition;
		}
		cometDefinition.vfxStartDistance = Instance.minVFXStartDistance + (double)Instance.vfxStartDistribution.Evaluate((float)Instance.generator.NextDouble()) * (Instance.maxVFXStartDistance - Instance.minVFXStartDistance);
		cometDefinition.comaWidthRatio = Instance.minComaSizeRatio + (double)Instance.comaSizeRatioDistribution.Evaluate((float)Instance.generator.NextDouble()) * (Instance.maxComaSizeRatio - Instance.minComaSizeRatio);
		cometDefinition.tailWidthRatio = (double)Instance.tailWidthDistribution.Evaluate((float)Instance.generator.NextDouble()) * Instance.maxTailWidthRatio;
		cometDefinition.tailMaxLengthRatio = Instance.minTailLengthRatio + (double)Instance.tailLengthRatioDistribution.Evaluate((float)Instance.generator.NextDouble()) * (Instance.maxTailLengthRatio - Instance.minTailLengthRatio);
		cometDefinition.ionTailWidthRatio = (double)Instance.tailWidthDistribution.Evaluate((float)Instance.generator.NextDouble()) * Instance.maxIonTailWidthRatio;
		double num = Instance.minGeyserRadiusRatio + (double)Instance.geysersRatioDistribution.Evaluate((float)Instance.generator.NextDouble()) * (Instance.maxGeyserRadiusRatio - Instance.minGeyserRadiusRatio);
		cometDefinition.numGeysers = (int)(num * (double)cometDefinition.radius);
		cometDefinition.numGeysers = Mathf.Min(cometDefinition.numGeysers, GameSettings.COMET_MAXIMUM_GEYSERS);
		double num2 = Instance.minNearDustEmitterRadiusRatio + Instance.generator.NextDouble() * (Instance.maxNearDustEmitterRadiusRatio - Instance.minNearDustEmitterRadiusRatio);
		cometDefinition.numNearDustEmitters = (int)(num2 * (double)cometDefinition.radius);
		cometDefinition.numGeysers = Mathf.Min(cometDefinition.numGeysers, GameSettings.COMET_MAXIMUM_NEAR_DUST_EMITTERS);
		return cometDefinition;
	}

	public void SpawnCometFragments(CometVessel cometVessel)
	{
		if (cometVessel == null || cometVessel.Vessel == null)
		{
			return;
		}
		string displayName = cometVessel.Vessel.GetDisplayName();
		int num = UnityEngine.Random.Range(minFragments, maxFragments + 1);
		ProtoVessel protoVessel = null;
		for (int i = 0; i < num; i++)
		{
			string text = "-" + (char)(65 + i);
			ProtoVessel protoVessel2 = SpawnCometFragment(displayName + text, cometVessel.Vessel, cometVessel.CometType, DiscoveryInfo.GetObjectClass(cometVessel.Vessel.DiscoveryInfo.size.Value), cometVessel.radius, i == 0);
			if (i == 0)
			{
				protoVessel = protoVessel2;
			}
		}
		GameObject objectToFollow = null;
		if (protoVessel != null && protoVessel.vesselRef != null)
		{
			objectToFollow = protoVessel.vesselRef.gameObject;
		}
		FXMonger.ExplodeWithDebris(cometVessel.transform.position, (float)DiscoveryInfo.GetObjectClass(cometVessel.Vessel.DiscoveryInfo.size.Value) * explosionScaleMultiplier, objectToFollow);
	}

	public ProtoVessel SpawnCometFragment(string cometName, Vessel originalComet, CometOrbitType cometOrbitType, UntrackedObjectClass explodeCometClass, float cometRadius, bool focusAfterSpawned)
	{
		UntrackedObjectClass untrackedObjectClass = UntrackedObjectClass.const_0;
		int num = (int)explodeCometClass;
		num -= UnityEngine.Random.Range(minFragmentClassStep, maxFragmentClassStep + 1);
		if (num >= 0 && Enum.IsDefined(typeof(UntrackedObjectClass), num))
		{
			untrackedObjectClass = (UntrackedObjectClass)num;
			ModuleComet component = originalComet.GetComponent<ModuleComet>();
			float fragmentDynamicPressureModifier = 0f;
			if (component != null)
			{
				fragmentDynamicPressureModifier = component.fragmentDynamicPressureModifier + UnityEngine.Random.Range(fragmentMaxPressureModifier, fragmentMinPressureModifier);
			}
			Orbit orbit = new Orbit(originalComet.orbit.inclination, originalComet.orbit.eccentricity, originalComet.orbit.semiMajorAxis, originalComet.orbit.double_0, originalComet.orbit.argumentOfPeriapsis, originalComet.orbit.meanAnomalyAtEpoch, originalComet.orbit.epoch, originalComet.orbit.referenceBody);
			Vector3d pos = originalComet.orbit.pos;
			float num2 = cometRadius * fragmentInstanceOffset;
			pos.x += UnityEngine.Random.Range(0f - num2, num2);
			pos.y += UnityEngine.Random.Range(0f - num2, num2);
			pos.z += UnityEngine.Random.Range(0f - num2, num2);
			Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(0f - fragmentAngleVariation, fragmentAngleVariation), new Vector3d(originalComet.srf_vel_direction.x, 0.0, 0.0));
			Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f - fragmentAngleVariation, fragmentAngleVariation), new Vector3d(0.0, originalComet.srf_vel_direction.y, 0.0));
			orbit.UpdateFromStateVectors(pos, quaternion2 * quaternion * originalComet.orbit.vel, originalComet.orbit.referenceBody, Planetarium.GetUniversalTime());
			orbit.Init();
			orbit.UpdateFromUT(Planetarium.GetUniversalTime());
			int seed = UnityEngine.Random.Range(0, int.MaxValue);
			ProtoVessel protoVessel = DiscoverableObjectsUtil.SpawnComet(cometName, orbit, GenerateDefinition(cometOrbitType, untrackedObjectClass, seed), (uint)seed, untrackedObjectClass, originalComet.DiscoveryInfo.lastObservedTime, originalComet.DiscoveryInfo.referenceLifetime, optimizedCollider: true, fragmentDynamicPressureModifier);
			protoVessel.vesselRef.DiscoveryInfo.SetLevel(originalComet.DiscoveryInfo.Level);
			protoVessel.vesselRef.ignoreCollisionsFrames = fragmentIgnoreCollisions;
			protoVessel.vesselRef.externalTemperature = originalComet.externalTemperature;
			protoVessel.vesselRef.atmosphericTemperature = originalComet.atmosphericTemperature;
			protoVessel.vesselRef.vesselName = cometName;
			if (focusAfterSpawned)
			{
				Vessel cometFragmentTofocus = protoVessel.vesselRef;
				if (FlightGlobals.fetch != null && FlightGlobals.fetch.activeVessel.persistentId == originalComet.persistentId && cometFragmentTofocus.DiscoveryInfo.Level == DiscoveryLevels.Owned)
				{
					StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
					{
						if (FlightGlobals.fetch != null && cometFragmentTofocus != null)
						{
							FlightGlobals.SetActiveVessel(cometFragmentTofocus, clearDeadVessels: false);
						}
					}));
				}
			}
			return protoVessel;
		}
		return null;
	}

	public void SetCollider(bool enabled)
	{
		for (int i = 0; i < comets.Count; i++)
		{
			comets[i].Comet.SetCollider(enabled);
		}
	}

	public void SetGeyser(bool enabled, float emission)
	{
		for (int i = 0; i < comets.Count; i++)
		{
			comets[i].Comet.SetGeyser(enabled, emission);
		}
	}

	public void SetDust(bool enabled, float emission)
	{
		for (int i = 0; i < comets.Count; i++)
		{
			comets[i].Comet.SetDust(enabled, emission);
		}
	}

	public void SetComa(bool enabled)
	{
		for (int i = 0; i < comets.Count; i++)
		{
			comets[i].Comet.SetComa(enabled);
		}
	}

	public void SetIonTail(bool enabled, float emission)
	{
		for (int i = 0; i < comets.Count; i++)
		{
			comets[i].Comet.SetIonTail(enabled, emission);
		}
	}

	public void SetDustTail(bool enabled, float emission, float size)
	{
		for (int i = 0; i < comets.Count; i++)
		{
			comets[i].Comet.SetDustTail(enabled, emission, size);
		}
	}

	public void ReprimeParticles()
	{
		for (int i = 0; i < comets.Count; i++)
		{
			comets[i].Comet.ReprimeParticles();
		}
	}

	public void LoadTailColorLists()
	{
		managerDef.TryGetValue("ionTailColorP67", ref ionTailColorP67);
		if (managerDef.HasNode("ionTailColors"))
		{
			ionTailColors = LoadColorsList(managerDef.GetNode("ionTailColors"));
		}
		else
		{
			ionTailColors = new List<CometTailColor>();
			ionTailColors.Add(new CometTailColor
			{
				emitterColor = new Color(0.04313725f, 0.2906635f, 13f / 15f, 23f / 51f),
				chanceWeight = 5
			});
		}
		if (managerDef.HasNode("dustTailColors"))
		{
			dustTailColors = LoadColorsList(managerDef.GetNode("dustTailColors"));
			return;
		}
		dustTailColors = new List<CometTailColor>();
		dustTailColors.Add(new CometTailColor
		{
			emitterColor = new Color(105f / 106f, 0.9433554f, 0.7429245f, 0.03137255f),
			chanceWeight = 5
		});
	}

	public List<CometTailColor> LoadColorsList(ConfigNode colorsNode)
	{
		List<CometTailColor> list = new List<CometTailColor>();
		ConfigNode[] nodes = colorsNode.GetNodes("weightedColor");
		for (int i = 0; i < nodes.Length; i++)
		{
			CometTailColor cometTailColor = new CometTailColor();
			cometTailColor.Load(nodes[i]);
			list.Add(cometTailColor);
		}
		if (list.Count < 1)
		{
			CometTailColor item = new CometTailColor();
			list.Add(item);
		}
		return list;
	}

	public static Color GenerateWeightedIonTailColor(CometVessel comet)
	{
		if (!(Instance == null) && Instance.ionTailColors != null && Instance.ionTailColors.Count >= 1)
		{
			if (comet != null && comet.seed == 1633779195)
			{
				return Instance.ionTailColorP67;
			}
			int num = 0;
			int count = Instance.ionTailColors.Count;
			while (count-- > 0)
			{
				num += Instance.ionTailColors[count].chanceWeight;
			}
			int num2 = new KSPRandom(comet.seed).Next(0, num);
			int count2 = Instance.ionTailColors.Count;
			while (count2-- > 0)
			{
				int chanceWeight = Instance.ionTailColors[count2].chanceWeight;
				if (num2 >= chanceWeight)
				{
					num2 -= chanceWeight;
					continue;
				}
				num2 = count2;
				break;
			}
			Debug.Log($"[CometManager]: Generated Ion Tail Color: #{num2} = {Instance.ionTailColors[num2].emitterColor}");
			return Instance.ionTailColors[num2].emitterColor;
		}
		return new Color(0.04313725f, 0.2906635f, 13f / 15f, 23f / 51f);
	}

	public static Color GenerateWeightedDustTailColor(CometVessel comet)
	{
		if (Instance == null)
		{
			return new Color(105f / 106f, 0.9433554f, 0.7429245f, 0.03137255f);
		}
		int num = 0;
		int count = Instance.dustTailColors.Count;
		while (count-- > 0)
		{
			num += Instance.dustTailColors[count].chanceWeight;
		}
		int num2 = new KSPRandom(comet.seed).Next(0, num);
		int count2 = Instance.dustTailColors.Count;
		while (count2-- > 0)
		{
			int chanceWeight = Instance.dustTailColors[count2].chanceWeight;
			if (num2 >= chanceWeight)
			{
				num2 -= chanceWeight;
				continue;
			}
			num2 = count2;
			break;
		}
		Debug.Log($"[CometManager]: Generated Dust Tail Color: #{num2} = {Instance.dustTailColors[num2].emitterColor}");
		return Instance.dustTailColors[num2].emitterColor;
	}
}
