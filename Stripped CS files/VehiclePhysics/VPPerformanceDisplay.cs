using EdyCommonTools;
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

	public bool visible = true;

	public float dataRecordingTime = 60f;

	public float refreshInterval = 0.02f;

	public bool startRecording;

	[Header("Display")]
	public float panRate = 0.5f;

	public float zoomRate = 1f;

	[Space(5f)]
	public ViewportMode viewMode;

	[FormerlySerializedAs("smallViewport")]
	public DataLogger.DisplaySettings smallDisplay;

	[FormerlySerializedAs("largeViewport")]
	public DataLogger.DisplaySettings largeDisplay;

	public ReferenceSpecs referenceSpecs = new ReferenceSpecs();

	[Header("Controls")]
	public KeyCode toggleRecordKey = KeyCode.Keypad0;

	public KeyCode leftPanAndZoomKey = KeyCode.Keypad4;

	public KeyCode rightPanAndZoomKey = KeyCode.Keypad6;

	public KeyCode upPanAndZoomKey = KeyCode.Keypad8;

	public KeyCode downPanAndZoomKey = KeyCode.Keypad2;

	public KeyCode toggleViewModeKey = KeyCode.KeypadPeriod;

	public KeyCode resetViewKey = KeyCode.Keypad5;

	public KeyCode nextChartKey = KeyCode.KeypadPlus;

	public KeyCode prevChartKey = KeyCode.KeypadMinus;

	[Header("Labels")]
	public Color textColor = Color.white;

	public Font font;

	public PerformanceChart m_customChart;

	public DataLogger m_dataLogger;

	public bool m_recording;

	public float m_lastRefreshTime;

	public GUIStyle m_textStyle = new GUIStyle();

	public PerformanceChart[] m_telemetryCharts = new PerformanceChart[6]
	{
		new EssentialsChart(),
		new AccelerationsChart(),
		new EngineChart(),
		new WheelspinChart(),
		new SuspensionTravelChart(),
		new WheelLoadChart()
	};

	public Chart m_lastChart;

	public PerformanceChart m_currentChart;

	public PerformanceChart customChart
	{
		get
		{
			return m_customChart;
		}
		set
		{
			if (m_customChart != value)
			{
				m_customChart = value;
				chart = Chart.Custom;
				if (base.vehicle.initialized)
				{
					InitializeCurrentChart();
				}
			}
		}
	}

	public VPPerformanceDisplay()
	{
		smallDisplay = new DataLogger.DisplaySettings();
		smallDisplay.width = 512;
		smallDisplay.height = 256;
		smallDisplay.chartAlpha = 0.6f;
		smallDisplay.textAlpha = 1f;
		smallDisplay.backgroundColor = GColor.Alpha(Color.black, 0.9f);
	}

	public override void OnEnableComponent()
	{
		m_dataLogger = new DataLogger(dataRecordingTime, Time.fixedDeltaTime, (viewMode == ViewportMode.Small) ? smallDisplay : largeDisplay);
		m_dataLogger.dotGrid.resolution.x = Time.fixedDeltaTime;
		m_dataLogger.autoRefresh = false;
		m_lastChart = (Chart)(-1);
		UpdateTextProperties();
		if (startRecording)
		{
			StartRecord();
		}
	}

	public override void OnDisableComponent()
	{
		m_dataLogger.ReleaseTexture();
	}

	public void OnValidate()
	{
		UpdateTextProperties();
	}

	public void UpdateTextProperties()
	{
		m_textStyle.font = font;
		m_textStyle.fontSize = 10;
		m_textStyle.normal.textColor = textColor;
	}

	public void InitializeCurrentChart()
	{
		if (chart == Chart.Custom)
		{
			m_currentChart = m_customChart;
		}
		else
		{
			m_currentChart = m_telemetryCharts[(int)chart];
		}
		m_dataLogger.Restart();
		if (m_currentChart != null)
		{
			m_currentChart.vehicle = base.vehicle;
			m_currentChart.dataLogger = m_dataLogger;
			m_currentChart.reference = referenceSpecs;
			m_currentChart.Initialize();
			m_currentChart.ResetView();
			m_currentChart.SetupChannels();
		}
	}

	public void OnGUI()
	{
		if (visible)
		{
			m_dataLogger.ConfigureDisplay((viewMode == ViewportMode.Small) ? smallDisplay : largeDisplay);
			m_dataLogger.GUIDrawGraphic();
			m_dataLogger.GUIDrawLabels(m_textStyle);
			string label = string.Format("PRG{0}: {1}", (int)chart, (m_currentChart != null) ? m_currentChart.Title() : "empty");
			m_dataLogger.GUILabel(6, 4, label, m_textStyle);
			TextAnchor alignment = m_textStyle.alignment;
			m_textStyle.alignment = TextAnchor.UpperRight;
			m_dataLogger.GUILabelOnTexture(6, 4, $"FRM{m_dataLogger.FrameCount(),7}", m_textStyle);
			m_textStyle.alignment = alignment;
		}
	}

	public override int GetUpdateOrder()
	{
		return -1000;
	}

	public override void FixedUpdateVehicle()
	{
		if (m_lastChart != chart)
		{
			m_lastChart = chart;
			InitializeCurrentChart();
		}
		if (m_recording && m_currentChart != null)
		{
			m_currentChart.RecordData();
			m_dataLogger.NextFrame();
		}
	}

	public void Update()
	{
		if (!visible)
		{
			return;
		}
		bool num = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		if (Input.GetKeyDown(toggleRecordKey))
		{
			ToggleRecord();
		}
		if (num)
		{
			if (Input.GetKey(rightPanAndZoomKey))
			{
				m_dataLogger.HorizontalZoom(zoomRate * Time.unscaledDeltaTime);
			}
			if (Input.GetKey(leftPanAndZoomKey))
			{
				m_dataLogger.HorizontalZoom((0f - zoomRate) * Time.unscaledDeltaTime);
			}
			if (Input.GetKey(upPanAndZoomKey))
			{
				m_dataLogger.VerticalZoom(zoomRate * Time.unscaledDeltaTime);
			}
			if (Input.GetKey(downPanAndZoomKey))
			{
				m_dataLogger.VerticalZoom((0f - zoomRate) * Time.unscaledDeltaTime);
			}
		}
		else
		{
			float num2 = 0f;
			float num3 = 0f;
			if (Input.GetKey(leftPanAndZoomKey))
			{
				num2 -= panRate;
			}
			if (Input.GetKey(rightPanAndZoomKey))
			{
				num2 += panRate;
			}
			if (Input.GetKey(upPanAndZoomKey))
			{
				num3 += panRate;
			}
			if (Input.GetKey(downPanAndZoomKey))
			{
				num3 -= panRate;
			}
			m_dataLogger.MovePosition(num2 * Time.unscaledDeltaTime, num3 * Time.unscaledDeltaTime);
		}
		if (Input.GetKeyDown(nextChartKey))
		{
			NextChart();
		}
		if (Input.GetKeyDown(prevChartKey))
		{
			PrevChart();
		}
		if (Input.GetKeyDown(toggleViewModeKey))
		{
			ToggleViewMode();
		}
		if (Input.GetKeyDown(resetViewKey))
		{
			ResetView();
		}
		if (!m_recording || Time.time - m_lastRefreshTime >= refreshInterval)
		{
			m_dataLogger.Refresh();
			m_lastRefreshTime = Time.time;
		}
	}

	public void StartRecord()
	{
		m_dataLogger.ResetData();
		m_recording = true;
	}

	public void StopRecord()
	{
		m_recording = false;
	}

	public void ToggleRecord()
	{
		if (m_recording)
		{
			StopRecord();
		}
		else
		{
			StartRecord();
		}
	}

	public bool IsRecording()
	{
		return m_recording;
	}

	public void NextChart()
	{
		chart++;
		if (chart > Chart.Custom || (chart == Chart.Custom && m_customChart == null))
		{
			chart = Chart.Essentials;
		}
	}

	public void PrevChart()
	{
		chart--;
		if (chart < Chart.Essentials)
		{
			chart = ((m_customChart != null) ? Chart.Custom : Chart.WheelLoad);
		}
	}

	public void HorizontalZoom(float zoomRateAndDir)
	{
		if (zoomRateAndDir != 0f)
		{
			m_dataLogger.HorizontalZoom(zoomRateAndDir * zoomRate * Time.unscaledDeltaTime);
		}
	}

	public void VerticalZoom(float zoomRateAndDir)
	{
		if (zoomRateAndDir != 0f)
		{
			m_dataLogger.VerticalZoom(zoomRateAndDir * zoomRate * Time.unscaledDeltaTime);
		}
	}

	public void HorizontalPan(float panRateAndDir)
	{
		if (panRateAndDir != 0f)
		{
			m_dataLogger.MovePosition(panRateAndDir * panRate * Time.unscaledDeltaTime, 0f);
		}
	}

	public void VerticalPan(float panRateAndDir)
	{
		if (panRateAndDir != 0f)
		{
			m_dataLogger.MovePosition(0f, panRateAndDir * panRate * Time.unscaledDeltaTime);
		}
	}

	public void ResetView()
	{
		if (m_currentChart != null)
		{
			m_currentChart.ResetView();
		}
	}

	public void ToggleViewMode()
	{
		if (viewMode == ViewportMode.Small)
		{
			viewMode = ViewportMode.Large;
		}
		else
		{
			viewMode = ViewportMode.Small;
		}
	}
}
