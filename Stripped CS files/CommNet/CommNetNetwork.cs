using UnityEngine;

namespace CommNet;

public class CommNetNetwork : MonoBehaviour
{
	public CommNetwork commNet;

	[SerializeField]
	public double unpackedInterval = 5.0;

	[SerializeField]
	public double packedInterval = 0.5;

	public double prevUpdate = double.MinValue;

	public bool queueRebuild;

	[SerializeField]
	public bool graphDirty;

	public Vessel focusedVessel;

	public static CommNetNetwork Instance { get; set; }

	public virtual CommNetwork CommNet
	{
		get
		{
			return commNet;
		}
		set
		{
			commNet = value;
		}
	}

	public virtual double UnpackedInterval
	{
		get
		{
			return unpackedInterval;
		}
		set
		{
			unpackedInterval = value;
		}
	}

	public virtual double PackedInterval
	{
		get
		{
			return packedInterval;
		}
		set
		{
			packedInterval = value;
		}
	}

	public virtual bool GraphDirty => graphDirty;

	public static bool Initialized => Instance != null;

	public virtual void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.DestroyImmediate(Instance);
		}
		Instance = this;
		if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
		{
			GameEvents.onPlanetariumTargetChanged.Add(OnMapFocusChange);
		}
		GameEvents.OnGameSettingsApplied.Add(ResetNetwork);
		Reset();
	}

	public virtual void OnMapFocusChange(MapObject target)
	{
		focusedVessel = target.vessel;
	}

	public virtual void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		GameEvents.onPlanetariumTargetChanged.Remove(OnMapFocusChange);
		GameEvents.OnGameSettingsApplied.Remove(ResetNetwork);
	}

	public virtual void Update()
	{
		double num = Time.timeSinceLevelLoad;
		if (!queueRebuild && !commNet.IsDirty && !(prevUpdate + ((!(FlightGlobals.ActiveVessel != null) || FlightGlobals.ActiveVessel.packed) ? packedInterval : unpackedInterval) <= num) && prevUpdate + ((focusedVessel != null) ? packedInterval : 0.0) > num)
		{
			graphDirty = true;
			return;
		}
		commNet.Rebuild();
		prevUpdate = num;
		graphDirty = false;
		queueRebuild = false;
	}

	public virtual void QueueRebuild()
	{
		queueRebuild = true;
	}

	public static void Add(CommNode node)
	{
		if (Instance != null && Instance.commNet != null)
		{
			Instance.commNet.Add(node);
		}
	}

	public static void Remove(CommNode node)
	{
		if (Instance != null && Instance.commNet != null)
		{
			Instance.commNet.Remove(node);
		}
	}

	public static void Add(Occluder occluder)
	{
		if (Instance != null && Instance.commNet != null)
		{
			Instance.commNet.Add(occluder);
		}
	}

	public static void Remove(Occluder occluder)
	{
		if (Instance != null && Instance.commNet != null)
		{
			Instance.commNet.Remove(occluder);
		}
	}

	public void ResetNetwork()
	{
		commNet = new CommNetwork();
		GameEvents.CommNet.OnNetworkInitialized.Fire();
	}

	public static void Reset()
	{
		if (Instance != null)
		{
			Instance.ResetNetwork();
		}
	}

	[ContextMenu("Debug Info")]
	public void DebugInfo()
	{
		Debug.Log(commNet.CreateDebug());
	}
}
