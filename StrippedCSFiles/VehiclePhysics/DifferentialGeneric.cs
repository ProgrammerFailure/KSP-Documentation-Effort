using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class DifferentialGeneric : Block
{
	public float torqueGeometry;

	public float stiffness;

	public float damping;

	private Connection m_input;

	private Connection m_output1;

	private Connection m_output2;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DifferentialGeneric()
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
