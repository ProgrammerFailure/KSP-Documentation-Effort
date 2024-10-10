using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Expansions;
using Expansions.Missions;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyOptionsMenu : MonoBehaviour
{
	public class SortedVerticalLayout
	{
		public int order;

		public DialogGUIVerticalLayout layout;

		public string LocalizedTitle;

		public SortedVerticalLayout(DialogGUIVerticalLayout layout, int order, string LocalizedTitle)
		{
			this.order = order;
			this.layout = layout;
			this.LocalizedTitle = LocalizedTitle;
		}
	}

	public GameParameters gPars;

	public GameParameters backupPars;

	public Game.Modes gameMode;

	public Callback<GameParameters, bool> OnDismiss = delegate
	{
	};

	public Rect dialogRect;

	public float width = 1024f;

	public float height = 500f;

	public UISkinDef skin;

	public bool isNewGame;

	public static GameObject minisettingsSource;

	public string currentSection;

	public string CurrentSection => currentSection;

	public static DifficultyOptionsMenu Create(Game.Modes mode, GameParameters initialParams, bool newGame, Callback<GameParameters, bool> OnDismiss)
	{
		return Create(mode, initialParams, newGame, OnDismiss, null);
	}

	public static DifficultyOptionsMenu Create(Game.Modes mode, GameParameters initialParams, bool newGame, Callback<GameParameters, bool> OnDismiss, GameObject miniSettings)
	{
		if (miniSettings != null)
		{
			minisettingsSource = miniSettings;
		}
		DifficultyOptionsMenu difficultyOptionsMenu = new GameObject("DifficultyOptionsMenu").AddComponent<DifficultyOptionsMenu>();
		difficultyOptionsMenu.gameMode = mode;
		difficultyOptionsMenu.isNewGame = newGame;
		difficultyOptionsMenu.gPars = initialParams;
		ConfigNode node = new ConfigNode();
		initialParams.Save(node);
		difficultyOptionsMenu.backupPars = new GameParameters(node);
		if (!newGame)
		{
			difficultyOptionsMenu.gPars.preset = GameParameters.Preset.Custom;
		}
		difficultyOptionsMenu.OnDismiss = OnDismiss;
		difficultyOptionsMenu.dialogRect = new Rect(0.5f, 0.5f, difficultyOptionsMenu.width, difficultyOptionsMenu.height);
		difficultyOptionsMenu.skin = UISkinManager.GetSkin("MainMenuSkin");
		difficultyOptionsMenu.CreateDifficultWindow();
		GameEvents.OnDifficultySettingsShown.Fire(difficultyOptionsMenu);
		return difficultyOptionsMenu;
	}

	public void Start()
	{
		InputLockManager.SetControlLock("difficultyOptionsMenu");
	}

	public PopupDialog CreateDifficultWindow()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUIHorizontalLayout item = new DialogGUIHorizontalLayout(false, false, 2f, new RectOffset(), TextAnchor.UpperCenter, new DialogGUIToggleButton(() => gPars.preset == GameParameters.Preset.Easy, Localizer.Format("#autoLoc_6002171", GameParameters.GetPresetColorHex(GameParameters.Preset.Easy)), delegate(bool b)
		{
			if (b)
			{
				SetPreset(GameParameters.Preset.Easy);
			}
		}, 80f, 28f), new DialogGUIToggleButton(() => gPars.preset == GameParameters.Preset.Normal, Localizer.Format("#autoLoc_6002172", GameParameters.GetPresetColorHex(GameParameters.Preset.Normal)), delegate(bool b)
		{
			if (b)
			{
				SetPreset(GameParameters.Preset.Normal);
			}
		}, 80f, 28f), new DialogGUIToggleButton(() => gPars.preset == GameParameters.Preset.Moderate, Localizer.Format("#autoLoc_6002173", GameParameters.GetPresetColorHex(GameParameters.Preset.Moderate)), delegate(bool b)
		{
			if (b)
			{
				SetPreset(GameParameters.Preset.Moderate);
			}
		}, 80f, 28f), new DialogGUIToggleButton(() => gPars.preset == GameParameters.Preset.Hard, Localizer.Format("#autoLoc_6002174", GameParameters.GetPresetColorHex(GameParameters.Preset.Hard)), delegate(bool b)
		{
			if (b)
			{
				SetPreset(GameParameters.Preset.Hard);
			}
		}, 80f, 28f), new DialogGUIToggleButton(() => gPars.preset == GameParameters.Preset.Custom, Localizer.Format("#autoLoc_6002175", GameParameters.GetPresetColorHex(GameParameters.Preset.Custom)), delegate(bool b)
		{
			if (b)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
		}, 80f, 28f));
		if (isNewGame)
		{
			list.Add(item);
		}
		DialogGUILabel dialogGUILabel = new DialogGUILabel(() => string.Format(Localizer.Format("#autoLOC_189637", (gPars.Difficulty.RespawnTimer / 3600f).ToString("N0"))));
		DialogGUISlider dialogGUISlider = new DialogGUISlider(() => gPars.Difficulty.RespawnTimer / 3600f, 1f, 100f, wholeNumbers: true, 236f, 28f, delegate(float v)
		{
			if (!Mathf.Approximately(gPars.Difficulty.RespawnTimer, Mathf.Round(v) * 3600f))
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.RespawnTimer = Mathf.Round(v) * 3600f;
		});
		dialogGUISlider.tooltipText = Localizer.Format("#autoLOC_189646");
		dialogGUILabel.OptionInteractableCondition = (dialogGUISlider.OptionInteractableCondition = () => gPars.Difficulty.MissingCrewsRespawn);
		DialogGUIToggle dialogGUIToggle = new DialogGUIToggle(() => gPars.Difficulty.AutoHireCrews, Localizer.Format("#autoLOC_189651"), delegate(bool b)
		{
			if (b != gPars.Difficulty.AutoHireCrews)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.AutoHireCrews = b;
		}, 250f);
		dialogGUIToggle.tooltipText = Localizer.Format("#autoLOC_189658");
		DialogGUIToggle child = new DialogGUIToggle(() => gPars.Difficulty.MissingCrewsRespawn, Localizer.Format("#autoLOC_189677"), delegate(bool b)
		{
			if (b != gPars.Difficulty.MissingCrewsRespawn)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.MissingCrewsRespawn = b;
		}, 250f);
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(false, false, 2f, new RectOffset(), TextAnchor.UpperLeft, new DialogGUIBox(Localizer.Format("#autoLOC_189661"), 250f, 18f, null), new DialogGUISpace(4f), new DialogGUIToggle(() => gPars.Flight.CanRestart, Localizer.Format("#autoLOC_189662"), delegate(bool b)
		{
			if (b != gPars.Flight.CanRestart)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Flight.CanRestart = b;
			gPars.Flight.CanLeaveToEditor = gPars.Flight.CanRestart;
		}, 250f), new DialogGUIToggle(() => gPars.Flight.CanQuickLoad, Localizer.Format("#autoLOC_189670"), delegate(bool b)
		{
			if (b != gPars.Flight.CanQuickLoad)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Flight.CanQuickLoad = b;
		}, 250f));
		if (gameMode != Game.Modes.MISSION && gameMode != Game.Modes.MISSION_BUILDER)
		{
			dialogGUIVerticalLayout.AddChild(child);
			dialogGUIVerticalLayout.AddChild(dialogGUILabel);
			dialogGUIVerticalLayout.AddChild(dialogGUISlider);
			dialogGUIVerticalLayout.AddChild(dialogGUIToggle);
		}
		if ((gameMode == Game.Modes.CAREER && isNewGame) || gameMode == Game.Modes.MISSION)
		{
			DialogGUIToggle dialogGUIToggle2 = new DialogGUIToggle(() => gPars.Difficulty.BypassEntryPurchaseAfterResearch, Localizer.Format("#autoLOC_189690"), delegate(bool b)
			{
				if (b != gPars.Difficulty.BypassEntryPurchaseAfterResearch)
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Difficulty.BypassEntryPurchaseAfterResearch = b;
			}, 250f);
			dialogGUIToggle2.tooltipText = Localizer.Format("#autoLOC_189697");
			dialogGUIToggle2.OptionInteractableCondition = () => gameMode != Game.Modes.MISSION || gPars.CustomParams<MissionParamsGeneral>().enableFunding;
			dialogGUIVerticalLayout.AddChild(dialogGUIToggle2);
		}
		dialogGUIVerticalLayout.AddChild(new DialogGUIToggle(() => gPars.Difficulty.IndestructibleFacilities, Localizer.Format("#autoLOC_189700"), delegate(bool b)
		{
			if (b != gPars.Difficulty.IndestructibleFacilities)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.IndestructibleFacilities = b;
		}, 250f));
		dialogGUIVerticalLayout.AddChild(new DialogGUIToggle(() => gPars.Difficulty.AllowStockVessels, Localizer.Format("#autoLOC_189707"), delegate(bool b)
		{
			if (b != gPars.Difficulty.AllowStockVessels)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.AllowStockVessels = b;
		}, 250f));
		dialogGUIVerticalLayout.AddChild(new DialogGUIToggle(() => gPars.Difficulty.persistKerbalInventories, Localizer.Format("#autoLOC_6002565"), delegate(bool b)
		{
			if (b != gPars.Difficulty.persistKerbalInventories)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.persistKerbalInventories = b;
		}, 250f));
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && gameMode != Game.Modes.MISSION && gameMode != Game.Modes.MISSION_BUILDER)
		{
			dialogGUIVerticalLayout.AddChild(new DialogGUIToggle(() => gPars.Difficulty.AllowOtherLaunchSites, Localizer.Format("#autoLOC_6005017"), delegate(bool b)
			{
				if (b != gPars.Difficulty.AllowOtherLaunchSites)
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Difficulty.AllowOtherLaunchSites = b;
			}, 250f));
		}
		DialogGUIVerticalLayout dialogGUIVerticalLayout2 = new DialogGUIVerticalLayout(false, false, 2f, new RectOffset(), TextAnchor.UpperLeft, new DialogGUIBox(Localizer.Format("#autoLOC_189717"), 250f, 18f, null), new DialogGUISpace(4f));
		if (gameMode == Game.Modes.CAREER && isNewGame)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189725", gPars.Career.StartingFunds.ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.StartingFunds / 1000f, 0f, 500f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.StartingFunds, Mathf.Round(v) * 1000f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.StartingFunds = Mathf.Round(v) * 1000f;
			}));
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189736", gPars.Career.StartingScience.ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.StartingScience / 10f, 0f, 500f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.StartingScience, Mathf.Round(v) * 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.StartingScience = Mathf.Round(v) * 10f;
			}));
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189747", gPars.Career.StartingReputation.ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.StartingReputation / 10f, -100f, 100f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.StartingReputation, Mathf.Round(v) * 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.StartingReputation = Mathf.Round(v) * 10f;
			}));
		}
		if (gameMode == Game.Modes.CAREER || gameMode == Game.Modes.SCIENCE_SANDBOX)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189762", (gPars.Career.ScienceGainMultiplier * 100f).ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.ScienceGainMultiplier * 10f, 1f, 100f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.ScienceGainMultiplier, Mathf.Round(v) / 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.ScienceGainMultiplier = Mathf.Round(v) / 10f;
			}));
		}
		if (gameMode == Game.Modes.CAREER)
		{
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189776", (gPars.Career.FundsGainMultiplier * 100f).ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.FundsGainMultiplier * 10f, 1f, 100f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.FundsGainMultiplier, Mathf.Round(v) / 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.FundsGainMultiplier = Mathf.Round(v) / 10f;
			}));
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189786", (gPars.Career.RepGainMultiplier * 100f).ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.RepGainMultiplier * 10f, 1f, 100f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.RepGainMultiplier, Mathf.Round(v) / 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.RepGainMultiplier = Mathf.Round(v) / 10f;
			}));
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189796", (gPars.Career.FundsLossMultiplier * 100f).ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.FundsLossMultiplier * 10f, 1f, 100f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.FundsLossMultiplier, Mathf.Round(v) / 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.FundsLossMultiplier = Mathf.Round(v) / 10f;
			}));
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189806", (gPars.Career.RepLossMultiplier * 100f).ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.RepLossMultiplier * 10f, 1f, 100f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.RepLossMultiplier, Mathf.Round(v) / 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.RepLossMultiplier = Mathf.Round(v) / 10f;
			}));
			dialogGUIVerticalLayout2.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189816", gPars.Career.RepLossDeclined.ToString("N0"))));
			dialogGUIVerticalLayout2.AddChild(new DialogGUISlider(() => gPars.Career.RepLossDeclined, 0f, 10f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Career.RepLossDeclined, Mathf.Round(v)))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Career.RepLossDeclined = Mathf.Round(v);
			}));
		}
		DialogGUIVerticalLayout dialogGUIVerticalLayout3 = new DialogGUIVerticalLayout(false, false, 2f, new RectOffset(), TextAnchor.UpperLeft, new DialogGUIBox(Localizer.Format("#autoLOC_189828"), 250f, 18f, null), new DialogGUISpace(4f));
		dialogGUIVerticalLayout3.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189833", (gPars.Difficulty.ReentryHeatScale * 100f).ToString("N0"))));
		dialogGUIVerticalLayout3.AddChild(new DialogGUISlider(() => gPars.Difficulty.ReentryHeatScale * 10f, 0f, 12f, wholeNumbers: true, 236f, 28f, delegate(float v)
		{
			if (!Mathf.Approximately(gPars.Difficulty.ReentryHeatScale, Mathf.Round(v) / 10f))
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.ReentryHeatScale = Mathf.Round(v) / 10f;
		}));
		if (isNewGame)
		{
			dialogGUIVerticalLayout3.AddChild(new DialogGUILabel(() => Localizer.Format("#autoLOC_189847", (gPars.Difficulty.ResourceAbundance * 100f).ToString("N0"))));
			dialogGUIVerticalLayout3.AddChild(new DialogGUISlider(() => gPars.Difficulty.ResourceAbundance * 10f, 1f, 12f, wholeNumbers: true, 236f, 28f, delegate(float v)
			{
				if (!Mathf.Approximately(gPars.Difficulty.ResourceAbundance, Mathf.Round(v) / 10f))
				{
					gPars.preset = GameParameters.Preset.Custom;
				}
				gPars.Difficulty.ResourceAbundance = Mathf.Round(v) / 10f;
			}));
		}
		dialogGUIVerticalLayout3.AddChild(new DialogGUIToggle(() => gPars.Difficulty.EnableCommNet, Localizer.Format("#autoLOC_189857"), delegate(bool b)
		{
			if (b != gPars.Difficulty.EnableCommNet)
			{
				gPars.preset = GameParameters.Preset.Custom;
			}
			gPars.Difficulty.EnableCommNet = b;
		}, 250f));
		GameParameters.GameMode currentGameModeFilter = ((gameMode == Game.Modes.MISSION) ? GameParameters.GameMode.MISSION : ((gameMode == Game.Modes.MISSION_BUILDER) ? GameParameters.GameMode.MISSION : ((gameMode == Game.Modes.CAREER) ? GameParameters.GameMode.CAREER : ((gameMode != Game.Modes.SCIENCE_SANDBOX) ? GameParameters.GameMode.SANDBOX : GameParameters.GameMode.SCIENCE))));
		ListDictionary<string, SortedVerticalLayout> listDictionary = new ListDictionary<string, SortedVerticalLayout>();
		for (int i = 0; i < GameParameters.ParameterTypes.Count; i++)
		{
			Type parameterType = GameParameters.ParameterTypes[i];
			GameParameters.CustomParameterNode customParamNode = gPars.CustomParams(parameterType);
			if (customParamNode == null || (customParamNode.GameMode & currentGameModeFilter) == 0)
			{
				continue;
			}
			DialogGUIVerticalLayout dialogGUIVerticalLayout4 = new DialogGUIVerticalLayout(250f, -1f, 2f, new RectOffset(0, 0, 4, 4), TextAnchor.UpperLeft);
			float num = 2f;
			MemberInfo[] members = customParamNode.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			for (int j = 0; j < members.Length; j++)
			{
				MemberInfo member = members[j];
				FieldInfo field = members[j] as FieldInfo;
				PropertyInfo property = members[j] as PropertyInfo;
				if (field == null && property == null)
				{
					continue;
				}
				GameParameters.CustomParameterUI[] array = (GameParameters.CustomParameterUI[])member.GetCustomAttributes(typeof(GameParameters.CustomParameterUI), inherit: true);
				if (array == null || array.Length == 0)
				{
					continue;
				}
				GameParameters.CustomParameterUI customParameterUI = array[0];
				if ((customParameterUI.gameMode & currentGameModeFilter) == 0 || (!isNewGame && customParameterUI.newGameOnly) || (property != null && (!property.CanRead || !property.CanWrite || property.GetIndexParameters().Length != 0)))
				{
					continue;
				}
				Type type = ((property != null) ? property.PropertyType : field.FieldType);
				Func<object> get = () => (property != null) ? property.GetValue(gPars.CustomParams(parameterType), null) : field.GetValue(gPars.CustomParams(parameterType));
				Action<object> set = delegate(object o)
				{
					if (property != null)
					{
						property.SetValue(gPars.CustomParams(parameterType), o, null);
					}
					else
					{
						field.SetValue(gPars.CustomParams(parameterType), o);
					}
				};
				IList validValues = null;
				try
				{
					validValues = customParamNode.ValidValues(member);
				}
				catch (Exception exception)
				{
					Debug.LogError($"Error calling custom ValidValues method in type {customParamNode.GetType()}:");
					Debug.LogException(exception);
				}
				DialogGUIBase dialogGUIBase = null;
				DialogGUIBase dialogGUIBase2 = null;
				if (validValues != null && validValues.Count > 0)
				{
					dialogGUIBase = new DialogGUILabel(flexH: false, () => $"{customParameterUI.title}: {get()}", 250f, -1f);
					dialogGUIBase2 = new DialogGUISlider(() => validValues.IndexOf(get()), 0f, validValues.Count - 1, wholeNumbers: true, 224f, -1f, delegate(float v)
					{
						if (!Mathf.Approximately(v, validValues.IndexOf(get())))
						{
							gPars.preset = GameParameters.Preset.Custom;
						}
						set(validValues[(int)v]);
					});
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase);
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase2);
					num += 44f;
				}
				else if (type == typeof(bool))
				{
					dialogGUIBase = (dialogGUIBase2 = new DialogGUIToggle(() => (bool)get(), customParameterUI.title, delegate(bool b)
					{
						if (b != (bool)get())
						{
							gPars.preset = GameParameters.Preset.Custom;
						}
						set(b);
					}, 250f));
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase);
					num += 32f;
				}
				else if (type == typeof(string))
				{
					int num2 = ((!(customParameterUI is GameParameters.CustomStringParameterUI customStringParameterUI)) ? 1 : Math.Max(customStringParameterUI.lines, 1));
					dialogGUIBase = (dialogGUIBase2 = new DialogGUILabel(() => string.IsNullOrEmpty(customParameterUI.title) ? ((string)get()) : $"{customParameterUI.title}: {(string)get()}", 214f, 12f + 14f * (float)(num2 - 1)));
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase);
					num += 12f + 14f * (float)(num2 - 1);
				}
				else if (type == typeof(int))
				{
					GameParameters.CustomIntParameterUI customIntParameterUI = customParameterUI as GameParameters.CustomIntParameterUI;
					int num3 = customIntParameterUI?.minValue ?? 0;
					int num4 = customIntParameterUI?.maxValue ?? 100;
					int num5 = customIntParameterUI?.stepSize ?? 1;
					string displayFormat2 = ((customIntParameterUI != null) ? customIntParameterUI.displayFormat : "N0");
					int multiplier2 = num5;
					dialogGUIBase = new DialogGUILabel(flexH: false, () => $"{customParameterUI.title}: {((int)get()).ToString(displayFormat2)}", 250f, -1f);
					dialogGUIBase2 = new DialogGUISlider(() => (int)get() / multiplier2, num3 / multiplier2, num4 / multiplier2, wholeNumbers: true, 224f, 28f, delegate(float v)
					{
						if ((int)v != (int)get() / multiplier2)
						{
							gPars.preset = GameParameters.Preset.Custom;
						}
						set((int)v * multiplier2);
					});
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase);
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase2);
					num += 44f;
				}
				else if (!(type == typeof(float)) && !(type == typeof(double)))
				{
					if (!type.IsEnum)
					{
						continue;
					}
					Array values = Enum.GetValues(type);
					dialogGUIBase = new DialogGUILabel(flexH: false, () => $"{customParameterUI.title}: {get()}", 250f, -1f);
					dialogGUIBase2 = new DialogGUISlider(() => (int)get(), 0f, values.Length - 1, wholeNumbers: true, 224f, 28f, delegate(float v)
					{
						if (v != (float)(int)get())
						{
							gPars.preset = GameParameters.Preset.Custom;
						}
						set((int)v);
					});
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase);
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase2);
					num += 44f;
				}
				else
				{
					GameParameters.CustomFloatParameterUI customFloatParameterUI = customParameterUI as GameParameters.CustomFloatParameterUI;
					float minValue = customFloatParameterUI?.minValue ?? 0f;
					float num6 = customFloatParameterUI?.maxValue ?? 1f;
					float logBase = ((customFloatParameterUI == null || customFloatParameterUI.logBase <= 1f) ? 0f : customFloatParameterUI.logBase);
					int num7 = ((customFloatParameterUI == null || customFloatParameterUI.stepCount <= 1) ? 11 : customFloatParameterUI.stepCount);
					string displayFormat = ((customFloatParameterUI != null) ? customFloatParameterUI.displayFormat : "N0");
					bool asPercentage = customFloatParameterUI != null && logBase == 0f && customFloatParameterUI.asPercentage;
					bool addTextField = customFloatParameterUI.addTextField;
					float multiplier = (asPercentage ? 0.01f : 1f);
					bool minIsZero = minValue == 0f;
					if (logBase > 1f)
					{
						minValue = (minIsZero ? (-2f) : Mathf.Log(minValue, logBase));
						num6 = Mathf.Log(num6, logBase);
					}
					Func<float> getValue = delegate
					{
						float num13 = (float)Convert.ChangeType(get(), typeof(float));
						if (logBase <= 1f)
						{
							return num13 / multiplier;
						}
						return Mathf.Approximately(num13, 0f) ? (minValue / multiplier) : (Mathf.Log(num13, logBase) / multiplier);
					};
					string percentageRoundFormat = displayFormat.Replace('P', 'N');
					multiplier *= (num6 - minValue) / (float)(num7 - 1);
					if (addTextField)
					{
						DialogGUITextInput dialogGUITextInput = new DialogGUITextInput(((float)Convert.ChangeType(get(), typeof(float))).ToString(displayFormat), multiline: false, 64, delegate(string n)
						{
							float num11 = (float)Convert.ChangeType(n, typeof(float));
							if (!Mathf.Approximately(num11, getValue()))
							{
								gPars.preset = GameParameters.Preset.Custom;
							}
							float num12 = num11;
							if (logBase > 1f)
							{
								num12 = ((Mathf.Approximately(num12, minValue) && minIsZero) ? 0f : Mathf.Pow(logBase, num12));
							}
							num12 = ((!displayFormat.StartsWith("P")) ? float.Parse(num12.ToString(displayFormat)) : (float.Parse((num12 * 100f).ToString(percentageRoundFormat)) / 100f));
							set(num12);
							return n;
						}, delegate
						{
							float num10 = (float)Convert.ChangeType(get(), typeof(float));
							if (asPercentage)
							{
								num10 *= 100f;
							}
							return string.Format("{0}{1}", num10.ToString(displayFormat), asPercentage ? " %" : "");
						}, TMP_InputField.ContentType.DecimalNumber, 24f);
						dialogGUIBase = new DialogGUIHorizontalLayout(new DialogGUILabel(customParameterUI.title + ":"), dialogGUITextInput, new DialogGUISpace(40f));
					}
					else
					{
						dialogGUIBase = new DialogGUILabel(flexH: false, delegate
						{
							float num9 = (float)Convert.ChangeType(get(), typeof(float));
							if (asPercentage)
							{
								num9 *= 100f;
							}
							return string.Format("{0}: {1}{2}", customParameterUI.title, num9.ToString(displayFormat), asPercentage ? " %" : "");
						}, 250f, -1f);
					}
					dialogGUIBase2 = new DialogGUISlider(getValue, minValue / multiplier, num6 / multiplier, wholeNumbers: false, 224f, 28f, delegate(float v)
					{
						if (!Mathf.Approximately(v, getValue()))
						{
							gPars.preset = GameParameters.Preset.Custom;
						}
						float num8 = v * multiplier;
						if (logBase > 1f)
						{
							num8 = ((Mathf.Approximately(num8, minValue) && minIsZero) ? 0f : Mathf.Pow(logBase, num8));
						}
						num8 = ((!displayFormat.StartsWith("P")) ? float.Parse(num8.ToString(displayFormat)) : (float.Parse((num8 * 100f).ToString(percentageRoundFormat)) / 100f));
						set(num8);
					});
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase);
					dialogGUIVerticalLayout4.AddChild(dialogGUIBase2);
					num += 44f;
				}
				dialogGUIBase.OptionInteractableCondition = (dialogGUIBase2.OptionInteractableCondition = delegate
				{
					try
					{
						if (!isNewGame && currentGameModeFilter == GameParameters.GameMode.MISSION)
						{
							if (customParameterUI.unlockedDuringMission)
							{
								return customParamNode.Interactible(member, gPars);
							}
							return false;
						}
						return customParamNode.Interactible(member, gPars);
					}
					catch (Exception exception3)
					{
						Debug.LogError($"Error calling custom Interactible method in type {customParamNode.GetType()}:");
						Debug.LogException(exception3);
					}
					return true;
				});
				dialogGUIBase.OptionEnabledCondition = (dialogGUIBase2.OptionEnabledCondition = delegate
				{
					try
					{
						return customParamNode.Enabled(member, gPars);
					}
					catch (Exception exception2)
					{
						Debug.LogError($"Error calling custom Enabled method in type {customParamNode.GetType()}:");
						Debug.LogException(exception2);
					}
					return true;
				});
				if (!string.IsNullOrEmpty(customParameterUI.toolTip))
				{
					dialogGUIBase2.tooltipText = Localizer.Format(customParameterUI.toolTip);
				}
				else
				{
					dialogGUIBase2.tooltipText = "";
				}
			}
			DialogGUIVerticalLayout dialogGUIVerticalLayout5 = new DialogGUIVerticalLayout(250f, -1f, 4f, new RectOffset(), TextAnchor.UpperLeft);
			if (!string.IsNullOrEmpty(customParamNode.Title))
			{
				dialogGUIVerticalLayout5.AddChild(new DialogGUIBox(customParamNode.Title, 250f, 18f, null));
			}
			if (num >= 410f)
			{
				dialogGUIVerticalLayout4.minHeight = num;
				dialogGUIVerticalLayout5.AddChild(new DialogGUIScrollList(new Vector2(250f, 398f), hScroll: false, vScroll: true, new DialogGUIHorizontalLayout(250f, -1f, new DialogGUISpace(0f), dialogGUIVerticalLayout4, new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.MinSize))));
			}
			else
			{
				dialogGUIVerticalLayout5.AddChild(dialogGUIVerticalLayout4);
			}
			if (dialogGUIVerticalLayout4.children.Count == 0)
			{
				dialogGUIVerticalLayout4.AddChild(new DialogGUILabel(""));
			}
			listDictionary.Add(customParamNode.Section, new SortedVerticalLayout(dialogGUIVerticalLayout5, customParamNode.SectionOrder, customParamNode.DisplaySection));
		}
		if (!isNewGame && currentGameModeFilter == GameParameters.GameMode.MISSION)
		{
			dialogGUIVerticalLayout.OptionInteractableCondition = () => false;
			dialogGUIVerticalLayout3.OptionInteractableCondition = () => false;
		}
		Dictionary<string, List<SortedVerticalLayout>>.KeyCollection.Enumerator enumerator = listDictionary.Keys.GetEnumerator();
		List<string> list2 = new List<string>();
		try
		{
			while (enumerator.MoveNext())
			{
				list2.Add(enumerator.Current);
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		list2.Sort();
		if (list2.Contains("Mission"))
		{
			list2.Remove("Mission");
			list2.Insert(0, "Mission");
		}
		list2.Insert(0, "Basic");
		currentSection = "Basic";
		List<DialogGUIToggleButton> list3 = new List<DialogGUIToggleButton>();
		List<DialogGUIHorizontalLayout> list4 = new List<DialogGUIHorizontalLayout>();
		for (int k = 0; k < list2.Count; k++)
		{
			string section = list2[k];
			List<SortedVerticalLayout> list5;
			if (section == "Basic")
			{
				list5 = new List<SortedVerticalLayout>();
				list5.Add(new SortedVerticalLayout(dialogGUIVerticalLayout, 0, "#autoLOC_190231"));
				list5.Add(new SortedVerticalLayout(dialogGUIVerticalLayout3, 1, "#autoLoc_6002170"));
				if (gameMode == Game.Modes.CAREER || gameMode == Game.Modes.SCIENCE_SANDBOX)
				{
					list5.Add(new SortedVerticalLayout(dialogGUIVerticalLayout2, 2, "#autoLOC_190231"));
				}
			}
			else
			{
				list5 = listDictionary[section];
				list5.Sort((SortedVerticalLayout a, SortedVerticalLayout b) => a.order.CompareTo(b.order));
			}
			for (int l = 0; l < list5.Count; l += 3)
			{
				string text = ((list5[l].LocalizedTitle == string.Empty) ? section : Localizer.Format(list5[l].LocalizedTitle));
				string lbel = ((list5.Count <= 3) ? text : $"{text} ({l / 3 + 1})");
				DialogGUIToggleButton item2 = new DialogGUIToggleButton(() => currentSection == section, lbel, delegate(bool b)
				{
					if (b)
					{
						GameEvents.OnDifficultySettingTabChanging.Fire(currentSection, section);
						currentSection = section;
					}
				}, 200f, 30f);
				list3.Add(item2);
				DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout(-1f, 420f, 12f, new RectOffset(0, 0, 0, 0), TextAnchor.UpperLeft);
				list4.Add(dialogGUIHorizontalLayout);
				dialogGUIHorizontalLayout.AddChild(list5[l].layout);
				if (l + 1 < list5.Count)
				{
					dialogGUIHorizontalLayout.AddChild(list5[l + 1].layout);
				}
				if (l + 2 < list5.Count)
				{
					dialogGUIHorizontalLayout.AddChild(list5[l + 2].layout);
				}
				else
				{
					dialogGUIHorizontalLayout.AddChild(new DialogGUISpace(228f));
				}
				dialogGUIHorizontalLayout.OptionEnabledCondition = () => currentSection == section;
			}
		}
		DialogGUIVerticalLayout dialogGUIVerticalLayout6 = new DialogGUIVerticalLayout(false, true, 4f, new RectOffset(7, 7, 16, 8), TextAnchor.UpperCenter, new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize));
		for (int m = 0; m < list4.Count; m++)
		{
			dialogGUIVerticalLayout6.AddChild(list4[m]);
		}
		dialogGUIVerticalLayout6.AddChild(new DialogGUIFlexibleSpace());
		DialogGUIBase[] list6 = list3.ToArray();
		DialogGUIBase dialogGUIBase3 = new DialogGUIVerticalLayout(224f, -1f, list6);
		if (list3.Count > 13)
		{
			dialogGUIBase3 = new DialogGUIScrollList(new Vector2(224f, Mathf.Clamp(34f * (float)list3.Count, 0f, 540f)), new Vector2(224f, 34f * (float)list3.Count), hScroll: false, vScroll: true, dialogGUIBase3 as DialogGUILayoutBase);
		}
		list.Add(new DialogGUIHorizontalLayout(dialogGUIBase3, dialogGUIVerticalLayout6));
		list.Add(new DialogGUIHorizontalLayout(false, false, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUISpace((list4.Count > 1) ? 150f : 0f), new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_190323"), delegate
		{
			Dismiss(commitChanges: false);
		}, 60f, 30f, true), new DialogGUISpace(8f), new DialogGUIButton(Localizer.Format("#autoLOC_190328"), delegate
		{
			Dismiss(commitChanges: true);
		}, 60f, 30f, true)));
		PopupDialog popupDialog = PopupDialog.SpawnPopupDialog(Vector2.one * 0.5f, Vector2.one * 0.5f, new MultiOptionDialog("GameDifficulty", "", Localizer.Format("#autoLOC_190335"), skin, dialogRect, list.ToArray()), persistAcrossScenes: false, skin);
		popupDialog.OnDismiss = delegate
		{
			Dismiss(commitChanges: false);
		};
		LayoutElement component = popupDialog.GetComponent<LayoutElement>();
		if (component != null)
		{
			component.preferredHeight = 645f;
		}
		MenuNavigation.SpawnMenuNavigation(popupDialog.gameObject, Navigation.Mode.Automatic, limitCheck: true);
		return popupDialog;
	}

	public void Dismiss(bool commitChanges)
	{
		if (commitChanges)
		{
			OnDismiss(gPars, arg2: true);
		}
		else
		{
			OnDismiss(backupPars, arg2: false);
		}
		GameEvents.OnDifficultySettingsDismiss.Fire(this, commitChanges);
		InputLockManager.RemoveControlLock("difficultyOptionsMenu");
		if (minisettingsSource != null)
		{
			minisettingsSource.SetActive(value: true);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetPreset(GameParameters.Preset p)
	{
		gPars = GameParameters.GetDefaultParameters(gameMode, p);
	}
}
