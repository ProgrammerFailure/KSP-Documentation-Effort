using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterDeployableSolarPanelEfficiency : AdjusterDeployableSolarPanelBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100223")]
	public float solarPanelEfficiencyMultiplier = 10f;

	public AdjusterDeployableSolarPanelEfficiency()
	{
		guiName = "Solar Panel Efficiency";
	}

	public AdjusterDeployableSolarPanelEfficiency(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100224";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100225", Mathf.Round(solarPanelEfficiencyMultiplier / 100f)));
	}

	public override float ApplyEfficiencyAdjustment(float efficiency)
	{
		return efficiency * solarPanelEfficiencyMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "solarPanelEfficiencyMultiplier")
		{
			return Localizer.Format("#autoLOC_8100226", solarPanelEfficiencyMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("solarPanelEfficiencyMultiplier", solarPanelEfficiencyMultiplier);
		node.AddValue("solarMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("solarPanelEfficiencyMultiplier", ref solarPanelEfficiencyMultiplier);
		if (!node.HasValue("solarMultiplierIsHundredBased"))
		{
			solarPanelEfficiencyMultiplier *= 100f;
		}
	}
}
