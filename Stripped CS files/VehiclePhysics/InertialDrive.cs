using UnityEngine;

namespace VehiclePhysics;

public class InertialDrive : Block
{
	public float motorInput;

	public float maxMotorTorque = 100f;

	public float clutchInput;

	public float inertia = 0.5f;

	public float u1 = 10f;

	public float u2;

	public float u3 = 0.03f;

	public float maxRpm = 500f;

	public float damping = 0.95f;

	public Connection m_output;

	public float float_0;

	public float float_1 = 1f;

	public float float_2;

	public float rpm => float_0 / float_1 * Block.WToRpm;

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

	public float ComputeInertia()
	{
		if (!(inertia < 0.01f))
		{
			return inertia;
		}
		return 0.01f;
	}

	public override void GetState(ref State state_0)
	{
		float_1 = ComputeInertia();
		state_0.float_0 = float_0;
	}

	public override void SetSubstepState(State state_0)
	{
		float_0 = state_0.float_0;
	}

	public override void EvaluateTorqueDownstream()
	{
		float num = motorInput * maxMotorTorque;
		float num2 = Mathf.Abs(float_0);
		float num3 = float_1 * Mathf.Abs(maxRpm) * Block.RpmToW;
		if (num2 > num3)
		{
			num = 0f;
		}
		float num4 = num2 / float_1;
		float num5 = (float_1 * u1 + num4 * (u2 + u3 * u3 * num4)) * Mathf.Sign(0f - float_0);
		float num6 = float_0 / float_1;
		float num7 = num + num5;
		float num8 = (m_output.float_0 + float_0 + (m_output.Tr * damping + num7) * Solver.deltaTime) / (m_output.float_1 + float_1);
		float num9 = float_1 * (num8 - num6) / Solver.deltaTime - num7;
		num9 *= Mathf.Clamp01(1f - clutchInput);
		m_output.outTd = 0f - num9;
		float_2 = num9 + num7;
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
