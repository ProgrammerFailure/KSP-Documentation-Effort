using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleAsteroidInfo : ModuleSpaceObjectInfo
{
	protected ModuleAsteroid baseMod;

	protected List<ModuleSpaceObjectResource> resInfoList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAsteroidInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupAsteroidResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}
}
