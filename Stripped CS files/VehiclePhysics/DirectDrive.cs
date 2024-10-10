using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class DirectDrive : Block
{
	public float motorInput;

	public float maxMotorTorque = 100f;

	public float maxRpm = 500f;

	public float damping = 0.95f;

	public Connection m_output;

	public float angularVelocity => m_output.float_0 / m_output.float_1;

	public float torque => m_output.outTd;

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

	public override void EvaluateTorqueDownstream()
	{
		float value = motorInput * maxMotorTorque;
		float num = m_output.float_1 * MathUtility.FastAbs(maxRpm) * Block.RpmToW;
		float max = (num - m_output.float_0) / Solver.deltaTime - m_output.Tr * damping;
		float min = (0f - num - m_output.float_0) / Solver.deltaTime - m_output.Tr * damping;
		m_output.outTd = Mathf.Clamp(value, min, max);
	}
}
