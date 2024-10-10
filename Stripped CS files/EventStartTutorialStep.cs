public class EventStartTutorialStep : IntermediateTutorialPageStep
{
	public const string intermediateTitleLoc = "#autoLOC_9990010";

	public EventStartTutorialStep(IntermediateTutorial tutorialScenario, TutorialScenario.TutorialFSM tutorialFsm)
		: base(tutorialScenario, tutorialFsm)
	{
	}

	public override void AddTutorialStepConfig()
	{
		AddTutorialStepConfig("eventExplanation", "#autoLOC_9990010", "#autoLOC_9990017", base.OnEnterEmpty);
		AddTutorialStepConfig("eventApplication", "#autoLOC_9990010", "#autoLOC_9990018", base.OnEnterEmpty);
	}
}
