using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class Gear : Block
{
	public float ratio;

	private Connection m_input;

	private Connection m_output;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Gear()
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