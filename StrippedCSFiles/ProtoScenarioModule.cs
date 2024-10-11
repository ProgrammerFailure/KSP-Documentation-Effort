using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ProtoScenarioModule
{
	private ConfigNode moduleValues;

	public string moduleName;

	public List<GameScenes> targetScenes;

	public ScenarioModule moduleRef;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoScenarioModule(ScenarioModule module)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoScenarioModule(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTargetScenes(GameScenes[] scenes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioModule Load(ScenarioRunner host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetData()
	{
		throw null;
	}
}
