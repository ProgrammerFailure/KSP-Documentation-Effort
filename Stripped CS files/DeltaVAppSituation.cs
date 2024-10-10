using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeltaVAppSituation : MonoBehaviour
{
	public enum altChangeInProgress
	{
		None,
		Text,
		Slider
	}

	[SerializeField]
	public TMP_Dropdown bodySelector;

	[SerializeField]
	public Toggle atmosphereSeaLevel;

	[SerializeField]
	public Toggle atmosphereAltitude;

	[SerializeField]
	public Toggle atmosphereVacuum;

	public CanvasGroup seaLevelDisableGroup;

	public CanvasGroup altitudeDisableGroup;

	[SerializeField]
	public TMP_InputField altitudeText;

	[SerializeField]
	public Slider altitudeSlider;

	[SerializeField]
	public TextMeshProUGUI pressureLabel;

	[SerializeField]
	public CanvasGroup atmosphereControls;

	[SerializeField]
	public float atmosphereControlsOffAlpha = 0.4f;

	public DeltaVSituationOptions situation = DeltaVSituationOptions.Vaccum;

	public CelestialBody selectedBody;

	public double altitudeKM;

	public bool ready;

	public altChangeInProgress altitudeChangeInProgress;

	public bool sliderDragging;

	public VesselDeltaV vesselDeltaV;

	public static string cacheAutoLOC_7001410;

	public DeltaVSituationOptions Situation => situation;

	public CelestialBody SelectedBody => selectedBody;

	public double AltitudeKM => altitudeKM;

	public double Altitude => altitudeKM * 1000.0;

	public bool IsBodyDropDownExpanded => bodySelector.IsExpanded;

	public void Awake()
	{
		seaLevelDisableGroup = atmosphereSeaLevel.GetComponent<CanvasGroup>();
		altitudeDisableGroup = atmosphereAltitude.GetComponent<CanvasGroup>();
		if (DeltaVGlobals.DeltaVAppValues != null)
		{
			situation = DeltaVGlobals.DeltaVAppValues.situation;
			altitudeKM = DeltaVGlobals.DeltaVAppValues.altitude / 1000.0;
			selectedBody = DeltaVGlobals.DeltaVAppValues.body;
		}
		atmosphereSeaLevel.isOn = Situation == DeltaVSituationOptions.SeaLevel;
		atmosphereAltitude.isOn = Situation == DeltaVSituationOptions.Altitude;
		atmosphereVacuum.isOn = Situation == DeltaVSituationOptions.Vaccum;
		altitudeSlider.value = (float)altitudeKM;
		altitudeText.text = altitudeKM.ToString();
		SetAtmosphereSituation(Situation);
	}

	public void Start()
	{
		atmosphereSeaLevel.onValueChanged.AddListener(ToggleSeaLevelOn);
		atmosphereAltitude.onValueChanged.AddListener(ToggleAltitudeOn);
		atmosphereVacuum.onValueChanged.AddListener(ToggleVacuumOn);
		altitudeSlider.onValueChanged.AddListener(AltitudeSliderChanged);
		altitudeText.onValueChanged.AddListener(AltitudeTextChanged);
		bodySelector.onValueChanged.AddListener(BodyChanged);
		RefreshBodyList();
		ready = true;
	}

	public void OnDestroy()
	{
		atmosphereSeaLevel.onValueChanged.RemoveListener(ToggleSeaLevelOn);
		atmosphereAltitude.onValueChanged.RemoveListener(ToggleAltitudeOn);
		atmosphereVacuum.onValueChanged.RemoveListener(ToggleVacuumOn);
		altitudeSlider.onValueChanged.RemoveListener(AltitudeSliderChanged);
		altitudeText.onValueChanged.RemoveListener(AltitudeTextChanged);
		bodySelector.onValueChanged.RemoveListener(BodyChanged);
	}

	public void BodyChanged(int value)
	{
		if (value > FlightGlobals.Bodies.Count)
		{
			return;
		}
		selectedBody = FlightGlobals.Bodies[value];
		bool flag;
		if (!(flag = selectedBody.atmosphere && selectedBody != Planetarium.fetch.Sun))
		{
			atmosphereVacuum.isOn = true;
		}
		else
		{
			switch (Situation)
			{
			case DeltaVSituationOptions.SeaLevel:
				atmosphereSeaLevel.isOn = true;
				break;
			case DeltaVSituationOptions.Altitude:
				atmosphereAltitude.isOn = true;
				break;
			default:
				atmosphereVacuum.isOn = true;
				break;
			}
			altitudeSlider.maxValue = (float)selectedBody.atmosphereDepth / 1000f;
		}
		seaLevelDisableGroup.interactable = flag;
		seaLevelDisableGroup.alpha = (flag ? 1f : atmosphereControlsOffAlpha);
		altitudeDisableGroup.interactable = flag;
		altitudeDisableGroup.alpha = (flag ? 1f : atmosphereControlsOffAlpha);
		SetAtmosphereSituation(Situation);
		if (ready)
		{
			DeltaVApp.Instance.usage.body++;
		}
	}

	public void RefreshBodyList()
	{
		if (FlightGlobals.fetch == null)
		{
			return;
		}
		bodySelector.ClearOptions();
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			string text = FlightGlobals.Bodies[i].name;
			string text2 = FlightGlobals.Bodies[i].displayName.LocalizeRemoveGender();
			bodySelector.options.Add(new TMP_Dropdown.OptionData(text2));
			if (text == FlightGlobals.GetHomeBodyName())
			{
				bodySelector.value = i;
			}
		}
		selectedBody = FlightGlobals.Bodies[bodySelector.value];
	}

	public void ToggleSeaLevelOn(bool value)
	{
		if (value)
		{
			SetAtmosphereSituation(DeltaVSituationOptions.SeaLevel);
			if (ready)
			{
				DeltaVApp.Instance.usage.situation++;
			}
		}
	}

	public void ToggleAltitudeOn(bool value)
	{
		if (value)
		{
			SetAtmosphereSituation(DeltaVSituationOptions.Altitude);
			if (ready)
			{
				DeltaVApp.Instance.usage.situation++;
			}
		}
	}

	public void ToggleVacuumOn(bool value)
	{
		if (value)
		{
			SetAtmosphereSituation(DeltaVSituationOptions.Vaccum);
			if (ready)
			{
				DeltaVApp.Instance.usage.situation++;
			}
		}
	}

	public void SetAtmosphereSituation(DeltaVSituationOptions option)
	{
		bool flag = Situation != option;
		if (selectedBody != null && DeltaVGlobals.DeltaVAppValues != null)
		{
			flag = DeltaVGlobals.DeltaVAppValues.body == null || flag || DeltaVGlobals.DeltaVAppValues.body.name != selectedBody.name;
		}
		situation = option;
		if (selectedBody != null && selectedBody.atmosphere && selectedBody != Planetarium.fetch.Sun && Situation == DeltaVSituationOptions.Altitude)
		{
			atmosphereControls.alpha = 1f;
			atmosphereControls.interactable = true;
		}
		else
		{
			atmosphereControls.alpha = atmosphereControlsOffAlpha;
			atmosphereControls.interactable = false;
		}
		UpdatePressureDisplay();
		if (flag)
		{
			UpdateVesselDeltaVValues();
			GameEvents.onDeltaVAppAtmosphereChanged.Fire(Situation);
		}
	}

	public void AltitudeSliderChanged(float value)
	{
		if (altitudeChangeInProgress == altChangeInProgress.None)
		{
			altitudeChangeInProgress = altChangeInProgress.Slider;
			altitudeText.text = value.ToString();
			altitudeKM = value;
			UpdatePressureDisplay();
			altitudeChangeInProgress = altChangeInProgress.None;
			if (ready)
			{
				DeltaVApp.Instance.usage.atmosphereSlider++;
			}
		}
	}

	public void AltitudeTextChanged(string value)
	{
		if (altitudeChangeInProgress == altChangeInProgress.None)
		{
			altitudeChangeInProgress = altChangeInProgress.Text;
			float num = 0f;
			if (value != "")
			{
				num = float.Parse(value);
			}
			altitudeSlider.value = Mathf.Clamp(num, 0f, altitudeSlider.maxValue);
			altitudeKM = num;
			UpdatePressureDisplay();
			UpdateVesselDeltaVValues();
			altitudeChangeInProgress = altChangeInProgress.None;
			if (ready)
			{
				DeltaVApp.Instance.usage.atmosphereText++;
			}
		}
	}

	public void SliderStartDrag()
	{
		sliderDragging = true;
	}

	public void SliderPointerUp()
	{
		if (!sliderDragging)
		{
			AltitudeSliderChanged(altitudeSlider.value);
			UpdateVesselDeltaVValues();
		}
	}

	public void SliderEndDrag()
	{
		AltitudeSliderChanged(altitudeSlider.value);
		UpdateVesselDeltaVValues();
		sliderDragging = false;
	}

	public void UpdatePressureDisplay()
	{
		switch (Situation)
		{
		case DeltaVSituationOptions.SeaLevel:
			pressureLabel.text = $"{SelectedBody.GetPressure(0.0):0.000}" + cacheAutoLOC_7001410;
			break;
		case DeltaVSituationOptions.Altitude:
			pressureLabel.text = $"{SelectedBody.GetPressure(Altitude):0.000}" + cacheAutoLOC_7001410;
			break;
		default:
			pressureLabel.text = "0.000" + cacheAutoLOC_7001410;
			break;
		}
	}

	public bool UpdateVesselDeltaVValues(bool recalcIfChanges = true, bool forceRecalc = false)
	{
		if (!(EditorLogic.fetch == null) && EditorLogic.fetch.ship != null && !(EditorLogic.fetch.ship.vesselDeltaV == null))
		{
			if (vesselDeltaV == null)
			{
				vesselDeltaV = EditorLogic.fetch.ship.vesselDeltaV;
			}
			bool flag = false;
			if (DeltaVGlobals.DeltaVAppValues.body.name != selectedBody.name)
			{
				DeltaVGlobals.DeltaVAppValues.body = selectedBody;
				flag = true;
			}
			if (DeltaVGlobals.DeltaVAppValues.situation != Situation)
			{
				DeltaVGlobals.DeltaVAppValues.situation = Situation;
				flag = true;
			}
			if (DeltaVGlobals.DeltaVAppValues.altitude != Altitude)
			{
				DeltaVGlobals.DeltaVAppValues.altitude = Altitude;
				flag = true;
			}
			if (forceRecalc || (flag && recalcIfChanges))
			{
				vesselDeltaV.SetCalcsDirty(resetPartCaches: false);
			}
			return true;
		}
		return false;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7001410 = Localizer.Format("#autoLOC_7001410");
	}

	public bool AltitudeHasFocus()
	{
		return altitudeText.isFocused;
	}
}
