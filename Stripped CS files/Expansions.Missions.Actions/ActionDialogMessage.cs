using System.Collections;
using System.ComponentModel;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using ns11;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionDialogMessage : ActionModule
{
	public enum DialogMessageArea
	{
		[Description("#autoLOC_8002036")]
		Left,
		[Description("#autoLOC_8002037")]
		Center,
		[Description("#autoLOC_8002038")]
		Right
	}

	[MEGUI_TextArea(tabStop = true, checkpointValidation = CheckpointValidationType.None, resetValue = "", guiName = "#autoLOC_8006000")]
	public string messageHeading;

	[MEGUI_TextArea(tabStop = true, checkpointValidation = CheckpointValidationType.None, resetValue = "", guiName = "#autoLOC_8006001")]
	public string message;

	[MEGUI_MissionInstructor(gapDisplay = true, guiName = "#autoLOC_8006002")]
	public MissionInstructor missionInstructor;

	[MEGUI_NumberRange(minValue = 135f, checkpointValidation = CheckpointValidationType.None, canBePinned = false, maxValue = 300f, resetValue = "135", guiName = "#autoLOC_8006004", Tooltip = "#autoLOC_8006005")]
	public int textAreaSize;

	[MEGUI_Dropdown(checkpointValidation = CheckpointValidationType.None, canBePinned = false, resetValue = "", guiName = "#autoLOC_8002034", Tooltip = "#autoLOC_8002035")]
	public DialogMessageArea screenArea;

	[MEGUI_Checkbox(guiName = "#autoLOC_8002031", Tooltip = "#autoLOC_8002032")]
	public bool pauseMission;

	[MEGUI_Checkbox(onValueChange = "OnautoClose", canBePinned = false, checkpointValidation = CheckpointValidationType.None, guiName = "#autoLOC_8002039", Tooltip = "#autoLOC_8002040")]
	public bool autoClose;

	[MEGUI_NumberRange(minValue = 10f, canBePinned = false, checkpointValidation = CheckpointValidationType.None, onControlCreated = "OnautoCloseTimeoutCreated", maxValue = 120f, resetValue = "20", guiName = "#autoLOC_8002041", Tooltip = "#autoLOC_8002042")]
	public int autoCloseTimeOut;

	public bool isBriefingMessage;

	public bool autoGrowDialogHeight;

	public MEGUIParameterNumberRange autoCloseTimeOutRange;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8006007");
		messageHeading = "";
		message = "";
		textAreaSize = 135;
		pauseMission = false;
		screenArea = DialogMessageArea.Center;
		autoClose = false;
		autoCloseTimeOut = 20;
		missionInstructor = new MissionInstructor();
	}

	public override void Initialize(MENode node)
	{
		base.Initialize(node);
	}

	public override IEnumerator Fire()
	{
		if (pauseMission)
		{
			node.isPaused = true;
		}
		else
		{
			node.isPaused = false;
		}
		string instructorName = missionInstructor.instructorName;
		instructorName = instructorName.Replace("Instructor_", "GAPKerbal_");
		instructorName = instructorName.Replace("Strategy_", "GAPKerbal_");
		string instructorName2 = "";
		if (instructorName.Contains("Veteran"))
		{
			if (missionInstructor.vintageSuit)
			{
				instructorName += "_Vintage";
			}
			if (instructorName.Contains("Jeb_"))
			{
				instructorName2 = Localizer.Format("#autoLOC_20803");
				instructorName = instructorName.Replace("Jeb_", "");
			}
			else if (instructorName.Contains("Bill_"))
			{
				instructorName2 = Localizer.Format("#autoLOC_20811");
				instructorName = instructorName.Replace("Bill_", "");
			}
			else if (instructorName.Contains("Bob_"))
			{
				instructorName2 = Localizer.Format("#autoLOC_20819");
				instructorName = instructorName.Replace("Bob_", "");
			}
			else if (instructorName.Contains("Val_"))
			{
				instructorName2 = Localizer.Format("#autoLOC_20827");
				instructorName = instructorName.Replace("Val_", "");
			}
		}
		MessageNodeDialog.Spawn(messageHeading, message, instructorName, "#autoLOC_8000259", dialogClosed, textAreaSize, screenArea, autoClose, autoCloseTimeOut, autoGrowDialogHeight, instructorName2);
		SendStateMessage(messageHeading, message, MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ACHIEVE);
		yield break;
	}

	public void dialogClosed()
	{
		node.isPaused = false;
		if (isBriefingMessage)
		{
			node.mission.briefingNodeActive = false;
		}
	}

	public void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		if (MessageSystem.Instance != null)
		{
			MessageSystem.Instance.AddMessage(new MessageSystem.Message(title, message, color, icon), animate: false);
		}
		Debug.Log("[Mission]: (" + Localizer.Format(title) + "): " + Localizer.Format(message));
	}

	public void OnautoCloseTimeoutCreated(MEGUIParameterNumberRange parameter)
	{
		autoCloseTimeOutRange = parameter;
	}

	public void OnautoClose(bool value)
	{
		autoCloseTimeOutRange.gameObject.SetActive(value);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004012");
	}

	public override void ParameterSetupComplete()
	{
		base.ParameterSetupComplete();
		OnautoClose(autoClose);
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "missionInstructor")
		{
			return missionInstructor.GetNodeBodyParameterString(field);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		if (!string.IsNullOrEmpty(messageHeading))
		{
			node.AddValue("messageHeading", messageHeading);
		}
		if (!string.IsNullOrEmpty(message))
		{
			string text = message.Replace("\n", "\\n");
			text = text.Replace("\t", "\\t");
			node.AddValue("message", text);
		}
		missionInstructor.Save(node);
		node.AddValue("textAreaSize", textAreaSize);
		node.AddValue("pauseMission", pauseMission);
		node.AddValue("screenArea", screenArea);
		node.AddValue("autoClose", autoClose);
		node.AddValue("autoCloseTimeOut", autoCloseTimeOut);
		node.AddValue("isBriefingMessage", isBriefingMessage);
		node.AddValue("autoGrowDialogHeight", autoGrowDialogHeight);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("messageHeading", ref messageHeading);
		node.TryGetValue("message", ref message);
		message = message.Replace("\\n", "\n");
		message = message.Replace("\\t", "\t");
		node.TryGetValue("textAreaSize", ref textAreaSize);
		node.TryGetValue("pauseMission", ref pauseMission);
		node.TryGetEnum("screenArea", ref screenArea, DialogMessageArea.Center);
		node.TryGetValue("autoClose", ref autoClose);
		node.TryGetValue("autoCloseTimeOut", ref autoCloseTimeOut);
		node.TryGetValue("isBriefingMessage", ref isBriefingMessage);
		if (missionInstructor == null)
		{
			missionInstructor = new MissionInstructor(base.node, UpdateNodeBodyUI);
		}
		missionInstructor.Load(node);
		if (!node.TryGetValue("autoGrowDialogHeight", ref autoGrowDialogHeight))
		{
			node.TryGetValue("maxDialogHeight", ref autoGrowDialogHeight);
		}
	}
}
