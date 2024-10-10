using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v170_fuelTank2_2 : PartReplace
{
	public override string Description => "Replaces the old Rockomax X200-16 Fuel Tank for the new one.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.7 Rockomax X200-16 replace";

	public override Version TargetVersion => new Version(1, 7, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "fuelTank2-2";
		replacementPartName = "Rockomax16.BW";
		refTransformName = "Rockomax16.BW";
		posOffset = Vector3.zero;
		rotOffset = Quaternion.identity;
		att0Offset = Vector3.zero;
		childPosOffset = Vector3.zero;
		attNOffsets = new List<attachNodeOffset>();
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
