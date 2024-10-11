using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScenarioRunner : MonoBehaviour
{
	protected List<ProtoScenarioModule> protoModules;

	protected List<ScenarioModule> modules;

	public static ScenarioRunner Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioRunner()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void onLevelWasLoaded(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddMainMenuScenarios()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioModule AddModule(string moduleName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioModule AddModule(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LoadModules(ProtoScenarioModule protoModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LoadModules(List<ProtoScenarioModule> protoModules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ClearModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ScenarioModule> GetLoadedModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ProtoScenarioModule> GetUpdatedProtoModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetProtoModules(List<ProtoScenarioModule> protoModules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetProtoModules(ProtoScenarioModule protoModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveModule(ScenarioModule module)
	{
		throw null;
	}
}
