using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionValidationEntry : MonoBehaviour
{
	[SerializeField]
	public TMP_Text validationText;

	[SerializeField]
	public Toggle toggleComponent;

	public MissionValidationTestResult result;

	public MissionValidationEntry Create(MissionValidationTestResult result, ToggleGroup group)
	{
		MissionValidationEntry missionValidationEntry = Object.Instantiate(this);
		missionValidationEntry.transform.SetParent(group.transform, worldPositionStays: false);
		missionValidationEntry.result = result;
		missionValidationEntry.toggleComponent.group = group;
		return missionValidationEntry;
	}

	public void Awake()
	{
		toggleComponent.onValueChanged.AddListener(OnToggleValueChange);
	}

	public void Start()
	{
		validationText.text = "";
		if (result != null)
		{
			TMP_Text tMP_Text = validationText;
			tMP_Text.text = tMP_Text.text + "<b><color=#" + ColorUtility.ToHtmlStringRGB(MissionEditorValidator.GetValidationColor(result.status)) + ">" + result.status.displayDescription() + "</color></b>";
			tMP_Text = validationText;
			tMP_Text.text = tMP_Text.text + " <color=#a0a0a0>[" + result.type + "] " + Localizer.Format(result.nodeName) + "</color>\n";
			TMP_Text tMP_Text2 = validationText;
			tMP_Text2.text = tMP_Text2.text + "<pos=25>" + result.message;
		}
		else
		{
			validationText.text = "#autoLOC_8200081";
		}
	}

	public void OnToggleValueChange(bool value)
	{
		if (value && result != null && MissionEditorLogic.Instance.EditorMission.nodes.ContainsKey(result.nodeId))
		{
			MENode mENode = MissionEditorLogic.Instance.EditorMission.nodes[result.nodeId];
			if (mENode != null)
			{
				MENodeCanvas.Instance.UICamera.transform.localPosition = mENode.editorPosition;
				mENode.guiNode.Select(deselectOtherNodes: true);
			}
		}
	}
}
