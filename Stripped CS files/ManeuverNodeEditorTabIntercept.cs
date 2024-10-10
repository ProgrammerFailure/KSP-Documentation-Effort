using System.Collections.Generic;
using ns12;
using ns9;
using TMPro;
using UnityEngine;

public class ManeuverNodeEditorTabIntercept : ManeuverNodeEditorTab
{
	[SerializeField]
	public TextMeshProUGUI intercept1Label;

	[SerializeField]
	public TextMeshProUGUI intercept2Label;

	[SerializeField]
	public TooltipController_Text intercept1DistanceTooltip;

	[SerializeField]
	public TooltipController_Text intercept2DistanceTooltip;

	[SerializeField]
	public TooltipController_Text intercept1TimeTooltip;

	[SerializeField]
	public TooltipController_Text intercept2TimeTooltip;

	[SerializeField]
	public TooltipController_Text phaseAngleTooltip;

	public IntersectInformation intersectInfo;

	public TextMeshProUGUI intercept1DistanceLabel;

	public TextMeshProUGUI intercept1TimeLabel;

	public TextMeshProUGUI intercept2DistanceLabel;

	public TextMeshProUGUI intercept2TimeLabel;

	public TextMeshProUGUI phaseAngleLabel;

	public ManeuverNodeEditorTabButton editorTab;

	public TooltipController_Text buttonTooltip;

	public bool patchesAheadLimitOK;

	public bool approachMode;

	public bool approachStrings;

	public Orbit orbitToDisplay;

	public Orbit orbitContainingUT;

	public Vector3d clApprPos;

	public Vector3d clApprTgtPos;

	public double clApprSeparation;

	public static string cacheAutoLOC_7003285;

	public static string cacheAutoLOC_6002332;

	public static string cacheAutoLOC_7001411;

	public static string cacheAutoLOC_6005034_1;

	public static string cacheAutoLOC_6005034_2;

	public static string cacheAutoLOC_6005035_1;

	public static string cacheAutoLOC_6005035_2;

	public static string cacheAutoLOC_6005036_1;

	public static string cacheAutoLOC_6005036_2;

	public static string cacheAutoLOC_6005021_1;

	public static string cacheAutoLOC_6005021_2;

	public static string cacheAutoLOC_6005022_1;

	public static string cacheAutoLOC_6005022_2;

	public static string cacheAutoLOC_6005023_1;

	public static string cacheAutoLOC_6005023_2;

	public void Start()
	{
		SetEditorTab();
		SetInitialValues();
		UpdateUIElements();
	}

	public void SetEditorTab()
	{
		if (mannodeEditorManager == null)
		{
			mannodeEditorManager = ManeuverNodeEditorManager.Instance;
		}
		if (!(editorTab == null))
		{
			return;
		}
		List<ManeuverNodeEditorTab> leftTabs = mannodeEditorManager.LeftTabs;
		int num = 0;
		while (true)
		{
			if (num < leftTabs.Count)
			{
				if (leftTabs[num].tabName.Equals("Intercept"))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		editorTab = mannodeEditorManager.GetTabToggle(num, ManeuverNodeEditorTabPosition.LEFT);
		buttonTooltip = editorTab.tooltip;
		buttonTooltip.textString = ((FlightGlobals.ActiveVessel.targetObject != null) ? Localizer.Format("#autoLOC_6005040", FlightGlobals.ActiveVessel.targetObject.GetDisplayName()) : Localizer.Format("#autoLOC_6005040", "#autoLOC_7001219"));
	}

	public void SetIntersectOrApproach()
	{
		if (approachMode && !approachStrings)
		{
			intercept1Label.text = cacheAutoLOC_6005034_1;
			intercept2Label.text = cacheAutoLOC_6005034_2;
			intercept1DistanceTooltip.textString = cacheAutoLOC_6005035_1;
			intercept2DistanceTooltip.textString = cacheAutoLOC_6005035_2;
			intercept1TimeTooltip.textString = cacheAutoLOC_6005036_1;
			intercept2TimeTooltip.textString = cacheAutoLOC_6005036_2;
			approachStrings = true;
			phaseAngleTooltip.textString = Localizer.Format("#autoLOC_6005037", FlightGlobals.ActiveVessel.GetDisplayName(), FlightGlobals.ActiveVessel.targetObject.GetDisplayName());
		}
		else if (!approachMode && approachStrings)
		{
			intercept1Label.text = cacheAutoLOC_6005021_1;
			intercept2Label.text = cacheAutoLOC_6005021_2;
			intercept1DistanceTooltip.textString = cacheAutoLOC_6005022_1;
			intercept2DistanceTooltip.textString = cacheAutoLOC_6005022_2;
			intercept1TimeTooltip.textString = cacheAutoLOC_6005023_1;
			intercept2TimeTooltip.textString = cacheAutoLOC_6005023_2;
			approachStrings = false;
			phaseAngleTooltip.textString = Localizer.Format("#autoLOC_6005037", FlightGlobals.ActiveVessel.GetDisplayName(), FlightGlobals.ActiveVessel.targetObject.GetDisplayName());
		}
	}

	public override void SetInitialValues()
	{
		patchesAheadLimitOK = GameVariables.Instance.GetPatchesAheadLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.TrackingStation)) > 0;
		intercept1Label.text = cacheAutoLOC_6005021_1;
		intercept2Label.text = cacheAutoLOC_6005021_2;
		intercept1DistanceTooltip.textString = cacheAutoLOC_6005022_1;
		intercept2DistanceTooltip.textString = cacheAutoLOC_6005021_2;
		intercept1TimeTooltip.textString = cacheAutoLOC_6005023_1;
		intercept2TimeTooltip.textString = cacheAutoLOC_6005023_2;
		phaseAngleTooltip.textString = ((FlightGlobals.ActiveVessel.targetObject == null) ? Localizer.Format("#autoLOC_6005037", FlightGlobals.ActiveVessel.GetDisplayName(), "#autoLOC_7001219") : Localizer.Format("#autoLOC_6005037", FlightGlobals.ActiveVessel.GetDisplayName(), FlightGlobals.ActiveVessel.targetObject.GetDisplayName()));
	}

	public override void UpdateUIElements()
	{
		if (FlightGlobals.ActiveVessel.targetObject == null)
		{
			mannodeEditorManager.SetCurrentTab(0, ManeuverNodeEditorTabPosition.LEFT);
			return;
		}
		SetEditorTab();
		FlightUIMode mode = FlightUIModeController.Instance.Mode;
		if (mode != FlightUIMode.MANEUVER_EDIT)
		{
			if (FlightGlobals.ActiveVessel.orbit.patchEndTransition == Orbit.PatchTransitionType.ENCOUNTER)
			{
				orbitToDisplay = FlightGlobals.ActiveVessel.orbit.nextPatch;
				while (orbitToDisplay.nextPatch != null)
				{
					intersectInfo = OrbitUtil.CalculateIntersections(orbitToDisplay, FlightGlobals.ActiveVessel.targetObject.GetOrbit());
					if (orbitToDisplay.closestTgtApprUT > 0.0 || intersectInfo.numberOfIntersections > 0)
					{
						break;
					}
					orbitToDisplay = orbitToDisplay.nextPatch;
				}
				if (!orbitToDisplay.activePatch)
				{
					orbitToDisplay = FlightGlobals.ActiveVessel.orbit;
				}
			}
			else if (FlightGlobals.ActiveVessel.orbit.patchEndTransition == Orbit.PatchTransitionType.ESCAPE)
			{
				orbitToDisplay = FlightGlobals.ActiveVessel.orbit.nextPatch;
				while (orbitToDisplay.nextPatch != null)
				{
					intersectInfo = OrbitUtil.CalculateIntersections(orbitToDisplay, FlightGlobals.ActiveVessel.targetObject.GetOrbit());
					if (orbitToDisplay.referenceBody == FlightGlobals.ActiveVessel.targetObject.GetOrbit().referenceBody)
					{
						break;
					}
					orbitToDisplay = orbitToDisplay.nextPatch;
				}
				if (!orbitToDisplay.activePatch)
				{
					orbitToDisplay = FlightGlobals.ActiveVessel.orbit;
				}
			}
			else
			{
				orbitToDisplay = FlightGlobals.ActiveVessel.orbit;
			}
		}
		else if (mannodeEditorManager.SelectedManeuverNode != null)
		{
			orbitToDisplay = mannodeEditorManager.SelectedManeuverNode.nextPatch;
		}
		else
		{
			orbitToDisplay = FlightGlobals.ActiveVessel.orbit;
		}
		if (FlightGlobals.ActiveVessel.targetObject.GetVessel() != null)
		{
			approachMode = false;
			if (Orbit.PeApIntersects(orbitToDisplay, FlightGlobals.ActiveVessel.targetObject.GetOrbit(), 10000.0))
			{
				intersectInfo = OrbitUtil.CalculateIntersections(orbitToDisplay, FlightGlobals.ActiveVessel.targetObject.GetVessel().orbit);
				intercept1DistanceLabel.text = KSPUtil.PrintSI(intersectInfo.intersect1Distance, cacheAutoLOC_7001411, 6);
				intercept1TimeLabel.text = cacheAutoLOC_6002332 + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - intersectInfo.intersect1UT, 3, explicitPositive: true);
				if (intersectInfo.numberOfIntersections > 1)
				{
					intercept2DistanceLabel.text = KSPUtil.PrintSI(intersectInfo.intersect2Distance, cacheAutoLOC_7001411, 6);
					intercept2TimeLabel.text = cacheAutoLOC_6002332 + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - intersectInfo.intersect2UT, 3, explicitPositive: true);
				}
				else
				{
					intercept2DistanceLabel.text = cacheAutoLOC_7003285;
					intercept2TimeLabel.text = cacheAutoLOC_7003285;
				}
			}
			else
			{
				intercept1DistanceLabel.text = cacheAutoLOC_7003285;
				intercept1TimeLabel.text = cacheAutoLOC_7003285;
				intercept2DistanceLabel.text = cacheAutoLOC_7003285;
				intercept2TimeLabel.text = cacheAutoLOC_7003285;
			}
		}
		else
		{
			approachMode = true;
			intercept2DistanceLabel.text = cacheAutoLOC_7003285;
			intercept2TimeLabel.text = cacheAutoLOC_7003285;
			if (FlightUIModeController.Instance.Mode != FlightUIMode.MANEUVER_EDIT)
			{
				orbitContainingUT = orbitToDisplay;
			}
			else
			{
				orbitContainingUT = orbitToDisplay;
				while (orbitContainingUT.nextPatch != null && !(orbitContainingUT.referenceBody.name == FlightGlobals.ActiveVessel.targetObject.GetName()))
				{
					orbitContainingUT = orbitContainingUT.nextPatch;
				}
				if (!orbitContainingUT.activePatch)
				{
					for (int i = 0; i < FlightGlobals.ActiveVessel.patchedConicRenderer.flightPlanRenders.Count; i++)
					{
						orbitContainingUT = FlightGlobals.ActiveVessel.patchedConicRenderer.flightPlanRenders[i].patch;
						if (orbitContainingUT.referenceBody == FlightGlobals.ActiveVessel.orbit.referenceBody && orbitContainingUT.closestTgtApprUT > 0.0)
						{
							break;
						}
					}
				}
			}
			if (orbitContainingUT.referenceBody.name == FlightGlobals.ActiveVessel.targetObject.GetName())
			{
				intercept1TimeLabel.text = cacheAutoLOC_6002332 + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - (orbitContainingUT.StartUT + orbitContainingUT.timeToPe), 3, explicitPositive: true);
				intercept1DistanceLabel.text = KSPUtil.PrintSI(orbitContainingUT.PeA, cacheAutoLOC_7001411, 6);
			}
			else
			{
				intercept1TimeLabel.text = ((orbitContainingUT.closestTgtApprUT > 0.0) ? (cacheAutoLOC_6002332 + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - orbitContainingUT.closestTgtApprUT, 3, explicitPositive: true)) : cacheAutoLOC_7003285);
				clApprPos = orbitContainingUT.getRelativePositionAtUT(orbitContainingUT.closestTgtApprUT);
				clApprTgtPos = FlightGlobals.ActiveVessel.targetObject.GetOrbit().getRelativePositionAtUT(orbitContainingUT.closestTgtApprUT);
				clApprSeparation = (clApprPos - clApprTgtPos).magnitude;
				intercept1DistanceLabel.text = ((!(clApprSeparation > 0.0) || orbitContainingUT.closestTgtApprUT <= 0.0) ? cacheAutoLOC_7003285 : KSPUtil.PrintSI(clApprSeparation, cacheAutoLOC_7001411, 6));
				intercept1DistanceLabel.text = ((!(clApprSeparation > 0.0) || orbitContainingUT.closestTgtApprUT <= 0.0) ? cacheAutoLOC_7003285 : KSPUtil.PrintSI(clApprSeparation, cacheAutoLOC_7001411, 6));
			}
		}
		phaseAngleLabel.text = OrbitUtil.CurrentPhaseAngle(orbitToDisplay, FlightGlobals.ActiveVessel.targetObject.GetOrbit()).ToString("N1") + " °";
		SetIntersectOrApproach();
	}

	public override bool IsTabInteractable()
	{
		if (InputLockManager.IsLocked(ControlTypes.FLIGHTUIMODE))
		{
			return false;
		}
		if (!patchesAheadLimitOK)
		{
			if (editorTab == null)
			{
				SetEditorTab();
			}
			editorTab.GetComponent<TooltipController_Text>().textString = Localizer.Format("#autoLOC_6005046");
		}
		else if (FlightGlobals.ActiveVessel != null)
		{
			if (patchesAheadLimitOK)
			{
				return FlightGlobals.ActiveVessel.targetObject != null;
			}
			return false;
		}
		return false;
	}

	public IntersectInformation GetInterceptValues()
	{
		return intersectInfo;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7003285 = Localizer.Format("#autoLOC_7003285");
		cacheAutoLOC_6002332 = Localizer.Format("#autoLOC_6002332").Replace("-", " ");
		cacheAutoLOC_7001411 = Localizer.Format("#autoLOC_7001411");
		cacheAutoLOC_6005034_1 = Localizer.Format("#autoLOC_6005034", "1");
		cacheAutoLOC_6005034_2 = Localizer.Format("#autoLOC_6005034", "2");
		cacheAutoLOC_6005035_1 = Localizer.Format("#autoLOC_6005035", "1");
		cacheAutoLOC_6005035_2 = Localizer.Format("#autoLOC_6005035", "2");
		cacheAutoLOC_6005036_1 = Localizer.Format("#autoLOC_6005036", "1");
		cacheAutoLOC_6005036_2 = Localizer.Format("#autoLOC_6005036", "2");
		cacheAutoLOC_6005021_1 = Localizer.Format("#autoLOC_6005021", "1");
		cacheAutoLOC_6005021_2 = Localizer.Format("#autoLOC_6005021", "2");
		cacheAutoLOC_6005022_1 = Localizer.Format("#autoLOC_6005022", "1");
		cacheAutoLOC_6005022_2 = Localizer.Format("#autoLOC_6005022", "2");
		cacheAutoLOC_6005023_1 = Localizer.Format("#autoLOC_6005023", "1");
		cacheAutoLOC_6005023_2 = Localizer.Format("#autoLOC_6005023", "2");
	}
}
