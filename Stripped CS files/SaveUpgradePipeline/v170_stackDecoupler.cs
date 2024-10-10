using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v170_stackDecoupler : PartReplace
{
	public override string Description => "Replaces the old TR-18A Stack Decoupler for the TD-12 Decoupler.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.7 TR-18A replacement";

	public override Version TargetVersion => new Version(1, 7, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "stackDecoupler";
		replacementPartName = "Decoupler.1";
		refTransformName = "Decoupler.1";
		posOffset = new Vector3(0f, 0.0150518f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0.0150518f, 0f);
		childPosOffset = new Vector3(0f, -0.0829949f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "top",
				offset = new Vector3(0f, -0.0150518f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom",
				offset = new Vector3(0f, 0.0829949f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
