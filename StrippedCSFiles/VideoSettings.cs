using System.Runtime.CompilerServices;

public class VideoSettings : DialogGUIVerticalLayout, ISettings
{
	private int resolutionWidth;

	private int resolutionHeight;

	private string[] availableResolutions;

	private int resolutionSelected;

	private string resolutionStrSelected;

	private bool fullscreen;

	private int antiAliasing;

	private int textureQuality;

	private int syncVBL;

	private int lightQuality;

	private int shadowQuality;

	private int frameLimit;

	private int qualityLevel;

	private float ambientLightBoostFactor;

	private float ambientLightBoostFactorMapOnly;

	private float ambientLightBoostFactorEditOnly;

	private bool useSM3Shaders;

	private bool terrainScatter;

	private int terrainScatterDensity;

	private int terrainDetailLevel;

	private int aerofxQuality;

	private string[] aerofxQualitySteps;

	private bool underwaterEnabled;

	private bool surfaceFx;

	private bool highlightFX;

	private int conicPatchDrawMode;

	private int conicPatchLimit;

	private int reflectionProbeRefreshMode;

	private string[] reflectionProbeRefreshModeSteps;

	private int reflectionProbeTextureResolution;

	private string[] reflectionProbeTextureResolutionSteps;

	private float commnetLowColorBrightnessFactor;

	private float partHighlighterBrightnessFactor;

	private bool partHighlighterInFlight;

	private bool celestialBodiesCastShadows;

	private string[] aAString;

	private string[] textureString;

	private string[] vblString;

	private string[] frameString;

	private string[] qualityLevelNames;

	private string[] conicPatchModeString;

	private string frameLimitStrSelected;

	private bool supportsDepthMapRT;

	private bool supportsSM3shaders;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VideoSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIBase[] DrawMiniSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetAvailableResolutions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplySettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void OnUpdate()
	{
		throw null;
	}
}
