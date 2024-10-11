using System;
using System.Runtime.CompilerServices;

namespace SaveUpgradePipeline;

public abstract class UpgradeScript
{
	public LoadContext ContextMask;

	protected string nodeUrlCraft;

	protected string nodeUrlSFS;

	public abstract string Name { get; }

	public abstract string Description { get; }

	public abstract Version EarliestCompatibleVersion { get; }

	public abstract Version TargetVersion { get; }

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected UpgradeScript()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init(LoadContext contextMask, string nodeUrlCraft, string nodeUrlSFS)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AppliesInContext(LoadContext ctx)
	{
		throw null;
	}

	public abstract TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName);

	public abstract void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode);

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual TestResult Test(ConfigNode n, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Upgrade(ConfigNode n, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CheckMinVersion(Version v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool CheckMaxVersion(Version v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Version GetCfgNodeVersion(ConfigNode n, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual TestResult VersionTest(Version v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string GetNodeURL(LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void RecurseNodes(ConfigNode node, string[] urlNodes, int level, Callback<ConfigNode, ConfigNode> cb, ConfigNode parent = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LogTestResults(string nodeName, TestResult test)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
