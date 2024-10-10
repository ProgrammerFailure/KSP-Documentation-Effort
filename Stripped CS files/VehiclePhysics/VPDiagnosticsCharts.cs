using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Diagnostics Chart Set", 21)]
[RequireComponent(typeof(VPPerformanceDisplay))]
public class VPDiagnosticsCharts : MonoBehaviour
{
	public enum Charts
	{
		AbsDiagnostics,
		AxleSuspension,
		SuspensionAnalysis,
		KineticEnergy
	}

	public Charts chart;

	public int monitoredWheel;

	public float maxBrakeTorque = 3200f;

	public PerformanceChart[] m_charts;

	public AbsDiagnosticsChart m_absDiagnosticsChart = new AbsDiagnosticsChart();

	public VPPerformanceDisplay m_perfComponent;

	public void OnEnable()
	{
		m_perfComponent = GetComponent<VPPerformanceDisplay>();
		m_charts = new PerformanceChart[4];
		m_charts[0] = m_absDiagnosticsChart;
		m_charts[1] = new AxleSuspensionChart();
		m_charts[2] = new SuspensionAnalysisChart();
		m_charts[3] = new KineticEnergyChart();
	}

	public void FixedUpdate()
	{
		m_absDiagnosticsChart.monitoredWheel = monitoredWheel;
		m_absDiagnosticsChart.maxBrakeTorque = maxBrakeTorque;
		m_perfComponent.customChart = m_charts[(int)chart];
	}
}
