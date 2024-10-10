using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterLiftingSurfaceLiftForce : AdjusterLiftingSurfaceBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100243")]
	public float liftForceMultiplier = 10f;

	public AdjusterLiftingSurfaceLiftForce()
	{
		guiName = "Lifting surface lift force";
	}

	public AdjusterLiftingSurfaceLiftForce(MENode node)
		: base(node)
	{
		guiName = "#autoLOC_8100244";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100245", Mathf.Round(liftForceMultiplier / 100f)));
	}

	public override Vector3 ApplyLiftForceAdjustment(Vector3 liftForce)
	{
		return liftForce * liftForceMultiplier / 100f;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "liftForceMultiplier")
		{
			return Localizer.Format("#autoLOC_8100246", liftForceMultiplier);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("liftForceMultiplier", liftForceMultiplier);
		node.AddValue("liftMultiplierIsHundredBased", value: true);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("liftForceMultiplier", ref liftForceMultiplier);
		if (!node.HasValue("liftMultiplierIsHundredBased"))
		{
			liftForceMultiplier *= 100f;
		}
	}
}
