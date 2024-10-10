using System.Collections.Generic;
using ns12;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManeuverNodeEditorTabVectorInput : ManeuverNodeEditorTab
{
	[SerializeField]
	public TMP_InputField proRetrogradeField;

	public InputFieldHandler proRetrogradeFieldHandler;

	[SerializeField]
	public TMP_InputField normalField;

	public InputFieldHandler normalFieldHandler;

	[SerializeField]
	public TMP_InputField radialField;

	public InputFieldHandler radialFieldHandler;

	[SerializeField]
	public TMP_InputField timeField;

	public TimeInputFieldHandler timeFieldHander;

	[SerializeField]
	public Toggle utInputMode;

	[SerializeField]
	public TextMeshProUGUI timeUnits;

	[SerializeField]
	public TooltipController_Text utTooltip;

	public Vector3d newDeltaV;

	public ManeuverNodeEditorTabButton editorTab;

	public bool isEditing;

	public bool UTMode = true;

	public static string cacheAutoLOC_6002317;

	public static string cacheAutoLOC_6001498;

	public static string cacheAutoLOC_6005047;

	public static string cacheAutoLOC_6005048;

	public void Start()
	{
		if ((bool)FlightUIModeController.Instance)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		List<ManeuverNodeEditorTab> rightTabs = mannodeEditorManager.RightTabs;
		for (int i = 0; i < rightTabs.Count; i++)
		{
			if (rightTabs[i].tabName == "VectorInput")
			{
				editorTab = mannodeEditorManager.GetTabToggle(i, ManeuverNodeEditorTabPosition.RIGHT);
			}
		}
		proRetrogradeFieldHandler = new InputFieldHandler(this, proRetrogradeField, () => (mannodeEditorManager.SelectedManeuverNode == null) ? 0.0 : mannodeEditorManager.SelectedManeuverNode.DeltaV.z, delegate(double value)
		{
			mannodeEditorManager.ModifyBurnVector(NavBallVector.PROGRADE, value);
		});
		normalFieldHandler = new InputFieldHandler(this, normalField, () => (mannodeEditorManager.SelectedManeuverNode == null) ? 0.0 : mannodeEditorManager.SelectedManeuverNode.DeltaV.y, delegate(double value)
		{
			mannodeEditorManager.ModifyBurnVector(NavBallVector.NORMAL, value);
		});
		radialFieldHandler = new InputFieldHandler(this, radialField, () => (mannodeEditorManager.SelectedManeuverNode == null) ? 0.0 : mannodeEditorManager.SelectedManeuverNode.DeltaV.x, delegate(double value)
		{
			mannodeEditorManager.ModifyBurnVector(NavBallVector.RADIAL, value);
		});
		timeFieldHander = new TimeInputFieldHandler(this, timeField, () => (mannodeEditorManager.SelectedManeuverNode == null) ? 0.0 : mannodeEditorManager.SelectedManeuverNode.double_0, delegate(double value)
		{
			mannodeEditorManager.SelectedManeuverNode.double_0 = value;
			mannodeEditorManager.SelectedManeuverNode.attachedGizmo.double_0 = value;
			FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
		});
		editorTab.toggle.onValueChanged.AddListener(OnTabToggleClicked);
		utInputMode.onValueChanged.AddListener(ChangeInputMode);
		UpdateUIElements();
		utTooltip.textString = cacheAutoLOC_6005048;
	}

	public override void UpdateUIElements()
	{
		if (!isEditing && mannodeEditorManager != null && mannodeEditorManager.SelectedManeuverNode != null)
		{
			proRetrogradeFieldHandler.UpdateField();
			normalFieldHandler.UpdateField();
			radialFieldHandler.UpdateField();
			timeFieldHander.UpdateField();
		}
	}

	public void OnFieldSelected(string text)
	{
		if (!FlightGlobals.ActiveVessel.isEVA)
		{
			isEditing = true;
			InputLockManager.SetControlLock("mannodeInputEdit");
		}
	}

	public void OnFieldDeselected(string text)
	{
		isEditing = false;
		InputLockManager.RemoveControlLock("mannodeInputEdit");
	}

	public override void SetInitialValues()
	{
		if (mannodeEditorManager == null)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		_ = mannodeEditorManager.SelectedManeuverNode;
	}

	public void OnTabToggleClicked(bool state)
	{
		if (mannodeEditorManager == null)
		{
			mannodeEditorManager = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		if (state)
		{
			proRetrogradeFieldHandler.UpdateField();
			normalFieldHandler.UpdateField();
			radialFieldHandler.UpdateField();
			timeFieldHander.UpdateField();
		}
	}

	public void ChangeInputMode(bool state)
	{
		UTMode = state;
		timeFieldHander.ChangeInputMode(state);
		if (state)
		{
			timeUnits.gameObject.SetActive(value: false);
			utTooltip.textString = cacheAutoLOC_6005048;
		}
		else
		{
			timeUnits.gameObject.SetActive(value: true);
			utTooltip.textString = cacheAutoLOC_6005047;
		}
	}

	public void OnDestroy()
	{
		editorTab.toggle.onValueChanged.RemoveListener(OnTabToggleClicked);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6002317 = Localizer.Format("#autoLOC_6002317");
		cacheAutoLOC_6001498 = Localizer.Format("#autoLOC_6001498").Replace("-", " ");
		cacheAutoLOC_6005047 = Localizer.Format("#autoLOC_6005047");
		cacheAutoLOC_6005048 = Localizer.Format("#autoLOC_6005048");
	}
}
