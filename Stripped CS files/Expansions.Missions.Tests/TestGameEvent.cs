using System;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[] { })]
public class TestGameEvent : TestModule, IScoreableObjective
{
	public bool eventTriggered;

	public bool eventSuscribed;

	public override void Awake()
	{
		base.Awake();
		title = "Game Event Test";
	}

	public override bool Test()
	{
		if (!eventSuscribed)
		{
			SuscribeToEvent();
		}
		return eventTriggered;
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("eventTriggered", eventTriggered);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("eventTriggered", ref eventTriggered);
	}

	public void SuscribeToEvent()
	{
		GameEvents.onStageActivate.Add(EventCallback_StageSeparation);
		eventSuscribed = true;
	}

	public void EventCallback_StageSeparation(int eventReport)
	{
		eventTriggered = true;
	}

	public object GetScoreModifier(Type scoreModule)
	{
		return null;
	}
}
