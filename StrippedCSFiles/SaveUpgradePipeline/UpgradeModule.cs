using System;
using System.Runtime.CompilerServices;

namespace SaveUpgradePipeline;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class UpgradeModule : Attribute
{
	public LoadContext loadContext;

	public string sfsNodeUrl;

	public string craftNodeUrl;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UpgradeModule(LoadContext loadContext)
	{
		throw null;
	}
}
