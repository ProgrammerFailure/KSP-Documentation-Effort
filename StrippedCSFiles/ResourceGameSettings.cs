using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ResourceGameSettings
{
	internal static int defaultMaxDeltaTime;

	private List<DepletionData> _DepletionInfo;

	private List<BiomeLockData> _BiomeLockInfo;

	private List<PlanetScanData> _PlanetScanInfo;

	public int Seed
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

	public int MaxDeltaTime
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

	public ConfigNode SettingsNode
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

	public int ROCMissionSeed
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
	public ResourceGameSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ResourceGameSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GenerateNewSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GenerateNewROCMissionSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<DepletionData> SetupDepletionInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<BiomeLockData> SetupBiomeLockInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<PlanetScanData> SetupPlanetScanInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<DepletionData> GetDepletionInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<BiomeLockData> GetBiomeLockInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PlanetScanData> GetPlanetScanInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetValue(ConfigNode config, string name, int currentValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetValue(ConfigNode config, string name, float currentValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveDepletionNodeValue(int planetId, string resource, int x, int y, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveBiomeUnlockNode(int planetId, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SavePlanetUnlockNode(int planetId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DepletionNode CheckDepletionNode(int planetId, string resource, int x, int y)
	{
		throw null;
	}
}
