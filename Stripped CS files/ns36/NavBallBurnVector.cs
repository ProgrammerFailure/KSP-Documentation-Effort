using System;
using System.Collections.Generic;
using ns11;
using ns2;
using ns9;
using SoftMasking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns36;

public class NavBallBurnVector : MonoBehaviour
{
	public Transform vectorProgr;

	public Transform indicationArrow;

	public NavBall navBall;

	public ns11.HelixGauge deltaVGauge;

	public ns11.HelixGauge deltaVGaugeRed;

	public Transform StageMarkersParent;

	public TextMeshProUGUI ebtText;

	public TextMeshProUGUI TdnText;

	public TextMeshProUGUI readoutText;

	public TextMeshProUGUI sbtText;

	public TextMeshProUGUI bbPercentText;

	public Button btnWarpTo;

	public Button btnAccept;

	public Button btnDismiss;

	public Button btnPercentMinus;

	public Button btnPercentPlus;

	public UIPanelTransitionToggle[] navBallCollapseGroups;

	public double dVremaining;

	public double estimatedBurnTime;

	public double startBurnTime;

	public float accuracy;

	public Vector3 direction;

	public PatchedConicSolver solver;

	[SerializeField]
	public GameObject stageMarkerPrefab;

	[SerializeField]
	public RectTransform stageMarkerMaskRect;

	public DictionaryValueList<int, RotationalGaugeOffsetMarker> stageMarkers;

	[SerializeField]
	public bool enoughDeltaV;

	[SerializeField]
	public int burnBeforePercent;

	[SerializeField]
	public GameObject burnBeforeObject;

	public static double epsilon = 1E-10;

	public bool nodeDeltaVChanged;

	public double nodeDeltaV;

	public bool vesselDeltaVChanged;

	public double vesselTotalDeltaV;

	[SerializeField]
	public List<int> stagesProcessed;

	[SerializeField]
	public Transform EBTextPosBasicMode;

	[SerializeField]
	public Transform EBTextPosExtMode;

	[SerializeField]
	public Transform TdnTextPosBasicMode;

	[SerializeField]
	public Transform TdnTextPosExtMode;

	[SerializeField]
	public double startBurn;

	public static string cacheAutoLOC_258912;

	public static string cacheAutoLOC_460852;

	public void Awake()
	{
		GameEvents.onVesselChange.Add(OnVesselSwitch);
		GameEvents.OnGameSettingsApplied.Add(onGameSettingsApplied);
		btnPercentMinus.onClick.AddListener(btnPercentMinusClick);
		btnPercentPlus.onClick.AddListener(btnPercentPlusClick);
		stageMarkers = new DictionaryValueList<int, RotationalGaugeOffsetMarker>();
		stagesProcessed = new List<int>();
		burnBeforePercent = (int)Math.Round((double)(GameSettings.DELTAV_BURN_PERCENTAGE * 100f) / 10.0) * 10;
		bbPercentText.text = Localizer.Format("#autoLOC_8002209", burnBeforePercent);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselChange.Remove(OnVesselSwitch);
		GameEvents.OnGameSettingsApplied.Remove(onGameSettingsApplied);
		btnPercentMinus.onClick.RemoveListener(btnPercentMinusClick);
		btnPercentPlus.onClick.RemoveListener(btnPercentPlusClick);
	}

	public void Start()
	{
		vectorProgr.gameObject.SetActive(value: false);
		ebtText.enabled = false;
		TdnText.enabled = false;
		sbtText.enabled = false;
		deltaVGauge.gameObject.SetActive(value: false);
		deltaVGaugeRed.gameObject.SetActive(value: false);
		indicationArrow.gameObject.SetActive(value: false);
		burnBeforeObject.SetActive(value: false);
		btnWarpTo.onClick.AddListener(OnWarpToButton);
		btnAccept.onClick.AddListener(OnDismissButton);
		btnDismiss.onClick.AddListener(OnDismissButton);
		btnDismiss.interactable = true;
		nodeDeltaVChanged = true;
		nodeDeltaV = double.MaxValue;
	}

	public void OnVesselSwitch(Vessel v)
	{
		solver = v.patchedConicSolver;
		ClearStageMarkers();
	}

	public void onGameSettingsApplied()
	{
		if (solver != null && solver.maneuverNodes.Count != 0 && navBallIsExpanded())
		{
			sbtText.enabled = GameSettings.EXTENDED_BURNTIME;
			burnBeforeObject.SetActive(GameSettings.EXTENDED_BURNTIME);
			btnPercentMinus.interactable = GameSettings.EXTENDED_BURNTIME;
			btnPercentPlus.interactable = GameSettings.EXTENDED_BURNTIME;
			ebtText.transform.SetParent(GameSettings.EXTENDED_BURNTIME ? EBTextPosExtMode : EBTextPosBasicMode);
			ebtText.transform.localPosition = Vector3.zero;
			TdnText.transform.SetParent(GameSettings.EXTENDED_BURNTIME ? TdnTextPosExtMode : TdnTextPosBasicMode);
			TdnText.transform.localPosition = Vector3.zero;
		}
	}

	public void SetAdvancedMode(bool mode)
	{
		GameSettings.EXTENDED_BURNTIME = mode;
		onGameSettingsApplied();
	}

	public void LateUpdate()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		if (solver != null && solver.maneuverNodes.Count != 0 && navBallIsExpanded())
		{
			if (!deltaVGauge.gameObject.activeInHierarchy)
			{
				vectorProgr.gameObject.SetActive(value: true);
				deltaVGauge.gameObject.SetActive(value: true);
				ebtText.enabled = true;
				TdnText.enabled = true;
				sbtText.enabled = GameSettings.EXTENDED_BURNTIME;
				burnBeforeObject.SetActive(GameSettings.EXTENDED_BURNTIME);
				ebtText.transform.SetParent(GameSettings.EXTENDED_BURNTIME ? EBTextPosExtMode : EBTextPosBasicMode);
				ebtText.transform.localPosition = Vector3.zero;
				TdnText.transform.SetParent(GameSettings.EXTENDED_BURNTIME ? TdnTextPosExtMode : TdnTextPosBasicMode);
				TdnText.transform.localPosition = Vector3.zero;
				btnPercentMinus.interactable = GameSettings.EXTENDED_BURNTIME;
				btnPercentPlus.interactable = GameSettings.EXTENDED_BURNTIME;
			}
			direction = solver.maneuverNodes[0].GetBurnVector(solver.maneuverNodes[0].patch);
			dVremaining = direction.magnitude;
			deltaVGauge.currentValue = (float)dVremaining;
			if (Math.Abs(nodeDeltaV - solver.maneuverNodes[0].DeltaV.magnitude) > epsilon)
			{
				nodeDeltaVChanged = true;
				nodeDeltaV = solver.maneuverNodes[0].DeltaV.magnitude;
				deltaVGauge.MaxValue = (float)nodeDeltaV;
				if (FlightGlobals.ActiveVessel.VesselDeltaV != null && FlightGlobals.ActiveVessel.VesselDeltaV.IsReady)
				{
					for (int i = 0; i < stageMarkers.Count; i++)
					{
						if (stageMarkers.TryGetValue(stageMarkers.KeyAt(i), out var val))
						{
							val.maxValue = deltaVGauge.MaxValue;
							double value = (double)val.maxValue - ((double)val.maxValue - nodeDeltaV) - val.StageDV;
							val.SetValue(value);
						}
					}
				}
			}
			double num = ((FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0f && StageManager.CurrentStage <= StageManager.LastStage) ? FlightGlobals.ActiveVessel.VesselDeltaV.TotalDeltaVActual : ((FlightGlobals.ActiveVessel.atmDensity > 0.0) ? FlightGlobals.ActiveVessel.VesselDeltaV.TotalDeltaVASL : FlightGlobals.ActiveVessel.VesselDeltaV.TotalDeltaVVac));
			if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.VesselDeltaV != null && Math.Abs(vesselTotalDeltaV - num) > epsilon)
			{
				vesselDeltaVChanged = true;
				vesselTotalDeltaV = num;
			}
			if (FlightGlobals.ActiveVessel.VesselDeltaV != null && !FlightGlobals.ActiveVessel.VesselDeltaV.SimulationRunning && FlightGlobals.ActiveVessel.VesselDeltaV.IsReady)
			{
				estimatedBurnTime = CalculateBurnTime();
				UpdateDVStageMarkers();
			}
			UpdateNavballBurnVectors();
			accuracy = 1f - deltaVGauge.currentValue / deltaVGauge.MaxValue;
			if (accuracy > 0.99f)
			{
				if (!btnAccept.gameObject.activeSelf)
				{
					btnAccept.gameObject.SetActive(value: true);
				}
				if (btnDismiss.gameObject.activeSelf)
				{
					btnDismiss.gameObject.SetActive(value: false);
				}
			}
			else
			{
				if (btnAccept.gameObject.activeSelf)
				{
					btnAccept.gameObject.SetActive(value: false);
				}
				if (!btnDismiss.gameObject.activeSelf)
				{
					btnDismiss.gameObject.SetActive(value: true);
				}
			}
			if (!btnDismiss.interactable)
			{
				btnDismiss.interactable = true;
			}
			UpdateBurnUIText();
			AutoStepNodes();
			btnWarpTo.interactable = startBurn > (double)GameSettings.WARP_TO_MANNODE_MARGIN && InputLockManager.GetControlLock("TimeWarpTo") == ControlTypes.None;
		}
		else
		{
			if (deltaVGauge.gameObject.activeSelf)
			{
				vectorProgr.gameObject.SetActive(value: false);
				indicationArrow.gameObject.SetActive(value: false);
				deltaVGauge.gameObject.SetActive(value: false);
				ebtText.enabled = false;
				TdnText.enabled = false;
				sbtText.enabled = false;
				burnBeforeObject.SetActive(value: false);
			}
			if (deltaVGaugeRed.gameObject.activeSelf)
			{
				deltaVGaugeRed.gameObject.SetActive(value: false);
			}
			ClearStageMarkers();
			startBurn = 0.0;
			nodeDeltaV = double.MaxValue;
		}
	}

	public void UpdateNavballBurnVectors()
	{
		vectorProgr.localPosition = navBall.attitudeGymbal * (direction.normalized * navBall.VectorUnitScale);
		if (direction.magnitude > navBall.VectorVelocityThreshold && vectorProgr.transform.localPosition.z >= navBall.VectorUnitCutoff)
		{
			if (!vectorProgr.gameObject.activeSelf)
			{
				vectorProgr.gameObject.SetActive(value: true);
			}
			if (indicationArrow.gameObject.activeSelf)
			{
				indicationArrow.gameObject.SetActive(value: false);
			}
			return;
		}
		if (vectorProgr.gameObject.activeSelf)
		{
			vectorProgr.gameObject.SetActive(value: false);
		}
		if (!vectorProgr.gameObject.activeSelf)
		{
			if (!indicationArrow.gameObject.activeSelf)
			{
				indicationArrow.gameObject.SetActive(value: true);
			}
			Vector3 localPosition = vectorProgr.localPosition;
			Vector3 localPosition2 = localPosition - Vector3.Dot(localPosition, Vector3.forward) * Vector3.forward;
			localPosition2.Normalize();
			localPosition2 *= navBall.VectorUnitScale * 0.6f;
			indicationArrow.localPosition = localPosition2;
			float num = 57.29578f * Mathf.Acos(localPosition2.x / Mathf.Sqrt(localPosition2.x * localPosition2.x + localPosition2.y * localPosition2.y));
			if (localPosition2.y < 0f)
			{
				num += 2f * (180f - num);
			}
			if (float.IsNaN(num))
			{
				num = 0f;
			}
			Quaternion localRotation = Quaternion.Euler(num + 90f, 270f, 90f);
			indicationArrow.localRotation = localRotation;
		}
	}

	public void UpdateBurnUIText()
	{
		string text = cacheAutoLOC_258912;
		readoutText.text = Localizer.Format("#autoLOC_460807", dVremaining.ToString("0.0"));
		ebtText.text = "<color=#98EE00>";
		if (GameSettings.DELTAV_BURN_ESTIMATE_COLORS && !enoughDeltaV)
		{
			ebtText.text = "<color=#FF0000>";
		}
		string text2 = "";
		if (double.IsInfinity(estimatedBurnTime))
		{
			text2 = text;
		}
		else if (estimatedBurnTime > 1.0)
		{
			text2 = KSPUtil.PrintTime(estimatedBurnTime, 2, explicitPositive: false);
		}
		else if (estimatedBurnTime < 1.0 && estimatedBurnTime > 0.0)
		{
			text2 = ((!(estimatedBurnTime < 0.10000000149011612)) ? (estimatedBurnTime.ToString("0.0") + Localizer.Format("#autoLOC_6002317")) : ("0.1" + Localizer.Format("#autoLOC_6002317")));
		}
		ebtText.text += Localizer.Format("#autoLOC_460808", text2);
		ebtText.text += "</color>";
		TdnText.text = Localizer.Format("#autoLOC_460809", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - solver.maneuverNodes[0].double_0, 3, explicitPositive: true));
		startBurn = 0.0;
		if (burnBeforePercent == 0)
		{
			startBurn = solver.maneuverNodes[0].double_0 - Planetarium.GetUniversalTime();
		}
		else if (burnBeforePercent == 100)
		{
			startBurn = solver.maneuverNodes[0].double_0 - Planetarium.GetUniversalTime() - estimatedBurnTime;
		}
		else
		{
			startBurn = solver.maneuverNodes[0].double_0 - Planetarium.GetUniversalTime() - startBurnTime;
		}
		solver.maneuverNodes[0].startBurnIn = startBurn;
		if (GameSettings.DELTAV_BURN_TIME_COLORS)
		{
			if (startBurn <= 0.0 && FlightGlobals.ActiveVessel.ctrlState.mainThrottle <= 0f)
			{
				sbtText.text = "<color=#FF0000>";
			}
			else if (startBurn > 0.0 && startBurn < 10.0)
			{
				sbtText.text = "<color=#FF9600>";
			}
			else
			{
				sbtText.text = "<color=#98EE00>";
			}
		}
		else
		{
			sbtText.text = "<color=#98EE00>";
		}
		TextMeshProUGUI textMeshProUGUI = sbtText;
		textMeshProUGUI.text = textMeshProUGUI.text + Localizer.Format("#autoLOC_8002210", KSPUtil.PrintTime(startBurn, 2, explicitPositive: false)) + "</color>";
	}

	public void UpdateDVStageMarkers()
	{
		VesselDeltaV vesselDeltaV = FlightGlobals.ActiveVessel.VesselDeltaV;
		if (nodeDeltaVChanged || vesselDeltaVChanged)
		{
			if (vesselTotalDeltaV < nodeDeltaV)
			{
				if (!deltaVGaugeRed.gameObject.activeSelf)
				{
					deltaVGaugeRed.gameObject.SetActive(value: true);
				}
				deltaVGaugeRed.MaxValue = (float)nodeDeltaV;
				deltaVGaugeRed.currentValue = deltaVGaugeRed.MaxValue - (deltaVGaugeRed.MaxValue - deltaVGauge.currentValue) - (float)vesselTotalDeltaV;
			}
			else if (deltaVGaugeRed.gameObject.activeSelf)
			{
				deltaVGaugeRed.gameObject.SetActive(value: false);
			}
			nodeDeltaVChanged = false;
			vesselDeltaVChanged = false;
		}
		if (vesselDeltaV.OperatingStageInfo.Count == 0)
		{
			ClearStageMarkers();
			return;
		}
		double num = dVremaining;
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		stagesProcessed.Clear();
		while (!(num <= 0.0) && num3 < vesselDeltaV.OperatingStageInfo.Count)
		{
			DeltaVStageInfo deltaVStageInfo = vesselDeltaV.OperatingStageInfo[num3];
			double num5 = ((FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0f && StageManager.CurrentStage <= deltaVStageInfo.stage) ? deltaVStageInfo.deltaVActual : ((FlightGlobals.ActiveVessel.atmDensity > 0.0) ? deltaVStageInfo.deltaVatASL : deltaVStageInfo.deltaVinVac));
			if (num5 <= 0.0)
			{
				num3++;
				continue;
			}
			num -= num5;
			if (!stageMarkers.TryGetValue(deltaVStageInfo.stage, out var val))
			{
				GameObject obj = UnityEngine.Object.Instantiate(stageMarkerPrefab);
				val = obj.GetComponent<RotationalGaugeOffsetMarker>();
				val.transform.SetParent(StageMarkersParent);
				val.transform.localPosition = Vector3.zero;
				val.transform.localScale = Vector3.one;
				val.offsetObject.transform.SetParent(StageMarkersParent);
				val.offsetObject.transform.localPosition = Vector3.zero;
				val.offsetObject.transform.localScale = Vector3.one;
				SoftMask component = obj.GetComponent<SoftMask>();
				if (component != null)
				{
					component.separateMask = stageMarkerMaskRect;
				}
				stageMarkers.Add(deltaVStageInfo.stage, val);
			}
			val.maxValue = deltaVGauge.MaxValue;
			num2 += num5;
			if (Math.Abs(num2 - val.StageDV) > epsilon)
			{
				val.StageDV = num2;
				double value = (double)(val.maxValue - (val.maxValue - deltaVGauge.currentValue)) - num2;
				val.SetValue(value);
			}
			if (stagesProcessed.Count > 0)
			{
				if (stageMarkers.TryGetValue(stagesProcessed[stagesProcessed.Count - 1], out var val2) && val2 != null)
				{
					val.SetNextToValue(val2.CurrentAngle);
					num4 = ((Mathf.Abs(val.CurrentAngle - val2.CurrentAngle) < 5f) ? (num4 + 1) : 0);
				}
			}
			else
			{
				val.SetNextToValue(val.maxRot);
			}
			int opacity = 255;
			if (num4 == 2)
			{
				opacity = 127;
			}
			else if (num4 == 3)
			{
				opacity = 63;
			}
			else if (num4 > 3)
			{
				opacity = 26;
			}
			val.SetTextField(deltaVStageInfo.stage.ToString(), opacity);
			val.ToggleOffsetMarker(deltaVStageInfo.stage != vesselDeltaV.lowestStageWithDeltaV && num > 0.0);
			stagesProcessed.Add(deltaVStageInfo.stage);
			num3++;
		}
		RemoveRemainingStageMarkers(stagesProcessed);
	}

	public double CalculateBurnTime()
	{
		VesselDeltaV vesselDeltaV = FlightGlobals.ActiveVessel.VesselDeltaV;
		float num = 0f;
		double num2 = 0.0;
		startBurnTime = 0.0;
		float num3 = 0f;
		enoughDeltaV = false;
		bool flag = false;
		float num4 = 0f;
		if (burnBeforePercent != 0 && burnBeforePercent != 100)
		{
			num4 = (float)dVremaining * (float)burnBeforePercent / 100f;
		}
		else
		{
			flag = true;
		}
		for (int i = 0; i < vesselDeltaV.OperatingStageInfo.Count; i++)
		{
			DeltaVStageInfo deltaVStageInfo = vesselDeltaV.OperatingStageInfo[i];
			bool runningActive;
			double num5 = ((runningActive = FlightGlobals.ActiveVessel.ctrlState.mainThrottle > 0f && StageManager.CurrentStage <= deltaVStageInfo.stage) ? deltaVStageInfo.deltaVActual : ((FlightGlobals.ActiveVessel.atmDensity > 0.0) ? deltaVStageInfo.deltaVatASL : deltaVStageInfo.deltaVinVac));
			if (!enoughDeltaV)
			{
				if ((double)num + num5 > dVremaining)
				{
					double num6 = deltaVStageInfo.CalculateTimeRequiredDV(runningActive, (float)dVremaining - num);
					num2 += num6;
					enoughDeltaV = true;
				}
				else
				{
					num += (float)num5;
					num2 += deltaVStageInfo.stageBurnTime;
				}
			}
			if (!flag)
			{
				if (num3 + (float)num5 > num4)
				{
					double num7 = deltaVStageInfo.CalculateTimeRequiredDV(runningActive, num4 - num3);
					startBurnTime += num7;
					flag = true;
				}
				else
				{
					num3 += (float)num5;
					startBurnTime += deltaVStageInfo.stageBurnTime;
				}
			}
			if (enoughDeltaV && flag)
			{
				break;
			}
		}
		return num2;
	}

	public void AutoStepNodes()
	{
		if (solver.maneuverNodes.Count > 1 && Math.Abs(Planetarium.GetUniversalTime() - solver.maneuverNodes[0].double_0) > Math.Abs(Planetarium.GetUniversalTime() - solver.maneuverNodes[1].double_0))
		{
			solver.maneuverNodes[0].RemoveSelf();
		}
	}

	public void ClearStageMarkers()
	{
		for (int i = 0; i < stageMarkers.ValuesList.Count; i++)
		{
			stageMarkers.ValuesList[i].gameObject.DestroyGameObject();
		}
		stageMarkers.Clear();
	}

	public void RemoveRemainingStageMarkers(List<int> stagesProcessed)
	{
		int count = stageMarkers.KeysList.Count;
		while (count-- > 0)
		{
			if (!stagesProcessed.Contains(stageMarkers.KeysList[count]))
			{
				RotationalGaugeOffsetMarker rotationalGaugeOffsetMarker = stageMarkers.ValuesList[count];
				stageMarkers.Remove(stageMarkers.KeysList[count]);
				rotationalGaugeOffsetMarker.gameObject.DestroyGameObject();
			}
		}
	}

	public void btnPercentMinusClick()
	{
		if (burnBeforePercent > 0)
		{
			burnBeforePercent -= 10;
			GameSettings.DELTAV_BURN_PERCENTAGE = (float)burnBeforePercent / 100f;
			GameSettings.SaveGameSettingsOnly();
			bbPercentText.text = Localizer.Format("#autoLOC_8002209", burnBeforePercent);
		}
	}

	public void btnPercentPlusClick()
	{
		if (burnBeforePercent <= 90)
		{
			burnBeforePercent += 10;
			GameSettings.DELTAV_BURN_PERCENTAGE = (float)burnBeforePercent / 100f;
			GameSettings.SaveGameSettingsOnly();
			bbPercentText.text = Localizer.Format("#autoLOC_8002209", burnBeforePercent);
		}
	}

	public void OnDismissButton()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.MANNODE_DELETE))
		{
			if (InputLockManager.IsLocked(ControlTypes.WARPTO_LOCK))
			{
				TimeWarp.fetch.CancelAutoWarp(0);
			}
			solver.maneuverNodes[0].RemoveSelf();
			ManeuverNodeEditorManager.Instance.SetManeuverNodeInitialValues();
		}
		else
		{
			ScreenMessages.PostScreenMessage(cacheAutoLOC_460852, 3f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public void OnWarpToButton()
	{
		if (FlightGlobals.ActiveVessel.patchedConicSolver != null && FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0)
		{
			double double_ = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[0].startBurnIn + Planetarium.GetUniversalTime() - (double)GameSettings.WARP_TO_MANNODE_MARGIN;
			TimeWarp.fetch.WarpTo(double_);
		}
	}

	public bool navBallIsExpanded()
	{
		int num = navBallCollapseGroups.Length;
		do
		{
			if (num-- <= 0)
			{
				return true;
			}
		}
		while (navBallCollapseGroups[num].expanded);
		return false;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_258912 = Localizer.Format("#autoLOC_258912");
		cacheAutoLOC_460852 = Localizer.Format("#autoLOC_460852");
	}
}
