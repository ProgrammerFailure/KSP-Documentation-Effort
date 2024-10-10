using System;
using System.Collections.Generic;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionValidationDialog : MonoBehaviour
{
	public static MissionValidationDialog Instance;

	[SerializeField]
	public MissionValidationEntry validationEntryPrefab;

	[SerializeField]
	public Button btnValidate;

	[SerializeField]
	public ToggleGroup entryGroup;

	[SerializeField]
	public Button btnOK;

	[SerializeField]
	public TMP_Dropdown modeDropdown;

	public static Mission currentMission;

	public Callback afterOKCallback;

	public Callback afterCancelCallback;

	public string[] modeNames;

	public static MissionValidationDialog Display(Mission currentMission, Callback afterOKCallback = null, Callback afterCancelCallback = null)
	{
		if (Instance == null)
		{
			UnityEngine.Object @object = MissionsUtils.MEPrefab("_UI5/Dialogs/MissionValidationDialog/prefabs/MissionValidationDialog.prefab");
			if (@object == null)
			{
				Debug.LogError("[MissionBriefingDialog]: Unable to load the Asset");
				return null;
			}
			Instance = ((GameObject)UnityEngine.Object.Instantiate(@object)).GetComponent<MissionValidationDialog>();
		}
		Instance.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		MissionValidationDialog.currentMission = currentMission;
		Instance.PrintValidationResults(currentMission.ValidationResults);
		Instance.afterOKCallback = afterOKCallback;
		Instance.afterCancelCallback = afterCancelCallback;
		return Instance;
	}

	public void Start()
	{
		BuildDropDown();
		btnOK.onClick.AddListener(OnOK);
		btnValidate.onClick.AddListener(OnButtonValidate);
	}

	public void BuildDropDown()
	{
		modeNames = Enum.GetNames(typeof(ValidatorMode));
		if (modeNames.Length == 0)
		{
			return;
		}
		modeDropdown.options.Clear();
		int value = 0;
		for (int i = 0; i < modeNames.Length; i++)
		{
			modeDropdown.options.Add(new TMP_Dropdown.OptionData(((ValidatorMode)i).displayDescription()));
			if (modeNames[i] == GameSettings.MISSION_VALIDATOR_MODE.ToString())
			{
				value = i;
			}
		}
		modeDropdown.value = value;
		modeDropdown.RefreshShownValue();
	}

	public void OnDestroy()
	{
		btnOK.onClick.RemoveListener(OnOK);
		btnValidate.onClick.RemoveListener(OnButtonValidate);
	}

	public void OnCancel()
	{
		if (afterCancelCallback != null)
		{
			afterCancelCallback();
		}
		DestroyDialog();
	}

	public void OnOK()
	{
		ValidatorMode value = (ValidatorMode)modeDropdown.value;
		if (value != GameSettings.MISSION_VALIDATOR_MODE)
		{
			GameSettings.MISSION_VALIDATOR_MODE = value;
			MissionEditorValidator.Instance.mode = value;
			GameSettings.SaveGameSettingsOnly();
		}
		if (afterOKCallback != null)
		{
			afterOKCallback();
		}
		DestroyDialog();
	}

	public void DestroyDialog()
	{
		UIMasterController.Instance.UnregisterNonModalDialog(GetComponent<CanvasGroup>());
		UnityEngine.Object.Destroy(base.gameObject);
		currentMission = null;
	}

	public void OnButtonValidate()
	{
		if (MissionEditorLogic.Instance == null)
		{
			Debug.LogError("Cant access the MissionEditorLogic Instance to do the validation");
		}
		MissionEditorLogic.Instance.RunValidator();
		PrintValidationResults(currentMission.ValidationResults);
	}

	public void PrintValidationResults(List<MissionValidationTestResult> results, bool includePasses = true)
	{
		bool flag = true;
		ClearValidationResults();
		if (results != null)
		{
			for (int i = 0; i < results.Count; i++)
			{
				MissionValidationTestResult missionValidationTestResult = results[i];
				if (includePasses || missionValidationTestResult.status != 0)
				{
					flag = false;
					validationEntryPrefab.Create(missionValidationTestResult, entryGroup);
				}
			}
		}
		if (flag)
		{
			validationEntryPrefab.Create(null, entryGroup);
		}
	}

	public void ClearValidationResults()
	{
		int i = 0;
		for (int childCount = entryGroup.transform.childCount; i < childCount; i++)
		{
			UnityEngine.Object.Destroy(entryGroup.transform.GetChild(i).gameObject);
		}
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			OnCancel();
		}
	}
}
