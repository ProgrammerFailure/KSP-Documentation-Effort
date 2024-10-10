using System;
using System.Globalization;
using KSPTrackIR;
using UnityEngine;

public class TrackIR : MonoBehaviour, IConfigNode
{
	[Serializable]
	public class AxisProperties : IConfigNode
	{
		public float upperClamp = 10f;

		public float lowerClamp = -10f;

		public float factor = 0.0001f;

		public float raw;

		public float axis;

		public float GetAxis(float rawInput)
		{
			rawInput *= factor;
			rawInput = Mathf.Clamp(rawInput, lowerClamp, upperClamp);
			axis = rawInput;
			return rawInput;
		}

		public float GetAxis()
		{
			return axis;
		}

		public void Load(ConfigNode node)
		{
			if (node.HasValue("upperClamp"))
			{
				upperClamp = float.Parse(node.GetValue("upperClamp"), CultureInfo.InvariantCulture);
			}
			if (node.HasValue("lowerClamp"))
			{
				lowerClamp = float.Parse(node.GetValue("lowerClamp"), CultureInfo.InvariantCulture);
			}
			if (node.HasValue("factor"))
			{
				factor = float.Parse(node.GetValue("factor"), CultureInfo.InvariantCulture);
			}
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("upperClamp", upperClamp);
			node.AddValue("lowerClamp", lowerClamp);
			node.AddValue("factor", factor);
		}
	}

	public class Settings : IConfigNode
	{
		public TrackIR instance;

		public ConfigNode node;

		public ConfigNode Node => node;

		public TrackIR Instance
		{
			get
			{
				if (FindInstance())
				{
					return instance;
				}
				return null;
			}
		}

		public bool FindInstance()
		{
			instance = TrackIR.Instance;
			return instance != null;
		}

		public void Load(ConfigNode node)
		{
			this.node = node;
			if (FindInstance())
			{
				instance.Load(node);
			}
		}

		public void Save(ConfigNode node)
		{
			if (FindInstance())
			{
				instance.Save(node);
			}
			else if (this.node != null)
			{
				if (this.node.name.Equals("TRACKIR"))
				{
					this.node.CopyTo(node);
				}
				else if (this.node.HasNode("TRACKIR"))
				{
					node.SetNode("TRACKIR", this.node.GetNode("TRACKIR"), createIfNotFound: true);
				}
			}
		}
	}

	public static TrackIR Instance;

	public bool available;

	public bool running;

	public Vector3 headPos;

	public Vector3 headAngles;

	public Quaternion headRot;

	public AxisProperties LinX;

	public AxisProperties LinY;

	public AxisProperties LinZ;

	public AxisProperties Pitch;

	public AxisProperties Yaw;

	public AxisProperties Roll;

	public bool activeFlight = true;

	public bool activeIVA = true;

	public bool activeEVA = true;

	public bool activeMap = true;

	public bool activeKSC = true;

	public bool activeTrackingStation = true;

	public bool activeEditors = true;

	public string dllPath = "";

	public bool verbose = true;

	public Client client;

	public Client.TrackIRData trackIRdata;

	public bool Available => available;

	public bool Running => running;

	public Vector3 HeadPosition => headPos;

	public Vector3 HeadAngles => headAngles;

	public Quaternion HeadRotation => headRot;

	[ContextMenu("Initialize")]
	public bool Init()
	{
		if (running)
		{
			return false;
		}
		try
		{
			client = new Client(dllPath, verbose);
			available = client.Init();
		}
		catch (Exception ex)
		{
			Debug.Log($"[KSPTrackIR]: {ex.Message}");
			available = false;
		}
		running = available;
		return running;
	}

	public void TrackIRUpdate()
	{
		if (running)
		{
			if (client != null && available)
			{
				trackIRdata = client.GetTrackIRData();
				headPos.x = LinX.GetAxis(0f - trackIRdata.x);
				headPos.y = LinY.GetAxis(trackIRdata.y);
				headPos.z = LinZ.GetAxis(0f - trackIRdata.z);
				headAngles.y = Yaw.GetAxis(0f - trackIRdata.yaw) * 57.29578f;
				headAngles.x = Pitch.GetAxis(trackIRdata.pitch) * 57.29578f;
				headAngles.z = Roll.GetAxis(trackIRdata.roll) * 57.29578f;
				headRot = Quaternion.Euler(headAngles);
			}
			else
			{
				Debug.LogError("[KSPTrackIR]: Client Unavailable! Cannot update!");
				running = false;
				available = false;
			}
		}
	}

	[ContextMenu("Shutdown")]
	public void Shutdown()
	{
		if (client != null && running)
		{
			running = false;
			client.Shutdown();
		}
	}

	public void OnSettingsApplied()
	{
		if (GameSettings.TRACKIR_ENABLED)
		{
			Init();
		}
		else
		{
			Shutdown();
		}
	}

	public void Awake()
	{
		Instance = this;
		if (GameSettings.TRACKIR.Node != null)
		{
			Load(GameSettings.TRACKIR.Node);
		}
		GameEvents.onLevelWasLoaded.Add(OnSceneLoaded);
		GameEvents.OnGameSettingsApplied.Add(OnSettingsApplied);
	}

	public void OnSceneLoaded(GameScenes scn)
	{
		if (GameSettings.TRACKIR_ENABLED)
		{
			Init();
		}
	}

	public void Update()
	{
		if (running)
		{
			TrackIRUpdate();
		}
	}

	public void OnDestroy()
	{
		if (running)
		{
			Shutdown();
		}
		GameEvents.onLevelWasLoaded.Remove(OnSceneLoaded);
		GameEvents.OnGameSettingsApplied.Remove(OnSettingsApplied);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("activeFlight"))
		{
			activeFlight = bool.Parse(node.GetValue("activeFlight"));
		}
		if (node.HasValue("activeIVA"))
		{
			activeIVA = bool.Parse(node.GetValue("activeIVA"));
		}
		if (node.HasValue("activeEVA"))
		{
			activeEVA = bool.Parse(node.GetValue("activeEVA"));
		}
		if (node.HasValue("activeMap"))
		{
			activeMap = bool.Parse(node.GetValue("activeMap"));
		}
		if (node.HasValue("activeKSC"))
		{
			activeKSC = bool.Parse(node.GetValue("activeKSC"));
		}
		if (node.HasValue("activeTrackingStation"))
		{
			activeTrackingStation = bool.Parse(node.GetValue("activeTrackingStation"));
		}
		if (node.HasValue("activeEditors"))
		{
			activeEditors = bool.Parse(node.GetValue("activeEditors"));
		}
		if (node.HasNode("LinX"))
		{
			LinX.Load(node.GetNode("LinX"));
		}
		if (node.HasNode("LinY"))
		{
			LinY.Load(node.GetNode("LinY"));
		}
		if (node.HasNode("LinZ"))
		{
			LinZ.Load(node.GetNode("LinZ"));
		}
		if (node.HasNode("Pitch"))
		{
			Pitch.Load(node.GetNode("Pitch"));
		}
		if (node.HasNode("Yaw"))
		{
			Yaw.Load(node.GetNode("Yaw"));
		}
		if (node.HasNode("Roll"))
		{
			Roll.Load(node.GetNode("Roll"));
		}
		node.TryGetValue("dllPath", ref dllPath);
		node.TryGetValue("verbose", ref verbose);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("activeFlight", activeFlight);
		node.AddValue("activeIVA", activeIVA);
		node.AddValue("activeEVA", activeEVA);
		node.AddValue("activeMap", activeMap);
		node.AddValue("activeKSC", activeKSC);
		node.AddValue("activeTrackingStation", activeTrackingStation);
		node.AddValue("activeEditors", activeEditors);
		LinX.Save(node.AddNode("LinX"));
		LinY.Save(node.AddNode("LinY"));
		LinZ.Save(node.AddNode("LinZ"));
		Pitch.Save(node.AddNode("Pitch"));
		Yaw.Save(node.AddNode("Yaw"));
		Roll.Save(node.AddNode("Roll"));
		node.AddValue("dllPath", dllPath);
		node.AddValue("verbose", verbose);
	}
}
