using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Performance Display", 20)]
public class VPPerformanceDisplay : VehicleBehaviour
{
	public enum Chart
	{
		Essentials,
		Accelerations,
		Engine,
		Wheelspin,
		SuspensionTravel,
		WheelLoad,
		Custom
	}

	public enum ViewportMode
	{
		Small,
		Large
	}

	public Chart chart;

	public bool visible;

	public float dataRecordingTime;

	public float refreshInterval;

	public bool startRecording;

	[Header("Display")]
	public float panRate;

	public float zoomRate;

	[Space(5f)]
	public ViewportMode viewMode;

	[FormerlySerializedAs("smallViewport")]
	public DataLogger.DisplaySettings smallDisplay;

	[FormerlySerializedAs("largeViewport")]
	public DataLogger.DisplaySettings largeDisplay;

	public ReferenceSpecs referenceSpecs;

	[Header("Controls")]
	public KeyCode toggleRecordKey;

	public KeyCode leftPanAndZoomKey;

	public KeyCode rightPanAndZoomKey;

	public KeyCode upPanAndZoomKey;

	public KeyCode downPanAndZoomKey;

	public KeyCode toggleViewModeKey;

	public KeyCode resetViewKey;

	public KeyCode nextChartKey;

	public KeyCode prevChartKey;

	[Header("Labels")]
	public Color textColor;

	public Font font;

	private PerformanceChart m_customChart;

	private DataLogger m_dataLogger;

	private bool m_recording;

	private float m_lastRefreshTime;

	private GUIStyle m_textStyle;

	private PerformanceChart[] m_telemetryCharts;

	private Chart m_lastChart;

	private PerformanceChart m_currentChart;

	public PerformanceChart customChart
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
	public VPPerformanceDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableComponent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableComponent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextProperties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeCurrentChart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetUpdateOrder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartRecord()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StopRecord()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleRecord()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsRecording()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NextChart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PrevChart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HorizontalZoom(float zoomRateAndDir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void VerticalZoom(float zoomRateAndDir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HorizontalPan(float panRateAndDir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void VerticalPan(float panRateAndDir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleViewMode()
	{
		throw null;
	}
}
