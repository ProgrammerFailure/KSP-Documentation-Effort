using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Upgradeables;

[KSPScenario((ScenarioCreationOptions)1056, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.EDITOR,
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION
})]
public class ScenarioUpgradeableFacilities : ScenarioModule
{
	public class ProtoUpgradeable
	{
		public ConfigNode configNode;

		public List<UpgradeableFacility> facilityRefs;

		private float level;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ProtoUpgradeable(UpgradeableFacility facility)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ProtoUpgradeable(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddRef(UpgradeableFacility facility)
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetLevel()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetLevelCount()
		{
			throw null;
		}
	}

	public static ScenarioUpgradeableFacilities Instance;

	public static Dictionary<string, ProtoUpgradeable> protoUpgradeables;

	private static Dictionary<string, string> slashSanitizedStrings;

	private static Dictionary<SpaceCenterFacility, string> facilityStrings;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioUpgradeableFacilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ScenarioUpgradeableFacilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CleanUpUnmanagedData(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool RegisterUpgradeable(UpgradeableFacility facilityRef, string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UnregisterUpgradeable(UpgradeableFacility facilityRef, string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetFacilityName(SpaceCenterFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetFacilityLevel(SpaceCenterFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetFacilityLevel(string facilityId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SlashSanitize(string instr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetFacilityLevelCount(SpaceCenterFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetFacilityLevelCount(string facilityId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsRunway(string launchLocation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsLaunchPad(string launchLocation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode getInitialState(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode getInitialMissionState(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheatFacilities()
	{
		throw null;
	}
}
