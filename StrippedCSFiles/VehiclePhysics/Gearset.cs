using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class Gearset : Block
{
	public class Settings
	{
		public float[] ratios;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	public int gearInput;

	public Settings settings;

	private Connection m_input;

	private Connection m_output;

	private float m_ratio;

	private float m_invRatio;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Gearset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CheckConnections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PreStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ComputeStateUpstream()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void EvaluateTorqueDownstream()
	{
		throw null;
	}
}
