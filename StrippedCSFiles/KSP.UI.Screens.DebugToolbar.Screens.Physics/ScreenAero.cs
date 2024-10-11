using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Physics;

public class ScreenAero : MonoBehaviour
{
	public Toggle dataActionMenus;

	public Toggle dataGUI;

	public Toggle forcesInFlight;

	public float forceDisplayScaleMin;

	public float forceDisplayScaleMax;

	public Slider forceDisplayScaleSlider;

	public TextMeshProUGUI forceDisplayScaleText;

	public float globalLiftMin;

	public float globalLiftMax;

	public Slider globalLiftSlider;

	public TextMeshProUGUI globalLiftText;

	public float liftDragMin;

	public float liftDragMax;

	public Slider liftDragSlider;

	public TextMeshProUGUI liftDragText;

	public float bodyLiftMin;

	public float bodyLiftMax;

	public Slider bodyLiftSlider;

	public TextMeshProUGUI bodyLiftText;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenAero()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddListeners()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDataActionMenusToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDataGUIToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnForcesInFlightToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnForceDisplayScaleSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGlobalLiftSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLiftDragSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBodyLiftSet(float value)
	{
		throw null;
	}
}
