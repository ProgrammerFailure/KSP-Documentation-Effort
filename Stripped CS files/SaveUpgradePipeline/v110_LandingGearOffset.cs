using System;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v110_LandingGearOffset : PartOffset
{
	public override string Description => "Applies a small upwards offset to smallGearBay parts";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.1 SmallGearBay Offset";

	public override Version TargetVersion => new Version(1, 1, 0);

	public override void Setup(out string pName, out Vector3 posOffset, out Quaternion rotOffset)
	{
		pName = "SmallGearBay";
		posOffset = new Vector3(0f, 0.1f, 0f);
		rotOffset = Quaternion.identity;
	}
}
