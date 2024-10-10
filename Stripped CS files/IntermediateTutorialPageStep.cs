using System;

public class IntermediateTutorialPageStep : TutorialPageConfig, IDisposable
{
	public IntermediateTutorial tutorialScenario;

	public TutorialScenario.TutorialFSM tutorial;

	public IntermediateTutorialPageStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorial)
	{
		this.tutorialScenario = tutorialScenario;
		this.tutorial = tutorial;
	}

	public void Dispose()
	{
		tutorialScenario = null;
	}
}
