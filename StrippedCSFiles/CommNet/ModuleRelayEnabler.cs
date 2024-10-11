using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CommNet;

public class ModuleRelayEnabler : PartModule, IResourceConsumer, IRelayEnabler
{
	public bool isRelaying;

	[KSPField(guiName = "#autoLOC_6001812")]
	public string status;

	[KSPField]
	public bool showStatus;

	private List<PartResourceDefinition> consumedResources;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleRelayEnabler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanRelay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanRelayUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		throw null;
	}
}
