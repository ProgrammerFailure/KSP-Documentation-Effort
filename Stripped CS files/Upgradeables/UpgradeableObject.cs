using System;
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

		public UpgradeableObject host;

		public Vector3 p0;

		public Vector3 s0;

		public Quaternion r0;

		public bool Spawned => facilityInstance != null;

		public void Setup(UpgradeableObject host)
		{
			this.host = host;
			p0 = host.facilityTransform.localPosition;
			r0 = host.facilityTransform.localRotation;
			s0 = host.facilityTransform.localScale;
		}

		public void Spawn()
		{
			if (facilityPrefab != null)
			{
				facilityInstance = UnityEngine.Object.Instantiate(facilityPrefab);
				facilityInstance.transform.parent = host.facilityTransform.parent;
				facilityInstance.transform.localPosition = p0;
				facilityInstance.transform.localRotation = r0;
				facilityInstance.transform.localScale = s0;
				facilityInstance.name = host.facilityTransform.name;
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(host.facilityTransform.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(host.facilityTransform.gameObject);
				}
				host.facilityTransform = facilityInstance.transform;
			}
		}

		public void Despawn()
		{
			GameObject gameObject = new GameObject(facilityInstance.name);
			gameObject.transform.parent = host.facilityTransform.parent;
			gameObject.transform.localPosition = p0;
			gameObject.transform.localRotation = r0;
			gameObject.transform.localScale = s0;
			host.facilityTransform = gameObject.transform;
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(facilityInstance);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(facilityInstance);
			}
			facilityInstance = null;
		}
	}

	public string id;

	[SerializeField]
	public bool preCompiledId;

	[SerializeField]
	public UpgradeLevel[] upgradeLevels;

	[SerializeField]
	public Transform facilityTransform;

	public int facilityLevel;

	public UpgradeLevel currentLevel;

	public bool setup;

	public UpgradeLevel[] UpgradeLevels
	{
		get
		{
			return upgradeLevels;
		}
		set
		{
			upgradeLevels = value;
		}
	}

	public Transform FacilityTransform
	{
		get
		{
			return facilityTransform;
		}
		set
		{
			facilityTransform = value;
		}
	}

	public int FacilityLevel => facilityLevel;

	public int MaxLevel => upgradeLevels.Length - 1;

	public UpgradeLevel CurrentLevel
	{
		get
		{
			return currentLevel;
		}
		set
		{
			currentLevel = value;
		}
	}

	public UpgradeableObject()
	{
	}

	[ContextMenu("Compile ID")]
	public void CompileID()
	{
		id = HierarchyUtil.CompileID(base.transform, "SpaceCenter");
		preCompiledId = true;
	}

	[ContextMenu("Clear ID")]
	public void ClearID()
	{
		id = "";
		preCompiledId = false;
	}

	[ContextMenu("Compile Assets")]
	public void CompileAssets()
	{
		UpgradeLevel[] array = upgradeLevels;
		foreach (UpgradeLevel obj in array)
		{
			obj.levelStats = KSCFacilityLevelText.CreateFromAsset(obj.levelText);
		}
	}

	public void SetupLevels()
	{
		int num = upgradeLevels.Length;
		while (num-- > 0)
		{
			upgradeLevels[num].Setup(this);
		}
		setup = true;
	}

	public void setLevel(int lvl)
	{
		if (!setup)
		{
			SetupLevels();
		}
		lvl = Mathf.Clamp(lvl, 0, MaxLevel);
		if (!upgradeLevels[lvl].Spawned)
		{
			Despawn(facilityLevel);
			facilityLevel = lvl;
			currentLevel = upgradeLevels[facilityLevel];
			currentLevel.Spawn();
			GameEvents.OnUpgradeableObjLevelChange.Fire(this, lvl);
		}
	}

	public void Despawn(int lvl)
	{
		lvl = Mathf.Clamp(lvl, 0, MaxLevel);
		Despawn(upgradeLevels[lvl]);
	}

	public void Despawn(UpgradeLevel lvl)
	{
		if (lvl.Spawned)
		{
			lvl.Despawn();
		}
	}

	public void Awake()
	{
		SetupLevels();
		OnAwake();
	}

	public void Start()
	{
		OnStart();
		setLevel(facilityLevel);
		GameEvents.onGameSceneSwitchRequested.Add(OnGameSceneSwitchRequested);
	}

	public void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> action)
	{
		GameScenes to = action.to;
		if ((HighLogic.LoadedScene != GameScenes.SPACECENTER || to != GameScenes.FLIGHT) && (HighLogic.LoadedScene != GameScenes.FLIGHT || to != GameScenes.SPACECENTER))
		{
			Despawn(facilityLevel);
		}
		if ((to == GameScenes.SPACECENTER || to == GameScenes.FLIGHT) && !upgradeLevels[facilityLevel].Spawned)
		{
			upgradeLevels[facilityLevel].Spawn();
			GameEvents.OnUpgradeableObjLevelChange.Fire(this, facilityLevel);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneSwitchRequested.Remove(OnGameSceneSwitchRequested);
		OnOnDestroy();
	}

	public abstract void OnAwake();

	public abstract void OnStart();

	public abstract void OnOnDestroy();

	public virtual void SetLevel(int lvl)
	{
		setLevel(lvl);
	}
}
