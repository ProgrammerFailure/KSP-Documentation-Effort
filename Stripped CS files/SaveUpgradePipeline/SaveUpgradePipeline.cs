using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SaveUpgradePipeline;

public class SaveUpgradePipeline
{
	public List<UpgradeScript> upgradeScripts;

	public List<Dictionary<UpgradeScript, LogEntry>> runLog;

	public Version lowestVersion;

	public bool initialized;

	public event Callback<ConfigNode, LoadContext, Version> OnSetCfgNodeVersion;

	public void RunLogCoD()
	{
		if (runLog == null)
		{
			runLog = new List<Dictionary<UpgradeScript, LogEntry>>();
		}
		else
		{
			runLog.Clear();
		}
	}

	public void Init(List<Assembly> asms)
	{
		this.OnSetCfgNodeVersion = OnSetCfgNodeVersion_Default;
		ReflectUpgradeScripts(out upgradeScripts, asms);
		initialized = true;
	}

	public static void ReflectUpgradeScripts(out List<UpgradeScript> upgradeScripts, List<Assembly> asms)
	{
		List<ReflectionUtil.AttributedType<UpgradeModule>> attributedTypesInAssemblies = ReflectionUtil.GetAttributedTypesInAssemblies<UpgradeScript, UpgradeModule>(asms);
		upgradeScripts = (from uSc in attributedTypesInAssemblies.Select(InitScript)
			orderby uSc.TargetVersion, uSc.EarliestCompatibleVersion
			select uSc).ToList();
	}

	public static UpgradeScript InitScript(ReflectionUtil.AttributedType<UpgradeModule> uMod)
	{
		UpgradeScript obj = Activator.CreateInstance(uMod.Type) as UpgradeScript;
		obj.Init(uMod.Attribute.loadContext, uMod.Attribute.craftNodeUrl, uMod.Attribute.sfsNodeUrl);
		return obj;
	}

	public ConfigNode Run(ConfigNode node, LoadContext ctx, Version AppVersion, out bool runSuccess, out string runInfo)
	{
		RunLogCoD();
		return Run(node, ctx, AppVersion, out runSuccess, out runInfo, ref runLog);
	}

	public ConfigNode Run(ConfigNode node, LoadContext ctx, Version AppVersion, out bool runSuccess, out string runInfo, ref List<Dictionary<UpgradeScript, LogEntry>> log)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		List<UpgradeScript> scripts = upgradeScripts.Where((UpgradeScript uSC) => SanityCheck(uSC, AppVersion)).ToList();
		ConfigNode node2 = null;
		Version cfgVersion = NodeUtil.GetCfgVersion(node, ctx);
		runInfo = "Original File Version: " + cfgVersion.ToString(3) + "\n";
		int num = 0;
		IterationResult iterationResult;
		for (iterationResult = IterationResult.Continue; iterationResult == IterationResult.Continue; iterationResult = RunIteration(node, ref node2, ctx, scripts, log))
		{
			num++;
		}
		stopwatch.Stop();
		switch (iterationResult)
		{
		default:
			runSuccess = false;
			runInfo += string.Empty;
			break;
		case IterationResult.Fail:
			runSuccess = false;
			runInfo = runInfo + "Upgrades Failed. " + num + " iterations. Runtime: " + stopwatch.ElapsedMilliseconds + "ms";
			UnityEngine.Debug.LogWarning("[SaveUpgradePipeline]: " + runInfo);
			break;
		case IterationResult.Continue:
			runSuccess = false;
			runInfo = runInfo + "Upgrades Finished with result Continue! This is madness!" + num + " iterations.Runtime: " + stopwatch.ElapsedMilliseconds + "ms";
			UnityEngine.Debug.LogError("[SaveUpgradePipeline]: " + runInfo);
			break;
		case IterationResult.Pass:
			runSuccess = true;
			runInfo = runInfo + "Upgrades Completed. " + num + " iterations. Runtime: " + stopwatch.ElapsedMilliseconds + "ms";
			break;
		}
		return node2 ?? node;
	}

	public IterationResult RunIteration(ConfigNode srcNode, ref ConfigNode node, LoadContext ctx, List<UpgradeScript> scripts, List<Dictionary<UpgradeScript, LogEntry>> log)
	{
		Dictionary<UpgradeScript, LogEntry> dictionary = ((log.Count > 0) ? log[log.Count - 1] : null);
		Dictionary<UpgradeScript, LogEntry> row = new Dictionary<UpgradeScript, LogEntry>();
		log.Add(row);
		ConfigNode node2 = node ?? srcNode;
		int count = scripts.Count;
		TestResult testResult;
		do
		{
			if (count-- > 0)
			{
				testResult = RunTest(scripts[count], node2, ctx);
				row.Add(scripts[count], new LogEntry(testResult, upgraded: false));
				continue;
			}
			if (row.Values.All((LogEntry r) => r.testResult == TestResult.Pass))
			{
				return IterationResult.Pass;
			}
			if (row.Values.All((LogEntry r) => r.testResult == TestResult.TooEarly))
			{
				return IterationResult.Fail;
			}
			if (!TestExceptionCases(log))
			{
				return IterationResult.Fail;
			}
			if (node == null)
			{
				node = srcNode.CreateCopy();
			}
			int count2 = scripts.Count;
			while (count2-- > 0)
			{
				if (row[scripts[count2]].testResult == TestResult.Upgradeable)
				{
					if (dictionary != null && dictionary[scripts[count2]].upgraded)
					{
						row[scripts[count2]].testResult = TestResult.Pass;
						row[scripts[count2]].upgraded = true;
					}
					else
					{
						node = RunUpgrade(scripts[count2], node, ctx);
						row[scripts[count2]].upgraded = true;
					}
				}
			}
			lowestVersion = new Version(int.MaxValue, int.MaxValue, int.MaxValue);
			UpgradeScript[] array = row.Keys.Where((UpgradeScript usc) => row[usc].testResult == TestResult.Upgradeable || row[usc].testResult == TestResult.TooEarly || row[usc].upgraded).ToArray();
			int num = array.Length;
			while (num-- > 0)
			{
				Version version = ((row[array[num]].testResult != TestResult.TooEarly) ? array[num].TargetVersion : array[num].EarliestCompatibleVersion);
				if (version < lowestVersion)
				{
					lowestVersion = version;
				}
			}
			this.OnSetCfgNodeVersion(node, ctx, lowestVersion);
			return IterationResult.Continue;
		}
		while (testResult != 0);
		return IterationResult.Fail;
	}

	public static bool SanityCheck(UpgradeScript uSC, Version AppVersion)
	{
		if (uSC.TargetVersion <= uSC.EarliestCompatibleVersion)
		{
			UnityEngine.Debug.LogError("[SaveUpgradePipeline]: A script's target version should never be LEqual to its earliest-compat version. " + uSC.Name + " will be skipped.");
			return false;
		}
		if (!(uSC.TargetVersion > AppVersion) && !(uSC.EarliestCompatibleVersion > AppVersion))
		{
			return true;
		}
		UnityEngine.Debug.LogError("[SaveUpgradePipeline]: A script's versions should never exceed the current application version " + uSC.Name + " will be skipped.");
		return false;
	}

	public static bool TestExceptionCases(List<Dictionary<UpgradeScript, LogEntry>> log)
	{
		if (log.Count > 1)
		{
			Dictionary<UpgradeScript, LogEntry> dictionary = log[log.Count - 2];
			Dictionary<UpgradeScript, LogEntry> dictionary2 = log[log.Count - 1];
			foreach (UpgradeScript key in dictionary2.Keys)
			{
				if (dictionary2[key].testResult <= TestResult.Upgradeable && dictionary[key].testResult == TestResult.Pass && !dictionary[key].upgraded)
				{
					throw new InvalidOperationException(string.Concat("[SaveUpgradePipeline] ", key.Name, "returned ", dictionary2[key].testResult, " after having passed in a previous iteration. this doesn't make sense."));
				}
			}
		}
		return true;
	}

	public static void OnSetCfgNodeVersion_Default(ConfigNode n, LoadContext loadContext, Version version)
	{
		if (loadContext == LoadContext.flag_1)
		{
			n.GetNode("GAME").SetValue("version", version.ToString(3), createIfNotFound: true);
		}
		else
		{
			n.SetValue("version", version.ToString(3), createIfNotFound: true);
		}
	}

	public TestResult RunTest(UpgradeScript uSc, ConfigNode node, LoadContext loadContext)
	{
		if (!UtilMath.IsPowerOfTwo((int)loadContext))
		{
			throw new ArgumentException("Multiple LoadContexts cannot be used as a parameter in this method.");
		}
		if (uSc.AppliesInContext(loadContext))
		{
			if (node != null)
			{
				return uSc.Test(node, loadContext);
			}
			throw new ArgumentException("Cannot Run tests, given node is null!");
		}
		UnityEngine.Debug.LogError("[SaveUpgradePipeline]: Test Module " + uSc.Name + " does not apply for " + loadContext.ToString() + " loading!");
		return TestResult.Failed;
	}

	public ConfigNode RunUpgrade(UpgradeScript uSc, ConfigNode node, LoadContext loadContext)
	{
		if (!UtilMath.IsPowerOfTwo((int)loadContext))
		{
			throw new ArgumentException("Multiple LoadContexts cannot be used as a parameter in this method.");
		}
		if (uSc.AppliesInContext(loadContext))
		{
			if (node != null)
			{
				uSc.Upgrade(node, loadContext);
				return node;
			}
			throw new ArgumentException("Cannot Run upgrade, given node is null!");
		}
		UnityEngine.Debug.LogError("[SaveUpgradePipeline]: Test Module " + uSc.Name + " does not apply for " + loadContext.ToString() + " loading!");
		return null;
	}

	public void SetCfgNodeVersion(ConfigNode node, LoadContext ctx, Version version)
	{
		this.OnSetCfgNodeVersion(node, ctx, version);
	}

	public string PrintLog(List<Dictionary<UpgradeScript, LogEntry>> log, string cfgName, string runInfo)
	{
		string text = "<b>Upgrade Log for " + cfgName + ":</b>\n";
		int i = 0;
		for (int count = log.Count; i < count; i++)
		{
			string text2 = ">> #" + i + "\n";
			foreach (UpgradeScript key in log[i].Keys)
			{
				text2 = string.Concat(text2, "<b>", key, "</b>\t | ", log[i][key], "\n");
			}
			text += text2;
		}
		return text + runInfo;
	}
}
