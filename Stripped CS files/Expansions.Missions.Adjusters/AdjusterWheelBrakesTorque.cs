using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterWheelBrakesTorque : AdjusterWheelBrakesBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100263")]
	public float brakeTorqueMultiplier = 10f;

	public AdjusterWheelBrakesTorque()
	{
		guiName = "Wheel Brakes torque";
	}

	public AdjusterWheelBrakesTorque(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100264";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100265", Mathf.Round(brakeTorqueMultiplier / 100f)));
	}

	public override float ApplyTorqueAdjustment(float torque)
	{
		return torque * brakeTorqueMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "brakeTorqueMultiplier")
		{
			return Localizer.Format("#autoLOC_8100266", brakeTorqueMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("brakeTorqueMultiplier", brakeTorqueMultiplier);
		node.AddValue("brakeMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("brakeTorqueMultiplier", ref brakeTorqueMultiplier);
		if (!node.HasValue("brakeMultiplierIsHundredBased"))
		{
			brakeTorqueMultiplier *= 100f;
		}
	}
}
