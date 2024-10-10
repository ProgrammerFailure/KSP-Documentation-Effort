using System;
using System.Collections.Generic;
using ns11;
using ns2;
using ns9;
using UnityEngine;

public class TutorialScenario : ScenarioModule
{
	public class TutorialPage : KFSMState
	{
		public string windowTitle = "Tutorial Window";

		public KFSMEvent onAdvanceConditionMet;

		public KFSMEvent onStepBack;

		public Callback OnDrawContent = delegate
		{
		};

		public MultiOptionDialog dialog;

		public MultiOptionDialog Dialog
		{
			get
			{
				return dialog;
			}
			set
			{
				dialog = value;
			}
		}

		public TutorialPage(string name)
			: base(name)
		{
			windowTitle = "";
			onAdvanceConditionMet = new KFSMEvent(name + " complete");
			onAdvanceConditionMet.GoToStateOnEvent = null;
			onStepBack = new KFSMEvent("Stepped Back from " + name);
			onStepBack.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
			onStepBack.GoToStateOnEvent = null;
		}

		public void SetAdvanceCondition(KFSMEventCondition condition)
		{
			onAdvanceConditionMet.OnCheckCondition = condition;
		}
	}

	[Serializable]
	public class TutorialFSM : KerbalFSM
	{
		public List<TutorialPage> pages = new List<TutorialPage>();

		public TutorialPage lastPage;

		public void AddPage(TutorialPage st)
		{
			AddState(st);
			pages.Add(st);
		}

		public void AddPages(List<TutorialPage> st)
		{
			for (int i = 0; i < st.Count; i++)
			{
				AddPage(st[i]);
			}
		}

		public void AddState(TutorialPage st)
		{
			AddState((KFSMState)st);
			AddEvent(st.onAdvanceConditionMet, st);
			AddEvent(st.onStepBack, st);
		}

		public void StartTutorial(TutorialPage initialPage)
		{
			StartFSM(initialPage);
			lastPage = initialPage;
			int i = 0;
			for (int count = pages.Count; i < count; i++)
			{
				if (!(States[i] is TutorialPage))
				{
					continue;
				}
				TutorialPage page = pages[i];
				if (i < pages.Count - 1)
				{
					page.onAdvanceConditionMet.GoToStateOnEvent = pages[i + 1];
				}
				if (i > 0)
				{
					page.onStepBack.GoToStateOnEvent = pages[i - 1];
				}
				TutorialPage tutorialPage = page;
				tutorialPage.OnLeave = (KFSMStateChange)Delegate.Combine(tutorialPage.OnLeave, (KFSMStateChange)delegate
				{
					lastPage = page;
					if (page.dialog != null)
					{
						PopupDialog.DismissPopup(page.dialog.name);
						InputLockManager.RemoveControlLock("TutorialScenarioWindow");
					}
				});
			}
		}

		public void StartTutorial(string initialPageName)
		{
			int count = States.Count;
			KFSMState kFSMState;
			do
			{
				if (count-- > 0)
				{
					kFSMState = States[count];
					continue;
				}
				throw new Exception("[TutorialFSM ERROR]: Cannot Start. Could not find a TutorialPage-derived state called " + initialPageName + " in states list");
			}
			while (!(kFSMState is TutorialPage) || !(kFSMState.name == initialPageName));
			StartTutorial((TutorialPage)kFSMState);
		}

		public void GoToNextPage()
		{
			if (!fsmStarted)
			{
				Debug.LogError("[TutorialFSM Error]: FSM not started!");
			}
			else if (currentState is TutorialPage)
			{
				TutorialPage tutorialPage = (TutorialPage)currentState;
				if (tutorialPage.onAdvanceConditionMet.GoToStateOnEvent == null)
				{
					Debug.LogError("[TutorialFSM Error]: There is no next state defined for " + tutorialPage.name + ". Make sure it's not the last page.");
				}
				else
				{
					RunEvent(tutorialPage.onAdvanceConditionMet);
				}
			}
			else
			{
				Debug.LogError("[TutorialFSM Error]: State " + currentState.name + " is not a Tutorial Page. You can't call GoToNextPage from normal KFSMStates.");
			}
		}

		public void GoToLastPage()
		{
			if (!fsmStarted)
			{
				Debug.LogError("[TutorialFSM Error]: FSM not started!");
			}
			else if (currentState is TutorialPage)
			{
				TutorialPage tutorialPage = (TutorialPage)currentState;
				if (tutorialPage.onStepBack.GoToStateOnEvent == null)
				{
					Debug.LogError("[TutorialFSM Error]: There is no previous state defined for " + tutorialPage.name + ". Make sure it's not the first page.");
				}
				else
				{
					RunEvent(tutorialPage.onStepBack);
				}
			}
			else
			{
				Debug.LogError("[TutorialFSM Error]: State " + currentState.name + " is not a Tutorial Page. You can't call GoToNextPage from normal KFSMStates.");
			}
		}
	}

	public KerbalInstructor instructor;

	public GameObject mainlight;

	public RenderTexture instructorTexture;

	public string instructorPrefabName = "Instructor_Wernher";

	public string tutorialArrowPrefabName = "tutorialArrows";

	public string guiSkinName = "KSP window 1";

	public int instructorPortraitSize = 128;

	public int textureBorderRadius = 124;

	public string tutorialControlColorString = "#BADA55";

	public string tutorialHighlightColorString = "orange";

	public Rect rect = new Rect(100f, 80f, 400f, 180f);

	public Rect dRect;

	public Rect avatarRect;

	public PopupDialog dialogDisplay;

	public UISkinDef skin;

	public bool TutorialDialogEnabled = true;

	public bool ExclusiveTutorial = true;

	public TutorialFSM Tutorial;

	public DialogGUIVerticalLayout instructorSetup;

	public float dialogWidth = 450f;

	public float dialogPadSides = 70f;

	public TutorialPage currentPage;

	public bool closed;

	public void Start()
	{
		float x = CalcDialogXRatio();
		SetDialogRect(new Rect(x, 0.85f, dialogWidth, 190f));
		OnAssetSetup();
		GameObject gameObject = UnityEngine.Object.Instantiate(AssetBase.GetPrefab(instructorPrefabName));
		instructor = gameObject.GetComponent<KerbalInstructor>();
		if (instructorTexture != null)
		{
			instructorTexture.DiscardContents();
			instructorTexture.Release();
			UnityEngine.Object.Destroy(instructorTexture);
			instructorTexture = null;
		}
		instructorTexture = new RenderTexture(instructorPortraitSize, instructorPortraitSize, 24);
		instructor.SetupCamera(instructorTexture);
		skin = UISkinManager.GetSkin(guiSkinName);
		dRect = rect;
		instructorSetup = new DialogGUIVerticalLayout(new DialogGUIBox("", instructorPortraitSize, instructorPortraitSize, () => true, new DialogGUIHorizontalLayout(true, true, 0f, new RectOffset(2, 2, 2, 2), TextAnchor.MiddleCenter, new DialogGUIImage(new Vector2((float)instructorPortraitSize - 4f, (float)instructorPortraitSize - 4f), Vector2.zero, Color.white, instructorTexture))), new DialogGUILabel(Localizer.Format(instructor.CharacterName), skin.customStyles[1], expandW: false, expandH: true));
		Tutorial = new TutorialFSM();
		OnTutorialSetup();
		AnalyticsUtil.LogTutorialStart(this);
	}

	public float CalcDialogXRatio()
	{
		return ((float)Screen.width - dialogWidth * GameSettings.UI_SCALE - dialogPadSides) / (float)Screen.width / GameSettings.UI_SCALE;
	}

	public virtual void OnAssetSetup()
	{
	}

	public virtual void OnTutorialSetup()
	{
		TutorialPage tutorialPage = new TutorialPage("default page 1");
		tutorialPage.windowTitle = "Tutorial Window";
		tutorialPage.OnDrawContent = delegate
		{
			GUILayout.Label("This is the base class for the tutorial scenario. It has no content by itself. Please extend this class and override the OnTutorialSetup method to add your own pages.", GUILayout.ExpandHeight(expand: true));
		};
		Tutorial.AddPage(tutorialPage);
		Tutorial.StartTutorial(tutorialPage);
	}

	public void CompleteTutorial(bool destroySelf = true)
	{
		CloseTutorialWindow(destroySelf);
		AnalyticsUtil.LogTutorialCompleted(this);
	}

	public void CloseTutorialWindow(bool destroySelf = true)
	{
		if (instructor != null)
		{
			instructor.ClearCamera();
			if (instructor.gameObject != null)
			{
				UnityEngine.Object.Destroy(instructor.gameObject);
			}
		}
		InputLockManager.RemoveControlLock("TutorialScenarioWindow");
		Tutorial = new TutorialFSM();
		if (dialogDisplay != null)
		{
			UnityEngine.Object.Destroy(dialogDisplay.gameObject);
		}
		if (destroySelf)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void SetDialogRect(Rect r)
	{
		rect = r;
		dRect = r;
	}

	public string GetCurrentStateName()
	{
		if (Tutorial == null)
		{
			return null;
		}
		return Tutorial.currentStateName;
	}

	public void Update()
	{
		if (Tutorial.Started)
		{
			Tutorial.UpdateFSM();
		}
		currentPage = (TutorialPage)Tutorial.CurrentState;
		if (instructor != null)
		{
			instructor.GetComponent<AudioSource>().panStereo = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(0f, Screen.width, dRect.x + dRect.width * 0.5f));
			mainlight = instructor.gameObject.GetChild("mainlight");
			if (mainlight != null)
			{
				if (!HighLogic.LoadedSceneIsEditor && !(MissionControl.Instance != null) && !(Administration.Instance != null))
				{
					mainlight.SetActive(value: true);
				}
				else
				{
					mainlight.SetActive(value: false);
				}
			}
		}
		if (!Tutorial.Started)
		{
			if (!closed)
			{
				closed = true;
				InputLockManager.RemoveControlLock("TutorialScenarioWindow");
			}
			return;
		}
		closed = false;
		if (TutorialDialogEnabled && dialogDisplay == null)
		{
			currentPage.OnDrawContent();
			if (currentPage.dialog != null && currentPage.dialog.Options.IndexOf(instructorSetup) < 0)
			{
				List<DialogGUIBase> list = new List<DialogGUIBase>();
				DialogGUIHorizontalLayout item = new DialogGUIHorizontalLayout();
				list.Add(item);
				list.Add(instructorSetup);
				list.AddRange(currentPage.dialog.Options);
				currentPage.dialog.Options = list.ToArray();
			}
			currentPage.dialog.title = currentPage.windowTitle;
			if (HighLogic.LoadedScene == GameScenes.EDITOR)
			{
				dialogDisplay = PopupDialog.SpawnPopupDialog(new Vector2(-0.09f, 1f), new Vector2(-0.09f, 1f), currentPage.dialog, persistAcrossScenes: false, skin, isModal: false);
			}
			else
			{
				dialogDisplay = PopupDialog.SpawnPopupDialog(new Vector2(0f, 1f), new Vector2(0f, 1f), currentPage.dialog, persistAcrossScenes: false, skin, isModal: false);
			}
			dialogDisplay.gameObject.AddComponent<DialogMouseEnterControlLock>().Setup(ControlTypes.TUTORIALWINDOW, "TutorialScenarioWindow");
		}
		if (!TutorialDialogEnabled && dialogDisplay != null)
		{
			dialogDisplay.Dismiss();
			InputLockManager.RemoveControlLock("TutorialScenarioWindow");
		}
	}

	public void FixedUpdate()
	{
		if (Tutorial.Started)
		{
			Tutorial.FixedUpdateFSM();
		}
	}

	public void LateUpdate()
	{
		if (Tutorial.Started)
		{
			Tutorial.LateUpdateFSM();
		}
	}

	public void OnDestroy()
	{
		if (dialogDisplay != null)
		{
			dialogDisplay.Dismiss();
		}
		OnOnDestroy();
		CloseTutorialWindow(destroySelf: false);
	}

	public virtual void OnOnDestroy()
	{
	}

	public static ConfigNode GetTutorialNode(string name)
	{
		string text = "name";
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("TUTORIAL");
		int num = configNodes.Length;
		ConfigNode configNode;
		do
		{
			if (num-- > 0)
			{
				configNode = configNodes[num];
				continue;
			}
			return null;
		}
		while (!configNode.HasValue(text) || !(configNode.GetValue(text) == name));
		return configNode;
	}

	public TutorialPage CreateTutorialPage(string pageId, string titleLoc, KFSMStateChange onEnterCallback)
	{
		return new TutorialPage(pageId)
		{
			windowTitle = Localizer.Format(titleLoc),
			OnEnter = onEnterCallback
		};
	}

	public MultiOptionDialog CreateNextDialog(string pageId, string textLoc)
	{
		return new MultiOptionDialog(pageId, "", "", null, dRect, CreateDialog(pageId, textLoc, GetNextButtonUi(OnNextButtonClick)));
	}

	public MultiOptionDialog CreateOkDialog(string pageId, string textLoc)
	{
		return new MultiOptionDialog(pageId, "", "", null, dRect, CreateDialog(pageId, textLoc, GetOkButtonUi(OnOkButtonClick)));
	}

	public MultiOptionDialog CreateNoButtonDialog(string pageId, string textLoc)
	{
		return new MultiOptionDialog(pageId, "", "", null, dRect, CreateDialog(pageId, textLoc));
	}

	public MultiOptionDialog CreateDoneDialog(string pageId, string textLoc)
	{
		return new MultiOptionDialog(pageId, "", "", null, dRect, CreateDialog(pageId, textLoc, GetDoneButtonUi(OnDoneButtonClick)));
	}

	public MultiOptionDialog CreateMultiButtonDialog(string pageId, string textLoc, METutorialScenario.TutorialButtonType buttons)
	{
		List<DialogGUIButton> list = new List<DialogGUIButton>();
		if ((buttons & METutorialScenario.TutorialButtonType.Next) == METutorialScenario.TutorialButtonType.Next)
		{
			list.Add(GetNextButtonUi(OnNextButtonClick));
		}
		if ((buttons & METutorialScenario.TutorialButtonType.Ok) == METutorialScenario.TutorialButtonType.Ok)
		{
			list.Add(GetOkButtonUi(OnOkButtonClick));
		}
		if ((buttons & METutorialScenario.TutorialButtonType.Done) == METutorialScenario.TutorialButtonType.Done)
		{
			list.Add(GetDoneButtonUi(OnDoneButtonClick));
		}
		if ((buttons & METutorialScenario.TutorialButtonType.Continue) == METutorialScenario.TutorialButtonType.Continue)
		{
			list.Add(GetContinueButtonUi(OnContinueButtonClick));
		}
		return new MultiOptionDialog(pageId, "", "", null, dRect, CreateDialog(pageId, textLoc, list.ToArray()));
	}

	public DialogGUIBase[] CreateDialog(string pageId, string textLoc, DialogGUIButton button)
	{
		DialogGUIBase[] array = CreateDialog(pageId, textLoc);
		int num = array.Length + 1;
		DialogGUIBase[] array2 = new DialogGUIBase[num];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = array[i];
		}
		array2[num - 1] = button;
		return array2;
	}

	public DialogGUIBase[] CreateDialog(string pageId, string textLoc, DialogGUIButton[] button)
	{
		DialogGUIBase[] array = CreateDialog(pageId, textLoc);
		DialogGUIBase[] array2 = new DialogGUIBase[array.Length + button.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = array[i];
		}
		for (int j = 0; j < button.Length; j++)
		{
			array2[array.Length + j] = button[j];
		}
		return array2;
	}

	public DialogGUIBase[] CreateDialog(string pageId, string textLoc)
	{
		return new DialogGUIBase[2]
		{
			new DialogGUIVerticalLayout(sw: true),
			new DialogGUILabel(Localizer.Format(textLoc), expandW: false, expandH: true)
		};
	}

	public DialogGUIButton GetNextButtonUi(Callback callback)
	{
		return CreateButtonUi("#autoLOC_310130", callback);
	}

	public DialogGUIButton GetDoneButtonUi(Callback callback)
	{
		return CreateButtonUi("#autoLOC_310727", callback);
	}

	public DialogGUIButton GetContinueButtonUi(Callback callback)
	{
		return CreateButtonUi("#autoLOC_8300042", callback);
	}

	public DialogGUIButton GetOkButtonUi(Callback callback)
	{
		return CreateButtonUi("#autoLOC_417274", callback);
	}

	public DialogGUIButton CreateButtonUi(string textLoc, Callback callback)
	{
		return new DialogGUIButton(Localizer.Format(textLoc), callback, dismissOnSelect: true);
	}

	public virtual void OnDoneButtonClick()
	{
		CloseTutorialWindow(destroySelf: false);
	}

	public virtual void OnContinueButtonClick()
	{
		CloseTutorialWindow(destroySelf: false);
	}

	public virtual void OnNextButtonClick()
	{
		Tutorial.GoToNextPage();
	}

	public virtual void OnCloseButtonClick()
	{
		Tutorial.GoToNextPage();
	}

	public virtual void OnOkButtonClick()
	{
		Tutorial.GoToNextPage();
	}
}
