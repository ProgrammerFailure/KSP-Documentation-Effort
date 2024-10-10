using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterGeneratorEfficiency : AdjusterGeneratorBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100231")]
	public float generatorOutputMultiplier = 10f;

	public AdjusterGeneratorEfficiency()
	{
		guiName = "Generator output";
	}

	public AdjusterGeneratorEfficiency(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100232";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100233", Mathf.Round(generatorOutputMultiplier / 100f)));
	}

	public override float ApplyEfficiencyAdjustment(float efficiency)
	{
		return efficiency * generatorOutputMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "generatorOutputMultiplier")
		{
			return Localizer.Format("#autoLOC_8100234", generatorOutputMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("generatorOutputMultiplier", generatorOutputMultiplier);
		node.AddValue("generatorMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("generatorOutputMultiplier", ref generatorOutputMultiplier);
		if (!node.HasValue("generatorMultiplierIsHundredBased"))
		{
			generatorOutputMultiplier *= 100f;
		}
	}
}
