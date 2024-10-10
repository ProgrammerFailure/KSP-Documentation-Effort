using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class KSPScenario : Attribute
{
	public ScenarioCreationOptions createOptions;

	public GameScenes[] tgtScenes;

	public GameScenes[] TargetScenes => tgtScenes;

	public KSPScenario(ScenarioCreationOptions createOptions, params GameScenes[] tgtScenes)
	{
		this.createOptions = createOptions;
		this.tgtScenes = tgtScenes;
	}

	public bool HasCreateOption(ScenarioCreationOptions option)
	{
		return (createOptions & option) != 0;
	}

	public bool HasTargetScene(GameScenes scene)
	{
		GameScenes[] targetScenes = TargetScenes;
		int num = 0;
		while (true)
		{
			if (num < targetScenes.Length)
			{
				if (targetScenes[num] == scene)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
