using System.Collections.Generic;
using System.Runtime.CompilerServices;

public abstract class PartModuleFXSetter : PartModule
{
	[KSPField]
	public string fxModuleNames;

	public List<IScalarModule> fxModules;

	private int modulesCount;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PartModuleFXSetter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetFXModules(float scalar)
	{
		throw null;
	}
}
