using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight;

public class TemperatureGauge : MonoBehaviour, IComparable<TemperatureGauge>
{
	[SerializeField]
	private Slider progressBar;

	[SerializeField]
	private Graphic sliderFill;

	public RectTransform rTrf;

	private Vector3 scPos;

	private Vector3 uiPos;

	public static float zSpaceEasing;

	public static float zSpaceMidpoint;

	public static float zSpaceLength;

	protected Part part;

	protected float distanceFromCamera;

	protected float zOffset;

	public float edgeHighlightThreshold;

	public float gaugeThreshold;

	private float colorScale;

	private float edgeRatio;

	public bool highlightActive;

	public bool gaugeActive;

	public bool showGauge;

	public Part Part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float DistanceFromCamera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ZOffset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TemperatureGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TemperatureGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(Part p, float edgeHighlightThreshold, float gaugeThreshold)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GaugeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CompareTo(TemperatureGauge b)
	{
		throw null;
	}
}
