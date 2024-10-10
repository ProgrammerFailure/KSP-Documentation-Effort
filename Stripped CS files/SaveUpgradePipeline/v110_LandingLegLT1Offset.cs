using System;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v110_LandingLegLT1Offset : PartOffset
{
	public override string Description => "In 1.1.0, the LT1 legs were re-exported with a different pivot orientation and origin";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.1 LT1 Legs Offset\t\t";

	public override Version TargetVersion => new Version(1, 1, 0);

	public override void Setup(out string pName, out Vector3 posOffset, out Quaternion rotOffset)
	{
		pName = "landingLeg1";
		posOffset = new Vector3(0.15f, 0f, 0f);
		rotOffset = Quaternion.AngleAxis(-90f, Vector3.up) * Quaternion.AngleAxis(-22f, Vector3.right);
	}
}
