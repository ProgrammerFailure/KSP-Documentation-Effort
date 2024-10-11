using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResourceMap : MonoBehaviour
{
	private List<ResourceData> biomeConfigs;

	private List<ResourceData> planetConfigs;

	private List<ResourceData> globalConfigs;

	private Dictionary<int, List<double>> seedsCache;

	private int[] noiseSeed;

	private NoiseGenerator spx;

	private List<DepletionData> _DepletionInfo;

	private List<BiomeLockData> _BiomeLockInfo;

	private List<PlanetScanData> _PlanetScanInfo;

	public static ResourceMap Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public static bool Initialized
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<DepletionData> DepletionInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<BiomeLockData> BiomeLockInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<PlanetScanData> PlanetScanInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceMap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DepletionNode GetDepletionNode(int planetId, string resource, int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetDepletionNodeValue(int planetId, string resource, int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsBiomeUnlocked(int planetId, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsPlanetScanned(int planetId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDepletionNodeValue(int planetId, string resource, int x, int y, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnlockBiome(int planetId, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnlockPlanet(int planetId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> FetchAllResourceNames(HarvestTypes harvest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<string> DeDuplicate(List<string> input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IEnumerable<string> GetCacheNames(List<ResourceData> resList, HarvestTypes harvest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DistributionData GetBestResourceData(List<ResourceData> configs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GenerateAbundanceSeed(AbundanceRequest request, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string DetermineBiomeName(AbundanceRequest request)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAbundanceFromCache(AbundanceRequest request, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAbundance(AbundanceRequest request)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetDefaultSituation(HarvestTypes type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetDepletionNode(double latitude, double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PlanetaryResource GetResourceByName(string resName, CelestialBody body, HarvestTypes harvest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PlanetaryResource> GetResourceItemList(HarvestTypes harvest, CelestialBody body)
	{
		throw null;
	}
}
