using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResourceCache : MonoBehaviour
{
	public class AbundanceSummary
	{
		public int BodyId
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public string BiomeName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public string ResourceName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public float Abundance
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public HarvestTypes HarvestType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AbundanceSummary()
		{
			throw null;
		}
	}

	private static ResourceCache instance;

	private static List<AbundanceSummary> _abundanceCache;

	private static List<ResourceData> _globalResources;

	private static List<ResourceData> _planetaryResources;

	private static List<ResourceData> _biomeResources;

	public static ResourceCache Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<AbundanceSummary> AbundanceCache
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ResourceData> BiomeResources
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ResourceData> GlobalResources
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ResourceData> PlanetaryResources
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceCache()
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
	private AbundanceSummary RequestSummary(CelestialBody body, string biome, string resourceName, HarvestTypes type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<AbundanceSummary> LoadAbundanceCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<ResourceData> LoadResourceInfo(string node)
	{
		throw null;
	}
}
