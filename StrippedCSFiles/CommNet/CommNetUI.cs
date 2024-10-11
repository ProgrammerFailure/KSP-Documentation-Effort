using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vectrosity;

namespace CommNet;

public class CommNetUI : MonoBehaviour
{
	public enum DisplayMode
	{
		[Description("#autoLOC_6003083")]
		None,
		[Description("#autoLOC_6003084")]
		FirstHop,
		[Description("#autoLOC_6003085")]
		Path,
		[Description("#autoLOC_6003086")]
		VesselLinks,
		[Description("#autoLOC_6003087")]
		Network
	}

	private static int ModeCount;

	public Color colorHigh;

	public Color colorLow;

	public Color colorWhole;

	public float colorLerpPower;

	public float lineWidth2D;

	public float lineWidth3D;

	public bool swapHighLow;

	public bool smoothColor;

	public static DisplayMode Mode;

	public static DisplayMode ModeTrackingStation;

	public static DisplayMode ModeFlightMap;

	private static float _lowColorBrightness;

	protected Material lineMaterial;

	protected Texture lineTexture;

	protected bool draw3dLines;

	protected bool refreshLines;

	protected bool isShown;

	protected bool useTSBehavior;

	protected Vessel vessel;

	protected List<Vector3> points;

	protected VectorLine line;

	protected static Texture telemetryTexture;

	protected static Material telemetryMaterial;

	protected MapObject obj;

	public static CommNetUI Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public static float LowColorBrightnessFactor
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

	public virtual VectorLine Line
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Texture TelemetryTexture
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Material TelemetryMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommNetUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CommNetUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnMapFocusChange(MapObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CreateLine(ref VectorLine l, List<Vector3> points)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("UpdateLine")]
	public virtual void UpdateLine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDisplayVessel(Vessel v = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void NextMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PreviousMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SwitchMode(int step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ClampAndSetMode(ref DisplayMode curMode, DisplayMode newMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ResetMode()
	{
		throw null;
	}
}
