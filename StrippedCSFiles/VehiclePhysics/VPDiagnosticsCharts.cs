using System.Runtime.CompilerServices;
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

	public float maxBrakeTorque;

	private PerformanceChart[] m_charts;

	private AbsDiagnosticsChart m_absDiagnosticsChart;

	private VPPerformanceDisplay m_perfComponent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPDiagnosticsCharts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}
}
