using System.Runtime.CompilerServices;

public class GameplaySettingsScreen : DialogGUIVerticalLayout, ISettings
{
	private bool checkForUpdates;

	private bool simInBackground;

	private float maxDtPerFrame;

	private bool enableTemperatureGauges;

	private bool enableThermalHighlights;

	private bool advancedTweakables;

	private bool advancedMessagesApp;

	private bool confirmMessageDeletion;

	private bool extendedBurnIndicator;

	private bool ghostedNavMarkers;

	private bool evaDefaultHelmetOn;

	private bool evaDefaultNeckRingOn;

	private bool evaCheckLadderend;

	private bool doubleClickMouseLook;

	private float camWobbleGainExt;

	private float camWobbleGainInt;

	private int maximumVesselBudget;

	public int[] VesselBudgetOptions;

	private bool enableSpaceCenterCrew;

	private bool dontShowLauncher;

	private bool showVersionNumer;

	private bool showVesselLabels;

	private bool useKerbinTime;

	private float uiScale;

	private float uiScale_Time;

	private float uiScale_Altimeter;

	private float uiScale_MapOptions;

	private float uiScale_StagingStack;

	private float uiScale_Apps;

	private float uiScale_Mode;

	private float uiScale_NavBall;

	private float uiScale_Crew;

	private float uiPos_NavBall;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameplaySettingsScreen()
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
	public void ResetUIScaling()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetUIAdjustments()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplySettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyUIScalingAndAdjustments()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ApplyUIElementScale(FlightUIElements e, float scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ApplyNavBallHPos(float pos)
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
