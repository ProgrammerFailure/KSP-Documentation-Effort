using UnityEngine;

namespace VehiclePhysics;

public class InertialFlywheel : Block
{
	public float torque;

	public float lockRatio = 1f;

	public float inertia = 1f;

	public float maxRpm = 10f;

	public float damping = 1f;

	public float viscousCouplingRate = 100f;

	public float viscousLockedRatio = 0.95f;

	public float float_0;

	public float float_1;

	public float m_viscousDt;

	public Connection m_output;

	public float Tlock { get; set; }

	public float rpm => float_0 / inertia * Block.WToRpm;

	public override void Initialize()
	{
		SetInputs(0);
		SetOutputs(1);
	}

	public override bool CheckConnections()
	{
		m_output = base.outputs[0];
		return m_output != null;
	}

	public override void PreStep()
	{
		lockRatio = Mathf.Clamp01(lockRatio);
		inertia = Mathf.Max(inertia, 0.01f);
		m_viscousDt = 1f / viscousCouplingRate;
	}

	public override void GetState(ref State state_0)
	{
		state_0.float_0 = float_0;
	}

	public override void SetSubstepState(State state_0)
	{
		float_0 = state_0.float_0;
	}

	public override void ComputeStateUpstream()
	{
	}

	public override void EvaluateTorqueDownstream()
	{
		float num = inertia * Mathf.Abs(maxRpm) * Block.RpmToW;
		float max = (num - float_0) / Solver.deltaTime - m_output.Tr * damping;
		float min = (0f - num - float_0) / Solver.deltaTime - m_output.Tr * damping;
		float num2 = Mathf.Clamp(torque, min, max);
		float a = Mathf.Max(Solver.deltaTime, m_viscousDt);
		a = Mathf.Lerp(a, Solver.deltaTime, Mathf.InverseLerp(viscousLockedRatio, 1f, lockRatio));
		Tlock = 1f / ((inertia + m_output.float_1) * a) * (m_output.float_1 * (float_0 + num2 * a) - inertia * (m_output.float_0 + m_output.Tr * damping * a));
		float_1 = num2 - Tlock * lockRatio;
		m_output.outTd = Tlock * lockRatio;
	}

	public override void GetSubstepDerivative(ref Derivative derivative_0)
	{
		derivative_0.float_0 = float_1;
	}

	public override void SetState(State state_0)
	{
		float_0 = state_0.float_0;
	}

	public void Reset()
	{
		float_0 = 0f;
	}
}
