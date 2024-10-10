using System.Collections.Generic;

public class TutorialPageConfig
{
	public List<TutorialPageConfig> tutoStepsConfig;

	public string pageId { get; set; }

	public string pageTitleLocId { get; set; }

	public string pageDialogLocId { get; set; }

	public KFSMStateChange onEnterCallback { get; set; }

	public METutorialScenario.TutorialButtonType pageButtonType { get; set; }

	public TutorialPageConfig(string pageId, string pageTitleLocId, string pageDialogLocId, KFSMStateChange onEnterCallback, METutorialScenario.TutorialButtonType pageButtonType = METutorialScenario.TutorialButtonType.Next)
	{
		this.pageId = pageId;
		this.pageTitleLocId = pageTitleLocId;
		this.pageDialogLocId = pageDialogLocId;
		this.onEnterCallback = onEnterCallback;
		this.pageButtonType = pageButtonType;
	}

	public TutorialPageConfig()
	{
		InitializeTutoStepsConfig();
		AddTutorialStepConfig();
	}

	public virtual void AddTutorialStepConfig()
	{
	}

	public void InitializeTutoStepsConfig()
	{
		tutoStepsConfig = new List<TutorialPageConfig>();
	}

	public void AddTutorialStepConfig(string pageId, string pageTitleLocId, string pageDialogLocId, KFSMStateChange onEnterCallback, METutorialScenario.TutorialButtonType pageButtonType = METutorialScenario.TutorialButtonType.Next)
	{
		TutorialPageConfig item = new TutorialPageConfig(pageId, pageTitleLocId, pageDialogLocId, onEnterCallback, pageButtonType);
		tutoStepsConfig.Add(item);
	}

	public void OnEnterEmpty(KFSMState state)
	{
	}
}
