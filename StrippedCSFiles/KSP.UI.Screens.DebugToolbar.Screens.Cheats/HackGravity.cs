using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Cheats;

public class HackGravity : MonoBehaviour
{
	public Toggle toggle;

	public Slider factorSlider;

	public TextMeshProUGUI text;

	public Button resetButton;

	public double gravityFactor;

	public double minGravity;

	public double maxGravity;

	protected bool hacked;

	protected Dictionary<CelestialBody, double> originalGeeForces;

	protected bool dirty;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HackGravity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnToggleChanged(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSliderChanged(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnResetClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetupValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CreateOriginalGeeForces()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetHack(bool hack)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetGravityFactor(double factor)
	{
		throw null;
	}
}
