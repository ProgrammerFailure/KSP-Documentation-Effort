namespace VehiclePhysics;

public class Gear : Block
{
	public float ratio = 1f;

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

	public override void ComputeStateUpstream()
	{
		float num = 1f / ratio;
		m_input.float_0 = m_output.float_0 * num;
		m_input.float_1 = m_output.float_1 * num * num;
		m_input.Tr = m_output.Tr * num;
	}

	public override void EvaluateTorqueDownstream()
	{
		m_output.outTd = m_input.outTd * ratio;
	}
}
