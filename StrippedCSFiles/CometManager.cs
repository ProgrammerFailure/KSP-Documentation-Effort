using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CometManager : MonoBehaviour
{
	[Serializable]
	public class CometTailColor : IConfigNode
	{
		public Color emitterColor;

		[SerializeField]
		public int chanceWeight;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CometTailColor()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Load(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Save(ConfigNode node)
		{
			throw null;
		}
	}

	private List<Vessel> comets;

	[SerializeField]
	private float sentinelAngle;

	private ConfigNode[] cometDefs;

	private ConfigNode managerDef;

	public List<CometOrbitType> cometDefinitions;

	public float maxSpawnDistance;

	public float minSpawnDistance;

	public float maxSentinelSpawnDistance;

	public float minSentinelSpawnDistance;

	public float explosionScaleMultiplier;

	public FloatCurve maxCometsPerUntrackedObjects;

	public FloatCurve spawnChance;

	public Transform dustTailParticleFarEffect;

	public Transform ionTailParticleFarEffect;

	public Transform comaNearEffect;

	public double maxVFXStartDistance;

	public double minVFXStartDistance;

	public FloatCurve vfxStartDistribution;

	public double maxComaSizeRatio;

	public double minComaSizeRatio;

	public FloatCurve comaSizeRatioDistribution;

	public double maxTailWidthRatio;

	public double maxIonTailWidthRatio;

	public FloatCurve tailWidthDistribution;

	public double maxTailLengthRatio;

	public double minTailLengthRatio;

	public FloatCurve tailLengthRatioDistribution;

	public FloatCurve vfxSizeFromDistance;

	public FloatCurve nearDustVFXSizeFromDistance;

	public FloatCurve geyserVFXSizeFromDistance;

	[SerializeField]
	private double vFXUpdateFrequency;

	[SerializeField]
	private double vFXUpdateFrequencyInAtmosphere;

	[SerializeField]
	public float atmospherePressureForVFXFade;

	[SerializeField]
	public FloatCurve atmospherePressureFadeDistributionForVFXFade;

	[SerializeField]
	public float gravityAdditionForVFXFadeStart;

	[SerializeField]
	public float gravityMultiplierForVFXFadeEnd;

	[SerializeField]
	public float gravitySubtractionForVFXFadeEnd;

	[SerializeField]
	public double minGeyserRadiusRatio;

	[SerializeField]
	public double maxGeyserRadiusRatio;

	[SerializeField]
	public FloatCurve geysersRatioDistribution;

	[SerializeField]
	public float geyserVisibleDistance;

	public GameObject geyserEmitter;

	[SerializeField]
	public float geyserSizeRadiusMultiplier;

	[SerializeField]
	public float nearDustVisibleRadiusMultiplier;

	[SerializeField]
	public double minNearDustEmitterRadiusRatio;

	[SerializeField]
	public double maxNearDustEmitterRadiusRatio;

	public GameObject nearDustEmitter;

	[SerializeField]
	public float vesselRangeOrbitLoadOverride;

	[SerializeField]
	public float vesselRangeOrbitUnloadOverride;

	[SerializeField]
	public float vesselRangeOrbitPackOverride;

	[SerializeField]
	public float vesselRangeOrbitUnpackOverride;

	public float fragmentdynamicPressurekPa;

	public int minFragments;

	public int maxFragments;

	public int minFragmentClassStep;

	public int maxFragmentClassStep;

	public float fragmentMaxPressureModifier;

	public float fragmentMinPressureModifier;

	public float fragmentMinSafeTime;

	public float fragmentMaxSafeTime;

	public float fragmentInstanceOffset;

	public float fragmentAngleVariation;

	public int fragmentIgnoreCollisions;

	private KSPRandom generator;

	[SerializeField]
	private float currentComets;

	[SerializeField]
	private float cometCap;

	[SerializeField]
	private float cometChance;

	private static double spawnSMADefault;

	private static float minSpawnDefault;

	private static float maxSpawnDefault;

	private static float minSentinelSpawnDefault;

	private static float maxSentinelSpawnDefault;

	[Tooltip("A list of colors with weightings that will be used to pick an ion tail tint color. Higher the weight the more chance it is chosen.")]
	public List<CometTailColor> ionTailColors;

	[Tooltip("A list of colors with weightings that will be used to pick an dust tail tint color. Higher the weight the more chance it is chosen.")]
	public List<CometTailColor> dustTailColors;

	[SerializeField]
	internal Color ionTailColorP67;

	public static CometManager Instance
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

	public List<Vessel> Comets
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<Vessel> DiscoveredComets
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal static float SentinelDiscoveryArc
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double VFXUpdateFrequency
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static double VFXUpdateFrequencyInAtmosphere
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CometManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
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
	private void OnGameDatabaseLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselAdded(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselRemoved(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetSpawnChance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ShouldSpawnComet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CometOrbitType GenerateWeightedCometType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GenerateHomeSpawnDistance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GenerateSentinelSpawnDistance(double distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CometOrbitType GetCometOrbitType(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadManagerDefinitions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadCometTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CometDefinition GenerateDefinition(CometOrbitType cometType, UntrackedObjectClass objClass, int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnCometFragments(CometVessel cometVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ProtoVessel SpawnCometFragment(string cometName, Vessel originalComet, CometOrbitType cometOrbitType, UntrackedObjectClass explodeCometClass, float cometRadius, bool focusAfterSpawned)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetCollider(bool enabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetGeyser(bool enabled, float emission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetDust(bool enabled, float emission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetComa(bool enabled)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetIonTail(bool enabled, float emission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetDustTail(bool enabled, float emission, float size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ReprimeParticles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadTailColorLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<CometTailColor> LoadColorsList(ConfigNode colorsNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color GenerateWeightedIonTailColor(CometVessel comet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color GenerateWeightedDustTailColor(CometVessel comet)
	{
		throw null;
	}
}
