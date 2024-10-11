using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class KSPScenario : Attribute
{
	public ScenarioCreationOptions createOptions;

	public GameScenes[] tgtScenes;

	public GameScenes[] TargetScenes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPScenario(ScenarioCreationOptions createOptions, params GameScenes[] tgtScenes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCreateOption(ScenarioCreationOptions option)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasTargetScene(GameScenes scene)
	{
		throw null;
	}
}
