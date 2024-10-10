using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterDataTransmitterPower : AdjusterDataTransmitterBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100219")]
	public float transmitterPowerMultiplier = 10f;

	public AdjusterDataTransmitterPower()
	{
		guiName = "Transmitter power";
	}

	public AdjusterDataTransmitterPower(MENode node)
		: base(node)
	{
		guiName = Localizer.Format("#autoLOC_8100220");
	}

	public override void Activate()
	{
		base.Activate();
		UpdatePowerValueDisplay();
	}

	public void PowerValueChanged(BaseField field, object obj)
	{
		UpdatePowerValueDisplay();
	}

	public void UpdatePowerValueDisplay()
	{
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100221", Mathf.Round(transmitterPowerMultiplier / 100f)));
		ModuleDataTransmitter moduleDataTransmitter = adjustedModule as ModuleDataTransmitter;
		if (moduleDataTransmitter != null)
		{
			moduleDataTransmitter.UpdatePowerText();
		}
	}

	public override double ApplyPowerAdjustment(double power)
	{
		return power * ((double)transmitterPowerMultiplier / 100.0);
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "transmitterPowerMultiplier")
		{
			return Localizer.Format("#autoLOC_8100222", transmitterPowerMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("transmitterPowerMultiplier", transmitterPowerMultiplier);
		node.AddValue("transmitterMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("transmitterPowerMultiplier", ref transmitterPowerMultiplier);
		if (!node.HasValue("transmitterMultiplierIsHundredBased"))
		{
			transmitterPowerMultiplier *= 100f;
		}
	}
}
