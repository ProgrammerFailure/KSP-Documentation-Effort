using System;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManeuverNodeEditorTabVectorHandles : ManeuverNodeEditorTab
{
	[SerializeField]
	public Slider precisionSlider;

	[SerializeField]
	public TextMeshProUGUI sliderTimeDVString;

	[SerializeField]
	public Button prevOrbitButton;

	[SerializeField]
	public Button nextOrbitButton;

	[SerializeField]
	public Button progradeVectorHandle;

	[SerializeField]
	public Button retrogradeVectorHandle;

	[SerializeField]
	public Button normalVectorHandle;

	[SerializeField]
	public Button antiNormalVectorHandle;

	[SerializeField]
	public Button radialInVectorHandle;

	[SerializeField]
	public Button radialOutVectorHandle;

	[SerializeField]
	public Button timeStepUp;

	[SerializeField]
	public Button timeStepDown;

	public double baseVectorStepValue = 1.0;

	public double baseTimeStepValue = 10.0;

	public double vectorPullAmount;

	public int exponent;

	public float multiplier;

	public static string cacheAutoLOC_6002317;

	public static string cacheAutoLOC_7001415;

	public void Start()
	{
		if ((bool)FlightUIModeController.Instance)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		precisionSlider.onValueChanged.AddListener(OnPrecisionValueChanged);
		prevOrbitButton.onClick.AddListener(PrevOrbitLoop);
		nextOrbitButton.onClick.AddListener(NextOrbitLoop);
		progradeVectorHandle.onClick.AddListener(ProgradeStepUp);
		retrogradeVectorHandle.onClick.AddListener(RetrogradeStepUp);
		normalVectorHandle.onClick.AddListener(NormalStepUp);
		antiNormalVectorHandle.onClick.AddListener(AntiNormalStepUp);
		radialInVectorHandle.onClick.AddListener(RadialInStepUp);
		radialOutVectorHandle.onClick.AddListener(RadialOutStepUp);
		timeStepUp.onClick.AddListener(TimeStepUp);
		timeStepDown.onClick.AddListener(TimeStepDown);
		sliderTimeDVString.text = baseTimeStepValue.ToString("F2") + " " + cacheAutoLOC_6002317 + "\n" + baseVectorStepValue.ToString("F2") + " " + cacheAutoLOC_7001415;
	}

	public override void SetInitialValues()
	{
	}

	public override void UpdateUIElements()
	{
		if (mannodeEditorManager.SelectedManeuverNode != null && mannodeEditorManager.SelectedManeuverNode.attachedGizmo != null)
		{
			prevOrbitButton.interactable = mannodeEditorManager.SelectedManeuverNode.attachedGizmo.PreviousOrbitPossible();
			nextOrbitButton.interactable = mannodeEditorManager.SelectedManeuverNode.attachedGizmo.NextOrbitPossible();
		}
	}

	public void SetHandlesSensitivity()
	{
		progradeVectorHandle.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = baseVectorStepValue;
		retrogradeVectorHandle.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = 0.0 - baseVectorStepValue;
		normalVectorHandle.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = baseVectorStepValue;
		antiNormalVectorHandle.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = 0.0 - baseVectorStepValue;
		radialInVectorHandle.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = 0.0 - baseVectorStepValue;
		radialOutVectorHandle.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = baseVectorStepValue;
		timeStepDown.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = 0.0 - baseVectorStepValue;
		timeStepUp.GetComponent<ManeuverNodeEditorVectorHandle>().vectorModifyRate = baseVectorStepValue;
	}

	public void OnPrecisionValueChanged(float newValue)
	{
		precisionSlider.value = newValue;
		multiplier = precisionSlider.value % 3f;
		multiplier = ((multiplier == 0f) ? 5f : multiplier);
		exponent = (int)((precisionSlider.value - 1f) / 3f) - 1;
		baseVectorStepValue = (double)multiplier * Math.Pow(10.0, exponent - 1);
		baseTimeStepValue = (double)multiplier * Math.Pow(10.0, exponent);
		sliderTimeDVString.text = baseTimeStepValue.ToString("F2") + " " + cacheAutoLOC_6002317 + "\n" + baseVectorStepValue.ToString("F2") + " " + cacheAutoLOC_7001415;
		SetHandlesSensitivity();
		mannodeEditorManager.usage.precisionSlider++;
	}

	public void NextOrbitLoop()
	{
		mannodeEditorManager.usage.orbitSelection++;
		mannodeEditorManager.SelectedManeuverNode.attachedGizmo.NextOrbit();
	}

	public void PrevOrbitLoop()
	{
		mannodeEditorManager.usage.orbitSelection++;
		mannodeEditorManager.SelectedManeuverNode.attachedGizmo.PreviousOrbit();
	}

	public void TimeStepUp()
	{
		mannodeEditorManager.usage.utHandle++;
		mannodeEditorManager.SelectedManeuverNode.double_0 += baseTimeStepValue;
		mannodeEditorManager.SelectedManeuverNode.attachedGizmo.double_0 = mannodeEditorManager.SelectedManeuverNode.double_0;
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void TimeStepDown()
	{
		mannodeEditorManager.usage.utHandle++;
		mannodeEditorManager.SelectedManeuverNode.double_0 -= baseTimeStepValue;
		mannodeEditorManager.SelectedManeuverNode.attachedGizmo.double_0 = mannodeEditorManager.SelectedManeuverNode.double_0;
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void ProgradeStepUp()
	{
		vectorPullAmount = mannodeEditorManager.SelectedManeuverNode.DeltaV.z;
		vectorPullAmount += baseVectorStepValue;
		mannodeEditorManager.usage.vectorHandle++;
		mannodeEditorManager.ModifyBurnVector(NavBallVector.PROGRADE, vectorPullAmount);
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void RetrogradeStepUp()
	{
		vectorPullAmount = mannodeEditorManager.SelectedManeuverNode.DeltaV.z;
		vectorPullAmount -= baseVectorStepValue;
		mannodeEditorManager.usage.vectorHandle++;
		mannodeEditorManager.ModifyBurnVector(NavBallVector.PROGRADE, vectorPullAmount);
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void NormalStepUp()
	{
		vectorPullAmount = mannodeEditorManager.SelectedManeuverNode.DeltaV.y;
		vectorPullAmount += baseVectorStepValue;
		mannodeEditorManager.usage.vectorHandle++;
		mannodeEditorManager.ModifyBurnVector(NavBallVector.NORMAL, vectorPullAmount);
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void AntiNormalStepUp()
	{
		vectorPullAmount = mannodeEditorManager.SelectedManeuverNode.DeltaV.y;
		vectorPullAmount -= baseVectorStepValue;
		mannodeEditorManager.usage.vectorHandle++;
		mannodeEditorManager.ModifyBurnVector(NavBallVector.NORMAL, vectorPullAmount);
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void RadialInStepUp()
	{
		vectorPullAmount = mannodeEditorManager.SelectedManeuverNode.DeltaV.x;
		vectorPullAmount -= baseVectorStepValue;
		mannodeEditorManager.usage.vectorHandle++;
		mannodeEditorManager.ModifyBurnVector(NavBallVector.RADIAL, vectorPullAmount);
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void RadialOutStepUp()
	{
		vectorPullAmount = mannodeEditorManager.SelectedManeuverNode.DeltaV.x;
		vectorPullAmount += baseVectorStepValue;
		mannodeEditorManager.usage.vectorHandle++;
		mannodeEditorManager.ModifyBurnVector(NavBallVector.RADIAL, vectorPullAmount);
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6002317 = Localizer.Format("#autoLOC_6002317");
		cacheAutoLOC_7001415 = Localizer.Format("#autoLOC_7001415");
	}
}
