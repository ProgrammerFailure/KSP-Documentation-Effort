using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterAlternatorOutput : AdjusterAlternatorBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100151")]
	public float alternatorOutputMultiplier = 10f;

	public AdjusterAlternatorOutput()
	{
		guiName = "Alternator output";
	}

	public AdjusterAlternatorOutput(MENode node)
		: base(node)
	{
		guiName = Localizer.Format("#autoLOC_8100150");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100152", Mathf.Round(alternatorOutputMultiplier / 100f)));
	}

	public override float ApplyOutputAdjustment(float output)
	{
		return output * alternatorOutputMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "alternatorOutputMultiplier")
		{
			return Localizer.Format("#autoLOC_8100153", alternatorOutputMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("alternatorOutputMultiplier", alternatorOutputMultiplier);
		node.AddValue("alternatorMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("alternatorOutputMultiplier", ref alternatorOutputMultiplier);
		if (!node.HasValue("alternatorMultiplierIsHundredBased"))
		{
			alternatorOutputMultiplier *= 100f;
		}
	}
}
