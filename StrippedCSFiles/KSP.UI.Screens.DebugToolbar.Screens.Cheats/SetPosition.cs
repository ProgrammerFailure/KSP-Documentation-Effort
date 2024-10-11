using System.Runtime.CompilerServices;
using KSP.UI.Screens.Flight;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Cheats;

public class SetPosition : MonoBehaviour
{
	public Button bodyBackButton;

	public TextMeshProUGUI bodyNameText;

	public Button bodyForwardButton;

	public DebugScreenInputDouble latitudeInput;

	public DebugScreenInputDouble longitudeInput;

	public DebugScreenInputDouble altitudeInput;

	public DebugScreenInputDouble pitchInput;

	public DebugScreenInputDouble headingInput;

	public Slider easeInMultiplier;

	public TextMeshProUGUI easeInAmount;

	public Toggle easeToGroundToggle;

	public GameObject easeInStatusObject;

	public Button easeInDisableButton;

	public Toggle doNotPlaceUnderwaterToggle;

	public Button setSurfaceButton;

	public TextMeshProUGUI errorText;

	public Toggle overrideSafetyCheck;

	private int selectedBody;

	private NavBall navBall;

	private ScreenMessage easingInScreenMessage;

	private double altitudeSuggestedValue;

	private double maxAltitudeSuggestedValue;

	private bool error;

	private string errorMsg;

	public CelestialBody SelectedBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SetPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckForErrors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSetPositionClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBodyBackClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBodyForwardClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSelectedBodyText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselChanged(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneExit(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckForHiddenElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FormatValues(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSliderValue(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableActiveVesselEaseIn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetErrorMsg(string msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetSugestedAltitude(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetSelectedBodyIndex(CelestialBody celestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPositionValues(double lat, double lon, double alt)
	{
		throw null;
	}
}
