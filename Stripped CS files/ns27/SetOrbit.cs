using System;
using ns26;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns27;

public class SetOrbit : MonoBehaviour
{
	public Button bodyBackButton;

	public TextMeshProUGUI bodyNameText;

	public Button bodyForwardButton;

	public DebugScreenInputDouble eccInput;

	public DebugScreenInputDouble incInput;

	public DebugScreenInputDouble smaInput;

	public DebugScreenInputDouble mnaInput;

	public DebugScreenInputDouble lanInput;

	public DebugScreenInputDouble lpeInput;

	public DebugScreenInputDouble obtInput;

	public Button setOrbitButton;

	public TextMeshProUGUI errorText;

	public Button vesselBackButton;

	public TextMeshProUGUI vesselNameText;

	public Button vesselForwardButton;

	public DebugScreenInputDouble distanceInput;

	public Button rendezvousButton;

	public Toggle overrideSafetyCheck;

	public int selectedBody;

	public int selectedVessel;

	public static double safetyEnvelope = 1.025;

	public static float rendezvousDistance = 250f;

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

	public Vessel SelectedVessel
	{
		get
		{
			if (FlightGlobals.Vessels != null && FlightGlobals.Vessels.Count > selectedVessel)
			{
				return FlightGlobals.Vessels[selectedVessel];
			}
			return null;
		}
	}

	public void Awake()
	{
		bodyBackButton.onClick.AddListener(OnBodyBackClick);
		bodyForwardButton.onClick.AddListener(OnBodyForwardClick);
		vesselBackButton.onClick.AddListener(OnVesselBackClick);
		vesselForwardButton.onClick.AddListener(OnVesselForwardClick);
		setOrbitButton.onClick.AddListener(OnSetOrbitClick);
		rendezvousButton.onClick.AddListener(OnRendezvousClick);
		smaInput.inputField.onSelect.AddListener(ResetErrorMsg);
		eccInput.inputField.onSelect.AddListener(ResetErrorMsg);
		incInput.inputField.onSelect.AddListener(ResetErrorMsg);
		mnaInput.inputField.onSelect.AddListener(ResetErrorMsg);
		lanInput.inputField.onSelect.AddListener(ResetErrorMsg);
		lpeInput.inputField.onSelect.AddListener(ResetErrorMsg);
		obtInput.inputField.onSelect.AddListener(ResetErrorMsg);
		overrideSafetyCheck.isOn = false;
	}

	public void Start()
	{
		if (FlightGlobals.fetch != null)
		{
			selectedBody = FlightGlobals.GetHomeBodyIndex();
			SetSelectedBodyText();
			selectedVessel = 0;
			SetSelectedVesselText();
			smaInput.Value = SelectedBody.minOrbitalDistance * safetyEnvelope;
		}
		else
		{
			bodyNameText.text = string.Empty;
			smaInput.Value = 140000.0;
		}
		distanceInput.Value = rendezvousDistance;
		errorText.text = string.Empty;
	}

	public void OnSetOrbitClick()
	{
		errorText.text = string.Empty;
		if (!CheckForErrors())
		{
			FlightGlobals.fetch.SetShipOrbit(selectedBody, eccInput.Value, smaInput.Value, incInput.Value, lanInput.Value, mnaInput.Value, lpeInput.Value, obtInput.Value);
			FloatingOrigin.ResetTerrainShaderOffset();
		}
	}

	public bool CheckForErrors()
	{
		if (FlightGlobals.ActiveVessel == null)
		{
			errorText.text += Localizer.Format("#autoLOC_6001899");
			return true;
		}
		if (eccInput.Value == 1.0)
		{
			errorText.text += Localizer.Format("#autoLOC_6001900");
			eccInput.Value += 1E-10;
			return true;
		}
		if (eccInput.Value < 0.0)
		{
			errorText.text += Localizer.Format("#autoLOC_6001902");
			return true;
		}
		if (overrideSafetyCheck.isOn)
		{
			return false;
		}
		double num = (1.0 - eccInput.Value) * smaInput.Value;
		if (num < FlightGlobals.Bodies[selectedBody].minOrbitalDistance * safetyEnvelope)
		{
			errorText.text += Localizer.Format("#autoLOC_6001901");
			num = FlightGlobals.Bodies[selectedBody].minOrbitalDistance * safetyEnvelope;
			smaInput.Value = Math.Ceiling(num / (1.0 - eccInput.Value));
			return true;
		}
		if (incInput.Value > 180.0)
		{
			errorText.text += Localizer.Format("#autoLOC_6001903");
			return true;
		}
		if (incInput.Value < -180.0)
		{
			errorText.text += Localizer.Format("#autoLOC_6001904");
			return true;
		}
		if (double.IsInfinity(smaInput.Value))
		{
			errorText.text += Localizer.Format("#autoLOC_6001905");
			return true;
		}
		return false;
	}

	public void OnRendezvousClick()
	{
		bool flag = false;
		string text = string.Empty;
		Vessel vessel = SelectedVessel;
		if (FlightGlobals.ActiveVessel == null)
		{
			text += Localizer.Format("#autoLOC_6001899");
			flag = true;
		}
		if (vessel == null)
		{
			text += Localizer.Format("#autoLOC_6001906");
			flag = true;
		}
		else
		{
			if (vessel == FlightGlobals.ActiveVessel)
			{
				text += Localizer.Format("#autoLOC_6001907");
				flag = true;
			}
			Vessel.Situations situations = (vessel.loaded ? vessel.situation : vessel.protoVessel.situation);
			if (situations != Vessel.Situations.ORBITING && situations != Vessel.Situations.ESCAPING)
			{
				text += Localizer.Format("#autoLOC_6001908");
				flag = true;
			}
		}
		errorText.text = text;
		if (!flag)
		{
			try
			{
				rendezvousDistance = Convert.ToSingle(distanceInput.Value);
			}
			catch
			{
			}
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001909", rendezvousDistance, vessel.GetDisplayName()), 5f, ScreenMessageStyle.UPPER_CENTER);
			Vector3 vector = UnityEngine.Random.onUnitSphere * rendezvousDistance;
			FlightGlobals.fetch.SetShipOrbitRendezvous(vessel, vector, Vector3.zero);
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
			SetSelectedBodyText();
		}
	}

	public void SetSelectedBodyText()
	{
		CelestialBody celestialBody = SelectedBody;
		bodyNameText.text = ((celestialBody != null) ? Localizer.Format("#autoLOC_7001301", celestialBody.displayName) : string.Empty);
	}

	public void OnVesselBackClick()
	{
		if (!(FlightGlobals.fetch == null))
		{
			selectedVessel--;
			if (selectedVessel < 0)
			{
				selectedVessel = FlightGlobals.Vessels.Count - 1;
			}
			SetSelectedVesselText();
		}
	}

	public void OnVesselForwardClick()
	{
		if (!(FlightGlobals.fetch == null))
		{
			selectedVessel++;
			if (selectedVessel >= FlightGlobals.Vessels.Count)
			{
				selectedVessel = 0;
			}
			SetSelectedVesselText();
		}
	}

	public void SetSelectedVesselText()
	{
		Vessel vessel = SelectedVessel;
		vesselNameText.text = ((vessel != null) ? vessel.GetDisplayName() : string.Empty);
	}

	public void ResetErrorMsg(string msg)
	{
		errorText.text = string.Empty;
	}

	public void OnDestroy()
	{
		smaInput.inputField.onSelect.RemoveListener(ResetErrorMsg);
		eccInput.inputField.onSelect.RemoveListener(ResetErrorMsg);
		incInput.inputField.onSelect.RemoveListener(ResetErrorMsg);
		mnaInput.inputField.onSelect.RemoveListener(ResetErrorMsg);
		lanInput.inputField.onSelect.RemoveListener(ResetErrorMsg);
		lpeInput.inputField.onSelect.RemoveListener(ResetErrorMsg);
		obtInput.inputField.onSelect.RemoveListener(ResetErrorMsg);
	}
}
