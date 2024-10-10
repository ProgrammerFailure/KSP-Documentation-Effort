using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v1120_rocketNoseCone : PartReplace
{
	public override string Description => "Replaces the old Protective Rocket Nose Cone Mk7 for the new Protective Rocket Nose Cone Mk7.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.12 Protective Rocket Nose Cone Mk7 replacement";

	public override Version TargetVersion => new Version(1, 12, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "rocketNoseCone";
		replacementPartName = "rocketNoseCone.v2";
		refTransformName = "rocketNoseCone.v2";
		posOffset = new Vector3(0f, 0f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0f, 0f);
		childPosOffset = new Vector3(0f, 0f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "bottom",
				offset = new Vector3(0f, 0f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
