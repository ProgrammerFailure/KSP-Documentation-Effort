using System;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v180_ModuleControlSurface : UpgradeScript
{
	public override string Name => "1.8 ModuleControlSurface";

	public override string Description => "Upgrades control authority settings for control surfaces.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override Version TargetVersion => new Version(1, 8, 0);

	public override TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName)
	{
		nodeName = NodeUtil.GetPartNodeNameValue(node, loadContext);
		TestResult result = TestResult.Pass;
		ConfigNode[] nodes = node.GetNodes("MODULE");
		int num = nodes.Length;
		string value;
		do
		{
			if (num-- > 0)
			{
				value = nodes[num].GetValue("name");
				continue;
			}
			return result;
		}
		while (!(value == "ModuleControlSurface"));
		return TestResult.Upgradeable;
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		ConfigNode[] nodes = node.GetNodes("MODULE");
		int num = nodes.Length;
		while (num-- > 0)
		{
			string value = nodes[num].GetValue("name");
			if (value == "ModuleControlSurface")
			{
				ModuleControlSurface baseModule = GetBaseModule(node, loadContext);
				ConvertControlAuthority(nodes[num], baseModule);
				ConvertControlAuthorityAxisGroup(nodes[num]);
			}
		}
	}

	public ModuleControlSurface GetBaseModule(ConfigNode node, LoadContext loadContext)
	{
		string value;
		if (loadContext == LoadContext.Craft)
		{
			value = node.GetValue("part");
			value = value.Substring(0, value.IndexOf('_'));
		}
		else
		{
			value = node.GetValue("name");
		}
		Part partPrefab = PartLoader.getPartInfoByName(value).partPrefab;
		int count = partPrefab.Modules.Count;
		PartModule partModule;
		do
		{
			if (count-- > 0)
			{
				partModule = partPrefab.Modules[count];
				continue;
			}
			return null;
		}
		while (!(partModule.GetType().Name == "ModuleControlSurface"));
		return partModule as ModuleControlSurface;
	}

	public static void ConvertControlAuthority(ConfigNode mNode, ModuleControlSurface module)
	{
		float.TryParse(mNode.GetValue("authorityLimiter"), out var result);
		float value = module.ctrlSurfaceRange * result * 0.01f;
		mNode.AddValue("deployAngle", value);
	}

	public static void ConvertControlAuthorityAxisGroup(ConfigNode mNode)
	{
		ConfigNode node = mNode.GetNode("AXISGROUPS");
		if (node == null || node.nodes == null)
		{
			return;
		}
		int count = node.nodes.Count;
		while (count-- > 0)
		{
			ConfigNode configNode = node.nodes[count];
			if (configNode.name == "authorityLimiter")
			{
				configNode.name = "deployAngle";
			}
		}
	}
}
