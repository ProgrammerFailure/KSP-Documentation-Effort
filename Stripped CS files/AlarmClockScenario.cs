using System;
using System.Collections.Generic;
using ns11;
using ns2;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.EDITOR,
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION
})]
public class AlarmClockScenario : ScenarioModule
{
	public class SpriteMap
	{
		public readonly IDictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

		public Sprite this[string key]
		{
			get
			{
				if (_sprites.ContainsKey(key))
				{
					return _sprites[key];
				}
				Texture2D texture2D = LoadTexture(key);
				_sprites[key] = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
				return _sprites[key];
			}
		}
	}

	public static AlarmClockApp appInstance;

	public DictionaryValueList<uint, AlarmTypeBase> alarms;

	public SpriteMap alarmSprites;

	public const string textureBaseUrl = "Squad/Alarms/Icons/";

	public DictionaryValueList<Type, AlarmTypeBase> alarmTypeList;

	public List<Type> alarmTypes;

	public AlarmClockScenarioAudio audioController;

	public AlarmClockMessageDialog messagePrefab;

	public float warpChangeTimeSafteyMultiplier = 1.2f;

	public float warpChangeIndicatorDuration = 3f;

	public bool warpAffectIndicator;

	public DateTime warpAffectStartTime;

	public DictionaryValueList<MapObject.ObjectType, Type> mapNodeAddAlarmTypes;

	public AlarmClockSettings settings;

	public double currentUT;

	public double lastUT;

	public double uiUpdatePeriod;

	public int availableVesselChangeCooldown = 5;

	public int availableVesselChangeCooldownCounter;

	public Vessel availableVessel;

	public uint lastAvailableVesselId;

	public static AlarmClockScenario Instance { get; set; }

	public static AlarmClockApp AppInstance => appInstance;

	public static SpriteMap AlarmSprites
	{
		get
		{
			if (Instance == null)
			{
				return null;
			}
			return Instance.alarmSprites;
		}
	}

	public static List<Type> AlarmTypes
	{
		get
		{
			if (Instance == null)
			{
				return null;
			}
			return Instance.alarmTypes;
		}
	}

	public static AlarmClockScenarioAudio AudioController
	{
		get
		{
			if (Instance == null)
			{
				return null;
			}
			return Instance.audioController;
		}
	}

	public static AlarmClockMessageDialog MessagePrefab
	{
		get
		{
			if (Instance != null)
			{
				return Instance.messagePrefab;
			}
			return null;
		}
	}

	public bool WarpAffectIndicator
	{
		get
		{
			return warpAffectIndicator;
		}
		set
		{
			warpAffectIndicator = value;
		}
	}

	public static double UIUpdatePeriod
	{
		get
		{
			if (!(Instance == null))
			{
				return Instance.uiUpdatePeriod;
			}
			return 0.1;
		}
	}

	public bool IsAvailableVesselCooldownActive => availableVesselChangeCooldownCounter > 0;

	public static bool IsVesselAvailable
	{
		get
		{
			if (Instance == null)
			{
				return false;
			}
			return Instance.availableVessel != null;
		}
	}

	public static Vessel AvailableVessel
	{
		get
		{
			if (IsVesselAvailable)
			{
				return Instance.availableVessel;
			}
			return null;
		}
	}

	public uint AvailableVesselId
	{
		get
		{
			if (!(availableVessel == null))
			{
				return availableVessel.persistentId;
			}
			return 0u;
		}
	}

	public override void OnAwake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("[AlarmClockScenario]: Instance already exists!", Instance.gameObject);
			UnityEngine.Object.Destroy(Instance);
		}
		Instance = this;
		alarms = new DictionaryValueList<uint, AlarmTypeBase>();
		alarmSprites = new SpriteMap();
		mapNodeAddAlarmTypes = new DictionaryValueList<MapObject.ObjectType, Type>();
		messagePrefab = AssetBase.GetPrefab<AlarmClockMessageDialog>("AlarmClockMessageDialog");
		LoadTypes();
		if (audioController == null)
		{
			audioController = base.gameObject.AddComponent<AlarmClockScenarioAudio>();
		}
		WarpTransitionCalculator.CalcWarpRateTransitions();
		GameEvents.onManeuversLoaded.Add(OnManeuversLoaded);
	}

	public void OnDestroy()
	{
		GameEvents.onManeuversLoaded.Remove(OnManeuversLoaded);
	}

	public void OnManeuversLoaded(Vessel v, PatchedConicSolver s)
	{
		if (alarms != null)
		{
			for (int i = 0; i < alarms.Count; i++)
			{
				alarms.ValuesList[i].OnManeuversLoaded(v, s);
			}
		}
	}

	public void Update()
	{
		if (Instance == null)
		{
			return;
		}
		if (FlightGlobals.ActiveVessel != null)
		{
			availableVessel = FlightGlobals.ActiveVessel;
		}
		else if (SpaceTracking.Instance != null)
		{
			availableVessel = SpaceTracking.Instance.SelectedVessel;
		}
		if (AvailableVesselId != lastAvailableVesselId)
		{
			lastAvailableVesselId = AvailableVesselId;
			GameEvents.onAlarmAvailableVesselChanged.Fire(availableVessel);
			availableVesselChangeCooldownCounter = availableVesselChangeCooldown;
		}
		if (warpAffectIndicator && warpAffectStartTime.AddSeconds(warpChangeIndicatorDuration) < KSPUtil.SystemDateTime.DateTimeNow())
		{
			warpAffectIndicator = false;
		}
		bool flag = false;
		double num = double.MinValue;
		currentUT = Planetarium.GetUniversalTime();
		double num2 = 1.0;
		if (TimeWarp.fetch != null)
		{
			num2 = TimeWarp.CurrentRate;
		}
		double utSecondsTillNextUpdate = num2;
		for (int i = 0; i < alarms.Count; i++)
		{
			AlarmTypeBase alarmTypeBase = alarms.ValuesList[i];
			if (alarmTypeBase.ut < num)
			{
				flag = true;
			}
			num = alarmTypeBase.ut;
			alarmTypeBase.ScenarioUpdate(IsAvailableVesselCooldownActive);
			if (alarmTypeBase.TimeToAlarm <= 0.0 && !alarmTypeBase.Triggered)
			{
				alarmTypeBase.TriggerAlarm();
			}
			if (alarmTypeBase.Triggered)
			{
				_ = alarmTypeBase.Actioned;
			}
			if (!alarmTypeBase.Triggered && (alarmTypeBase.HaltWarp || alarmTypeBase.PauseGame))
			{
				HandleWarpActions(alarmTypeBase, currentUT, utSecondsTillNextUpdate);
			}
		}
		if (flag)
		{
			SortAlarmsByUTDescending();
		}
		if (availableVesselChangeCooldownCounter > 0)
		{
			availableVesselChangeCooldownCounter--;
		}
		WarpTransitionCalculator.CheckForTransitionChanges();
	}

	public static AlarmTypeBase GetNextAlarm()
	{
		return GetNextAlarm(Planetarium.GetUniversalTime());
	}

	public static AlarmTypeBase GetNextOrLastAlarm()
	{
		AlarmTypeBase alarmTypeBase = GetNextAlarm(Planetarium.GetUniversalTime());
		if (alarmTypeBase == null)
		{
			alarmTypeBase = GetLastAlarm(Planetarium.GetUniversalTime());
		}
		return alarmTypeBase;
	}

	public static AlarmTypeBase GetNextAlarm(double afterUT)
	{
		if (Instance == null)
		{
			return null;
		}
		int num = 0;
		while (true)
		{
			if (num < Instance.alarms.Count)
			{
				if (!(Instance.alarms.ValuesList[num].ut <= afterUT))
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return Instance.alarms.ValuesList[num];
	}

	public static AlarmTypeBase GetLastAlarm(double afterUT)
	{
		if (Instance == null)
		{
			return null;
		}
		int count = Instance.alarms.Count;
		do
		{
			if (count-- <= 0)
			{
				return null;
			}
		}
		while (Instance.alarms.ValuesList[count].ut >= afterUT);
		return Instance.alarms.ValuesList[count];
	}

	public static AlarmTypeBase CreateAlarmByType(Type alarmType)
	{
		if (Instance == null)
		{
			return null;
		}
		AlarmTypeBase alarmTypeBase = (AlarmTypeBase)Activator.CreateInstance(alarmType);
		if (alarmTypeBase == null)
		{
			Debug.LogError("[AlarmClockScenario]: Unabled to Initialize alarm type: " + alarmType.FullName);
			return null;
		}
		alarmTypeBase.title = alarmTypeBase.GetDefaultTitle();
		return alarmTypeBase;
	}

	public static uint GetUniqueAlarmID()
	{
		if (Instance == null)
		{
			Debug.LogWarning("[AlarmClockScenario]: Generating ID, but the scenario is not running at this time - no unique checks");
		}
		uint hashCode;
		do
		{
			hashCode = (uint)Guid.NewGuid().GetHashCode();
		}
		while (Instance != null && Instance.alarms.ContainsKey(hashCode) && hashCode != 0);
		return hashCode;
	}

	public static bool AddAlarm(AlarmTypeBase newAlarm)
	{
		if (Instance == null)
		{
			Debug.LogWarning($"[AlarmClockScenario]: Scenario is not running at this time - unable to add alarm id={newAlarm.Id}");
			return false;
		}
		if (Instance.alarms.ContainsKey(newAlarm.Id))
		{
			Debug.LogError($"[AlarmClockScenario]: Unable to add alarm id={newAlarm.Id} , id already exists");
			return false;
		}
		newAlarm.UpdateTimeToAlarm();
		Instance.alarms.Add(newAlarm.Id, newAlarm);
		Instance.alarms.SortList(Instance.OrderByUT);
		GameEvents.onAlarmAdded.Fire(newAlarm);
		return true;
	}

	public static bool AlarmExists(uint id)
	{
		if (Instance == null)
		{
			Debug.LogWarning($"[AlarmClockScenario]: Scenario is not running at this time - unable to search for alarm id={id}");
			return false;
		}
		return Instance.alarms.ContainsKey(id);
	}

	public static bool TryGetAlarm(uint id, out AlarmTypeBase alarm)
	{
		if (Instance == null)
		{
			Debug.LogWarning($"[AlarmClockScenario]: Scenario is not running at this time - unable to search for alarm id={id}");
			alarm = null;
			return false;
		}
		return Instance.alarms.TryGetValue(id, out alarm);
	}

	public static bool DeleteAlarm(AlarmTypeBase alarm)
	{
		return DeleteAlarm(alarm.Id);
	}

	public static bool DeleteAlarm(uint id)
	{
		if (Instance == null)
		{
			Debug.LogWarning($"[AlarmClockScenario]: Scenario is not running at this time - unable to delete alarm id={id}");
			return false;
		}
		if (!Instance.alarms.ContainsKey(id))
		{
			Debug.LogError($"[AlarmClockScenario]: Unable to delete alarm id={id} , id does not exist");
			return false;
		}
		GameEvents.onAlarmRemoving.Fire(Instance.alarms[id]);
		Instance.alarms.Remove(id);
		Instance.alarms.SortList(Instance.OrderByUT);
		GameEvents.onAlarmRemoved.Fire(id);
		return true;
	}

	public static bool PlaySound(string soundURL, int repeats = 1)
	{
		if (Instance == null)
		{
			Debug.LogWarning("[AlarmClockScenario]: Scenario is not running at this time - unable to play sound.");
			return false;
		}
		if (string.IsNullOrWhiteSpace(soundURL))
		{
			soundURL = Instance.settings.soundName;
		}
		if (repeats < 1)
		{
			repeats = Instance.settings.soundRepeats;
		}
		return Instance.audioController.PlaySound(soundURL, repeats);
	}

	public static bool MapNodeDefined(MapObject.ObjectType nodeType)
	{
		if (Instance == null)
		{
			return false;
		}
		return Instance.mapNodeAddAlarmTypes.Contains(nodeType);
	}

	public static AlarmTypeBase CreateAlarmByMapNodeType(MapObject.ObjectType nodeType)
	{
		if (!(Instance == null) && MapNodeDefined(nodeType))
		{
			return CreateAlarmByType(Instance.mapNodeAddAlarmTypes[nodeType]);
		}
		return null;
	}

	public static bool ShowAlarmMapButton(MapObject mapObject)
	{
		if (MapNodeDefined(mapObject.type))
		{
			return Instance.alarmTypeList[Instance.mapNodeAddAlarmTypes[mapObject.type]].ShowAlarmMapObject(mapObject);
		}
		return false;
	}

	public void LoadTypes()
	{
		alarmTypes = AssemblyLoader.GetSubclassesOfParentClass(typeof(AlarmTypeBase));
		int count = alarmTypes.Count;
		while (count-- > 0)
		{
			if (alarmTypes[count].IsAbstract)
			{
				alarmTypes.RemoveAt(count);
			}
		}
		alarmTypeList = new DictionaryValueList<Type, AlarmTypeBase>();
		for (int i = 0; i < alarmTypes.Count; i++)
		{
			Type type = alarmTypes[i];
			AlarmTypeBase alarmTypeBase = null;
			if (!type.IsAbstract)
			{
				try
				{
					alarmTypeBase = (AlarmTypeBase)Activator.CreateInstance(type);
				}
				catch (Exception)
				{
					Debug.LogError("[AlarmClockScenario]: Unabled to Initialize alarm type: " + type.FullName);
				}
			}
			if (alarmTypeBase != null)
			{
				MapObject.ObjectType objectType = alarmTypeBase.MapNodeType();
				if (objectType != 0 && !mapNodeAddAlarmTypes.ContainsKey(objectType))
				{
					mapNodeAddAlarmTypes.Add(objectType, type);
				}
				alarmTypeList.Add(type, alarmTypeBase);
			}
		}
	}

	public void HandleWarpActions(AlarmTypeBase alarm, double currentUT, double utSecondsTillNextUpdate)
	{
		if (!(currentUT + utSecondsTillNextUpdate * (double)warpChangeTimeSafteyMultiplier > alarm.ut))
		{
			return;
		}
		warpAffectIndicator = true;
		warpAffectStartTime = KSPUtil.SystemDateTime.DateTimeNow();
		TimeWarp fetch = TimeWarp.fetch;
		if (fetch != null)
		{
			if (fetch.current_rate_index > 0 && WarpTransitionCalculator.UTToRateTimesOne > alarm.TimeToAlarm)
			{
				fetch.CancelAutoWarp();
				TimeWarp.SetRate(fetch.current_rate_index - 1, instant: true);
			}
			else if (Planetarium.TimeScale <= (double)fetch.tgt_rate && fetch.current_rate_index > 0)
			{
				fetch.CancelAutoWarp();
				TimeWarp.SetRate(fetch.current_rate_index - 1, instant: false);
			}
		}
	}

	public static Texture2D LoadTexture(string textureName)
	{
		textureName = (textureName.Contains("/") ? textureName : ("Squad/Alarms/Icons/" + textureName));
		Texture2D texture2D = GameDatabase.Instance.GetTexture(textureName, asNormalMap: false);
		if (texture2D == null)
		{
			Debug.LogWarning("[AlarmClockScenario]: Unable to load alarm texture, using default: " + textureName);
			texture2D = GameDatabase.Instance.GetTexture("Squad/Alarms/Icons/default", asNormalMap: false);
			if (texture2D == null)
			{
				texture2D = new Texture2D(1, 1);
				texture2D.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
				texture2D.Apply();
			}
		}
		return texture2D;
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		uiUpdatePeriod = 0.1;
		node.TryGetValue("UIUpdatePeriod", ref uiUpdatePeriod);
		node.TryGetValue("warpChangeTimeSafteyMultiplier", ref warpChangeTimeSafteyMultiplier);
		node.TryGetValue("warpChangeIndicatorDuration", ref warpChangeIndicatorDuration);
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("ALARMS", ref node2))
		{
			List<AlarmTypeBase> list = AppUI_Data.CreateAppUIDataList<AlarmTypeBase>(node2.GetNodes("ALARM"));
			for (int i = 0; i < list.Count; i++)
			{
				alarms.Add(list[i].Id, list[i]);
			}
			alarms.SortList(OrderByUT);
		}
		settings = new AlarmClockSettings();
		ConfigNode node3 = new ConfigNode();
		if (node.TryGetNode("SETTINGS", ref node3))
		{
			settings.Load(node3);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		node.AddValue("UIUpdatePeriod", uiUpdatePeriod);
		node.AddValue("warpChangeTimeSafteyMultiplier", warpChangeTimeSafteyMultiplier);
		node.AddValue("warpChangeIndicatorDuration", warpChangeIndicatorDuration);
		ConfigNode configNode = new ConfigNode("ALARMS");
		for (int i = 0; i < alarms.Count; i++)
		{
			ConfigNode node2 = configNode.AddNode("ALARM");
			alarms.ValuesList[i].Save(node2);
		}
		node.AddNode(configNode);
		ConfigNode node3 = new ConfigNode();
		settings.Save(node3);
		node.AddNode("SETTINGS", node3);
	}

	public static void SortAlarmsByUTDescending()
	{
		if (Instance != null)
		{
			Instance.alarms.SortList(Instance.OrderByUT);
		}
	}

	public int OrderByUT(AlarmTypeBase alarm1, AlarmTypeBase alarm2)
	{
		return alarm1.ut.CompareTo(alarm2.ut);
	}
}
