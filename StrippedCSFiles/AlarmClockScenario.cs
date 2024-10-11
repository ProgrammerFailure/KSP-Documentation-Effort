using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
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
		private readonly IDictionary<string, Sprite> _sprites;

		public Sprite this[string key]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpriteMap()
		{
			throw null;
		}
	}

	internal static AlarmClockApp appInstance;

	public DictionaryValueList<uint, AlarmTypeBase> alarms;

	private SpriteMap alarmSprites;

	public const string textureBaseUrl = "Squad/Alarms/Icons/";

	private DictionaryValueList<Type, AlarmTypeBase> alarmTypeList;

	private List<Type> alarmTypes;

	private AlarmClockScenarioAudio audioController;

	private AlarmClockMessageDialog messagePrefab;

	public float warpChangeTimeSafteyMultiplier;

	public float warpChangeIndicatorDuration;

	private bool warpAffectIndicator;

	private DateTime warpAffectStartTime;

	private DictionaryValueList<MapObject.ObjectType, Type> mapNodeAddAlarmTypes;

	public AlarmClockSettings settings;

	private double currentUT;

	private double lastUT;

	public double uiUpdatePeriod;

	public int availableVesselChangeCooldown;

	public int availableVesselChangeCooldownCounter;

	private Vessel availableVessel;

	private uint lastAvailableVesselId;

	public static AlarmClockScenario Instance
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

	public static AlarmClockApp AppInstance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static SpriteMap AlarmSprites
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<Type> AlarmTypes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static AlarmClockScenarioAudio AudioController
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static AlarmClockMessageDialog MessagePrefab
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool WarpAffectIndicator
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

	public static double UIUpdatePeriod
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsAvailableVesselCooldownActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool IsVesselAvailable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vessel AvailableVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private uint AvailableVesselId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockScenario()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnManeuversLoaded(Vessel v, PatchedConicSolver s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AlarmTypeBase GetNextAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AlarmTypeBase GetNextOrLastAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AlarmTypeBase GetNextAlarm(double afterUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AlarmTypeBase GetLastAlarm(double afterUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AlarmTypeBase CreateAlarmByType(Type alarmType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint GetUniqueAlarmID()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AddAlarm(AlarmTypeBase newAlarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AlarmExists(uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryGetAlarm(uint id, out AlarmTypeBase alarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool DeleteAlarm(AlarmTypeBase alarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool DeleteAlarm(uint id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool PlaySound(string soundURL, int repeats = 1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool MapNodeDefined(MapObject.ObjectType nodeType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AlarmTypeBase CreateAlarmByMapNodeType(MapObject.ObjectType nodeType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool ShowAlarmMapButton(MapObject mapObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HandleWarpActions(AlarmTypeBase alarm, double currentUT, double utSecondsTillNextUpdate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D LoadTexture(string textureName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SortAlarmsByUTDescending()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int OrderByUT(AlarmTypeBase alarm1, AlarmTypeBase alarm2)
	{
		throw null;
	}
}
