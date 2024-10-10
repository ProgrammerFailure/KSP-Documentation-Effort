using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterLightIntensity : AdjusterLightBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100247")]
	public float lightIntensityMultiplier = 10f;

	public AdjusterLightIntensity()
	{
		guiName = "Light intensity";
	}

	public AdjusterLightIntensity(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100248";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100249", Mathf.Round(lightIntensityMultiplier / 100f)));
	}

	public override float ApplyIntensityAdjustment(float intensity)
	{
		return intensity * lightIntensityMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "lightIntensityMultiplier")
		{
			return Localizer.Format("#autoLOC_8100250", lightIntensityMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("lightIntensityMultiplier", lightIntensityMultiplier);
		node.AddValue("lightMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("lightIntensityMultiplier", ref lightIntensityMultiplier);
		if (!node.HasValue("lightMultiplierIsHundredBased"))
		{
			lightIntensityMultiplier *= 100f;
		}
	}
}
