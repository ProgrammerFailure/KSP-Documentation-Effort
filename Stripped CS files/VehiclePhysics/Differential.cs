using System;
using UnityEngine;

namespace VehiclePhysics;

public class Differential : Block
{
	public enum Type
	{
		Open,
		Locked,
		Viscous,
		ClutchPack,
		TorqueBias
	}

	[Serializable]
	public class Settings
	{
		public Type type = Type.Viscous;

		[Range(1f, 12f)]
		public float gearRatio = 3.7f;

		public float preload;

		[Range(0f, 1f)]
		public float powerStiffness = 0.2f;

		[Range(0f, 1f)]
		public float coastStiffness = 0.2f;

		public float clutchPreload = 50f;

		[Range(0f, 1f)]
		public float clutchPackFriction = 0.4f;

		[Range(10f, 90f)]
		public float powerAngle = 45f;

		[Range(10f, 90f)]
		public float coastAngle = 80f;

		public float torquePreload;

		[Range(1f, 10f)]
		public float powerRatio = 5f;

		[Range(1f, 10f)]
		public float coastRatio = 5f;
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
		float num = m_output1.float_1 + m_output2.float_1;
		float num2 = 1f / m_output1.float_1;
		float num3 = 1f / m_output2.float_1;
		float num4 = 1f / settings.gearRatio;
		m_input.float_0 = 0.5f * num * (m_output1.float_0 * num2 + m_output2.float_0 * num3) * num4;
		m_input.float_1 = num * num4 * num4;
		m_input.Tr = 0.5f * num * (m_output1.Tr * num2 + m_output2.Tr * num3) * num4;
	}

	public override void EvaluateTorqueDownstream()
	{
		float num = m_input.outTd * settings.gearRatio;
		m_output1.outTd = num * 0.5f;
		m_output2.outTd = num * 0.5f;
		bool flag = Mathf.Sign(num) > 0f;
		float num2 = 0f;
		float num3 = 0f;
		switch (settings.type)
		{
		case Type.Locked:
			num2 = ComputeLockingTorque(Solver.deltaTime);
			break;
		case Type.Viscous:
		{
			float num5 = (flag ? settings.powerStiffness : settings.coastStiffness);
			float viscousLockingDt = Solver.GetViscousLockingDt(num5);
			num2 = ComputeLockingTorque(viscousLockingDt);
			num3 = Mathf.Max(Mathf.Abs(num2 * num5), settings.preload);
			num2 = Mathf.Clamp(num2, 0f - num3, num3);
			break;
		}
		case Type.ClutchPack:
			num3 = Mathf.Max((flag ? Mathf.Cos(settings.powerAngle * ((float)Math.PI / 180f)) : Mathf.Cos(settings.coastAngle * ((float)Math.PI / 180f))) * settings.clutchPackFriction * Mathf.Abs(num), settings.clutchPreload);
			num2 = Mathf.Clamp(ComputeLockingTorque(Solver.deltaTime), 0f - num3, num3);
			break;
		case Type.TorqueBias:
		{
			float a = Mathf.Abs(m_output1.Tr - m_output2.Tr);
			float num4 = (flag ? settings.powerRatio : settings.coastRatio);
			float f = num * 0.5f * (num4 - 1f) / (num4 + 1f);
			num3 = Mathf.Min(a, Mathf.Abs(f));
			num2 = Mathf.Clamp(ComputeLockingTorque(Solver.deltaTime), 0f - num3, num3);
			break;
		}
		}
		m_output1.outTd += num2;
		m_output2.outTd -= num2;
	}

	public float ComputeLockingTorque(float dt)
	{
		return 1f / ((m_output1.float_1 + m_output2.float_1) * dt) * (m_output1.float_1 * (m_output2.float_0 + (m_output2.outTd + m_output2.Tr * damping) * dt) - m_output2.float_1 * (m_output1.float_0 + (m_output1.outTd + m_output1.Tr * damping) * dt));
	}
}
