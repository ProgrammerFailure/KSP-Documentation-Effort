using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Cycle : UI_Control
{
	public string[] stateNames;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_Cycle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node, object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node, object host)
	{
		throw null;
	}
}
