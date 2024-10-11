using System;
using System.Runtime.CompilerServices;

namespace SaveUpgradePipeline;

[UpgradeModule(LoadContext.SFS | LoadContext.Craft, sfsNodeUrl = "GAME/FLIGHTSTATE/VESSEL/PART", craftNodeUrl = "PART")]
public class v180_ModuleControlSurface : UpgradeScript
{
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
	public v180_ModuleControlSurface()
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
	private ModuleControlSurface GetBaseModule(ConfigNode node, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void ConvertControlAuthority(ConfigNode mNode, ModuleControlSurface module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void ConvertControlAuthorityAxisGroup(ConfigNode mNode)
	{
		throw null;
	}
}
