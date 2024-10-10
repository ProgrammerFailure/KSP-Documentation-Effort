using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v170_stackDecouplerMini : PartReplace
{
	public override string Description => "Replaces the old TR-2V Stack Decoupler for the TD-06 Decoupler.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.7 TR-2V replacement";

	public override Version TargetVersion => new Version(1, 7, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "stackDecouplerMini";
		replacementPartName = "Decoupler.0";
		refTransformName = "Decoupler.0";
		posOffset = new Vector3(0f, 0.0434094f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0.0434094f, 0f);
		childPosOffset = new Vector3(0f, -0.0434094f, 0f);
		attNOffsets = new List<attachNodeOffset>
		{
			new attachNodeOffset
			{
				id = "top",
				offset = new Vector3(0f, -0.0434094f, 0f)
			},
			new attachNodeOffset
			{
				id = "bottom",
				offset = new Vector3(0f, 0.0434094f, 0f)
			}
		};
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
