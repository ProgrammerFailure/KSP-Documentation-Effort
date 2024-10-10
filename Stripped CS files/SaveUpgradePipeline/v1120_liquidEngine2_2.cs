using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v1120_liquidEngine2_2 : PartReplace
{
	public override string Description => "Replaces the old RE-L10 Poodle Liquid Fuel Engine for the new RE-L10 Poodle Liquid Fuel Engine.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.12 RE-L10 Poodle Liquid Fuel Engine replacement";

	public override Version TargetVersion => new Version(1, 12, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "liquidEngine2-2";
		replacementPartName = "liquidEngine2-2.v2";
		refTransformName = "liquidEngine2-2.v2";
		posOffset = new Vector3(0f, 0.72694f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0.72694f, 0f);
		childPosOffset = new Vector3(0f, -0.7645644f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "top",
				offset = new Vector3(0f, -0.7269405f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom",
				offset = new Vector3(0f, -0.7645644f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
