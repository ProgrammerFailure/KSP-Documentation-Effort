using System;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v1123_dockingNode : ModuleReplace
{
	public override string Description => "Replaces the JPLRepo mod released to fix docking Nodes module with the stock one.";

	public override Version EarliestCompatibleVersion => new Version(1, 12, 0);

	public override string Name => "JPLRepo ModuleDockingNodeFixed replace";

	public override Version TargetVersion => new Version(1, 12, 2);

	public override void Setup(out string oldName, out string newName)
	{
		oldName = "ModuleDockingNodeFixed";
		newName = "ModuleDockingNode";
	}
}
