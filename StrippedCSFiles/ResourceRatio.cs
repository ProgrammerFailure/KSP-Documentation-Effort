using System;
using System.Runtime.CompilerServices;

[Serializable]
public struct ResourceRatio
{
	public string ResourceName;

	public double Ratio;

	public bool DumpExcess;

	public ResourceFlowMode FlowMode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceRatio(string resourceName, double ratio, bool dumpExcess)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceRatio(string resourceName, double ratio, bool dumpExcess, ResourceFlowMode flowMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
