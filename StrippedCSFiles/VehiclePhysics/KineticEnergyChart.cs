using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class KineticEnergyChart : PerformanceChart
{
	private DataLogger.Channel m_totalEnergy;

	private DataLogger.Channel m_linearEnergy;

	private DataLogger.Channel m_angularEnergy;

	private DataLogger.Channel m_totalEnergyDelta;

	private DataLogger.Channel m_linearEnergyDelta;

	private DataLogger.Channel m_angularEnergyDelta;

	private float m_lastTotalEnergy;

	private float m_lastLinearEnergy;

	private float m_lastAngularEnergy;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KineticEnergyChart()
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
