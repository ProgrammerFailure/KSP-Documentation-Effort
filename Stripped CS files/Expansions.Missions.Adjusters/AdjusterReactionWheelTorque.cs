using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterReactionWheelTorque : AdjusterReactionWheelBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100251")]
	public float pitchTorqueMultiplier = 10f;

	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100252")]
	public float rollTorqueMultiplier = 10f;

	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100253")]
	public float yawTorqueMultiplier = 10f;

	public AdjusterReactionWheelTorque()
	{
		guiName = "Reaction wheel torque";
	}

	public AdjusterReactionWheelTorque(MENode node)
		: base(node)
	{
		guiName = Localizer.Format("#autoLOC_8100254");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100255"));
	}

	public override Vector3 ApplyTorqueAdjustment(Vector3 torque)
	{
		torque.x *= pitchTorqueMultiplier / 100f;
		torque.y *= rollTorqueMultiplier / 100f;
		torque.z *= yawTorqueMultiplier / 100f;
		return torque;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "pitchTorqueMultiplier")
		{
			return Localizer.Format("#autoLOC_8100256", pitchTorqueMultiplier);
		}
		if (field.name == "rollTorqueMultiplier")
		{
			return Localizer.Format("#autoLOC_8100257", rollTorqueMultiplier);
		}
		if (field.name == "yawTorqueMultiplier")
		{
			return Localizer.Format("#autoLOC_8100258", yawTorqueMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("pitchTorqueMultiplier", pitchTorqueMultiplier);
		node.AddValue("rollTorqueMultiplier", rollTorqueMultiplier);
		node.AddValue("yawTorqueMultiplier", yawTorqueMultiplier);
		node.AddValue("pitchMultiplierIsHundredBased", value: true);
		node.AddValue("rollMultiplierIsHundredBased", value: true);
		node.AddValue("yawMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("pitchTorqueMultiplier", ref pitchTorqueMultiplier);
		node.TryGetValue("rollTorqueMultiplier", ref rollTorqueMultiplier);
		node.TryGetValue("yawTorqueMultiplier", ref yawTorqueMultiplier);
		if (!node.HasValue("pitchMultiplierIsHundredBased"))
		{
			pitchTorqueMultiplier *= 100f;
		}
		if (!node.HasValue("rollMultiplierIsHundredBased"))
		{
			rollTorqueMultiplier *= 100f;
		}
		if (!node.HasValue("yawMultiplierIsHundredBased"))
		{
			yawTorqueMultiplier *= 100f;
		}
	}
}
