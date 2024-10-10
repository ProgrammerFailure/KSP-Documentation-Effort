using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class probeCoreSphere : PartReplace
{
	public override string Description => "Replaces the old Probodobodyne Stayputnik for the new Probodobodyne Stayputnik.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.12 Probodobodyne Stayputnik replacement";

	public override Version TargetVersion => new Version(1, 12, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "probeCoreSphere";
		replacementPartName = "probeCoreSphere.v2";
		refTransformName = "probeCoreSphere.v2";
		posOffset = new Vector3(0f, -0.0638668f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0.0638668f, 0f);
		childPosOffset = new Vector3(0f, -0.0638668f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "bottom",
				offset = new Vector3(0f, 0.0638668f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
