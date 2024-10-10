using System.Collections;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Actions;

public class ActionMessage : ActionModule
{
	[MEGUI_TextArea(tabStop = true, checkpointValidation = CheckpointValidationType.None, resetValue = "", guiName = "#autoLOC_8000031")]
	public string message;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, tabStop = true, resetValue = "5", guiName = "#autoLOC_8000032", Tooltip = "#autoLOC_8000033")]
	public float duration;

	[MEGUI_Dropdown(checkpointValidation = CheckpointValidationType.None, resetValue = "UPPER_LEFT", guiName = "#autoLOC_8000034", Tooltip = "#autoLOC_8000035")]
	public ScreenMessageStyle style;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000031");
		message = "";
		duration = 5f;
		style = ScreenMessageStyle.UPPER_LEFT;
	}

	public override IEnumerator Fire()
	{
		ScreenMessages.PostScreenMessage(message, duration, style, persist: true);
		yield break;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004021");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		string text = message.Replace("\n", "\\n");
		text = text.Replace("\t", "\\t");
		node.AddValue("message", text);
		node.AddValue("duration", duration);
		node.AddValue("style", style.ToString());
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("message", ref message);
		message = message.Replace("\\n", "\n");
		message = message.Replace("\\t", "\t");
		node.TryGetValue("duration", ref duration);
		node.TryGetEnum("style", ref style, ScreenMessageStyle.UPPER_LEFT);
	}
}
