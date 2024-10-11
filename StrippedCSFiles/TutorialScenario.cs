using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialScenario : ScenarioModule
{
	public class TutorialPage : KFSMState
	{
		public string windowTitle;

		public KFSMEvent onAdvanceConditionMet;

		public KFSMEvent onStepBack;

		public Callback OnDrawContent;

		internal MultiOptionDialog dialog;

		public MultiOptionDialog Dialog
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TutorialPage(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetAdvanceCondition(KFSMEventCondition condition)
		{
			throw null;
		}
	}

	[Serializable]
	public class TutorialFSM : KerbalFSM
	{
		public List<TutorialPage> pages;

		public TutorialPage lastPage;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TutorialFSM()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddPage(TutorialPage st)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddPages(List<TutorialPage> st)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddState(TutorialPage st)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void StartTutorial(TutorialPage initialPage)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void StartTutorial(string initialPageName)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void GoToNextPage()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void GoToLastPage()
		{
			throw null;
		}
	}

	public KerbalInstructor instructor;

	public GameObject mainlight;

	public RenderTexture instructorTexture;

	public string instructorPrefabName;

	public string tutorialArrowPrefabName;

	public string guiSkinName;

	public int instructorPortraitSize;

	public int textureBorderRadius;

	public string tutorialControlColorString;

	public string tutorialHighlightColorString;

	protected Rect rect;

	protected Rect dRect;

	protected Rect avatarRect;

	protected PopupDialog dialogDisplay;

	protected UISkinDef skin;

	protected bool TutorialDialogEnabled;

	public bool ExclusiveTutorial;

	protected TutorialFSM Tutorial;

	private DialogGUIVerticalLayout instructorSetup;

	internal float dialogWidth;

	internal float dialogPadSides;

	protected TutorialPage currentPage;

	private bool closed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialScenario()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal float CalcDialogXRatio()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CompleteTutorial(bool destroySelf = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CloseTutorialWindow(bool destroySelf = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetDialogRect(Rect r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string GetCurrentStateName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static ConfigNode GetTutorialNode(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected TutorialPage CreateTutorialPage(string pageId, string titleLoc, KFSMStateChange onEnterCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MultiOptionDialog CreateNextDialog(string pageId, string textLoc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MultiOptionDialog CreateOkDialog(string pageId, string textLoc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MultiOptionDialog CreateNoButtonDialog(string pageId, string textLoc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MultiOptionDialog CreateDoneDialog(string pageId, string textLoc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MultiOptionDialog CreateMultiButtonDialog(string pageId, string textLoc, METutorialScenario.TutorialButtonType buttons)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected DialogGUIBase[] CreateDialog(string pageId, string textLoc, DialogGUIButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected DialogGUIBase[] CreateDialog(string pageId, string textLoc, DialogGUIButton[] button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected DialogGUIBase[] CreateDialog(string pageId, string textLoc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIButton GetNextButtonUi(Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIButton GetDoneButtonUi(Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIButton GetContinueButtonUi(Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIButton GetOkButtonUi(Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIButton CreateButtonUi(string textLoc, Callback callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDoneButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnContinueButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNextButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnCloseButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnOkButtonClick()
	{
		throw null;
	}
}
