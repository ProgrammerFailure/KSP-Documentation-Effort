using System.Runtime.CompilerServices;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Suspension Chart", 1)]
public class VPSuspensionGraph : VehicleBehaviour
{
	public enum Mode
	{
		TotalForce,
		SpringOnly,
		DamperOnly
	}

	public int wheel;

	public bool autoRange;

	public float manualForceRange;

	public float manualDepthRange;

	[Range(0f, 1f)]
	public float negativeRangeFactor;

	public Color color;

	public Color clampedColor;

	public Mode mode;

	private int width;

	private int height;

	private Rect rect;

	private float gridX;

	private float gridY;

	private bool autoClearBackground;

	public int positionX;

	public int positionY;

	public Font font;

	private TextureCanvas m_canvas;

	private GUIStyle m_style;

	private string m_text;

	private float m_lastGridX;

	private float m_lastGridY;

	public TextureCanvas canvas
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPSuspensionGraph()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetSuspensionLimits(VehicleBase.WheelState wheelState, out float maxContactDepth, out float maxForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawCanvasBackground()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}
}
