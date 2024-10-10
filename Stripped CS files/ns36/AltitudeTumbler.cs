using ns11;
using UnityEngine;
using UnityEngine.UI;

namespace ns36;

public class AltitudeTumbler : MonoBehaviour
{
	public class UsageStats
	{
		public int aglSelected;

		public int aslSelected;

		public bool UsageFound
		{
			get
			{
				if (aglSelected <= 0)
				{
					return aslSelected > 0;
				}
				return true;
			}
		}

		public void Reset()
		{
			aglSelected = 0;
			aslSelected = 0;
		}
	}

	public static AltitudeTumbler Instance;

	public ns11.Tumbler tumbler;

	public static double altitudeMultiplier = 1.0;

	public Button altitudeModeBtn;

	public AltimeterDisplayState aglMode;

	public SpriteTumblerObject modeTumbler;

	public float modeTumblerSharpness = 6f;

	public float lastUpdateTime;

	public UsageStats usage;

	public AltimeterDisplayState CurrentMode => aglMode;

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(this);
		}
		Instance = this;
		lastUpdateTime = Time.realtimeSinceStartup;
		usage = new UsageStats();
		SetTumblerPosition();
	}

	public void SetTumblerPosition()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -650f * Mathf.Pow(GameSettings.UI_SCALE, -1.077322f));
	}

	public void Start()
	{
		altitudeModeBtn.onClick.AddListener(ChangeAltimeterMode);
		GameEvents.onVesselChange.Add(OnVesselChange);
		if (FlightGlobals.ActiveVessel != null)
		{
			aglMode = FlightGlobals.ActiveVessel.altimeterDisplayState;
		}
		else
		{
			aglMode = AltimeterDisplayState.const_1;
		}
		SetModeTumbler(aglMode);
		GameEvents.onGameSceneSwitchRequested.Add(OnSceneSwitch);
		GameEvents.OnMapEntered.Add(OnMapEntered);
		GameEvents.onUIScaleChange.Add(SetTumblerPosition);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.onGameSceneSwitchRequested.Remove(OnSceneSwitch);
		GameEvents.OnMapEntered.Remove(OnMapEntered);
		GameEvents.onUIScaleChange.Remove(SetTumblerPosition);
	}

	public void OnVesselChange(Vessel vsl)
	{
		aglMode = vsl.altimeterDisplayState;
		SetModeTumbler(aglMode);
	}

	public void ChangeAltimeterMode()
	{
		switch (aglMode)
		{
		case AltimeterDisplayState.const_2:
			aglMode = AltimeterDisplayState.const_1;
			usage.aslSelected++;
			break;
		case AltimeterDisplayState.DEFAULT:
		case AltimeterDisplayState.const_1:
			aglMode = AltimeterDisplayState.const_2;
			usage.aglSelected++;
			break;
		}
		SetModeTumbler(aglMode);
	}

	public void SetModeTumbler(AltimeterDisplayState mode)
	{
		aglMode = mode;
		if (aglMode == AltimeterDisplayState.DEFAULT)
		{
			aglMode = AltimeterDisplayState.const_1;
		}
		if (FlightGlobals.ActiveVessel != null)
		{
			FlightGlobals.ActiveVessel.altimeterDisplayState = aglMode;
		}
		if (modeTumbler != null)
		{
			if (aglMode == AltimeterDisplayState.const_2)
			{
				modeTumbler.TumbleTo(0.0, 1);
			}
			else
			{
				modeTumbler.TumbleTo(1.0, 0);
			}
		}
		GameEvents.OnAltimeterDisplayModeToggle.Fire(aglMode);
	}

	public void Reset()
	{
		tumbler = GetComponent<ns11.Tumbler>();
	}

	public void LateUpdate()
	{
		if (modeTumbler != null)
		{
			float deltaTime = Time.realtimeSinceStartup - lastUpdateTime;
			lastUpdateTime = Time.realtimeSinceStartup;
			modeTumbler.UpdateDelta(deltaTime, modeTumblerSharpness);
		}
		if (FlightGlobals.ready && (object)tumbler != null)
		{
			if (aglMode == AltimeterDisplayState.const_2)
			{
				tumbler.SetValue(FlightGlobals.ActiveVessel.radarAltitude);
			}
			else
			{
				tumbler.SetValue(FlightGlobals.ActiveVessel.altitude * altitudeMultiplier);
			}
		}
	}

	public void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		if (!MapView.MapIsEnabled && usage.UsageFound)
		{
			AnalyticsUtil.AltimeterModeUsage(HighLogic.CurrentGame, usage);
			usage.Reset();
		}
	}

	public void OnMapEntered()
	{
		if (usage.UsageFound)
		{
			AnalyticsUtil.AltimeterModeUsage(HighLogic.CurrentGame, usage);
			usage.Reset();
		}
	}
}
