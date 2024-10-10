using ns9;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class FailureRCSFireRandomly : AdjusterRCSBase
{
	public FailureRCSFireRandomly()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100343";
	}

	public FailureRCSFireRandomly(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100343";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100344"));
	}

	public override Vector3 ApplyInputRotationAdjustment(Vector3 inputRotation)
	{
		return Vector3.zero;
	}

	public override Vector3 ApplyInputLinearAdjustment(Vector3 inputLinear)
	{
		return new Vector3(Random.value, Random.value, Random.value).normalized;
	}
}
