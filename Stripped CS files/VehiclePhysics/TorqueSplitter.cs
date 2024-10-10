using System;
using UnityEngine;

namespace VehiclePhysics;

public class TorqueSplitter : Block
{
	[Serializable]
	public class Settings
	{
		public float preload;

		[Range(0f, 1f)]
		public float stiffness = 1f;
	}

	public Settings settings = new Settings();

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
		m_input.float_0 = m_output1.float_0;
		m_input.float_1 = m_output1.float_1;
		m_input.Tr = m_output1.Tr;
		if (m_output2 != null)
		{
			float num = Mathf.Max(Mathf.Abs(m_output2.Tr * settings.stiffness), settings.preload);
			m_input.Tr += Mathf.Clamp(m_output2.Tr, 0f - num, num);
		}
	}

	public override void EvaluateTorqueDownstream()
	{
		m_output1.outTd = m_input.outTd;
		if (m_output2 != null)
		{
			float viscousLockingDt = Solver.GetViscousLockingDt(settings.stiffness);
			float num = 1f / ((m_output1.float_1 + m_output2.float_1) * viscousLockingDt) * (m_output1.float_1 * (m_output2.float_0 + m_output2.Tr * damping * viscousLockingDt) - m_output2.float_1 * (m_output1.float_0 + (m_output1.outTd + m_output1.Tr * damping) * viscousLockingDt));
			float num2 = Mathf.Max(Mathf.Abs(num * settings.stiffness), settings.preload);
			num = Mathf.Clamp(num, 0f - num2, num2);
			m_output1.outTd += num;
			m_output2.outTd = 0f - num;
		}
	}
}
