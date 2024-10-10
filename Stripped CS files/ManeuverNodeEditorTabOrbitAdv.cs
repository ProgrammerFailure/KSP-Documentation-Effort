using TMPro;
using UnityEngine;

public class ManeuverNodeEditorTabOrbitAdv : ManeuverNodeEditorTab
{
	[SerializeField]
	public TextMeshProUGUI orbitArgumentOfPeriapsis;

	[SerializeField]
	public TextMeshProUGUI orbitLongitudeOfAscendingNode;

	[SerializeField]
	public TextMeshProUGUI ejectionAngle;

	[SerializeField]
	public TextMeshProUGUI orbitEccentricity;

	[SerializeField]
	public TextMeshProUGUI orbitInclination;

	public Orbit orbitToDisplay;

	public bool patchesAheadLimitOK;

	public void Start()
	{
		if ((bool)FlightUIModeController.Instance)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		UpdateUIElements();
	}

	public override void SetInitialValues()
	{
		patchesAheadLimitOK = GameVariables.Instance.GetPatchesAheadLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) > 0;
		UpdateUIElements();
	}

	public override void UpdateUIElements()
	{
		if ((bool)FlightUIModeController.Instance)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		if (mannodeEditorManager.SelectedManeuverNode == null)
		{
			orbitToDisplay = FlightGlobals.ActiveVessel.orbit;
		}
		else
		{
			orbitToDisplay = mannodeEditorManager.SelectedManeuverNode.nextPatch;
		}
		orbitInclination.text = orbitToDisplay.inclination.ToString("F1") + " °";
		orbitEccentricity.text = orbitToDisplay.eccentricity.ToString("F4");
		ejectionAngle.text = OrbitUtil.CurrentEjectionAngle(orbitToDisplay, Planetarium.GetUniversalTime()).ToString("F1") + " °";
		orbitArgumentOfPeriapsis.text = orbitToDisplay.argumentOfPeriapsis.ToString("F1") + " °";
		orbitLongitudeOfAscendingNode.text = orbitToDisplay.double_0.ToString("F1") + " °";
	}

	public override bool IsTabInteractable()
	{
		if (patchesAheadLimitOK)
		{
			return InputLockManager.IsUnlocked(ControlTypes.FLIGHTUIMODE);
		}
		return false;
	}
}
