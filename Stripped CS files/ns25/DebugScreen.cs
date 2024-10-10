using System;
using System.Collections.Generic;
using Expansions.Missions;
using ns2;
using ns27;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns25;

public class DebugScreen : MonoBehaviour
{
	public static List<DebugScreen> debugScreens = new List<DebugScreen>();

	public RectTransform screenTransform;

	public RectTransform contentTransform;

	public UITreeView treeView;

	public Button exitButton;

	public TextMeshProUGUI titleText;

	public const string LockID = "DebugToolbar";

	public bool _inputLocked;

	public bool _cheatsLocked;

	[SerializeField]
	public string[] CheatScreenNames;

	public List<RectTransform> screens = new List<RectTransform>();

	public RectTransform currentScreen;

	public HackGravity hackGravity;

	public PauseOnVesselUnpack pauseOnVesselUnpack;

	public UnbreakableJoints unbreakableJoints;

	public NoCrashDamage noCrashDamage;

	public IgnoreMaxTemperature ignoreMaxTemperature;

	public InfinitePropellant infinitePropellant;

	public InfiniteElectricity infiniteElectricity;

	public BiomesVisibleInMap biomesVisibleInMap;

	public AllowPartClipping allowPartClipping;

	public NonStrictPartAttachment nonStrictPartAttachment;

	public IgnoreEVAConstructionMassLimit ignoreEVAConstructionMassLimit;

	public static List<UITreeView.Item> treeItems = new List<UITreeView.Item>();

	public static bool treeItemsDirty = true;

	public bool isShown { get; set; }

	public bool InputLocked
	{
		get
		{
			return _inputLocked;
		}
		set
		{
			if (_inputLocked != value)
			{
				_inputLocked = value;
				if (_inputLocked)
				{
					InputLockManager.SetControlLock(ControlTypes.ALLBUTCAMERAS, "DebugToolbar");
				}
				else
				{
					InputLockManager.RemoveControlLock("DebugToolbar");
				}
			}
		}
	}

	public void Awake()
	{
		exitButton.onClick.AddListener(OnExitClick);
		debugScreens.Add(this);
		isShown = true;
		GameEvents.onGameSceneLoadRequested.Add(onGameSceneLoad);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(onGameSceneLoad);
		debugScreens.Remove(this);
		InputLocked = false;
	}

	public void Update()
	{
		if (treeItemsDirty)
		{
			treeView.ClearItems();
			int i = 0;
			for (int count = treeItems.Count; i < count; i++)
			{
				if (!_cheatsLocked)
				{
					treeView.AddItem(treeItems[i]);
					continue;
				}
				bool flag = false;
				for (int j = 0; j < CheatScreenNames.Length; j++)
				{
					if (treeItems[i].name == CheatScreenNames[j])
					{
						flag = true;
					}
				}
				if (!flag)
				{
					treeView.AddItem(treeItems[i]);
				}
			}
			treeItemsDirty = false;
		}
		if (currentScreen == null)
		{
			treeView.SelectFirstItem(fireCallback: true);
		}
		if (isShown)
		{
			InputLocked = (bool)UIMasterController.Instance && RectTransformUtility.RectangleContainsScreenPoint(screenTransform, Input.mousePosition, UIMasterController.Instance.uiCamera);
		}
	}

	[ContextMenu("Show")]
	public void Show()
	{
		if (!isShown)
		{
			screenTransform.gameObject.SetActive(value: true);
			if (currentScreen == null)
			{
				treeView.SelectFirstItem(fireCallback: true);
			}
			AnalyticsUtil.LogDebugWindow(HighLogic.CurrentGame);
			isShown = true;
		}
	}

	[ContextMenu("Hide")]
	public void Hide()
	{
		if (isShown)
		{
			screenTransform.gameObject.SetActive(value: false);
			isShown = false;
			InputLocked = false;
		}
	}

	public void onGameSceneLoad(GameScenes scene)
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && !HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().cheatsEnabled)
		{
			_cheatsLocked = true;
			if (currentScreen != null)
			{
				ResetCheats();
				for (int i = 0; i < CheatScreenNames.Length; i++)
				{
					if (currentScreen.name == CheatScreenNames[i])
					{
						treeView.SelectFirstItem(fireCallback: true);
					}
				}
			}
		}
		else
		{
			_cheatsLocked = false;
		}
		treeItemsDirty = true;
	}

	public void ResetCheats()
	{
		if (hackGravity == null)
		{
			hackGravity = contentTransform.GetComponentInChildren<HackGravity>(includeInactive: true);
		}
		if (hackGravity != null && hackGravity.toggle.isOn)
		{
			hackGravity.OnResetClick();
			hackGravity.toggle.isOn = false;
		}
		if (pauseOnVesselUnpack == null)
		{
			pauseOnVesselUnpack = contentTransform.GetComponentInChildren<PauseOnVesselUnpack>(includeInactive: true);
		}
		if (pauseOnVesselUnpack != null && pauseOnVesselUnpack.toggle.isOn)
		{
			pauseOnVesselUnpack.Set(b: false);
		}
		if (unbreakableJoints == null)
		{
			unbreakableJoints = contentTransform.GetComponentInChildren<UnbreakableJoints>(includeInactive: true);
		}
		if (unbreakableJoints != null && unbreakableJoints.toggle.isOn)
		{
			unbreakableJoints.Set(b: false);
		}
		if (noCrashDamage == null)
		{
			noCrashDamage = contentTransform.GetComponentInChildren<NoCrashDamage>(includeInactive: true);
		}
		if (noCrashDamage != null && noCrashDamage.toggle.isOn)
		{
			noCrashDamage.Set(b: false);
		}
		if (ignoreMaxTemperature == null)
		{
			ignoreMaxTemperature = contentTransform.GetComponentInChildren<IgnoreMaxTemperature>(includeInactive: true);
		}
		if (ignoreMaxTemperature != null && ignoreMaxTemperature.toggle.isOn)
		{
			ignoreMaxTemperature.Set(b: false);
		}
		if (infinitePropellant == null)
		{
			infinitePropellant = contentTransform.GetComponentInChildren<InfinitePropellant>(includeInactive: true);
		}
		if (infinitePropellant != null && infinitePropellant.toggle.isOn)
		{
			infinitePropellant.Set(b: false);
		}
		if (infiniteElectricity == null)
		{
			infiniteElectricity = contentTransform.GetComponentInChildren<InfiniteElectricity>(includeInactive: true);
		}
		if (infiniteElectricity != null && infiniteElectricity.toggle.isOn)
		{
			infiniteElectricity.Set(b: false);
		}
		if (biomesVisibleInMap == null)
		{
			biomesVisibleInMap = contentTransform.GetComponentInChildren<BiomesVisibleInMap>(includeInactive: true);
		}
		if (biomesVisibleInMap != null && biomesVisibleInMap.toggle.isOn)
		{
			biomesVisibleInMap.Set(b: false);
		}
		if (allowPartClipping == null)
		{
			allowPartClipping = contentTransform.GetComponentInChildren<AllowPartClipping>(includeInactive: true);
		}
		if (allowPartClipping != null && allowPartClipping.toggle.isOn)
		{
			allowPartClipping.Set(b: false);
		}
		if (nonStrictPartAttachment == null)
		{
			nonStrictPartAttachment = contentTransform.GetComponentInChildren<NonStrictPartAttachment>(includeInactive: true);
		}
		if (nonStrictPartAttachment != null && nonStrictPartAttachment.toggle.isOn)
		{
			nonStrictPartAttachment.Set(b: false);
		}
		if (ignoreEVAConstructionMassLimit == null)
		{
			ignoreEVAConstructionMassLimit = contentTransform.GetComponentInChildren<IgnoreEVAConstructionMassLimit>(includeInactive: true);
		}
		if (ignoreEVAConstructionMassLimit != null && ignoreEVAConstructionMassLimit.toggle.isOn)
		{
			ignoreEVAConstructionMassLimit.Set(b: false);
		}
	}

	public static UITreeView.Item GetItem(string name)
	{
		int num = 0;
		int count = treeItems.Count;
		while (true)
		{
			if (num < count)
			{
				if (treeItems[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return treeItems[num];
	}

	public static UITreeView.Item AddContentItem(UITreeView.Item parent, string name, string text, Action onExpand, Action onSelect)
	{
		return AddItem(parent, name, text, onExpand, onSelect);
	}

	public static UITreeView.Item AddItem(UITreeView.Item parent, string name, string text, Action onExpand, Action onSelect)
	{
		UITreeView.Item item = new UITreeView.Item(parent, name, text, onExpand, onSelect);
		if (parent == null)
		{
			treeItems.Add(item);
		}
		else
		{
			parent.subItems.Add(item);
		}
		treeItemsDirty = true;
		return item;
	}

	public static UITreeView.Item AddContentScreen(UITreeView.Item parent, string name, string text, RectTransform prefab)
	{
		string title = (string.IsNullOrEmpty(text) ? name : text);
		string url = ((parent != null) ? (parent.url + "/") : string.Empty) + name;
		return AddContentItem(parent, name, text, null, delegate
		{
			OnSelectItem(title, url, prefab);
		});
	}

	public static UITreeView.Item AddContentScreen(string parentName, string name, string text, RectTransform prefab)
	{
		return AddContentScreen(GetItem(parentName), name, text, prefab);
	}

	public static void OnSelectItem(string title, string url, RectTransform screen)
	{
		if (!(screen == null))
		{
			int i = 0;
			for (int count = debugScreens.Count; i < count; i++)
			{
				debugScreens[i].SelectItem(title, url, screen);
			}
		}
	}

	public void SelectItem(string title, string url, RectTransform screen)
	{
		RectTransform rectTransform = null;
		int i = 0;
		for (int count = screens.Count; i < count; i++)
		{
			if (screens[i].name == url)
			{
				rectTransform = screens[i];
				break;
			}
		}
		if (rectTransform == null)
		{
			rectTransform = UnityEngine.Object.Instantiate(screen);
			rectTransform.SetParent(contentTransform, worldPositionStays: false);
			rectTransform.gameObject.SetActive(value: false);
			rectTransform.name = url;
			screens.Add(rectTransform);
		}
		if (currentScreen != null)
		{
			currentScreen.gameObject.SetActive(value: false);
		}
		currentScreen = rectTransform;
		currentScreen.gameObject.SetActive(value: true);
		titleText.text = title;
	}

	public void OnExitClick()
	{
		Hide();
	}
}
