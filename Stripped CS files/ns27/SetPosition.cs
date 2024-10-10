using System;
using EdyCommonTools;
using ns26;
using ns36;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns27;

public class SetPosition : MonoBehaviour
{
	public Button bodyBackButton;

	public TextMeshProUGUI bodyNameText;

	public Button bodyForwardButton;

	public DebugScreenInputDouble latitudeInput;

	public DebugScreenInputDouble longitudeInput;

	public DebugScreenInputDouble altitudeInput;

	public DebugScreenInputDouble pitchInput;

	public DebugScreenInputDouble headingInput;

	public Slider easeInMultiplier;

	public TextMeshProUGUI easeInAmount;

	public Toggle easeToGroundToggle;

	public GameObject easeInStatusObject;

	public Button easeInDisableButton;

	public Toggle doNotPlaceUnderwaterToggle;

	public Button setSurfaceButton;

	public TextMeshProUGUI errorText;

	public Toggle overrideSafetyCheck;

	public int selectedBody;

	public NavBall navBall;

	public ScreenMessage easingInScreenMessage;

	public double altitudeSuggestedValue;

	public double maxAltitudeSuggestedValue = 30.0;

	public bool error;

	public string errorMsg;

	public CelestialBody SelectedBody
	{
		get
		{
			if (FlightGlobals.Bodies != null && FlightGlobals.Bodies.Count > selectedBody)
			{
				return FlightGlobals.Bodies[selectedBody];
			}
			return null;
		}
	}

	public void Awake()
	{
		bodyBackButton.onClick.AddListener(OnBodyBackClick);
		bodyForwardButton.onClick.AddListener(OnBodyForwardClick);
		easeInMultiplier.onValueChanged.AddListener(UpdateSliderValue);
		easeInDisableButton.onClick.AddListener(DisableActiveVesselEaseIn);
		setSurfaceButton.onClick.AddListener(OnSetPositionClick);
		altitudeInput.inputField.onEndEdit.AddListener(FormatValues);
		pitchInput.inputField.onEndEdit.AddListener(FormatValues);
		headingInput.inputField.onEndEdit.AddListener(FormatValues);
		GameEvents.onLevelWasLoaded.Add(OnSceneExit);
		GameEvents.onVesselChange.Add(OnVesselChanged);
		GameEvents.onVesselSituationChange.Add(OnVesselSituationChanged);
		overrideSafetyCheck.isOn = false;
	}

	public void Start()
	{
		if (FlightGlobals.fetch != null)
		{
			selectedBody = FlightGlobals.GetHomeBodyIndex();
			SetSelectedBodyText();
			InitializeValues();
			navBall = UnityEngine.Object.FindObjectOfType<NavBall>();
		}
		else
		{
			bodyNameText.text = string.Empty;
		}
		errorText.text = string.Empty;
		easingInScreenMessage = new ScreenMessage(Localizer.Format("#autoLOC_6003101"), 3000f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void OnEnable()
	{
		InitializeValues();
	}

	public void OnDestroy()
	{
		altitudeInput.inputField.onEndEdit.RemoveListener(FormatValues);
		pitchInput.inputField.onEndEdit.RemoveListener(FormatValues);
		headingInput.inputField.onEndEdit.RemoveListener(FormatValues);
		easeInMultiplier.onValueChanged.RemoveListener(UpdateSliderValue);
		easeInDisableButton.onClick.RemoveListener(DisableActiveVesselEaseIn);
		GameEvents.onLevelWasLoaded.Remove(OnSceneExit);
		GameEvents.onVesselChange.Remove(OnVesselChanged);
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChanged);
	}

	public void InitializeValues()
	{
		if (!(FlightGlobals.fetch == null) && !(FlightGlobals.ActiveVessel == null))
		{
			latitudeInput.Value = FlightGlobals.ActiveVessel.latitude;
			longitudeInput.Value = FlightGlobals.ActiveVessel.longitude;
			altitudeInput.Value = Math.Round(FlightGlobals.ActiveVessel.radarAltitude, 2);
			pitchInput.Value = Math.Round(FlightGlobals.ActiveVessel.ctrlState.pitch, 2);
			navBall = UnityEngine.Object.FindObjectOfType<NavBall>();
			if (navBall != null)
			{
				headingInput.Value = Math.Round(Quaternion.Inverse(navBall.relativeGymbal).eulerAngles.y, 2);
			}
			easeInAmount.text = easeInMultiplier.value.ToString();
		}
	}

	public bool CheckForErrors()
	{
		errorText.text = string.Empty;
		if (!(FlightGlobals.ActiveVessel == null) && FlightGlobals.ActiveVessel.state != Vessel.State.DEAD)
		{
			if (overrideSafetyCheck.isOn)
			{
				return false;
			}
			altitudeSuggestedValue = GetSugestedAltitude(FlightGlobals.fetch.activeVessel);
			if (altitudeInput.Value < altitudeSuggestedValue)
			{
				errorText.text += Localizer.Format("#autoLOC_6006032", Math.Round(FlightGlobals.fetch.activeVessel.vesselSize.x, 2), Math.Round(FlightGlobals.fetch.activeVessel.vesselSize.y, 2), Math.Round(FlightGlobals.fetch.activeVessel.vesselSize.z, 2), Math.Round(altitudeSuggestedValue, 2));
				altitudeInput.Value = ((altitudeSuggestedValue < maxAltitudeSuggestedValue) ? altitudeSuggestedValue : maxAltitudeSuggestedValue);
				return true;
			}
			if (latitudeInput.Value > 90.0)
			{
				errorText.text += Localizer.Format("#autoLOC_8003379");
				latitudeInput.Value = 90.0;
				return true;
			}
			if (latitudeInput.Value < -90.0)
			{
				errorText.text += Localizer.Format("#autoLOC_8003380");
				latitudeInput.Value = -90.0;
				return true;
			}
			if (longitudeInput.Value > 180.0)
			{
				errorText.text += Localizer.Format("#autoLOC_8003381");
				longitudeInput.Value = 180.0;
				return true;
			}
			if (longitudeInput.Value < -180.0)
			{
				errorText.text += Localizer.Format("#autoLOC_8003382");
				longitudeInput.Value = -180.0;
				return true;
			}
			if (pitchInput.Value > 180.0)
			{
				errorText.text += Localizer.Format("#autoLOC_6006028");
				pitchInput.Value = MathUtility.ClampAngle((float)pitchInput.Value);
				return true;
			}
			if (pitchInput.Value < -180.0)
			{
				errorText.text += Localizer.Format("#autoLOC_6006029");
				pitchInput.Value = MathUtility.ClampAngle((float)pitchInput.Value);
				return true;
			}
			if (headingInput.Value > 360.0)
			{
				errorText.text += Localizer.Format("#autoLOC_6006023");
				headingInput.Value = MathUtility.ClampAngle360((float)headingInput.Value);
				return true;
			}
			if (headingInput.Value < -360.0)
			{
				errorText.text += Localizer.Format("#autoLOC_6006024");
				headingInput.Value = MathUtility.ClampAngle360((float)headingInput.Value);
				return true;
			}
			return false;
		}
		errorText.text += Localizer.Format("#autoLOC_6001899");
		return true;
	}

	public void OnSetPositionClick()
	{
		ResetErrorMsg(string.Empty);
		error = CheckForErrors();
		if (!error)
		{
			MapView.ExitMapView();
			FlightGlobals.fetch.SetVesselPosition(selectedBody, latitudeInput.Value, longitudeInput.Value, altitudeInput.Value, pitchInput.Value, headingInput.Value, doNotPlaceUnderwaterToggle.isOn, easeToGroundToggle.isOn, easeInMultiplier.value);
			CheckForHiddenElements();
			FloatingOrigin.ResetTerrainShaderOffset();
		}
	}

	public void OnBodyBackClick()
	{
		if (!(FlightGlobals.fetch == null))
		{
			selectedBody--;
			if (selectedBody < 0)
			{
				selectedBody = FlightGlobals.Bodies.Count - 1;
			}
			CheckForHiddenElements();
			SetSelectedBodyText();
		}
	}

	public void OnBodyForwardClick()
	{
		if (!(FlightGlobals.fetch == null))
		{
			selectedBody++;
			if (selectedBody >= FlightGlobals.Bodies.Count)
			{
				selectedBody = 0;
			}
			CheckForHiddenElements();
			SetSelectedBodyText();
		}
	}

	public void SetSelectedBodyText()
	{
		CelestialBody celestialBody = SelectedBody;
		bodyNameText.text = ((celestialBody != null) ? Localizer.Format("#autoLOC_7001301", celestialBody.displayName) : string.Empty);
	}

	public void OnVesselChanged(Vessel vessel)
	{
		CheckForHiddenElements();
		CheckForErrors();
	}

	public void OnVesselSituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
	{
		CheckForHiddenElements();
	}

	public void OnSceneExit(GameScenes scene)
	{
		CheckForHiddenElements();
	}

	public void CheckForHiddenElements()
	{
		if (FlightGlobals.Bodies[selectedBody].pqsController == null)
		{
			easeToGroundToggle.gameObject.SetActive(value: false);
		}
		else
		{
			easeToGroundToggle.gameObject.SetActive(value: true);
		}
		if (FlightGlobals.fetch != null && FlightGlobals.fetch.activeVessel != null && FlightGlobals.fetch.activeVessel.easingInToSurface)
		{
			easeInStatusObject.gameObject.SetActive(value: true);
			ScreenMessages.PostScreenMessage(easingInScreenMessage);
		}
		else
		{
			easeInStatusObject.gameObject.SetActive(value: false);
			ScreenMessages.RemoveMessage(easingInScreenMessage);
		}
	}

	public void FormatValues(string value)
	{
		altitudeInput.Value = Math.Round(altitudeInput.Value, 2);
		pitchInput.Value = Math.Round(pitchInput.Value, 2);
		headingInput.Value = Math.Round(headingInput.Value, 2);
	}

	public void UpdateSliderValue(float value)
	{
		easeInAmount.text = easeInMultiplier.value.ToString();
	}

	public void DisableActiveVesselEaseIn()
	{
		if (FlightGlobals.fetch != null && FlightGlobals.fetch.activeVessel != null)
		{
			FlightGlobals.fetch.ToggleVesselEaseIn(FlightGlobals.fetch.activeVessel, enableEaseIn: false);
		}
		CheckForHiddenElements();
	}

	public void ResetErrorMsg(string msg)
	{
		errorText.text = msg;
	}

	public void Update()
	{
		if (!CheatOptions.MiddleMouseClickSetPosition || !Input.GetMouseButtonDown(2))
		{
			return;
		}
		ResetErrorMsg(string.Empty);
		double sugestedAltitude = GetSugestedAltitude(FlightGlobals.fetch.activeVessel);
		if (MapView.MapIsEnabled)
		{
			Ray ray = PlanetariumCamera.Camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo, float.MaxValue, LayerMask.GetMask("Scaled Scenery")))
			{
				Vector3 vector = ScaledSpace.ScaledToLocalSpace(hitInfo.point);
				double lat;
				double lon;
				double alt;
				if (PlanetariumCamera.fetch.target.type == MapObject.ObjectType.Vessel)
				{
					FlightGlobals.ActiveVessel.mainBody.GetLatLonAlt(vector, out lat, out lon, out alt);
					GetSelectedBodyIndex(FlightGlobals.ActiveVessel.mainBody);
					SetSelectedBodyText();
				}
				else
				{
					PlanetariumCamera.fetch.target.celestialBody.GetLatLonAlt(vector, out lat, out lon, out alt);
					GetSelectedBodyIndex(PlanetariumCamera.fetch.target.celestialBody);
					SetSelectedBodyText();
				}
				SetPositionValues(lat, lon, sugestedAltitude);
			}
		}
		else
		{
			Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray2.origin, ray2.direction, out var hitInfo2, LayerMask.GetMask("Local Scenery")))
			{
				FlightGlobals.ActiveVessel.mainBody.GetLatLonAlt(hitInfo2.point, out var lat2, out var lon2, out var _);
				GetSelectedBodyIndex(FlightGlobals.ActiveVessel.mainBody);
				SetSelectedBodyText();
				SetPositionValues(lat2, lon2, sugestedAltitude);
			}
		}
	}

	public double GetSugestedAltitude(Vessel vessel)
	{
		return Math.Round(Mathf.Max(vessel.vesselSize.x, vessel.vesselSize.y, vessel.vesselSize.z), 2) * 0.5 + 5.0;
	}

	public void GetSelectedBodyIndex(CelestialBody celestialBody)
	{
		selectedBody = FlightGlobals.Bodies.IndexOf(celestialBody);
	}

	public void SetPositionValues(double lat, double lon, double alt)
	{
		latitudeInput.Value = lat;
		longitudeInput.Value = lon;
		altitudeInput.Value = ((alt < maxAltitudeSuggestedValue) ? alt : maxAltitudeSuggestedValue);
	}
}
