using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
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

	private static List<string> history;

	private static int historyIndex;

	private float panelWidth;

	private Vector2 originalContentSize;

	private float scaleFactor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPedia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static KSPedia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDatabaseReady()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AssignDelegates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateContents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UITreeView.Item InstantiateCategoryItem(UITreeView.Item parent, KSPediaDatabase.Category cat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UITreeView.Item InstantiateScreenItem(UITreeView.Item parent, KSPediaDatabase.Screen screen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ContentsShowScreen(string screenName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnShowScreen(KSPediaDatabase.Screen dbScreen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnHideScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnUnhide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPauseButtonState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnAspectControllerLayoutChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupScale(GameObject content, bool setup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Bounds CalculateRelativeRectBounds(RectTransform child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInstantiateScreenPrefab(GameObject content)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnCompileScreen(GameObject go, KSPediaDatabase.Screen dbScreen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetScreenTitle(KSPediaDatabase.Screen dbScreen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickBack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickForward()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickNext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickPrevious()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBtnPauseTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBtnPauseFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
