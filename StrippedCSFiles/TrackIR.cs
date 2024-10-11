using System;
using System.Runtime.CompilerServices;
using KSPTrackIR;
using UnityEngine;

public class TrackIR : MonoBehaviour, IConfigNode
{
	[Serializable]
	public class AxisProperties : IConfigNode
	{
		public float upperClamp;

		public float lowerClamp;

		public float factor;

		private float raw;

		private float axis;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AxisProperties()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetAxis(float rawInput)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetAxis()
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
	}

	public class Settings : IConfigNode
	{
		private TrackIR instance;

		private ConfigNode node;

		public ConfigNode Node
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public TrackIR Instance
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool FindInstance()
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
	}

	public static TrackIR Instance;

	private bool available;

	private bool running;

	private Vector3 headPos;

	private Vector3 headAngles;

	private Quaternion headRot;

	public AxisProperties LinX;

	public AxisProperties LinY;

	public AxisProperties LinZ;

	public AxisProperties Pitch;

	public AxisProperties Yaw;

	public AxisProperties Roll;

	public bool activeFlight;

	public bool activeIVA;

	public bool activeEVA;

	public bool activeMap;

	public bool activeKSC;

	public bool activeTrackingStation;

	public bool activeEditors;

	public string dllPath;

	public bool verbose;

	private Client client;

	private Client.TrackIRData trackIRdata;

	public bool Available
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Running
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 HeadPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 HeadAngles
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Quaternion HeadRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TrackIR()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Initialize")]
	public bool Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TrackIRUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Shutdown")]
	public void Shutdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
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
}
