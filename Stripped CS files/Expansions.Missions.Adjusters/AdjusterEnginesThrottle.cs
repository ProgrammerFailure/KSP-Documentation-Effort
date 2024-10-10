using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterEnginesThrottle : AdjusterEnginesBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100227")]
	public float engineThrottleMultiplier = 10f;

	public AdjusterEnginesThrottle()
	{
		guiName = "Engine throttle";
	}

	public AdjusterEnginesThrottle(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100228";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100229", Mathf.Round(engineThrottleMultiplier / 10f) / 10f));
	}

	public override float ApplyThrottleAdjustment(float throttle)
	{
		return throttle * engineThrottleMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "engineThrottleMultiplier")
		{
			return Localizer.Format("#autoLOC_8100230", engineThrottleMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("engineThrottleMultiplier", engineThrottleMultiplier);
		node.AddValue("engineMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("engineThrottleMultiplier", ref engineThrottleMultiplier);
		if (!node.HasValue("engineMultiplierIsHundredBased"))
		{
			engineThrottleMultiplier *= 100f;
		}
	}
}
