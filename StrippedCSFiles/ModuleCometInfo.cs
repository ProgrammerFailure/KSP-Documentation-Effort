using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleCometInfo : ModuleSpaceObjectInfo
{
	protected ModuleComet baseMod;

	protected List<ModuleSpaceObjectResource> resInfoList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleCometInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupCometResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}
}
