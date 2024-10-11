using System.Collections.Generic;
using System.Runtime.CompilerServices;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.FLIGHT,
	GameScenes.EDITOR,
	GameScenes.TRACKSTATION
})]
public class ScenarioDestructibles : ScenarioModule
{
	public class ProtoDestructible
	{
		public ConfigNode configNode;

		public List<DestructibleBuilding> dBuildingRefs;

		private bool isIntact;

		private float FacilityDamageFraction;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ProtoDestructible(DestructibleBuilding dBuilding)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ProtoDestructible(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddRef(DestructibleBuilding dBuilding)
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
		public float GetDamage()
		{
			throw null;
		}
	}

	public static ScenarioDestructibles Instance;

	public static Dictionary<string, ProtoDestructible> protoDestructibles;

	public static Dictionary<string, List<ProtoDestructible>> facilityToDestructibles;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioDestructibles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ScenarioDestructibles()
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
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetFacility(string dName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AddTo(Dictionary<string, List<ProtoDestructible>> dict, string key, ProtoDestructible pD)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveFrom(Dictionary<string, List<ProtoDestructible>> dict, string key, ProtoDestructible pD)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool RegisterDestructible(DestructibleBuilding dBuilding, string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UnregisterDestructible(DestructibleBuilding dBuilding, string id, bool saveState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetFacilityDamage(string facilityName)
	{
		throw null;
	}
}
