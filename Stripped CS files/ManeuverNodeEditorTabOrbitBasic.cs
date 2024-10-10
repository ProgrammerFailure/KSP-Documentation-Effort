using ns9;
using TMPro;
using UnityEngine;

public class ManeuverNodeEditorTabOrbitBasic : ManeuverNodeEditorTab
{
	[SerializeField]
	public TextMeshProUGUI apoapsisAltitude;

	[SerializeField]
	public TextMeshProUGUI apoapsisTime;

	[SerializeField]
	public TextMeshProUGUI periapsisAltitude;

	[SerializeField]
	public TextMeshProUGUI periapsisTime;

	[SerializeField]
	public TextMeshProUGUI orbitPeriod;

	public bool spaceDiscoveryUnlocked;

	public Orbit orbitToDisplay;

	public static string cacheAutoLOC_462439;

	public static string cacheAutoLOC_7001411;

	public static string cacheAutoLOC_6002332;

	public static string cacheAutoLOC_215362;

	public void Start()
	{
		if ((bool)FlightUIModeController.Instance)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
	}

	public override void SetInitialValues()
	{
		spaceDiscoveryUnlocked = GameVariables.Instance.GetPatchesAheadLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) > 0;
		UpdateUIElements();
	}

	public override void UpdateUIElements()
	{
		if (mannodeEditorManager == null)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		orbitToDisplay = ((mannodeEditorManager.SelectedManeuverNode == null) ? FlightGlobals.ActiveVessel.orbit : mannodeEditorManager.SelectedManeuverNode.nextPatch);
		apoapsisAltitude.text = (double.IsInfinity(FlightGlobals.ActiveVessel.orbit.ApA) ? cacheAutoLOC_462439 : KSPUtil.PrintSI(orbitToDisplay.ApA, cacheAutoLOC_7001411, 6));
		periapsisAltitude.text = (double.IsInfinity(FlightGlobals.ActiveVessel.orbit.PeA) ? cacheAutoLOC_462439 : KSPUtil.PrintSI(orbitToDisplay.PeA, cacheAutoLOC_7001411, 6));
		if (IsTimeAndPeriodUnlocked())
		{
			apoapsisTime.text = (double.IsInfinity(orbitToDisplay.timeToAp) ? cacheAutoLOC_462439 : (cacheAutoLOC_6002332 + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - (orbitToDisplay.StartUT + orbitToDisplay.timeToAp), 3, explicitPositive: true)));
			periapsisTime.text = (double.IsInfinity(orbitToDisplay.timeToPe) ? cacheAutoLOC_462439 : (cacheAutoLOC_6002332 + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - (orbitToDisplay.StartUT + orbitToDisplay.timeToPe), 3, explicitPositive: true)));
			orbitPeriod.text = (double.IsInfinity(orbitToDisplay.period) ? cacheAutoLOC_462439 : KSPUtil.PrintTime(orbitToDisplay.period, 4, explicitPositive: false));
		}
		else
		{
			apoapsisTime.text = cacheAutoLOC_215362;
			periapsisTime.text = cacheAutoLOC_215362;
			orbitPeriod.text = cacheAutoLOC_215362;
		}
	}

	public virtual bool IsTimeAndPeriodUnlocked()
	{
		return spaceDiscoveryUnlocked;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_462439 = Localizer.Format("#autoLOC_462439");
		cacheAutoLOC_7001411 = Localizer.Format("#autoLOC_7001411");
		cacheAutoLOC_6002332 = Localizer.Format("#autoLOC_6002332").Replace("-", " ");
		cacheAutoLOC_215362 = Localizer.Format("#autoLOC_215362").ToUpper();
	}
}
