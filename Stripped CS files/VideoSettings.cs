using System.Collections.Generic;
using CommNet;
using Highlighting;
using ns9;
using UnityEngine;

public class VideoSettings : DialogGUIVerticalLayout, ISettings
{
	public int resolutionWidth;

	public int resolutionHeight;

	public string[] availableResolutions;

	public int resolutionSelected;

	public string resolutionStrSelected;

	public bool fullscreen;

	public int antiAliasing;

	public int textureQuality;

	public int syncVBL;

	public int lightQuality;

	public int shadowQuality;

	public int frameLimit;

	public int qualityLevel;

	public float ambientLightBoostFactor;

	public float ambientLightBoostFactorMapOnly;

	public float ambientLightBoostFactorEditOnly;

	public bool useSM3Shaders;

	public bool terrainScatter;

	public int terrainScatterDensity;

	public int terrainDetailLevel;

	public int aerofxQuality;

	public string[] aerofxQualitySteps;

	public bool underwaterEnabled;

	public bool surfaceFx;

	public bool highlightFX;

	public int conicPatchDrawMode;

	public int conicPatchLimit;

	public int reflectionProbeRefreshMode;

	public string[] reflectionProbeRefreshModeSteps;

	public int reflectionProbeTextureResolution;

	public string[] reflectionProbeTextureResolutionSteps;

	public float commnetLowColorBrightnessFactor;

	public float partHighlighterBrightnessFactor;

	public bool partHighlighterInFlight = true;

	public bool celestialBodiesCastShadows = true;

	public string[] aAString;

	public string[] textureString;

	public string[] vblString;

	public string[] frameString;

	public string[] qualityLevelNames;

	public string[] conicPatchModeString;

	public string frameLimitStrSelected;

	public bool supportsDepthMapRT;

	public bool supportsSM3shaders;

	public void GetSettings()
	{
		qualityLevelNames = QualitySettings.names;
		qualityLevel = QualitySettings.GetQualityLevel();
		GetAvailableResolutions();
		resolutionWidth = GameSettings.SCREEN_RESOLUTION_WIDTH;
		resolutionHeight = GameSettings.SCREEN_RESOLUTION_HEIGHT;
		resolutionStrSelected = resolutionWidth + "x" + resolutionHeight;
		fullscreen = GameSettings.FULLSCREEN;
		antiAliasing = (int)Mathf.Max(0f, Mathf.Log(GameSettings.ANTI_ALIASING, 2f));
		aAString = new string[4]
		{
			Localizer.Format("#autoLOC_7000044"),
			"2x",
			"4x",
			"8x"
		};
		textureQuality = ((GameSettings.TEXTURE_QUALITY == 0) ? 3 : ((GameSettings.TEXTURE_QUALITY == 1) ? 2 : ((GameSettings.TEXTURE_QUALITY == 2) ? 1 : ((GameSettings.TEXTURE_QUALITY != 3) ? GameSettings.TEXTURE_QUALITY : 0))));
		textureString = new string[4]
		{
			Localizer.Format("#autoLOC_7000045"),
			Localizer.Format("#autoLOC_7000046"),
			Localizer.Format("#autoLOC_7000047"),
			Localizer.Format("#autoLOC_7000048")
		};
		syncVBL = GameSettings.SYNC_VBL;
		vblString = new string[3]
		{
			Localizer.Format("#autoLOC_7000058"),
			Localizer.Format("#autoLOC_7000059"),
			Localizer.Format("#autoLOC_7000060")
		};
		highlightFX = GameSettings.HIGHLIGHT_FX;
		lightQuality = GameSettings.LIGHT_QUALITY;
		shadowQuality = GameSettings.SHADOWS_QUALITY;
		shadowQuality = Mathf.Min(shadowQuality, 4);
		frameLimitStrSelected = GameSettings.FRAMERATE_LIMIT.ToString();
		frameString = new string[7]
		{
			Localizer.Format("#autoLOC_7000061"),
			Localizer.Format("#autoLOC_7000062"),
			Localizer.Format("#autoLOC_7000063"),
			Localizer.Format("#autoLOC_7000064"),
			Localizer.Format("#autoLOC_7000065"),
			Localizer.Format("#autoLOC_7000066"),
			Localizer.Format("#autoLOC_7000067")
		};
		ambientLightBoostFactor = Mathf.Clamp(GameSettings.AMBIENTLIGHT_BOOSTFACTOR, -1f, 1f);
		ambientLightBoostFactorMapOnly = Mathf.Clamp(GameSettings.AMBIENTLIGHT_BOOSTFACTOR_MAPONLY, -1f, 1f);
		ambientLightBoostFactorEditOnly = Mathf.Clamp(GameSettings.AMBIENTLIGHT_BOOSTFACTOR_EDITONLY, -1f, 1f);
		aerofxQuality = GameSettings.AERO_FX_QUALITY;
		aerofxQualitySteps = new string[4]
		{
			Localizer.Format("#autoLOC_7000049"),
			Localizer.Format("#autoLOC_7000050"),
			Localizer.Format("#autoLOC_7000051"),
			Localizer.Format("#autoLOC_7000052")
		};
		underwaterEnabled = GameSettings.FALLBACK_UNDERWATER_MODE == 1;
		surfaceFx = GameSettings.SURFACE_FX;
		conicPatchDrawMode = GameSettings.CONIC_PATCH_DRAW_MODE;
		conicPatchLimit = GameSettings.CONIC_PATCH_LIMIT;
		conicPatchModeString = new string[5]
		{
			Localizer.Format("#autoLOC_7000053"),
			Localizer.Format("#autoLOC_7000054"),
			Localizer.Format("#autoLOC_7000055"),
			Localizer.Format("#autoLOC_7000056"),
			Localizer.Format("#autoLOC_7000057")
		};
		reflectionProbeRefreshMode = GameSettings.REFLECTION_PROBE_REFRESH_MODE;
		reflectionProbeRefreshModeSteps = new string[4]
		{
			Localizer.Format("#autoLOC_8004257"),
			Localizer.Format("#autoLOC_8004258"),
			Localizer.Format("#autoLOC_8004259"),
			Localizer.Format("#autoLOC_8004260")
		};
		reflectionProbeTextureResolution = GameSettings.REFLECTION_PROBE_TEXTURE_RESOLUTION;
		reflectionProbeTextureResolutionSteps = new string[5] { "128", "256", "512", "1024", "2048" };
		commnetLowColorBrightnessFactor = Mathf.Clamp(GameSettings.COMMNET_LOWCOLOR_BRIGHTNESSFACTOR, 0.25f, 1f);
		partHighlighterBrightnessFactor = Mathf.Clamp(GameSettings.PART_HIGHLIGHTER_BRIGHTNESSFACTOR, 0f, 1f);
		partHighlighterInFlight = GameSettings.INFLIGHT_HIGHLIGHT;
		celestialBodiesCastShadows = GameSettings.CELESTIAL_BODIES_CAST_SHADOWS;
		int i = 0;
		for (int num = availableResolutions.Length; i < num; i++)
		{
			if (resolutionStrSelected == availableResolutions[i])
			{
				resolutionSelected = i;
				break;
			}
		}
		int j = 0;
		for (int num2 = frameString.Length; j < num2; j++)
		{
			if (frameLimitStrSelected + " FPS" == frameString[j])
			{
				frameLimit = j;
				break;
			}
		}
		supportsSM3shaders = SystemInfo.graphicsShaderLevel > 20;
		supportsDepthMapRT = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
		useSM3Shaders = !GameSettings.UNSUPPORTED_LEGACY_SHADER_TERRAIN && supportsSM3shaders;
		terrainScatter = GameSettings.PLANET_SCATTER;
		terrainScatterDensity = (int)(GameSettings.PLANET_SCATTER_FACTOR * 10f);
	}

	public void DrawSettings()
	{
		padding = new RectOffset(8, 8, 8, 0);
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.UpperLeft));
		children.Add(new DialogGUIFlexibleSpace());
		children.Add(new DialogGUIVerticalLayout());
		children.Add(new DialogGUIBox("SCENERY", -1f, 18f, null));
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Terrain Detail: ", 150f));
		if (PQSCache.Instance != null)
		{
			int count = PQSCache.PresetList.presets.Count;
			if (count > 0)
			{
				AddChild(new DialogGUISlider(() => PQSCache.PresetList.presetIndex, 0f, count - 1, wholeNumbers: true, 150f, -1f, delegate(float f)
				{
					PQSCache.PresetList.SetPreset(Mathf.FloorToInt(f));
				}));
				AddChild(new DialogGUISpace(30f));
				AddChild(new DialogGUILabel(() => PQSCache.PresetList.preset, 80f));
			}
		}
		else
		{
			AddChild(new DialogGUISlider(() => 1f, 0f, 3f, wholeNumbers: true, 150f, -1f, delegate
			{
			}));
			AddChild(new DialogGUILabel("Medium", 80f));
		}
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("SM3 Terrain Shaders: ", 150f));
		AddChild(new DialogGUIToggle(useSM3Shaders, () => (!useSM3Shaders) ? ((!supportsSM3shaders) ? "No Hardware Support" : Localizer.Format("#autoLOC_6001071")) : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			useSM3Shaders = b;
		}, 200f));
		children[children.Count - 1].OptionInteractableCondition = () => supportsSM3shaders;
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Terrain Scatters: ", 150f));
		AddChild(new DialogGUIToggle(terrainScatter, () => (!terrainScatter) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			terrainScatter = b;
		}, 150f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Scatter Density: ", 150f));
		AddChild(new DialogGUISlider(() => terrainScatterDensity, 0f, 10f, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			terrainScatterDensity = Mathf.FloorToInt(f);
		}));
		AddChild(new DialogGUISpace(50f));
		AddChild(new DialogGUILabel(() => terrainScatterDensity * 10 + "%", 80f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUISpace(10f));
		AddChild(new DialogGUILabel("<size=10><color=yellow>Scenery settings may require a game restart to take effect.</color></size>"));
		AddChild(new DialogGUISpace(20f));
		AddChild(new DialogGUIBox("RENDERING", -1f, 18f, null));
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Render Quality Level: ", 150f));
		AddChild(new DialogGUISlider(() => qualityLevel, 0f, qualityLevelNames.Length - 1, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			qualityLevel = Mathf.FloorToInt(f);
		}));
		AddChild(new DialogGUISpace(30f));
		AddChild(new DialogGUILabel(() => qualityLevelNames[qualityLevel], 80f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Texture Quality:", 150f));
		AddChild(new DialogGUIButton(() => (textureQuality > 0) ? "<" : "", delegate
		{
			if (textureQuality > 0)
			{
				textureQuality--;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILabel(() => "  " + textureString[textureQuality] + "  ", 85f));
		AddChild(new DialogGUIButton(() => (textureQuality < 3) ? ">" : "", delegate
		{
			if (textureQuality > 0)
			{
				textureQuality++;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Aerodynamic FX Quality:", 150f));
		if (supportsDepthMapRT)
		{
			AddChild(new DialogGUISlider(() => aerofxQuality, 0f, aerofxQualitySteps.Length - 1, wholeNumbers: true, 150f, -1f, delegate(float f)
			{
				aerofxQuality = Mathf.FloorToInt(f);
			}));
			AddChild(new DialogGUILabel(() => "  " + aerofxQualitySteps[aerofxQuality], 120f));
		}
		else
		{
			AddChild(new DialogGUISlider(() => 0f, 0f, aerofxQualitySteps.Length - 1, wholeNumbers: true, 150f, -1f, delegate
			{
			}));
			children[children.Count - 1].OptionInteractableCondition = () => false;
			AddChild(new DialogGUILabel("No Hardware Support", 120f));
		}
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Surface FX:", 150f));
		AddChild(new DialogGUIToggle(surfaceFx, () => (!surfaceFx) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			surfaceFx = b;
		}, 150f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Underwater FX:", 150f));
		AddChild(new DialogGUIToggle(underwaterEnabled, () => (!underwaterEnabled) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			underwaterEnabled = b;
		}, 150f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIFlexibleSpace());
		AddChild(new DialogGUIVerticalLayout());
		AddChild(new DialogGUIBox("VIDEO", -1f, 18f, null));
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Screen Resolution:", 150f));
		AddChild(new DialogGUIButton(() => (resolutionSelected != 0) ? "<" : "", delegate
		{
			if (resolutionSelected > 0)
			{
				resolutionSelected--;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILabel(() => "  " + availableResolutions[resolutionSelected] + "  ", 80f));
		AddChild(new DialogGUIButton(() => (resolutionSelected >= availableResolutions.Length - 1) ? ">" : "", delegate
		{
			if (resolutionSelected < availableResolutions.Length - 1)
			{
				resolutionSelected++;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Full Screen:", 150f));
		AddChild(new DialogGUIToggle(fullscreen, () => (!fullscreen) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			fullscreen = b;
		}, 150f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUISpace(30f));
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Anti-Aliasing:", 150f));
		AddChild(new DialogGUIButton(() => (antiAliasing != 0) ? "<" : "", delegate
		{
			antiAliasing = Mathf.Max(antiAliasing - 1, 0);
		}, 30f, 18f, false));
		AddChild(new DialogGUILabel(() => "  " + aAString[antiAliasing] + "  ", 38f));
		AddChild(new DialogGUIButton(() => (antiAliasing != aAString.Length - 1) ? ">" : "", delegate
		{
			antiAliasing = Mathf.Min(antiAliasing + 1, aAString.Length - 1);
		}, 30f, 18f, false));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("V Sync:", 150f));
		AddChild(new DialogGUIButton(() => (syncVBL != 0) ? "<" : "", delegate
		{
			if (syncVBL > 0)
			{
				syncVBL--;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILabel(() => "  " + vblString[syncVBL] + "  ", 140f));
		AddChild(new DialogGUIButton(() => (syncVBL != vblString.Length - 1) ? ">" : "", delegate
		{
			if (syncVBL < vblString.Length - 1)
			{
				syncVBL++;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Frame Limit:", 150f));
		AddChild(new DialogGUIButton(() => (frameLimit != 0) ? "<" : "", delegate
		{
			if (frameLimit > 0)
			{
				frameLimit--;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILabel(() => "  " + frameString[frameLimit], 70f));
		AddChild(new DialogGUIButton(() => (frameLimit != frameString.Length - 1) ? ">" : "", delegate
		{
			if (frameLimit < frameString.Length - 1)
			{
				frameLimit++;
			}
		}, 30f, 18f, false));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Pixel Light Count:", 150f));
		AddChild(new DialogGUISlider(() => lightQuality, 0f, 64f, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			lightQuality = Mathf.FloorToInt(f);
		}));
		AddChild(new DialogGUISpace(30f));
		AddChild(new DialogGUILabel(() => lightQuality.ToString(), 80f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		AddChild(new DialogGUILabel("Shadow Cascades:", 150f));
		AddChild(new DialogGUISlider(() => (float)shadowQuality / 2f, 0f, 2f, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			shadowQuality = Mathf.FloorToInt(f * 2f);
		}));
		AddChild(new DialogGUISpace(30f));
		AddChild(new DialogGUILabel(() => ((float)shadowQuality * 2f).ToString(), 80f));
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIFlexibleSpace());
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIFlexibleSpace());
		AddChild(new DialogGUILayoutEnd());
		AddChild(new DialogGUIFlexibleSpace());
		DialogGUIBase h = new DialogGUIHorizontalLayout();
		AddChild(h);
		h.OnUpdate = delegate
		{
			h.uiItem.SetActive(value: false);
		};
		h.AddChild(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter));
		h.AddChild(new DialogGUIFlexibleSpace());
		h.AddChild(new DialogGUILabel("* Game requires restart for changes to take effect *", 300f));
		h.AddChild(new DialogGUIFlexibleSpace());
		h.AddChild(new DialogGUILayoutEnd());
		h.AddChild(new DialogGUISpace(50f));
	}

	public DialogGUIBase[] DrawMiniSettings()
	{
		TextAnchor achr = TextAnchor.MiddleLeft;
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		list.Add(new DialogGUIBox(Localizer.Format("#autoLOC_150065"), -1f, 18f, null));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_150068"), 150f), new DialogGUIButton(() => (antiAliasing != 0) ? "<" : "", delegate
		{
			antiAliasing = Mathf.Max(antiAliasing - 1, 0);
		}, 30f, 18f, false), new DialogGUILabel(() => "  " + aAString[antiAliasing] + "  ", 38f), new DialogGUIButton(() => (antiAliasing != aAString.Length - 1) ? ">" : "", delegate
		{
			antiAliasing = Mathf.Min(antiAliasing + 1, aAString.Length - 1);
		}, 30f, 18f, false)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_150074"), 150f), new DialogGUIButton(() => (textureQuality > 0) ? "<" : "", delegate
		{
			if (textureQuality > 0)
			{
				textureQuality--;
			}
		}, 30f, 18f, false), new DialogGUILabel(() => "  " + textureString[textureQuality] + "  ", 85f), new DialogGUIButton(() => (textureQuality < 3) ? ">" : "", delegate
		{
			if (textureQuality < textureString.Length - 1)
			{
				textureQuality++;
			}
		}, 30f, 18f, false)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_150081"), 150f), new DialogGUISlider(() => lightQuality, 0f, 64f, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			lightQuality = Mathf.FloorToInt(f);
		}), new DialogGUISpace(30f), new DialogGUILabel(() => lightQuality.ToString(), 80f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_150087"), 150f), new DialogGUISlider(() => (float)shadowQuality / 2f, 0f, 2f, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			shadowQuality = Mathf.FloorToInt(f * 2f);
		}), new DialogGUISpace(30f), new DialogGUILabel(() => shadowQuality.ToString(), 80f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_6003019"), 150f), new DialogGUISlider(() => ambientLightBoostFactor, -1f, 1f, wholeNumbers: false, 150f, -1f, delegate(float f)
		{
			ambientLightBoostFactor = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => ambientLightBoostFactor.ToString("0%"), 80f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_6003089"), 150f), new DialogGUISlider(() => ambientLightBoostFactorMapOnly, -1f, 1f, wholeNumbers: false, 150f, -1f, delegate(float f)
		{
			ambientLightBoostFactorMapOnly = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => ambientLightBoostFactorMapOnly.ToString("0%"), 80f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_6003112"), 150f), new DialogGUISlider(() => ambientLightBoostFactorEditOnly, -1f, 1f, wholeNumbers: false, 150f, -1f, delegate(float f)
		{
			ambientLightBoostFactorEditOnly = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => ambientLightBoostFactorEditOnly.ToString("0%"), 80f)));
		DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_150093"), 150f));
		list.Add(dialogGUIHorizontalLayout);
		if (supportsDepthMapRT)
		{
			dialogGUIHorizontalLayout.AddChild(new DialogGUISlider(() => aerofxQuality, 0f, aerofxQualitySteps.Length - 1, wholeNumbers: true, 150f, -1f, delegate(float f)
			{
				aerofxQuality = Mathf.FloorToInt(f);
			}));
			dialogGUIHorizontalLayout.AddChild(new DialogGUILabel(() => "  " + aerofxQualitySteps[aerofxQuality], 120f));
		}
		else
		{
			DialogGUISlider dialogGUISlider = new DialogGUISlider(() => 0f, 0f, aerofxQualitySteps.Length - 1, wholeNumbers: true, 150f, -1f, delegate
			{
			});
			dialogGUISlider.OptionInteractableCondition = () => false;
			dialogGUIHorizontalLayout.AddChild(dialogGUISlider);
			dialogGUIHorizontalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_150105"), 120f));
		}
		DialogGUIHorizontalLayout dialogGUIHorizontalLayout2 = new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_8004262"), 150f));
		list.Add(dialogGUIHorizontalLayout2);
		dialogGUIHorizontalLayout2.AddChild(new DialogGUISlider(() => reflectionProbeRefreshMode, 0f, reflectionProbeRefreshModeSteps.Length - 1, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			reflectionProbeRefreshMode = Mathf.FloorToInt(f);
		}));
		dialogGUIHorizontalLayout2.AddChild(new DialogGUILabel(() => "  " + reflectionProbeRefreshModeSteps[reflectionProbeRefreshMode], 120f));
		DialogGUIHorizontalLayout dialogGUIHorizontalLayout3 = new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_8004264"), 150f));
		list.Add(dialogGUIHorizontalLayout3);
		dialogGUIHorizontalLayout3.AddChild(new DialogGUISlider(() => reflectionProbeTextureResolution, 0f, reflectionProbeTextureResolutionSteps.Length - 1, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			reflectionProbeTextureResolution = Mathf.FloorToInt(f);
		}));
		dialogGUIHorizontalLayout3.AddChild(new DialogGUILabel(() => "  " + reflectionProbeTextureResolutionSteps[reflectionProbeTextureResolution], 120f));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_150109"), 150f), new DialogGUIToggle(() => highlightFX, "", delegate(bool b)
		{
			highlightFX = b;
		}, 120f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_150121"), 150f), new DialogGUIButton(() => (conicPatchDrawMode > 0) ? "<" : "", delegate
		{
			if (conicPatchDrawMode > 0)
			{
				conicPatchDrawMode--;
			}
		}, 30f, 18f, false), new DialogGUILabel(() => "  " + conicPatchModeString[conicPatchDrawMode] + "  ", 100f), new DialogGUIButton(() => (conicPatchDrawMode < 4) ? ">" : "", delegate
		{
			if (conicPatchDrawMode < conicPatchModeString.Length - 1)
			{
				conicPatchDrawMode++;
			}
		}, 30f, 18f, false)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_150127"), 150f), new DialogGUISlider(() => conicPatchLimit, 2f, 6f, wholeNumbers: true, 150f, -1f, delegate(float f)
		{
			conicPatchLimit = (int)f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => conicPatchLimit.ToString(), 80f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_150133"), 150f), new DialogGUISlider(() => commnetLowColorBrightnessFactor, 0.25f, 1f, wholeNumbers: false, 150f, -1f, delegate(float f)
		{
			commnetLowColorBrightnessFactor = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => commnetLowColorBrightnessFactor.ToString("0%"), 80f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_150140"), 150f), new DialogGUISlider(() => partHighlighterBrightnessFactor, 0f, 1f, wholeNumbers: false, 150f, -1f, delegate(float f)
		{
			partHighlighterBrightnessFactor = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => partHighlighterBrightnessFactor.ToString("0%"), 80f)));
		list.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), achr, new DialogGUILabel(Localizer.Format("#autoLOC_150146"), 150f), new DialogGUIToggle(() => partHighlighterInFlight, "", delegate(bool b)
		{
			partHighlighterInFlight = b;
		}, 120f)));
		return list.ToArray();
	}

	public void GetAvailableResolutions()
	{
		List<Resolution> list = new List<Resolution>();
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution item = resolutions[i];
			if (Application.isEditor || (item.width >= 960 && item.height >= 720))
			{
				list.Add(item);
			}
		}
		availableResolutions = new string[list.Count];
		int j = 0;
		for (int count = list.Count; j < count; j++)
		{
			availableResolutions[j] = list[j].width + "x" + list[j].height;
		}
	}

	public void ApplySettings()
	{
		GameSettings.QUALITY_PRESET = qualityLevel;
		QualitySettings.SetQualityLevel(qualityLevel);
		GameSettings.ANTI_ALIASING = (int)Mathf.Pow(2f, antiAliasing);
		QualitySettings.antiAliasing = GameSettings.ANTI_ALIASING;
		GameSettings.TEXTURE_QUALITY = ((textureQuality == 0) ? 3 : ((textureQuality == 1) ? 2 : ((textureQuality == 2) ? 1 : ((textureQuality != 3) ? textureQuality : 0))));
		QualitySettings.masterTextureLimit = GameSettings.TEXTURE_QUALITY;
		GameSettings.SYNC_VBL = syncVBL;
		QualitySettings.vSyncCount = GameSettings.SYNC_VBL;
		GameSettings.LIGHT_QUALITY = lightQuality;
		QualitySettings.pixelLightCount = GameSettings.LIGHT_QUALITY;
		HighlightingSystem.FxEnabled = (GameSettings.HIGHLIGHT_FX = highlightFX);
		GameSettings.SHADOWS_QUALITY = shadowQuality;
		QualitySettings.shadowCascades = GameSettings.SHADOWS_QUALITY;
		if (frameLimit == 0)
		{
			GameSettings.FRAMERATE_LIMIT = -1;
		}
		else
		{
			GameSettings.FRAMERATE_LIMIT = int.Parse(frameString[frameLimit].Split(' ')[0]);
		}
		Application.targetFrameRate = GameSettings.FRAMERATE_LIMIT;
		GameSettings.AMBIENTLIGHT_BOOSTFACTOR = ambientLightBoostFactor;
		GameSettings.AMBIENTLIGHT_BOOSTFACTOR_MAPONLY = ambientLightBoostFactorMapOnly;
		GameSettings.AMBIENTLIGHT_BOOSTFACTOR_EDITONLY = ambientLightBoostFactorEditOnly;
		GameSettings.UNSUPPORTED_LEGACY_SHADER_TERRAIN = !useSM3Shaders;
		GameSettings.PLANET_SCATTER = terrainScatter;
		GameSettings.PLANET_SCATTER_FACTOR = (float)terrainScatterDensity / 10f;
		GameSettings.AERO_FX_QUALITY = aerofxQuality;
		GameSettings.REFLECTION_PROBE_REFRESH_MODE = reflectionProbeRefreshMode;
		GameSettings.REFLECTION_PROBE_TEXTURE_RESOLUTION = reflectionProbeTextureResolution;
		GameSettings.FALLBACK_UNDERWATER_MODE = (underwaterEnabled ? 1 : 2);
		GameSettings.CONIC_PATCH_DRAW_MODE = conicPatchDrawMode;
		GameSettings.CONIC_PATCH_LIMIT = conicPatchLimit;
		GameSettings.COMMNET_LOWCOLOR_BRIGHTNESSFACTOR = commnetLowColorBrightnessFactor;
		CommNetUI.LowColorBrightnessFactor = commnetLowColorBrightnessFactor;
		GameSettings.PART_HIGHLIGHTER_BRIGHTNESSFACTOR = partHighlighterBrightnessFactor;
		Highlighter.HighlighterLimit = partHighlighterBrightnessFactor;
		GameSettings.INFLIGHT_HIGHLIGHT = partHighlighterInFlight;
		GameSettings.CELESTIAL_BODIES_CAST_SHADOWS = celestialBodiesCastShadows;
		Debug.Log("ApplySettings: resolutionSelected: " + resolutionSelected);
		if (resolutionSelected >= 0 && resolutionSelected < availableResolutions.Length)
		{
			string[] array = availableResolutions[resolutionSelected].Split('x');
			resolutionWidth = int.Parse(array[0]);
			resolutionHeight = int.Parse(array[1]);
			if (!HighLogic.LoadedSceneHasPlanetarium)
			{
				Screen.SetResolution(resolutionWidth, resolutionHeight, fullscreen);
			}
			GameSettings.SCREEN_RESOLUTION_WIDTH = resolutionWidth;
			GameSettings.SCREEN_RESOLUTION_HEIGHT = resolutionHeight;
		}
		GameSettings.FULLSCREEN = fullscreen;
	}

	public string GetName()
	{
		return "Graphics";
	}

	public new void OnUpdate()
	{
		Update();
	}
}
