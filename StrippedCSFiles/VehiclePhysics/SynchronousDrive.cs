using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class SynchronousDrive : Block
{
	public float targetRpm;

	public float maxTorque;

	public float damping;

	private Connection m_output;

	public float angularVelocity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float torque
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SynchronousDrive()
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
	public override void EvaluateTorqueDownstream()
	{
		throw null;
	}
}
