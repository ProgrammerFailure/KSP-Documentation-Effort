using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v170_stackDecoupler1_2 : PartReplace
{
	public override string Description => "Replaces the old Rockomax Brand Decoupler for the TD-25 Decoupler.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.7 Rockomax Brand Decoupler replace";

	public override Version TargetVersion => new Version(1, 7, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "decoupler1-2";
		replacementPartName = "Decoupler.2";
		refTransformName = "Decoupler.2";
		posOffset = new Vector3(0f, 0.3508567f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0.3508567f, 0f);
		childPosOffset = new Vector3(0f, -0.2654699f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "top",
				offset = new Vector3(0f, -0.3508567f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom",
				offset = new Vector3(0f, 0.2654699f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
