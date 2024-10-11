using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class EngineChart : PerformanceChart
{
	private DataLogger.Channel m_rpm;

	private DataLogger.Channel m_load;

	private DataLogger.Channel m_speed;

	private DataLogger.Channel m_gear;

	private DataLogger.Channel m_torque;

	private DataLogger.Channel m_power;

	private DataLogger.Channel m_fuelRate;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EngineChart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string Title()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ResetView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetupChannels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RecordData()
	{
		throw null;
	}
}
