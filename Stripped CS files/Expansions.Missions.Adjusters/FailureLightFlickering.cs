using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class FailureLightFlickering : AdjusterLightBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100270")]
	public float maximumFlickerTime = 200f;

	public float currentIncrement = -0.1f;

	public float gradient = -0.1f;

	public FailureLightFlickering()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100271");
	}

	public FailureLightFlickering(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = Localizer.Format("#autoLOC_8100271");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100272"));
		ModuleLight moduleLight = adjustedModule as ModuleLight;
		if (moduleLight != null)
		{
			moduleLight.ToggleLightAction(KSPActionType.Activate);
			UpdateFlickeringData();
		}
	}

	public void UpdateFlickeringData()
	{
		if (currentIncrement < 0f || currentIncrement > 1f)
		{
			currentIncrement = Mathf.Clamp01(currentIncrement);
			gradient = -1f * Mathf.Sign(gradient) * (1f / Random.Range(0.1f, maximumFlickerTime / 100f));
		}
		currentIncrement += gradient * Time.deltaTime;
	}

	public override float ApplyIntensityAdjustment(float intensity)
	{
		UpdateFlickeringData();
		return intensity * currentIncrement;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "maximumFlickerTime")
		{
			return Localizer.Format("#autoLOC_8100273", Mathf.Round(maximumFlickerTime / 100f));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("maximumFlickerTime", maximumFlickerTime);
		node.AddValue("timeMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("maximumFlickerTime", ref maximumFlickerTime);
		if (!node.HasValue("timeMultiplierIsHundredBased"))
		{
			maximumFlickerTime *= 100f;
		}
	}
}
