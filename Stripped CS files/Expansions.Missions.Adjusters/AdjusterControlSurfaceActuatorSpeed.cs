using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterControlSurfaceActuatorSpeed : AdjusterControlSurfaceBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "100", guiName = "#autoLOC_8100217")]
	public float actuatorSpeedMultiplier = 100f;

	public AdjusterControlSurfaceActuatorSpeed()
	{
		guiName = "Actuator speed";
	}

	public AdjusterControlSurfaceActuatorSpeed(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100217";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100218", Mathf.Round(actuatorSpeedMultiplier / 100f)));
	}

	public override float ApplyActuatorSpeedAdjustment(float actuatorSpeed)
	{
		return actuatorSpeed * actuatorSpeedMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "actuatorSpeedMultiplier")
		{
			return Localizer.Format("#autoLOC_8100218", actuatorSpeedMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("actuatorSpeedMultiplier", actuatorSpeedMultiplier);
		node.AddValue("actuatorSpeedMultiplier", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("actuatorSpeedMultiplier", ref actuatorSpeedMultiplier);
		if (!node.HasValue("actuatorMultiplierIsHundredBased"))
		{
			actuatorSpeedMultiplier *= 100f;
		}
	}
}
