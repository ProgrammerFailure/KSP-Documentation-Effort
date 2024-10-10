using System;
using System.Collections.Generic;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class ApplicationLauncher : MonoBehaviour
{
	public delegate void OnShow();

	public delegate void OnHide();

	public delegate void OnReposition();

	[Flags]
	public enum AppScenes
	{
		NEVER = 0,
		ALWAYS = -1,
		SPACECENTER = 1,
		FLIGHT = 2,
		MAPVIEW = 4,
		flag_5 = 8,
		flag_6 = 0x10,
		TRACKSTATION = 0x20,
		MAINMENU = 0x40
	}

	public static bool Ready;

	public ApplauncherLayout currentLayout;

	public SimpleLayout prefab_verticalTopDown;

	public SimpleLayout prefab_horizontalRightLeft;

	public RectTransform launcherSpace;

	public RectTransform appSpace;

	public ApplicationLauncherButton listItemPrefab;

	public RectTransform tmpButtonStorage;

	public RectTransform tmpModButtonStorage;

	public int listItemWidth = 41;

	public OnShow onShow = delegate
	{
	};

	public OnHide onHide = delegate
	{
	};

	public OnReposition onReposition = delegate
	{
	};

	public List<ApplicationLauncherButton> appList = new List<ApplicationLauncherButton>();

	public List<ApplicationLauncherButton> appListHidden = new List<ApplicationLauncherButton>();

	public List<ApplicationLauncherButton> appListMod = new List<ApplicationLauncherButton>();

	public List<ApplicationLauncherButton> appListModHidden = new List<ApplicationLauncherButton>();

	public bool startedAtBottom;

	public bool applauncherInitialized;

	public UIRadioButton lastPinnedButton;

	public static ApplicationLauncher Instance { get; set; }

	public ApplauncherLayout CurrentLayout => currentLayout;

	public bool IsPositionedAtTop { get; set; }

	public void Awake()
	{
		Debug.Log("[ApplicationLauncher] Awake " + IsPositionedAtTop);
		if (Instance != null)
		{
			Debug.LogWarning("ApplicationLauncher already exist, destroying this instance");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoadRequested);
		GameEvents.onLevelWasLoadedGUIReady.Add(OnSceneLoadedGUIReady);
		GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
		GameEvents.onGUIAstronautComplexSpawn.Add(ShouldItHide);
		GameEvents.onGUIRnDComplexSpawn.Add(ShouldItHide);
		GameEvents.onGUIMissionControlSpawn.Add(ShouldItHide);
		GameEvents.onGUILaunchScreenSpawn.Add(ShouldItHide);
		GameEvents.onGUIAdministrationFacilitySpawn.Add(ShouldItHide);
		MapView.OnEnterMapView = (Callback)Delegate.Combine(MapView.OnEnterMapView, new Callback(DetermineVisibilityOfAllApps));
		GameEvents.onGUIAstronautComplexDespawn.Add(ShouldItShow);
		GameEvents.onGUIRnDComplexDespawn.Add(ShouldItShow);
		GameEvents.onGUIMissionControlDespawn.Add(ShouldItShow);
		GameEvents.onGUILaunchScreenDespawn.Add(ShouldItShow);
		GameEvents.onGUIAdministrationFacilityDespawn.Add(ShouldItShow);
		MapView.OnExitMapView = (Callback)Delegate.Combine(MapView.OnExitMapView, new Callback(DetermineVisibilityOfAllApps));
		IsPositionedAtTop = ShouldBeOnTop();
		startedAtBottom = !IsPositionedAtTop;
		StartupSequence();
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoadRequested);
		GameEvents.onLevelWasLoadedGUIReady.Remove(OnSceneLoadedGUIReady);
		GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
		GameEvents.onGUIAstronautComplexSpawn.Remove(ShouldItHide);
		GameEvents.onGUIRnDComplexSpawn.Remove(ShouldItHide);
		GameEvents.onGUIMissionControlSpawn.Remove(ShouldItHide);
		GameEvents.onGUILaunchScreenSpawn.Remove(ShouldItHide);
		GameEvents.onGUIAdministrationFacilitySpawn.Remove(ShouldItHide);
		MapView.OnEnterMapView = (Callback)Delegate.Remove(MapView.OnEnterMapView, new Callback(DetermineVisibilityOfAllApps));
		GameEvents.onGUIAstronautComplexDespawn.Remove(ShouldItShow);
		GameEvents.onGUIRnDComplexDespawn.Remove(ShouldItShow);
		GameEvents.onGUIMissionControlDespawn.Remove(ShouldItShow);
		GameEvents.onGUILaunchScreenDespawn.Remove(ShouldItShow);
		GameEvents.onGUIAdministrationFacilityDespawn.Remove(ShouldItShow);
		MapView.OnExitMapView = (Callback)Delegate.Remove(MapView.OnExitMapView, new Callback(DetermineVisibilityOfAllApps));
		GameEvents.onGUIApplicationLauncherDestroyed.Fire();
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void StartupSequence()
	{
		if (ShouldBeVisible())
		{
			if (!applauncherInitialized)
			{
				if (ShouldBeOnTop())
				{
					IsPositionedAtTop = true;
					SpawnVertical();
				}
				else
				{
					IsPositionedAtTop = false;
					SpawnHorizontal();
				}
				applauncherInitialized = true;
			}
			else if (!ShouldBeOnTop())
			{
				if (IsPositionedAtTop || startedAtBottom)
				{
					IsPositionedAtTop = false;
					startedAtBottom = false;
					SpawnHorizontal();
				}
			}
			else if (!IsPositionedAtTop)
			{
				IsPositionedAtTop = true;
				SpawnVertical();
			}
			if (HighLogic.LoadedScene == GameScenes.EDITOR)
			{
				if (currentLayout != null)
				{
					LayoutElement bottomLeftSpacer = currentLayout.GetBottomLeftSpacer();
					if (bottomLeftSpacer != null)
					{
						bottomLeftSpacer.gameObject.SetActive(value: true);
						bottomLeftSpacer.preferredHeight = 41f / GameSettings.UI_SCALE_APPS;
						bottomLeftSpacer.preferredWidth = 264f / GameSettings.UI_SCALE_APPS;
					}
					LayoutElement topRightSpacer = currentLayout.GetTopRightSpacer();
					if ((bool)topRightSpacer)
					{
						topRightSpacer.gameObject.SetActive(value: true);
						topRightSpacer.preferredHeight = 41f / GameSettings.UI_SCALE_APPS;
						topRightSpacer.preferredWidth = 80f / GameSettings.UI_SCALE_APPS;
					}
				}
			}
			else if (HighLogic.LoadedScene == GameScenes.FLIGHT)
			{
				if (currentLayout != null)
				{
					LayoutElement topRightSpacer2 = currentLayout.GetTopRightSpacer();
					if (topRightSpacer2 != null)
					{
						topRightSpacer2.gameObject.SetActive(value: false);
					}
					LayoutElement bottomLeftSpacer2 = currentLayout.GetBottomLeftSpacer();
					if (bottomLeftSpacer2 != null)
					{
						bottomLeftSpacer2.gameObject.SetActive(value: true);
						bottomLeftSpacer2.preferredHeight = 160f / GameSettings.UI_SCALE_APPS;
						bottomLeftSpacer2.preferredWidth = 41f / GameSettings.UI_SCALE_APPS;
					}
				}
			}
			else if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
			{
				if (currentLayout != null)
				{
					LayoutElement topRightSpacer3 = currentLayout.GetTopRightSpacer();
					if (topRightSpacer3 != null)
					{
						topRightSpacer3.gameObject.SetActive(value: false);
					}
					LayoutElement bottomLeftSpacer3 = currentLayout.GetBottomLeftSpacer();
					if (bottomLeftSpacer3 != null)
					{
						bottomLeftSpacer3.gameObject.SetActive(value: true);
						bottomLeftSpacer3.preferredHeight = 41f / GameSettings.UI_SCALE_APPS;
						bottomLeftSpacer3.preferredWidth = 75f / GameSettings.UI_SCALE_APPS;
					}
				}
			}
			else if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && currentLayout != null)
			{
				LayoutElement topRightSpacer4 = currentLayout.GetTopRightSpacer();
				if (topRightSpacer4 != null)
				{
					topRightSpacer4.gameObject.SetActive(value: false);
				}
				LayoutElement bottomLeftSpacer4 = currentLayout.GetBottomLeftSpacer();
				if (bottomLeftSpacer4 != null)
				{
					bottomLeftSpacer4.gameObject.SetActive(value: true);
					bottomLeftSpacer4.preferredHeight = 41f / GameSettings.UI_SCALE_APPS;
					bottomLeftSpacer4.preferredWidth = 240f / GameSettings.UI_SCALE_APPS;
				}
			}
			Show();
			DetermineVisibilityOfAllApps();
			ScaleModList();
			StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				onReposition();
			}));
			GameEvents.onGUIApplicationLauncherReady.Fire();
		}
		else
		{
			Hide();
		}
	}

	public void OnGameSettingsApplied()
	{
		StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			onReposition();
		}));
	}

	public void OnSceneLoadRequested(GameScenes scene)
	{
		GameEvents.onGUIApplicationLauncherUnreadifying.Fire(scene);
		Hide();
	}

	public void OnSceneLoadedGUIReady(GameScenes scene)
	{
		if (scene != 0)
		{
			Debug.Log("[ApplicationLauncher] OnSceneLoadedGUIReady: scene " + scene.ToString() + " ShouldBeVisible() " + ShouldBeVisible() + " ShouldBeOnTop() " + ShouldBeOnTop() + " iIsPositionedAtTop " + IsPositionedAtTop);
			StartupSequence();
		}
	}

	public void SpawnVertical()
	{
		SpawnSimpleLayout(prefab_verticalTopDown);
	}

	public void SpawnHorizontal()
	{
		SpawnSimpleLayout(prefab_horizontalRightLeft);
	}

	public void SpawnSimpleLayout(SimpleLayout layoutPrefab)
	{
		Debug.Log("[ApplicationLauncher] SpawnSimpleLayout: " + layoutPrefab.layoutName);
		List<UIListItem> list = new List<UIListItem>();
		List<UIListItem> list2 = new List<UIListItem>();
		if (currentLayout != null)
		{
			list = currentLayout.GetStockList().GetUiListItems();
			list2 = currentLayout.GetModList().GetUiListItems();
			currentLayout.GetStockList().ClearUI(tmpButtonStorage);
			currentLayout.GetModList().ClearUI(tmpModButtonStorage);
			UnityEngine.Object.Destroy(currentLayout.GetGameObject());
		}
		currentLayout = UnityEngine.Object.Instantiate(layoutPrefab);
		currentLayout.GetGameObject().transform.SetParent(launcherSpace, worldPositionStays: false);
		currentLayout.GetModListBtnUp().onPointerDownHold.AddListener(BtnUp);
		currentLayout.GetModListBtnDown().onPointerDownHold.AddListener(BtnDown);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			currentLayout.GetStockList().AddItem(list[i]);
		}
		int j = 0;
		for (int count2 = list2.Count; j < count2; j++)
		{
			currentLayout.GetModList().AddItem(list2[j]);
		}
	}

	public UIList GetKnowledgeBaseList()
	{
		return currentLayout.GetKnowledgeBaseList();
	}

	public void AddOnShowCallback(OnShow del)
	{
		onShow = (OnShow)Delegate.Combine(onShow, del);
	}

	public void RemoveOnShowCallback(OnShow del)
	{
		onShow = (OnShow)Delegate.Remove(onShow, del);
	}

	public void AddOnHideCallback(OnHide del)
	{
		onHide = (OnHide)Delegate.Combine(onHide, del);
	}

	public void RemoveOnHideCallback(OnHide del)
	{
		onHide = (OnHide)Delegate.Remove(onHide, del);
	}

	public void AddOnRepositionCallback(OnReposition del)
	{
		onReposition = (OnReposition)Delegate.Combine(onReposition, del);
	}

	public void RemoveOnRepositionCallback(OnReposition del)
	{
		onReposition = (OnReposition)Delegate.Remove(onReposition, del);
	}

	public void EnableMutuallyExclusive(ApplicationLauncherButton launcherButton)
	{
		launcherButton.onLeftClickBtn = (Callback<UIRadioButton>)Delegate.Combine(launcherButton.onLeftClickBtn, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistCLICK));
		launcherButton.onHoverBtn = (Callback<UIRadioButton>)Delegate.Combine(launcherButton.onHoverBtn, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistHOVER));
		launcherButton.onHoverOutBtn = (Callback<UIRadioButton>)Delegate.Combine(launcherButton.onHoverOutBtn, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistHOVEROUT));
		launcherButton.onHoverBtnActive = (Callback<UIRadioButton>)Delegate.Combine(launcherButton.onHoverBtnActive, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistHOVERing));
	}

	public void DisableMutuallyExclusive(ApplicationLauncherButton launcherButton)
	{
		launcherButton.onLeftClickBtn = (Callback<UIRadioButton>)Delegate.Remove(launcherButton.onLeftClickBtn, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistCLICK));
		launcherButton.onHoverBtn = (Callback<UIRadioButton>)Delegate.Remove(launcherButton.onHoverBtn, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistHOVER));
		launcherButton.onHoverOutBtn = (Callback<UIRadioButton>)Delegate.Remove(launcherButton.onHoverOutBtn, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistHOVEROUT));
		launcherButton.onHoverBtnActive = (Callback<UIRadioButton>)Delegate.Remove(launcherButton.onHoverBtnActive, new Callback<UIRadioButton>(MutuallyExclusiveStockApplistHOVERing));
	}

	public void Show()
	{
		launcherSpace.gameObject.SetActive(value: true);
		Ready = true;
		onShow();
	}

	public void Hide()
	{
		launcherSpace.gameObject.SetActive(value: false);
		onHide();
		Ready = false;
	}

	public void ShouldItShow()
	{
		if (ShouldBeVisible() && (HighLogic.LoadedScene != GameScenes.SPACECENTER || !(VesselSpawnDialog.Instance != null) || !VesselSpawnDialog.Instance.Visible))
		{
			Show();
		}
	}

	public void ShouldItHide()
	{
		if (ShouldBeVisible())
		{
			Hide();
		}
	}

	public void ShouldItHide(GameEvents.VesselSpawnInfo info)
	{
		if (ShouldBeVisible())
		{
			Hide();
		}
	}

	public bool ShouldBeOnTop()
	{
		if (HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			return HighLogic.LoadedScene == GameScenes.MAINMENU;
		}
		return true;
	}

	public bool ShouldBeVisible()
	{
		if (HighLogic.LoadedScene != GameScenes.EDITOR && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.MAINMENU && HighLogic.LoadedScene != GameScenes.SPACECENTER)
		{
			return HighLogic.LoadedScene == GameScenes.TRACKSTATION;
		}
		return true;
	}

	public bool ShouldBeVisible(ApplicationLauncherButton button)
	{
		if ((HighLogic.LoadedScene == GameScenes.FLIGHT && !MapView.MapIsEnabled && (button.VisibleInScenes & AppScenes.FLIGHT) != 0) || (HighLogic.LoadedScene == GameScenes.FLIGHT && MapView.MapIsEnabled && (button.VisibleInScenes & AppScenes.MAPVIEW) != 0) || (HighLogic.LoadedScene == GameScenes.SPACECENTER && (button.VisibleInScenes & AppScenes.SPACECENTER) != 0) || (HighLogic.LoadedScene == GameScenes.EDITOR && (button.VisibleInScenes & (AppScenes.flag_5 | AppScenes.flag_6)) != 0) || (HighLogic.LoadedScene == GameScenes.TRACKSTATION && (button.VisibleInScenes & AppScenes.TRACKSTATION) != 0))
		{
			return true;
		}
		if (HighLogic.LoadedScene == GameScenes.MAINMENU)
		{
			return (button.VisibleInScenes & AppScenes.MAINMENU) != 0;
		}
		return false;
	}

	public void DetermineVisibilityOfAllApps()
	{
		List<ApplicationLauncherButton> list = new List<ApplicationLauncherButton>();
		int i = 0;
		for (int count = appList.Count; i < count; i++)
		{
			if (!ShouldBeVisible(appList[i]))
			{
				list.Add(appList[i]);
			}
		}
		int j = 0;
		for (int count2 = list.Count; j < count2; j++)
		{
			SetHidden(list[j]);
		}
		list.Clear();
		int k = 0;
		for (int count3 = appListHidden.Count; k < count3; k++)
		{
			if (ShouldBeVisible(appListHidden[k]))
			{
				list.Add(appListHidden[k]);
			}
		}
		int l = 0;
		for (int count4 = list.Count; l < count4; l++)
		{
			SetVisible(list[l]);
		}
		list.Clear();
		int m = 0;
		for (int count5 = appListMod.Count; m < count5; m++)
		{
			if (!ShouldBeVisible(appListMod[m]))
			{
				list.Add(appListMod[m]);
			}
		}
		int n = 0;
		for (int count6 = list.Count; n < count6; n++)
		{
			SetHidden(list[n]);
		}
		list.Clear();
		int num = 0;
		for (int count7 = appListModHidden.Count; num < count7; num++)
		{
			if (ShouldBeVisible(appListModHidden[num]))
			{
				list.Add(appListModHidden[num]);
			}
		}
		int num2 = 0;
		for (int count8 = list.Count; num2 < count8; num2++)
		{
			SetVisible(list[num2]);
		}
		list.Clear();
	}

	public bool DetermineVisibility(ApplicationLauncherButton button)
	{
		bool num = ShouldBeVisible(button);
		if (num)
		{
			SetVisible(button);
			return num;
		}
		SetHidden(button);
		return num;
	}

	public void SetVisible(ApplicationLauncherButton button)
	{
		if (!appList.Contains(button))
		{
			if (appListHidden.Contains(button))
			{
				Debug.Log("[ApplicationLauncher] SetVisible: ", button.gameObject);
				appListHidden.Remove(button);
				appList.Add(button);
				button.gameObject.SetActive(value: true);
				ScaleModList();
				button.onEnable();
			}
			if (!appListMod.Contains(button) && appListModHidden.Contains(button))
			{
				appListModHidden.Remove(button);
				appListMod.Add(button);
				button.gameObject.SetActive(value: true);
				ScaleModList();
				button.onEnable();
			}
		}
	}

	public void SetHidden(ApplicationLauncherButton button)
	{
		if (!appListHidden.Contains(button))
		{
			if (appList.Contains(button))
			{
				Debug.Log("[ApplicationLauncher] SetHidden: ", button.gameObject);
				appList.Remove(button);
				appListHidden.Add(button);
				button.gameObject.SetActive(value: false);
				ScaleModList();
				button.onDisable();
			}
			if (!appListModHidden.Contains(button) && appListMod.Contains(button))
			{
				appListMod.Remove(button);
				appListModHidden.Add(button);
				button.gameObject.SetActive(value: false);
				ScaleModList();
				button.onDisable();
			}
		}
	}

	public ApplicationLauncherButton AddApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, Texture texture)
	{
		ApplicationLauncherButton applicationLauncherButton = UnityEngine.Object.Instantiate(listItemPrefab);
		EnableMutuallyExclusive(applicationLauncherButton);
		applicationLauncherButton.Setup(onTrue, onFalse, onHover, onHoverOut, onEnable, onDisable, texture);
		AddApplication(applicationLauncherButton);
		return applicationLauncherButton;
	}

	public ApplicationLauncherButton AddApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, Animator sprite)
	{
		ApplicationLauncherButton applicationLauncherButton = UnityEngine.Object.Instantiate(listItemPrefab);
		EnableMutuallyExclusive(applicationLauncherButton);
		applicationLauncherButton.Setup(onTrue, onFalse, onHover, onHoverOut, onEnable, onDisable, sprite);
		AddApplication(applicationLauncherButton);
		return applicationLauncherButton;
	}

	public void AddApplication(ApplicationLauncherButton button)
	{
		button.gameObject.SetActive(value: true);
		appList.Add(button);
		currentLayout.GetStockList().AddItem(button.container);
		ScaleModList();
	}

	public void RemoveApplication(ApplicationLauncherButton button)
	{
		appList.Remove(button);
		appListHidden.Remove(button);
		currentLayout.GetStockList().RemoveItem(button.container, deleteItem: true);
		DisableMutuallyExclusive(button);
		ScaleModList();
	}

	public void MutuallyExclusiveStockApplistCLICK(UIRadioButton btn)
	{
		if (btn.Value)
		{
			if (lastPinnedButton != null && lastPinnedButton != btn)
			{
				lastPinnedButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
			}
			lastPinnedButton = btn;
		}
		else
		{
			lastPinnedButton = null;
		}
	}

	public void MutuallyExclusiveStockApplistHOVER(UIRadioButton btn)
	{
		StopAllCoroutines();
		if (lastPinnedButton != null)
		{
			if (lastPinnedButton != btn)
			{
				lastPinnedButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
			}
			else if (!btn.Value)
			{
				lastPinnedButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
			}
		}
	}

	public void MutuallyExclusiveStockApplistHOVEROUT(UIRadioButton btn)
	{
		if (!(lastPinnedButton != null) || btn.Value)
		{
			return;
		}
		StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			if (lastPinnedButton != null)
			{
				lastPinnedButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
			}
		}));
	}

	public void MutuallyExclusiveStockApplistHOVERing(UIRadioButton btn)
	{
		StopAllCoroutines();
	}

	public bool Contains(ApplicationLauncherButton button, out bool hidden)
	{
		hidden = false;
		if (appListModHidden.Contains(button))
		{
			hidden = true;
		}
		if (!appListMod.Contains(button))
		{
			return appListModHidden.Contains(button);
		}
		return true;
	}

	public ApplicationLauncherButton AddModApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, AppScenes visibleInScenes, Texture texture)
	{
		ApplicationLauncherButton applicationLauncherButton = UnityEngine.Object.Instantiate(listItemPrefab);
		applicationLauncherButton.Setup(onTrue, onFalse, onHover, onHoverOut, onEnable, onDisable, texture);
		AddModApplication(applicationLauncherButton);
		applicationLauncherButton.VisibleInScenes = visibleInScenes;
		return applicationLauncherButton;
	}

	public ApplicationLauncherButton AddModApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, AppScenes visibleInScenes, Animator sprite)
	{
		ApplicationLauncherButton applicationLauncherButton = UnityEngine.Object.Instantiate(listItemPrefab);
		applicationLauncherButton.Setup(onTrue, onFalse, onHover, onHoverOut, onEnable, onDisable, sprite);
		AddModApplication(applicationLauncherButton);
		applicationLauncherButton.VisibleInScenes = visibleInScenes;
		return applicationLauncherButton;
	}

	public void AddModApplication(ApplicationLauncherButton button)
	{
		button.gameObject.SetActive(value: true);
		appListMod.Add(button);
		currentLayout.GetModList().AddItem(button.container);
		ScaleModList();
	}

	public void RemoveModApplication(ApplicationLauncherButton button)
	{
		appListMod.Remove(button);
		appListModHidden.Remove(button);
		currentLayout.GetModList().RemoveItem(button.container, deleteItem: true);
		DisableMutuallyExclusive(button);
		ScaleModList();
	}

	public void ScaleModList()
	{
		Canvas.ForceUpdateCanvases();
		if (GetModListSize() == 0)
		{
			currentLayout.GetModListDivider().gameObject.SetActive(value: false);
			currentLayout.GetModListBtnDown().gameObject.SetActive(value: false);
			currentLayout.GetModListBtnUp().gameObject.SetActive(value: false);
			return;
		}
		currentLayout.GetModListDivider().gameObject.SetActive(value: true);
		Debug.Log("ScaleModList: listSize " + GetModListSize() + " maxListSize " + GetModListMaxSize());
		if (GetModListSize() < GetModListMaxSize())
		{
			currentLayout.GetModListBtnDown().gameObject.SetActive(value: false);
			currentLayout.GetModListBtnUp().gameObject.SetActive(value: false);
		}
		else
		{
			currentLayout.GetModListBtnDown().gameObject.SetActive(value: true);
			currentLayout.GetModListBtnUp().gameObject.SetActive(value: true);
		}
	}

	public int GetModListMaxSize()
	{
		if (IsPositionedAtTop)
		{
			return (int)(currentLayout.GetModScrollRect().transform as RectTransform).sizeDelta.y;
		}
		return (int)(currentLayout.GetModScrollRect().transform as RectTransform).sizeDelta.x;
	}

	public int GetModListSize()
	{
		if (IsPositionedAtTop)
		{
			return (int)(currentLayout.GetModList().transform as RectTransform).sizeDelta.y;
		}
		return (int)(currentLayout.GetModList().transform as RectTransform).sizeDelta.x;
	}

	public int GetAppListSize()
	{
		return listItemWidth * appList.Count;
	}

	public void BtnUp(PointerEventData data)
	{
		if (IsPositionedAtTop)
		{
			currentLayout.GetModScrollRect().verticalNormalizedPosition = Mathf.Clamp01(currentLayout.GetModScrollRect().verticalNormalizedPosition + 1f * Time.deltaTime * U5Util.GetSimulatedScrollMultiplier(currentLayout.GetModScrollRect(), vertical: true));
		}
		else
		{
			currentLayout.GetModScrollRect().horizontalNormalizedPosition = Mathf.Clamp01(currentLayout.GetModScrollRect().horizontalNormalizedPosition + 1f * Time.deltaTime * U5Util.GetSimulatedScrollMultiplier(currentLayout.GetModScrollRect(), vertical: false));
		}
	}

	public void BtnDown(PointerEventData data)
	{
		if (IsPositionedAtTop)
		{
			currentLayout.GetModScrollRect().verticalNormalizedPosition = Mathf.Clamp01(currentLayout.GetModScrollRect().verticalNormalizedPosition - 1f * Time.deltaTime * U5Util.GetSimulatedScrollMultiplier(currentLayout.GetModScrollRect(), vertical: true));
		}
		else
		{
			currentLayout.GetModScrollRect().horizontalNormalizedPosition = Mathf.Clamp01(currentLayout.GetModScrollRect().horizontalNormalizedPosition - 1f * Time.deltaTime * U5Util.GetSimulatedScrollMultiplier(currentLayout.GetModScrollRect(), vertical: false));
		}
	}
}
