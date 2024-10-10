using System;

namespace SaveUpgradePipeline;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class UpgradeModule : Attribute
{
	public LoadContext loadContext;

	public string sfsNodeUrl;

	public string craftNodeUrl;

	public UpgradeModule(LoadContext loadContext)
	{
		this.loadContext = loadContext;
	}
}
