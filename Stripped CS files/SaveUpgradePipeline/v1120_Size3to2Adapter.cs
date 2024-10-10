using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v1120_Size3to2Adapter : PartReplace
{
	public override string Description => "Replaces the old Kerbodyne ADTP-2-3 for the new Kerbodyne ADTP-2-3.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.12 Kerbodyne ADTP-2-3 replacement";

	public override Version TargetVersion => new Version(1, 12, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "Size3to2Adapter";
		replacementPartName = "Size3To2Adapter.v2";
		refTransformName = "Size3To2Adapter.v2";
		posOffset = new Vector3(0f, 0f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0f, 0f);
		childPosOffset = new Vector3(0f, 0f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "top",
				offset = new Vector3(0f, 0.05017f, 0f)
			},
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
