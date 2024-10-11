using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ROCStats(string rocType)
		{
			throw null;
		}
	}

	private ConfigNode[] rocDefs;

	private ConfigNode[] cbDefs;

	private ConfigNode[] vfxForceDefs;

	[SerializeField]
	private DictionaryValueList<string, GameObject> rocTypeObjects;

	[SerializeField]
	private List<ROCDefinition> _rocDefinitions;

	[SerializeField]
	private List<PQSROCControl> _pqsRocControls;

	[SerializeField]
	private List<int> removedROCs;

	[SerializeField]
	private bool rocsEnabledInCurrentGame;

	internal bool debugROCFinder;

	internal bool debugROCScanPoints;

	internal bool debugROCStats;

	public DictionaryValueList<string, ROCStats> rocStats;

	internal Callback OnStatsChanged;

	public static ROCManager Instance
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

	public DictionaryValueList<string, GameObject> RocTypeObjects
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<ROCDefinition> rocDefinitions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<PQSROCControl> pqsRocControls
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool RocsEnabledInCurrentGame
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ROCManager()
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
	private void OnGameStateLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameNewStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionStarted(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetROCsStateForGame(bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetROCControlFromCB()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PopulateROCControlFromConfig(PQSROCControl ROCControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ValidateCBBiomeCombos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private CelestialBody ValidCelestialBody(string bodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ValidCBBiome(CelestialBody body, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableAllROCs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableAllROCs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveRemoveROCs(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadRemovedROCs(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ROCRemoved(int rocId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveROC(int rocId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearROCsCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetTerrainTag(GameObject obj, out GameObject terrainObj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddROCStats_Quad(PQSROCControl control, ROCDefinition r, PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddROCStats_ROCs(string rType, int count, float frequency)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ClearStats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SubtractROCStats_Quad(PQSROCControl control, ROCDefinition r, PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SubtractROCStats_ROCs(string rType, int count)
	{
		throw null;
	}
}
