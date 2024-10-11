using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER
})]
public class ScenarioDiscoverableObjects : ScenarioModule
{
	[CompilerGenerated]
	private sealed class _003CSpawnDaemon_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ScenarioDiscoverableObjects _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CSpawnDaemon_003Ed__33(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public float spawnInterval;

	public float maxUntrackedLifetime;

	public float minUntrackedLifetime;

	public int spawnOddsAgainst;

	public int spawnGroupMinLimit;

	public int spawnGroupMaxLimit;

	[KSPField(isPersistant = true)]
	public FloatCurve sizeCurve;

	[KSPField(isPersistant = true)]
	public int lastSeed;

	public UntrackedObjectClass minAsteroidClass;

	public UntrackedObjectClass maxAsteroidClass;

	private int currentSize;

	private int rndGroupSize;

	private int newGroupSize;

	private bool discoveryUnlocked;

	public List<uint> untrackedObjectIDs;

	public List<uint> discoveredObjectIDs;

	private string tmpIDs;

	private string[] tmpIDList;

	private uint tmpId;

	public static ScenarioDiscoverableObjects Instance
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioDiscoverableObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadGameDBSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<uint> loadIDList(ConfigNode node, string listName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveIDList(ConfigNode node, string name, List<uint> loadList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKnowledgeChanged(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> kChg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselDestroy(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSpawnDaemon_003Ed__33))]
	private IEnumerator SpawnDaemon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSpaceObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ExpireDiscoveredObjects(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Spawn An Asteroid")]
	public void SpawnAsteroid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Spawn Last Asteroid")]
	public void SpawnLastAsteroid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnHomeAsteroid(int asteroidSeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UntrackedObjectClass GetSizeCurveBasedClass(float minRange, float maxRange)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnDresAsteroid(int asteroidSeed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Spawn A Comet")]
	public void SpawnComet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SpawnComet(string typeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SpawnComet(CometOrbitType cometType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Check Spawn Probability")]
	public void DebugSpawnProbability()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetRandomDuration()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReachedBody(string bodyName)
	{
		throw null;
	}
}
