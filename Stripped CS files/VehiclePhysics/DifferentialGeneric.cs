using UnityEngine;

namespace VehiclePhysics;

public class DifferentialGeneric : Block
{
	public float torqueGeometry = 0.5f;

	public float stiffness = 0.2f;

	public float damping = 1f;

	public Connection m_input;

	public Connection m_output1;

	public Connection m_output2;

	public override void Initialize()
	{
		SetInputs(1);
		SetOutputs(2);
	}

	public override bool CheckConnections()
	{
		m_input = base.inputs[0];
		m_output1 = base.outputs[0];
		m_output2 = base.outputs[1];
		if (m_input != null && m_output1 != null)
		{
			return m_output2 != null;
		}
		return false;
	}

	public override void ComputeStateUpstream()
	{
		float num = Mathf.Clamp01(2f * (1f - torqueGeometry));
		float num2 = Mathf.Clamp01(2f * torqueGeometry);
		float num3 = m_output1.float_1 * num + m_output2.float_1 * num2;
		float num4 = num3 / (num + num2);
		float num5 = 1f / m_output1.float_1;
		float num6 = 1f / m_output2.float_1;
		m_input.float_0 = num4 * (m_output1.float_0 * num5 * num + m_output2.float_0 * num6 * num2);
		m_input.float_1 = num3;
		float num7 = num * (1f - stiffness) + stiffness;
		float num8 = num2 * (1f - stiffness) + stiffness;
		m_input.Tr = num4 * (m_output1.Tr * num5 * num7 + m_output2.Tr * num6 * num8);
	}

	public override void EvaluateTorqueDownstream()
	{
		m_output1.outTd = m_input.outTd * (1f - torqueGeometry);
		m_output2.outTd = m_input.outTd * torqueGeometry;
		float deltaTime = Solver.deltaTime;
		float num = 1f / ((m_output1.float_1 + m_output2.float_1) * deltaTime) * (m_output1.float_1 * (m_output2.float_0 + (m_output2.outTd + m_output2.Tr * damping) * deltaTime) - m_output2.float_1 * (m_output1.float_0 + (m_output1.outTd + m_output1.Tr * damping) * deltaTime));
		num *= stiffness;
		m_output1.outTd += num;
		m_output2.outTd -= num;
	}
}
