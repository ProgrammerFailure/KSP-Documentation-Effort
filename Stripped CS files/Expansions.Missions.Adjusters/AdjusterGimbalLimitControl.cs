using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterGimbalLimitControl : AdjusterGimbalBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100235")]
	public float pitchControlMultiplier = 10f;

	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100236")]
	public float rollControlMultiplier = 10f;

	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100237")]
	public float yawControlMultiplier = 10f;

	public AdjusterGimbalLimitControl()
	{
		guiName = "Gimbal limit control";
	}

	public AdjusterGimbalLimitControl(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100238";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100239"));
	}

	public override Vector3 ApplyControlAdjustment(Vector3 control)
	{
		control.x *= pitchControlMultiplier / 100f;
		control.y *= rollControlMultiplier / 100f;
		control.z *= yawControlMultiplier / 100f;
		return control;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "pitchControlMultiplier")
		{
			return Localizer.Format("#autoLOC_8100240", pitchControlMultiplier);
		}
		if (field.name == "rollControlMultiplier")
		{
			return Localizer.Format("#autoLOC_8100241", rollControlMultiplier);
		}
		if (field.name == "yawControlMultiplier")
		{
			return Localizer.Format("#autoLOC_8100242", yawControlMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("pitchControlMultiplier", pitchControlMultiplier);
		node.AddValue("rollControlMultiplier", rollControlMultiplier);
		node.AddValue("yawControlMultiplier", yawControlMultiplier);
		node.AddValue("pitchMultiplierIsHundredBased", value: true);
		node.AddValue("rollMultiplierIsHundredBased", value: true);
		node.AddValue("yawMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("pitchControlMultiplier", ref pitchControlMultiplier);
		node.TryGetValue("rollControlMultiplier", ref rollControlMultiplier);
		node.TryGetValue("yawControlMultiplier", ref yawControlMultiplier);
		if (!node.HasValue("pitchMultiplierIsHundredBased"))
		{
			pitchControlMultiplier *= 100f;
		}
		if (!node.HasValue("rollMultiplierIsHundredBased"))
		{
			rollControlMultiplier *= 100f;
		}
		if (!node.HasValue("yawMultiplierIsHundredBased"))
		{
			yawControlMultiplier *= 100f;
		}
	}
}
