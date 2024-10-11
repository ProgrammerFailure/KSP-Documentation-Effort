using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class TutorialPageConfig
{
	public List<TutorialPageConfig> tutoStepsConfig;

	public string pageId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public string pageTitleLocId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public string pageDialogLocId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public KFSMStateChange onEnterCallback
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public METutorialScenario.TutorialButtonType pageButtonType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialPageConfig(string pageId, string pageTitleLocId, string pageDialogLocId, KFSMStateChange onEnterCallback, METutorialScenario.TutorialButtonType pageButtonType = METutorialScenario.TutorialButtonType.Next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected TutorialPageConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AddTutorialStepConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeTutoStepsConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddTutorialStepConfig(string pageId, string pageTitleLocId, string pageDialogLocId, KFSMStateChange onEnterCallback, METutorialScenario.TutorialButtonType pageButtonType = METutorialScenario.TutorialButtonType.Next)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEnterEmpty(KFSMState state)
	{
		throw null;
	}
}
