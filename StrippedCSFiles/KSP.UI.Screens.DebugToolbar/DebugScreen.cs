using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.DebugToolbar.Screens.Cheats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar;

public class DebugScreen : MonoBehaviour
{
	private static List<DebugScreen> debugScreens;

	public RectTransform screenTransform;

	public RectTransform contentTransform;

	public UITreeView treeView;

	public Button exitButton;

	public TextMeshProUGUI titleText;

	public const string LockID = "DebugToolbar";

	private bool _inputLocked;

	private bool _cheatsLocked;

	[SerializeField]
	private string[] CheatScreenNames;

	private List<RectTransform> screens;

	private RectTransform currentScreen;

	private HackGravity hackGravity;

	private PauseOnVesselUnpack pauseOnVesselUnpack;

	private UnbreakableJoints unbreakableJoints;

	private NoCrashDamage noCrashDamage;

	private IgnoreMaxTemperature ignoreMaxTemperature;

	private InfinitePropellant infinitePropellant;

	private InfiniteElectricity infiniteElectricity;

	private BiomesVisibleInMap biomesVisibleInMap;

	private AllowPartClipping allowPartClipping;

	private NonStrictPartAttachment nonStrictPartAttachment;

	private IgnoreEVAConstructionMassLimit ignoreEVAConstructionMassLimit;

	private static List<UITreeView.Item> treeItems;

	private static bool treeItemsDirty;

	public bool isShown
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

	public bool InputLocked
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DebugScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DebugScreen()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Show")]
	public void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Hide")]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameSceneLoad(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ResetCheats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UITreeView.Item GetItem(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UITreeView.Item AddContentItem(UITreeView.Item parent, string name, string text, Action onExpand, Action onSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static UITreeView.Item AddItem(UITreeView.Item parent, string name, string text, Action onExpand, Action onSelect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UITreeView.Item AddContentScreen(UITreeView.Item parent, string name, string text, RectTransform prefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UITreeView.Item AddContentScreen(string parentName, string name, string text, RectTransform prefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OnSelectItem(string title, string url, RectTransform screen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SelectItem(string title, string url, RectTransform screen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitClick()
	{
		throw null;
	}
}
