using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterWheelMotorTorque : AdjusterWheelMotorBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100263")]
	public float motorTorqueMultiplier = 10f;

	public AdjusterWheelMotorTorque()
	{
		guiName = "Wheel Motor torque";
	}

	public AdjusterWheelMotorTorque(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100267";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100268", Mathf.Round(motorTorqueMultiplier / 100f)));
	}

	public override float ApplyTorqueAdjustment(float torque)
	{
		return torque * motorTorqueMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "motorTorqueMultiplier")
		{
			return Localizer.Format("#autoLOC_8100269", motorTorqueMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("motorTorqueMultiplier", motorTorqueMultiplier);
		node.AddValue("motorMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("motorTorqueMultiplier", ref motorTorqueMultiplier);
		if (!node.HasValue("motorMultiplierIsHundredBased"))
		{
			motorTorqueMultiplier *= 100f;
		}
	}
}
