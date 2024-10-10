using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSPAssets.KSPedia;

public class KSPedia : KSPediaController
{
	public delegate void ReadyCallback();

	public UITreeView treeViewController;

	public RectTransform contentPanel;

	public TextMeshProUGUI textTitle;

	public Button buttonBack;

	public Button buttonForward;

	public Button buttonNext;

	public Button buttonPrev;

	public Button buttonUp;

	public UIRadioButton buttonPause;

	public Button buttonClose;

	public bool displayEmptyCategories;

	public CanvasGroup canvasGroup;

	public ReadyCallback readyCallback;

	public static List<string> history = new List<string>();

	public static int historyIndex = 0;

	public float panelWidth;

	public Vector2 originalContentSize;

	public float scaleFactor = 1f;

	public override void OnDatabaseReady()
	{
		AssignDelegates();
		CreateContents();
		if (history.Count > 0)
		{
			ShowScreen(history[historyIndex]);
			return;
		}
		history.Insert(0, db.categories[0].titleScreen);
		ShowScreen(db.categories[0].titleScreen);
	}

	public void AssignDelegates()
	{
		if (buttonBack != null)
		{
			buttonBack.onClick.AddListener(OnClickBack);
		}
		if (buttonForward != null)
		{
			buttonForward.onClick.AddListener(OnClickForward);
		}
		if (buttonNext != null)
		{
			buttonNext.onClick.AddListener(OnClickNext);
		}
		if (buttonPrev != null)
		{
			buttonPrev.onClick.AddListener(OnClickPrevious);
		}
		if (buttonUp != null)
		{
			buttonUp.onClick.AddListener(OnClickUp);
		}
		if (buttonPause != null)
		{
			buttonPause.onTrue.AddListener(OnBtnPauseTrue);
			buttonPause.onFalse.AddListener(OnBtnPauseFalse);
		}
		if (buttonClose != null)
		{
			buttonClose.onClick.AddListener(OnClickClose);
		}
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
		GameEvents.onGamePause.Add(SetPauseButtonState);
		GameEvents.onGameUnpause.Add(SetPauseButtonState);
	}

	public void CreateContents()
	{
		int i = 0;
		for (int count = db.categories.Count; i < count; i++)
		{
			KSPediaDatabase.Category category = db.categories[i];
			UITreeView.Item parent = InstantiateCategoryItem(null, category);
			int j = 0;
			for (int count2 = category.screens.Count; j < count2; j++)
			{
				KSPediaDatabase.Screen screen = db.GetScreen(category.screens[j]);
				if (screen != null)
				{
					InstantiateScreenItem(parent, screen);
				}
			}
			int k = 0;
			for (int count3 = category.subcategories.Count; k < count3; k++)
			{
				KSPediaDatabase.Category category2 = category.subcategories[k];
				UITreeView.Item parent2 = InstantiateCategoryItem(parent, category2);
				int l = 0;
				for (int count4 = category2.screens.Count; l < count4; l++)
				{
					KSPediaDatabase.Screen screen2 = db.GetScreen(category2.screens[l]);
					if (screen2 != null)
					{
						InstantiateScreenItem(parent2, screen2);
					}
				}
			}
		}
	}

	public UITreeView.Item InstantiateCategoryItem(UITreeView.Item parent, KSPediaDatabase.Category cat)
	{
		if (!string.IsNullOrEmpty(cat.titleScreen))
		{
			string screenName = cat.titleScreen;
			return treeViewController.AddItem(parent, cat.name, cat.title, null, delegate
			{
				ContentsShowScreen(screenName);
			});
		}
		return treeViewController.AddItem(parent, cat.name, cat.title, null, null);
	}

	public UITreeView.Item InstantiateScreenItem(UITreeView.Item parent, KSPediaDatabase.Screen screen)
	{
		string screenName = ((screen != null) ? screen.name : "");
		return treeViewController.AddItem(parent, screenName, screen.title, null, delegate
		{
			ContentsShowScreen(screenName);
		});
	}

	public void ContentsShowScreen(string screenName)
	{
		if (history.Count == 0 || (historyIndex == 0 && history[0] != screenName) || (historyIndex > 0 && historyIndex < history.Count && history[historyIndex] != screenName))
		{
			if (historyIndex > 0)
			{
				history.RemoveRange(0, Mathf.Min(historyIndex, history.Count));
				historyIndex = 0;
			}
			history.Insert(0, screenName);
		}
		KSPediaController.Instance.ShowScreen(screenName);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
		GameEvents.onGamePause.Remove(SetPauseButtonState);
		GameEvents.onGameUnpause.Remove(SetPauseButtonState);
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
	}

	public override void OnShowScreen(KSPediaDatabase.Screen dbScreen)
	{
		textTitle.text = GetScreenTitle(dbScreen);
		if (buttonBack != null)
		{
			buttonBack.interactable = history.Count > 1 && historyIndex < history.Count - 1;
		}
		if (buttonForward != null)
		{
			buttonForward.interactable = historyIndex > 0;
		}
		if (buttonNext != null)
		{
			buttonNext.interactable = db.GetScreenNext(dbScreen) != null;
		}
		if (buttonPrev != null)
		{
			buttonPrev.interactable = db.GetScreenPrevious(dbScreen) != null;
		}
		if (buttonUp != null)
		{
			buttonUp.interactable = db.GetScreenUp(dbScreen) != null;
		}
		treeViewController.SelectItem(db.GetScreenUrl(dbScreen));
	}

	public override void OnHideScreen()
	{
	}

	public void OnUnhide()
	{
		SetPauseButtonState();
		if (history.Count > 0)
		{
			ShowScreen(history[historyIndex]);
		}
	}

	public void SetPauseButtonState()
	{
		if (FlightDriver.Pause)
		{
			buttonPause.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
		}
		else
		{
			buttonPause.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
		}
	}

	public void OnAspectControllerLayoutChange()
	{
		if (loadedSlides.ContainsKey(currentActiveSlideName))
		{
			SetupScale(loadedSlides[currentActiveSlideName], setup: false);
		}
	}

	public void SetupScale(GameObject content, bool setup)
	{
		if (!(content == null))
		{
			RectTransform rectTransform = content.transform as RectTransform;
			originalContentSize = rectTransform.sizeDelta;
			panelWidth = CalculateRelativeRectBounds(contentPanel.parent as RectTransform).size.x;
			scaleFactor = panelWidth / originalContentSize.x;
			rectTransform.SetParent(contentPanel, worldPositionStays: false);
			rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchoredPosition = Vector2.zero;
			contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, originalContentSize.y * scaleFactor);
			if (readyCallback != null)
			{
				readyCallback();
			}
		}
	}

	public static Bounds CalculateRelativeRectBounds(RectTransform child)
	{
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Vector3[] array = new Vector3[4];
		Matrix4x4 worldToLocalMatrix = child.worldToLocalMatrix;
		child.GetWorldCorners(array);
		for (int i = 0; i < 4; i++)
		{
			Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(array[i]);
			vector = Vector3.Min(lhs, vector);
			vector2 = Vector3.Max(lhs, vector2);
		}
		Bounds result = new Bounds(vector, Vector3.zero);
		result.Encapsulate(vector2);
		return result;
	}

	public override void OnInstantiateScreenPrefab(GameObject content)
	{
		SetupScale(content, setup: true);
	}

	public override void OnCompileScreen(GameObject go, KSPediaDatabase.Screen dbScreen)
	{
		base.OnCompileScreen(go, dbScreen);
	}

	public string GetScreenTitle(KSPediaDatabase.Screen dbScreen)
	{
		string text = "";
		KSPediaDatabase.Category category = db.GetCategory(dbScreen);
		text = ((!db.IsRootCategory(category)) ? Localizer.Format("<<1>> / <<2>>", Localizer.Format(db.GetRootCategory(category).title), Localizer.Format(category.title)) : Localizer.Format("<<1>>", Localizer.Format(category.title)));
		if (category.titleScreen != dbScreen.name)
		{
			text += Localizer.Format(" / <<1>>", Localizer.Format(dbScreen.title));
		}
		return text;
	}

	public void OnClickBack()
	{
		if (history.Count > historyIndex + 1)
		{
			historyIndex++;
			string text = history[historyIndex];
			ShowScreen(text);
		}
	}

	public void OnClickForward()
	{
		if (historyIndex > 0)
		{
			historyIndex--;
			string text = history[historyIndex];
			ShowScreen(text);
		}
	}

	public void OnClickNext()
	{
		if (history.Count > 0)
		{
			KSPediaDatabase.Screen screen = db.GetScreen(history[historyIndex]);
			KSPediaDatabase.Screen screenNext = db.GetScreenNext(screen);
			if (screenNext != null)
			{
				ContentsShowScreen(screenNext.name);
			}
		}
	}

	public void OnClickPrevious()
	{
		if (history.Count > 0)
		{
			KSPediaDatabase.Screen screen = db.GetScreen(history[historyIndex]);
			KSPediaDatabase.Screen screenPrevious = db.GetScreenPrevious(screen);
			if (screenPrevious != null)
			{
				ContentsShowScreen(screenPrevious.name);
			}
		}
	}

	public void OnClickUp()
	{
		if (history.Count > 0)
		{
			KSPediaDatabase.Screen screen = db.GetScreen(history[historyIndex]);
			KSPediaDatabase.Screen screenUp = db.GetScreenUp(screen);
			if (screenUp != null)
			{
				ContentsShowScreen(screenUp.name);
			}
		}
	}

	public void OnBtnPauseTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		if (!FlightDriver.Pause && HighLogic.LoadedSceneHasPlanetarium)
		{
			FlightDriver.SetPause(pauseState: true);
		}
	}

	public void OnBtnPauseFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		if (FlightDriver.Pause && HighLogic.LoadedSceneHasPlanetarium)
		{
			FlightDriver.SetPause(pauseState: false);
		}
	}

	public void OnClickClose()
	{
		if (FlightDriver.Pause)
		{
			FlightDriver.SetPause(pauseState: false);
		}
		KSPediaSpawner.Hide();
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape) && !UIMasterController.Instance.CameraMode)
		{
			OnClickClose();
		}
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			OnClickNext();
		}
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			OnClickPrevious();
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			OnClickBack();
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			OnClickForward();
		}
	}
}
