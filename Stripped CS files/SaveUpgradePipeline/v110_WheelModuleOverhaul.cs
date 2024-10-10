using System;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.flag_1 | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v110_WheelModuleOverhaul : UpgradeScript
{
	public enum OldGearDeploymentStates
	{
		RETRACTED,
		DEPLOYED,
		DEPLOYING,
		RETRACTING,
		UNDEFINED
	}

	public enum OldLegDeploymentStates
	{
		RETRACTED,
		RETRACTING,
		DEPLOYING,
		DEPLOYED,
		BROKEN,
		REPAIRING
	}

	public override string Name => "1.1 Wheel Module Overhaul";

	public override string Description => "Upgrades obsolete ModuleWheel, ModuleLandingGear, ModuleLandingLeg data into the new ModuleWheelBase and WheelSubmodules scheme.";

	public override Version EarliestCompatibleVersion => new Version(0, 21, 0);

	public override Version TargetVersion => new Version(1, 1, 0);

	public override TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName)
	{
		nodeName = NodeUtil.GetPartNodeNameValue(node, loadContext);
		TestResult result = TestResult.Pass;
		ConfigNode[] nodes = node.GetNodes("MODULE");
		int num = nodes.Length;
		while (num-- > 0)
		{
			switch (nodes[num].GetValue("name"))
			{
			case "ModuleWheel":
			case "ModuleLandingGear":
			case "ModuleLandingGearFixed":
			case "ModuleLandingLeg":
			case "ModuleSteering":
				return TestResult.Upgradeable;
			}
		}
		return result;
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		ConfigNode[] nodes = node.GetNodes("MODULE");
		int num = nodes.Length;
		while (num-- > 0)
		{
			switch (nodes[num].GetValue("name"))
			{
			case "ModuleSteering":
				SalvageSteering(node, nodes[num], loadContext);
				break;
			case "ModuleLandingLeg":
				SalvageLegDeployment(node, nodes[num], loadContext);
				break;
			case "ModuleLandingGearFixed":
				SalvageBrakes(node, nodes[num], loadContext);
				break;
			case "ModuleLandingGear":
				SalvageBrakes(node, nodes[num], loadContext);
				SalvageGearDeployment(node, nodes[num], loadContext);
				break;
			case "ModuleWheel":
				SalvageBrakes(node, nodes[num], loadContext);
				SalvageSteering(node, nodes[num], loadContext);
				SalvageMotor(node, nodes[num], loadContext);
				break;
			}
		}
	}

	public static void SalvageBrakes(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		ConfigNode configNode = new ConfigNode("MODULE");
		configNode.AddValue("name", "ModuleWheelBrakes");
		if (loadContext == LoadContext.flag_1)
		{
			bool flag = false;
			if (mNode.HasValue("brakesEngaged"))
			{
				flag = bool.Parse(mNode.GetValue("brakesEngaged"));
				configNode.AddValue("brakeInput", flag ? 1f : 0f);
			}
		}
		if (mNode.HasValue("brakesEnabled"))
		{
			bool flag2 = bool.Parse(mNode.GetValue("brakesEnabled"));
			configNode.AddValue("brakeTweak", flag2 ? 100f : 0f);
		}
		if (configNode.values.Count > 1)
		{
			dstNode.AddNode(configNode);
		}
	}

	public void SalvageSteering(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		ConfigNode configNode = new ConfigNode("MODULE");
		configNode.AddValue("name", "ModuleWheelSteering");
		if (mNode.HasValue("steeringLocked"))
		{
			bool flag = bool.Parse(mNode.GetValue("steeringLocked"));
			configNode.AddValue("steeringEnabled", !flag);
		}
		if (mNode.HasValue("invertSteering"))
		{
			bool value = bool.Parse(mNode.GetValue("invertSteering"));
			configNode.AddValue("steeringInvert", value);
		}
		if (configNode.values.Count > 1)
		{
			dstNode.AddNode(configNode);
			ConfigNode configNode2 = configNode.CreateCopy();
			configNode2.SetValue("name", "ModuleWheelMotorSteering");
			dstNode.AddNode(configNode2);
		}
	}

	public static void SalvageMotor(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		ConfigNode configNode = new ConfigNode("MODULE");
		configNode.AddValue("name", "ModuleWheelMotor");
		if (mNode.HasValue("motorEnabled"))
		{
			bool value = bool.Parse(mNode.GetValue("motorEnabled"));
			configNode.AddValue("motorEnabled", value);
		}
		if (configNode.values.Count > 1)
		{
			dstNode.AddNode(configNode);
		}
	}

	public static void SalvageDamage(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		ConfigNode configNode = new ConfigNode("MODULE");
		configNode.AddValue("name", "ModuleWheelDamage");
		if (mNode.HasValue("isDamaged"))
		{
			bool value = bool.Parse(mNode.GetValue("isDamaged"));
			configNode.AddValue("isDamaged", value);
		}
		if (configNode.values.Count > 1)
		{
			dstNode.AddNode(configNode);
		}
	}

	public static void SalvageGearDeployment(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		ConfigNode configNode = new ConfigNode("MODULE");
		configNode.AddValue("name", "ModuleWheelDeployment");
		if (mNode.HasValue("storedAnimationTime"))
		{
			float value = float.Parse(mNode.GetValue("storedAnimationTime"));
			configNode.AddValue("position", value);
		}
		switch (loadContext)
		{
		case LoadContext.Craft:
			if (mNode.HasValue("StartDeployed"))
			{
				bool flag = bool.Parse(mNode.GetValue("StartDeployed"));
				configNode.AddValue("stateString", flag ? "Deployed" : "Retracted");
			}
			break;
		case LoadContext.flag_1:
			if (mNode.HasValue("storedGearState"))
			{
				OldGearDeploymentStates st = (OldGearDeploymentStates)Enum.Parse(typeof(OldGearDeploymentStates), mNode.GetValue("storedGearState"));
				configNode.AddValue("stateString", DeployStateUpgrade(st));
			}
			break;
		}
		if (configNode.values.Count > 1)
		{
			dstNode.AddNode(configNode);
		}
	}

	public static void SalvageLegDeployment(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		ConfigNode configNode = new ConfigNode("MODULE");
		configNode.AddValue("name", "ModuleWheelDeployment");
		if (mNode.HasValue("savedAnimationTime"))
		{
			float value = float.Parse(mNode.GetValue("savedAnimationTime"));
			configNode.AddValue("position", value);
		}
		switch (loadContext)
		{
		case LoadContext.Craft:
			if (mNode.HasValue("StartDeployed"))
			{
				bool flag = bool.Parse(mNode.GetValue("StartDeployed"));
				configNode.AddValue("stateString", flag ? "Deployed" : "Retracted");
			}
			break;
		case LoadContext.flag_1:
			if (mNode.HasValue("savedLegState"))
			{
				OldLegDeploymentStates st = (OldLegDeploymentStates)Enum.Parse(typeof(OldLegDeploymentStates), mNode.GetValue("savedLegState"));
				configNode.AddValue("stateString", DeployStateUpgrade(st));
			}
			break;
		}
		if (configNode.values.Count > 1)
		{
			dstNode.AddNode(configNode);
		}
	}

	public static string DeployStateUpgrade(OldGearDeploymentStates st)
	{
		return st switch
		{
			OldGearDeploymentStates.DEPLOYED => "Deployed", 
			OldGearDeploymentStates.RETRACTED => "Retracted", 
			_ => string.Empty, 
		};
	}

	public static string DeployStateUpgrade(OldLegDeploymentStates st)
	{
		return st switch
		{
			OldLegDeploymentStates.DEPLOYED => "Deployed", 
			OldLegDeploymentStates.RETRACTED => "Retracted", 
			_ => string.Empty, 
		};
	}
}
