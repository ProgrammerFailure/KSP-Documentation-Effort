using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens;

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
		VAB = 8,
		SPH = 0x10,
		TRACKSTATION = 0x20,
		MAINMENU = 0x40
	}

	public static bool Ready;

	private ApplauncherLayout currentLayout;

	public SimpleLayout prefab_verticalTopDown;

	public SimpleLayout prefab_horizontalRightLeft;

	public RectTransform launcherSpace;

	public RectTransform appSpace;

	public ApplicationLauncherButton listItemPrefab;

	public RectTransform tmpButtonStorage;

	public RectTransform tmpModButtonStorage;

	private int listItemWidth;

	private OnShow onShow;

	private OnHide onHide;

	private OnReposition onReposition;

	private List<ApplicationLauncherButton> appList;

	private List<ApplicationLauncherButton> appListHidden;

	private List<ApplicationLauncherButton> appListMod;

	private List<ApplicationLauncherButton> appListModHidden;

	private bool startedAtBottom;

	private bool applauncherInitialized;

	private UIRadioButton lastPinnedButton;

	public static ApplicationLauncher Instance
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

	public ApplauncherLayout CurrentLayout
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsPositionedAtTop
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ApplicationLauncher()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StartupSequence()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadedGUIReady(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnVertical()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnHorizontal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnSimpleLayout(SimpleLayout layoutPrefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIList GetKnowledgeBaseList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnShowCallback(OnShow del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnShowCallback(OnShow del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnHideCallback(OnHide del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnHideCallback(OnHide del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnRepositionCallback(OnReposition del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnRepositionCallback(OnReposition del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableMutuallyExclusive(ApplicationLauncherButton launcherButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableMutuallyExclusive(ApplicationLauncherButton launcherButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShouldItShow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShouldItHide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShouldItHide(GameEvents.VesselSpawnInfo info)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ShouldBeOnTop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ShouldBeVisible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ShouldBeVisible(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DetermineVisibilityOfAllApps()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool DetermineVisibility(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetVisible(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetHidden(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ApplicationLauncherButton AddApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, Texture texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ApplicationLauncherButton AddApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, Animator sprite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddApplication(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveApplication(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MutuallyExclusiveStockApplistCLICK(UIRadioButton btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MutuallyExclusiveStockApplistHOVER(UIRadioButton btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MutuallyExclusiveStockApplistHOVEROUT(UIRadioButton btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MutuallyExclusiveStockApplistHOVERing(UIRadioButton btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(ApplicationLauncherButton button, out bool hidden)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ApplicationLauncherButton AddModApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, AppScenes visibleInScenes, Texture texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ApplicationLauncherButton AddModApplication(Callback onTrue, Callback onFalse, Callback onHover, Callback onHoverOut, Callback onEnable, Callback onDisable, AppScenes visibleInScenes, Animator sprite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddModApplication(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveModApplication(ApplicationLauncherButton button)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ScaleModList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetModListMaxSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetModListSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetAppListSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnUp(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnDown(PointerEventData data)
	{
		throw null;
	}
}
