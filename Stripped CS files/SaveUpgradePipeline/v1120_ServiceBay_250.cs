using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v1120_ServiceBay_250 : PartReplace
{
	public override string Description => "Replaces the old Service Bay (2.5m) for the new Service Bay (2.5m).";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.12 Service Bay (2.5m) replacement";

	public override Version TargetVersion => new Version(1, 12, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "ServiceBay.250";
		replacementPartName = "ServiceBay.250.v2";
		refTransformName = "ServiceBay.250.v2";
		posOffset = new Vector3(0f, 0f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0f, 0f);
		childPosOffset = new Vector3(0f, 0f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "top",
				offset = new Vector3(0f, 0f, 0f)
			},
			new attachNodeOffset
			{
				id = "top2",
				offset = new Vector3(0f, 0f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom",
				offset = new Vector3(0f, 0f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom2",
				offset = new Vector3(0f, 0f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
