using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ScalarModuleSetHandler : IConfigNode
{
	public string nodeName;

	public string[] fxModuleNames;

	public List<IScalarModule> fxModules;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScalarModuleSetHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScalarModuleSetHandler(string configValueName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Initialize(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetFXModules(float scalar)
	{
		throw null;
	}
}
