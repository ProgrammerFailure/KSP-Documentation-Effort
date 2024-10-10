namespace SaveUpgradePipeline;

public abstract class ModuleReplace : UpgradeScript
{
	public string moduleName;

	public string newModuleName;

	public ConfigNode[] moduleNodes;

	public ModuleReplace()
	{
	}

	public override void OnInit()
	{
		moduleNodes = null;
		Setup(out moduleName, out newModuleName);
	}

	public abstract void Setup(out string moduleName, out string newModuleName);

	public override TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName)
	{
		nodeName = moduleName;
		moduleNodes = NodeUtil.GetModuleNodesNamed(node, moduleName);
		if (moduleNodes != null && moduleNodes.Length != 0)
		{
			return TestResult.Upgradeable;
		}
		return TestResult.Pass;
	}

	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		for (int i = 0; i < moduleNodes.Length; i++)
		{
			if (moduleNodes[i].HasValue("name"))
			{
				moduleNodes[i].SetValue("name", newModuleName);
			}
		}
	}
}
