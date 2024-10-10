using UnityEngine;

namespace VehiclePhysics;

public class Clutch : Block
{
	public float lockRatio = 1f;

	public float inertiaRatio = 0.1f;

	public float float_0;

	public float float_1;

	public float float_2;

	public Connection m_input;

	public Connection m_output;

	public override void Initialize()
	{
		SetInputs(1);
		SetOutputs(1);
	}

	public override bool CheckConnections()
	{
		m_input = base.inputs[0];
		m_output = base.outputs[0];
		if (m_input != null)
		{
			return m_output != null;
		}
		return false;
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
		float_1 = m_output.float_1 * inertiaRatio;
		m_input.float_0 = Mathf.Lerp(float_0, m_output.float_0, lockRatio);
		m_input.float_1 = Mathf.Lerp(float_1, m_output.float_1, lockRatio);
		m_input.Tr = m_output.Tr * lockRatio;
	}

	public override void EvaluateTorqueDownstream()
	{
		m_output.outTd = m_input.outTd * lockRatio;
		float viscousLockingDt = Solver.GetViscousLockingDt(lockRatio);
		float b = (float_1 * m_input.float_0 / m_input.float_1 - float_0) / viscousLockingDt;
		float_2 = Mathf.Lerp(m_input.outTd, b, lockRatio);
	}

	public override void GetSubstepDerivative(ref Derivative derivative_0)
	{
		derivative_0.float_0 = float_2;
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
