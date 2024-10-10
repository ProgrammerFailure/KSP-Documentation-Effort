using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v1120_radialEngineMini : PartReplace
{
	public override string Description => "Replaces the old LV-1R Spider Liquid Fuel Engine for the new LV-1R Spider Liquid Fuel Engine.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override string Name => "1.12 LV-1R Spider Liquid Fuel Engine replacement";

	public override Version TargetVersion => new Version(1, 12, 0);

	public override void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets)
	{
		pName = "radialEngineMini";
		replacementPartName = "radialEngineMini.v2";
		refTransformName = "radialEngineMini.v2";
		posOffset = new Vector3(0f, 0f, 0f);
		rotOffset = Quaternion.identity;
		att0Offset = new Vector3(0f, 0f, 0f);
		childPosOffset = new Vector3(0f, 0f, 0f);
		attNOffsets = new List<attachNodeOffset>();
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		base.OnUpgrade(node, loadContext, parentNode);
	}
}
