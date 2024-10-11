using System;
using System.Runtime.CompilerServices;

public class IntermediateTutorialPageStep : TutorialPageConfig, IDisposable
{
	public IntermediateTutorial tutorialScenario;

	public TutorialScenario.TutorialFSM tutorial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IntermediateTutorialPageStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}
}
