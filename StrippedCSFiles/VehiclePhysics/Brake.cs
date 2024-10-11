using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class Brake : Block
{
	public float brakeInput;

	public float maxBrakeTorque;

	private Connection m_input;

	private Connection m_output;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Brake()
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
