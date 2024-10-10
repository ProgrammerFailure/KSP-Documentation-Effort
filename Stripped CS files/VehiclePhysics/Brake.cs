using UnityEngine;

namespace VehiclePhysics;

public class Brake : Block
{
	public float brakeInput;

	public float maxBrakeTorque = 500f;

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
		if (m_output != null)
		{
			return m_output.input.GetType() == typeof(Wheel);
		}
		return false;
	}

	public override void ComputeStateUpstream()
	{
		if (m_input != null)
		{
			m_input.float_0 = m_output.float_0;
			m_input.float_1 = m_output.float_1;
			m_input.Tr = m_output.Tr;
		}
	}

	public override void EvaluateTorqueDownstream()
	{
		if (m_input != null)
		{
			m_output.outTd = m_input.outTd;
		}
		float brakeTorque = Mathf.Clamp01(brakeInput) * maxBrakeTorque;
		((Wheel)m_output.input).AddBrakeTorque(brakeTorque);
	}
}
