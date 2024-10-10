using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MissionEditorValidator : MonoBehaviour
{
	[SerializeField]
	public List<MEValidityHighlighColor> highlightColors;

	[SerializeField]
	public ValidationColors validationColors;

	public ValidatorMode mode = ValidatorMode.AutoAfterRun;

	public bool highlightGUINodes = true;

	public static MissionEditorValidator Instance { get; set; }

	public bool IsValid
	{
		get
		{
			int num = 0;
			while (true)
			{
				if (num < Results.Count)
				{
					if (Results[num].status != 0)
					{
						break;
					}
					num++;
					continue;
				}
				return true;
			}
			return false;
		}
	}

	public ValidationStatus Status
	{
		get
		{
			if (Results == null)
			{
				return ValidationStatus.None;
			}
			ValidationStatus result = ValidationStatus.Pass;
			for (int i = 0; i < Results.Count; i++)
			{
				if (Results[i].status == ValidationStatus.Warn)
				{
					result = ValidationStatus.Warn;
				}
				if (Results[i].status == ValidationStatus.Fail)
				{
					result = ValidationStatus.Fail;
					break;
				}
			}
			return result;
		}
	}

	public Mission Mission { get; set; }

	public List<MissionValidationTestResult> Results { get; set; }

	public static ValidatorMode Mode
	{
		get
		{
			if (Instance != null)
			{
				return Instance.mode;
			}
			return ValidatorMode.Manual;
		}
	}

	public void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		Instance = this;
		mode = GameSettings.MISSION_VALIDATOR_MODE;
	}

	public void Start()
	{
	}

	public static Color GetStatusColor()
	{
		if (Instance != null)
		{
			return Instance.Status switch
			{
				ValidationStatus.Pass => Instance.validationColors.passColor, 
				ValidationStatus.Warn => Instance.validationColors.warnColor, 
				ValidationStatus.Fail => Instance.validationColors.failColor, 
				_ => Instance.validationColors.offColor, 
			};
		}
		return Instance.validationColors.offColor;
	}

	public static Color GetValidationColor(ValidationStatus status)
	{
		if (Instance != null)
		{
			int num = 0;
			while (true)
			{
				if (num < Instance.highlightColors.Count)
				{
					if (Instance.highlightColors[num].status == status)
					{
						break;
					}
					num++;
					continue;
				}
				return Color.red;
			}
			return Instance.highlightColors[num].highlightColor;
		}
		Debug.LogWarning("[MissionEditorLogic] Not instantiated - returning red");
		return Color.red;
	}

	public static void ResetValidator()
	{
		if (Instance != null)
		{
			Instance.Mission = null;
			Instance.Results = null;
		}
	}

	public static void RunValidationOnParamChange()
	{
		if (!(Instance == null) && !(Instance.Mission == null) && (Mode == ValidatorMode.AutoAfterRun || Mode == ValidatorMode.FullAuto) && (Mode != ValidatorMode.AutoAfterRun || Instance.Mission.hasBeenValidated))
		{
			RunValidation(Instance.Mission);
		}
	}

	public static void RunValidation(Mission mission)
	{
		if (Instance == null)
		{
			Debug.LogError("[MissionEditorValidator] Insatnce is null - unable to run the validator");
			return;
		}
		Instance.Mission = mission;
		Instance.Results = new List<MissionValidationTestResult>();
		List<MENode>.Enumerator listEnumerator = Instance.Mission.nodes.GetListEnumerator();
		try
		{
			while (listEnumerator.MoveNext())
			{
				MENode current = listEnumerator.Current;
				if (current.guiNode != null)
				{
					current.guiNode.ClearValidityIndicators();
				}
				if (current.isStartNode)
				{
					Instance.Mission.situation.RunValidationWrapper(Instance);
				}
				else
				{
					current.RunValidationWrapper(Instance);
				}
			}
		}
		finally
		{
			listEnumerator.Dispose();
		}
		Instance.Mission.validationResults = Instance.Results;
		Instance.Mission.hasBeenValidated = true;
		MissionEditorLogic.UpdateValidationLed();
	}

	public void AddResults(List<MissionValidationTestResult> results)
	{
		Results.AddRange(results);
	}

	public void AddNodePass(MENode node, string message)
	{
		AddNodeResult(node, ValidationStatus.Pass, message);
	}

	public void AddNodeWarn(MENode node, string message)
	{
		AddNodeResult(node, ValidationStatus.Warn, message);
	}

	public void AddNodeFail(MENode node, string message)
	{
		AddNodeResult(node, ValidationStatus.Fail, message);
	}

	public void AddNodeResult(MENode node, ValidationStatus result, string message)
	{
		int num = 0;
		while (true)
		{
			if (num < Results.Count)
			{
				if (!(Results[num].nodeId == node.id) || Results[num].status != result || !(Results[num].message == message))
				{
					num++;
					continue;
				}
				break;
			}
			MissionValidationTestResult missionValidationTestResult = new MissionValidationTestResult
			{
				nodeId = node.id,
				nodeName = node.Title,
				status = result,
				type = Localizer.Format("#autoLOC_8006049"),
				message = message
			};
			Results.Add(missionValidationTestResult);
			if (highlightGUINodes && result != 0 && node.guiNode != null)
			{
				node.guiNode.SetValidityIndicators(missionValidationTestResult);
			}
			break;
		}
	}

	public static ValidatorMode NextMode()
	{
		if (Instance != null)
		{
			int num = (int)Instance.mode;
			num++;
			if (!Enum.IsDefined(typeof(ValidatorMode), num))
			{
				num = 0;
			}
			Instance.mode = (ValidatorMode)num;
			return Instance.mode;
		}
		return ValidatorMode.Manual;
	}
}
