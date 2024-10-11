using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Upgradeables;

public abstract class UpgradeableObject : MonoBehaviour
{
	[Serializable]
	public class UpgradeLevel
	{
		public float levelCost;

		public KSCUpgradeableLevelText levelText;

		public KSCFacilityLevelText levelStats;

		public GameObject facilityPrefab;

		public GameObject facilityInstance;

		private UpgradeableObject host;

		private Vector3 p0;

		private Vector3 s0;

		private Quaternion r0;

		public bool Spawned
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UpgradeLevel()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(UpgradeableObject host)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Spawn()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Despawn()
		{
			throw null;
		}
	}

	public string id;

	[SerializeField]
	protected bool preCompiledId;

	[SerializeField]
	protected UpgradeLevel[] upgradeLevels;

	[SerializeField]
	protected Transform facilityTransform;

	protected int facilityLevel;

	protected UpgradeLevel currentLevel;

	protected bool setup;

	public UpgradeLevel[] UpgradeLevels
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Transform FacilityTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public int FacilityLevel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int MaxLevel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UpgradeLevel CurrentLevel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected UpgradeableObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Compile ID")]
	public void CompileID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Clear ID")]
	public void ClearID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Compile Assets")]
	public void CompileAssets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupLevels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setLevel(int lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Despawn(int lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Despawn(UpgradeLevel lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	protected abstract void OnAwake();

	protected abstract void OnStart();

	protected abstract void OnOnDestroy();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetLevel(int lvl)
	{
		throw null;
	}
}
