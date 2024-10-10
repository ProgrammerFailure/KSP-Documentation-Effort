using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v1120_stackBiCoupler : PartReplace
{
	public override string Description => "Replaces the old TVR-200 Stack Bi-Coupler for the new TVR-200 Stack Bi-Coupler.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.12 TVR-200 Stack Bi-Coupler replacement";

	public override Version TargetVersion => new Version(1, 12, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "stackBiCoupler";
		replacementPartName = "stackBiCoupler.v2";
		refTransformName = "stackBiCoupler.v2";
		posOffset = new Vector3(0f, -0.082814f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, -0.082814f, 0f);
		childPosOffset = new Vector3(0f, -0.002008f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "top",
				offset = new Vector3(0f, 0.0828145f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom1",
				offset = new Vector3(0f, -0.002008f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom2",
				offset = new Vector3(0f, -0.002008f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
