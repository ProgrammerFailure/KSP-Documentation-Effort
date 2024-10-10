using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterActiveRadiatorMaxEnergyTransfer : AdjusterActiveRadiatorBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100213")]
	public float radiatorMaxEnergyTransferMultiplier = 10f;

	public AdjusterActiveRadiatorMaxEnergyTransfer()
	{
		guiName = Localizer.Format("#autoLOC_8100214");
	}

	public AdjusterActiveRadiatorMaxEnergyTransfer(MENode node)
		: base(node)
	{
		guiName = Localizer.Format("#autoLOC_8100214");
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100215", Mathf.Round(radiatorMaxEnergyTransferMultiplier / 100f)));
	}

	public override double ApplyMaxEnergyTransferAdjustment(double maxEnergyTransfer)
	{
		return maxEnergyTransfer * (double)radiatorMaxEnergyTransferMultiplier / 100.0;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "radiatorMaxEnergyTransferMultiplier")
		{
			return Localizer.Format("#autoLOC_8100216", radiatorMaxEnergyTransferMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("radiatorMaxEnergyTransferMultiplier", radiatorMaxEnergyTransferMultiplier);
		node.AddValue("radMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("radiatorMaxEnergyTransferMultiplier", ref radiatorMaxEnergyTransferMultiplier);
		if (!node.HasValue("radMultiplierIsHundredBased"))
		{
			radiatorMaxEnergyTransferMultiplier *= 100f;
		}
	}
}
