using System.Collections.Generic;
using ns9;
using UnityEngine;

public class DeltaVGlobals : MonoBehaviour
{
	public static bool ready;

	public DeltaVAppValues deltaVAppValues;

	public static DeltaVGlobals _fetch;

	public static bool _doStockSimulations = true;

	[SerializeField]
	public static List<int> propellantsToIgnore = null;

	public static DeltaVAppValues DeltaVAppValues
	{
		get
		{
			if (fetch == null)
			{
				return null;
			}
			return fetch.deltaVAppValues;
		}
	}

	public static DeltaVGlobals fetch
	{
		get
		{
			if (_fetch == null)
			{
				_fetch = (DeltaVGlobals)Object.FindObjectOfType(typeof(DeltaVGlobals));
			}
			return _fetch;
		}
	}

	public static bool DoStockSimulations => _doStockSimulations;

	public static List<int> PropellantsToIgnore
	{
		get
		{
			if (propellantsToIgnore == null)
			{
				propellantsToIgnore = new List<int>();
				propellantsToIgnore.Add("IntakeAir".GetHashCode());
				propellantsToIgnore.Add("ElectricCharge".GetHashCode());
			}
			return propellantsToIgnore;
		}
		set
		{
			propellantsToIgnore = value;
			if (!propellantsToIgnore.Contains("IntakeAir".GetHashCode()))
			{
				propellantsToIgnore.Add("IntakeAir".GetHashCode());
			}
			if (!propellantsToIgnore.Contains("ElectricCharge".GetHashCode()))
			{
				propellantsToIgnore.Add("ElectricCharge".GetHashCode());
			}
		}
	}

	public void Awake()
	{
		_fetch = this;
		ready = false;
		GameEvents.OnFlightGlobalsReady.Add(InitDeltaVGlobal);
		GameEvents.onGameSceneSwitchRequested.Add(OnSceneSwitch);
	}

	public void Start()
	{
		_doStockSimulations = GameSettings.DELTAV_CALCULATIONS_ENABLED;
	}

	public void OnDestroy()
	{
		GameEvents.OnFlightGlobalsReady.Remove(InitDeltaVGlobal);
		GameEvents.onGameSceneSwitchRequested.Remove(OnSceneSwitch);
		if (_fetch != null && _fetch == this)
		{
			_fetch = null;
		}
	}

	public void InitDeltaVGlobal(bool value)
	{
		if (!ready)
		{
			deltaVAppValues = new DeltaVAppValues();
			CelestialBody homeBody = FlightGlobals.GetHomeBody();
			if (homeBody != null && homeBody.atmosphere)
			{
				deltaVAppValues.situation = DeltaVSituationOptions.SeaLevel;
			}
			InitInfoLines();
			DeltaVAppValues.LoadEnabledInfoLines(GameSettings.STAGE_GROUP_INFO_ITEMS);
			ready = true;
		}
	}

	public void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		if (scn.to == GameScenes.FLIGHT)
		{
			deltaVAppValues.situation = DeltaVSituationOptions.Altitude;
		}
	}

	public virtual void InitInfoLines()
	{
		DeltaVAppValues.infoLines.Add(new DeltaVAppValues.InfoLine("DELTAV", Localizer.Format("#autoLOC_8003206"), Localizer.Format("#autoLOC_8003227"), (DeltaVStageInfo s, DeltaVSituationOptions sit) => s.GetSituationDeltaV(sit).ToString("0"), "m/s"));
		DeltaVAppValues.infoLines.Add(new DeltaVAppValues.InfoLine("ISP", Localizer.Format("#autoLOC_8003207"), Localizer.Format("#autoLOC_8003228"), (DeltaVStageInfo s, DeltaVSituationOptions sit) => s.GetSituationISP(sit).ToString("0"), "s"));
		DeltaVAppValues.infoLines.Add(new DeltaVAppValues.InfoLine("THRUST", Localizer.Format("#autoLOC_8003208"), Localizer.Format("#autoLOC_8003229"), (DeltaVStageInfo s, DeltaVSituationOptions sit) => s.GetSituationThrust(sit).ToString("0.00"), "kN"));
		DeltaVAppValues.infoLines.Add(new DeltaVAppValues.InfoLine("TWR", Localizer.Format("#autoLOC_8003209"), Localizer.Format("#autoLOC_8003230"), (DeltaVStageInfo s, DeltaVSituationOptions sit) => s.GetSituationTWR(sit).ToString("0.00")));
		DeltaVAppValues.infoLines.Add(new DeltaVAppValues.InfoLine("STARTMASS", Localizer.Format("#autoLOC_8003210"), Localizer.Format("#autoLOC_8003211"), (DeltaVStageInfo s, DeltaVSituationOptions sit) => s.startMass.ToString("0.0"), "t"));
		DeltaVAppValues.infoLines.Add(new DeltaVAppValues.InfoLine("ENDMASS", Localizer.Format("#autoLOC_8003212"), Localizer.Format("#autoLOC_8003213"), (DeltaVStageInfo s, DeltaVSituationOptions sit) => s.endMass.ToString("0.0"), "t"));
		DeltaVAppValues.infoLines.Add(new DeltaVAppValues.InfoLine("BURNTIME", Localizer.Format("#autoLOC_8003214"), Localizer.Format("#autoLOC_8003215"), (DeltaVStageInfo s, DeltaVSituationOptions sit) => s.stageBurnTime.ToString("0"), "s"));
	}

	public static bool EnableStockSimluations()
	{
		if (!GameSettings.DELTAV_CALCULATIONS_ENABLED)
		{
			Debug.Log("[DeltaVStatics]: Unable to enable Stock Calculations - overridden by GameSettings");
			return false;
		}
		_doStockSimulations = true;
		return true;
	}

	public static bool DisableStockSimluations()
	{
		_doStockSimulations = false;
		return true;
	}
}
