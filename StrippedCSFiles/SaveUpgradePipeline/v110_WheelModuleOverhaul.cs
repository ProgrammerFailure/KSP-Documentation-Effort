using System;
using System.Runtime.CompilerServices;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.SFS | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
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

	public override string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override string Description
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override Version EarliestCompatibleVersion
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override Version TargetVersion
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public v110_WheelModuleOverhaul()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void SalvageBrakes(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SalvageSteering(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void SalvageMotor(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void SalvageDamage(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void SalvageGearDeployment(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void SalvageLegDeployment(ConfigNode dstNode, ConfigNode mNode, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string DeployStateUpgrade(OldGearDeploymentStates st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string DeployStateUpgrade(OldLegDeploymentStates st)
	{
		throw null;
	}
}
