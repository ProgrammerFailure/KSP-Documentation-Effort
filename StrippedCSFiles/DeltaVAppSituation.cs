using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeltaVAppSituation : MonoBehaviour
{
	private enum altChangeInProgress
	{
		None,
		Text,
		Slider
	}

	[SerializeField]
	private TMP_Dropdown bodySelector;

	[SerializeField]
	private Toggle atmosphereSeaLevel;

	[SerializeField]
	private Toggle atmosphereAltitude;

	[SerializeField]
	private Toggle atmosphereVacuum;

	private CanvasGroup seaLevelDisableGroup;

	private CanvasGroup altitudeDisableGroup;

	[SerializeField]
	private TMP_InputField altitudeText;

	[SerializeField]
	private Slider altitudeSlider;

	[SerializeField]
	private TextMeshProUGUI pressureLabel;

	[SerializeField]
	private CanvasGroup atmosphereControls;

	[SerializeField]
	private float atmosphereControlsOffAlpha;

	private DeltaVSituationOptions situation;

	private CelestialBody selectedBody;

	private double altitudeKM;

	private bool ready;

	private altChangeInProgress altitudeChangeInProgress;

	private bool sliderDragging;

	private VesselDeltaV vesselDeltaV;

	private static string cacheAutoLOC_7001410;

	public DeltaVSituationOptions Situation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public CelestialBody SelectedBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double AltitudeKM
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double Altitude
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal bool IsBodyDropDownExpanded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVAppSituation()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BodyChanged(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshBodyList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleSeaLevelOn(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleAltitudeOn(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleVacuumOn(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAtmosphereSituation(DeltaVSituationOptions option)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AltitudeSliderChanged(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AltitudeTextChanged(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SliderStartDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SliderPointerUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SliderEndDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePressureDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool UpdateVesselDeltaVValues(bool recalcIfChanges = true, bool forceRecalc = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AltitudeHasFocus()
	{
		throw null;
	}
}
