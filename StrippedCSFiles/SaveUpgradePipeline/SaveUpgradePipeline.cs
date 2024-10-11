using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SaveUpgradePipeline;

public class SaveUpgradePipeline
{
	public List<UpgradeScript> upgradeScripts;

	public List<Dictionary<UpgradeScript, LogEntry>> runLog;

	private Version lowestVersion;

	internal bool initialized;

	public event Callback<ConfigNode, LoadContext, Version> OnSetCfgNodeVersion
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		remove
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SaveUpgradePipeline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void RunLogCoD()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init(List<Assembly> asms)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void ReflectUpgradeScripts(out List<UpgradeScript> upgradeScripts, List<Assembly> asms)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static UpgradeScript InitScript(ReflectionUtil.AttributedType<UpgradeModule> uMod)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode Run(ConfigNode node, LoadContext ctx, Version AppVersion, out bool runSuccess, out string runInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode Run(ConfigNode node, LoadContext ctx, Version AppVersion, out bool runSuccess, out string runInfo, ref List<Dictionary<UpgradeScript, LogEntry>> log)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private IterationResult RunIteration(ConfigNode srcNode, ref ConfigNode node, LoadContext ctx, List<UpgradeScript> scripts, List<Dictionary<UpgradeScript, LogEntry>> log)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool SanityCheck(UpgradeScript uSC, Version AppVersion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool TestExceptionCases(List<Dictionary<UpgradeScript, LogEntry>> log)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void OnSetCfgNodeVersion_Default(ConfigNode n, LoadContext loadContext, Version version)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestResult RunTest(UpgradeScript uSc, ConfigNode node, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode RunUpgrade(UpgradeScript uSc, ConfigNode node, LoadContext loadContext)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCfgNodeVersion(ConfigNode node, LoadContext ctx, Version version)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string PrintLog(List<Dictionary<UpgradeScript, LogEntry>> log, string cfgName, string runInfo)
	{
		throw null;
	}
}
