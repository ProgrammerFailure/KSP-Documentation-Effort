using System.Runtime.CompilerServices;

namespace SaveUpgradePipeline;

public abstract class ModuleReplace : UpgradeScript
{
	protected string moduleName;

	protected string newModuleName;

	private ConfigNode[] moduleNodes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ModuleReplace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInit()
	{
		throw null;
	}

	protected abstract void Setup(out string moduleName, out string newModuleName);

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
}
