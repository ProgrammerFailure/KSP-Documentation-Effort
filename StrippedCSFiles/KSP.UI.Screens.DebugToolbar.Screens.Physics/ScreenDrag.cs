using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Physics;

public class ScreenDrag : MonoBehaviour
{
	public Toggle applyDrag;

	public Toggle dragSpheres;

	public Toggle dragAcceleration;

	public Toggle nonPhysicalDrag;

	public Toggle nonPhysicalCoM;

	public float globalDragMin;

	public float globalDragMax;

	public Slider globalDragSlider;

	public TextMeshProUGUI globalDragText;

	public float cubeMultiplierMin;

	public float cubeMultiplierMax;

	public Slider cubeMultiplierSlider;

	public TextMeshProUGUI cubeMultiplierText;

	public float angularDragMin;

	public float angularDragMax;

	public Slider angularDragSlider;

	public TextMeshProUGUI angularDragText;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenDrag()
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
	private void CheckInteractable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnApplyDragToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDragSpheresToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDragAccelerationToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNonPhysicalDragToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNonPhysicalCoMToggle(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGlobalDragSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCubeMultiplierSet(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAngularDragSet(float value)
	{
		throw null;
	}
}
