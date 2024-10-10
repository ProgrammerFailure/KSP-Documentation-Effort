using System.Collections.Generic;
using UnityEngine;

namespace VehiclePhysics;

public class Solver
{
	public class StateVector
	{
		public Block.State[] m_states;

		public Block.State[] states => m_states;

		public StateVector(int length)
		{
			m_states = new Block.State[length];
		}
	}

	public class DerivativeVector
	{
		public Block.Derivative[] m_derivatives;

		public Block.Derivative[] derivatives => m_derivatives;

		public DerivativeVector(int length)
		{
			m_derivatives = new Block.Derivative[length];
		}
	}

	public Block[] m_blocks = new Block[0];

	public int m_wheelCount;

	public StateVector[] m_states;

	public DerivativeVector[] m_derivatives;

	public bool m_RK4enabled;

	public const float minInertia = 0.01f;

	public const float minWheelRadius = 0.01f;

	public const float minWheelMass = 0.01f;

	public static float viscousCouplingRate = 100f;

	public static float viscousLockedRatio = 0.95f;

	public static float time { get; set; }

	public static float deltaTime { get; set; }

	public string resultMessage { get; set; }

	public static void SetDebugTime(float t, float dt)
	{
	}

	public bool Initialize(Wheel[] wheels, bool enableRK4)
	{
		List<Block> list = new List<Block>();
		bool flag = true;
		resultMessage = "Initializing | ";
		Wheel[] array = wheels;
		foreach (Wheel item in array)
		{
			list.Add(item);
		}
		m_wheelCount = wheels.Length;
		int num = 0;
		while (num < list.Count)
		{
			int count = list.Count;
			for (int j = num; j < count; j++)
			{
				Block.Connection[] inputs = list[j].inputs;
				foreach (Block.Connection connection in inputs)
				{
					if (connection != null && !list.Contains(connection.output))
					{
						Block output = connection.output;
						list.Add(output);
						bool flag2 = output.CheckConnections();
						flag = flag && flag2;
						if (!flag2)
						{
							resultMessage += $"Failed: {output}@{j} | ";
						}
					}
				}
			}
			num = count;
		}
		if (!flag)
		{
			list = new List<Block>();
			array = wheels;
			foreach (Wheel item2 in array)
			{
				list.Add(item2);
			}
		}
		m_blocks = list.ToArray();
		if (enableRK4)
		{
			m_states = new StateVector[2];
			m_derivatives = new DerivativeVector[5];
		}
		else
		{
			m_states = new StateVector[1];
			m_derivatives = new DerivativeVector[1];
		}
		m_RK4enabled = enableRK4;
		int k = 0;
		for (int num2 = m_states.Length; k < num2; k++)
		{
			m_states[k] = new StateVector(m_blocks.Length);
		}
		int l = 0;
		for (int num3 = m_derivatives.Length; l < num3; l++)
		{
			m_derivatives[l] = new DerivativeVector(m_blocks.Length);
		}
		resultMessage += (flag ? "Ok." : "Aborted.");
		return flag;
	}

	public void Integrate(float t, float dt, int steps, bool useRK4)
	{
		if (m_RK4enabled && useRK4)
		{
			IntegrateRK4(m_states[0].states, m_states[1].states, m_derivatives[0].derivatives, m_derivatives[1].derivatives, m_derivatives[2].derivatives, m_derivatives[3].derivatives, m_derivatives[4].derivatives, t, dt);
		}
		else
		{
			IntegrateEuler(m_states[0].states, m_derivatives[0].derivatives, t, dt, steps);
		}
	}

	public static float GetViscousLockingDt(float lockRatio)
	{
		float num = Mathf.Max(deltaTime, 1f / viscousCouplingRate);
		if (lockRatio > viscousLockedRatio)
		{
			num = ((!(lockRatio >= 1f)) ? Mathf.Lerp(num, deltaTime, Mathf.InverseLerp(viscousLockedRatio, 1f, lockRatio)) : deltaTime);
		}
		return num;
	}

	public static void EulerStep(ref Block.State In, ref Block.Derivative Der, ref Block.State Out)
	{
		Out.float_0 = In.float_0 + Der.float_0 * deltaTime;
		Out.vector2_0 = In.vector2_0 + Der.vector2_0 * deltaTime;
		Out.Lr = In.Lr + Der.Tr * deltaTime;
	}

	public static void EulerStepL(ref Block.State In, ref Block.Derivative Der, ref Block.State Out)
	{
		Out.float_0 = In.float_0 + Der.float_0 * deltaTime;
		Out.Lr = In.Lr + Der.Tr * deltaTime;
	}

	public void EulerStep(Block.State[] In, Block.Derivative[] Der, Block.State[] Out)
	{
		int i = 0;
		for (int wheelCount = m_wheelCount; i < wheelCount; i++)
		{
			EulerStep(ref In[i], ref Der[i], ref Out[i]);
		}
		int j = m_wheelCount;
		for (int num = In.Length; j < num; j++)
		{
			EulerStepL(ref In[j], ref Der[j], ref Out[j]);
		}
	}

	public void IntegrateEuler(Block.State[] state_0, Block.Derivative[] derivative_0, float t, float dt, int subSteps)
	{
		time = t;
		deltaTime = dt;
		int i = 0;
		for (int num = m_blocks.Length; i < num; i++)
		{
			m_blocks[i].PreStep();
			m_blocks[i].GetState(ref state_0[i]);
		}
		float num2 = dt / (float)subSteps;
		for (int j = 0; j < subSteps; j++)
		{
			time = t;
			deltaTime = num2;
			ComputeDerivative(state_0, derivative_0);
			EulerStep(state_0, derivative_0, state_0);
			t += num2;
		}
		time = t + dt;
		deltaTime = dt;
		int k = 0;
		for (int num3 = m_blocks.Length; k < num3; k++)
		{
			m_blocks[k].SetState(state_0[k]);
		}
	}

	public void EvaluateRK4(Block.State[] state_0, Block.State[] Stemp, Block.Derivative[] DIn, float t, float dt, Block.Derivative[] DOut)
	{
		time = t;
		deltaTime = dt;
		EulerStep(state_0, DIn, Stemp);
		time = t + dt;
		ComputeDerivative(Stemp, DOut);
	}

	public void ComputeRK4Derivative(ref Block.Derivative derivative_0, ref Block.Derivative derivative_1, ref Block.Derivative derivative_2, ref Block.Derivative derivative_3, ref Block.Derivative DOut)
	{
		DOut.float_0 = 1f / 6f * (derivative_0.float_0 + 2f * (derivative_1.float_0 + derivative_2.float_0) + derivative_3.float_0);
		DOut.vector2_0 = 1f / 6f * (derivative_0.vector2_0 + 2f * (derivative_1.vector2_0 + derivative_2.vector2_0) + derivative_3.vector2_0);
		DOut.Tr = 1f / 6f * (derivative_0.Tr + 2f * (derivative_1.Tr + derivative_2.Tr) + derivative_3.Tr);
	}

	public void ComputeRK4DerivativeT(ref Block.Derivative derivative_0, ref Block.Derivative derivative_1, ref Block.Derivative derivative_2, ref Block.Derivative derivative_3, ref Block.Derivative DOut)
	{
		DOut.float_0 = 1f / 6f * (derivative_0.float_0 + 2f * (derivative_1.float_0 + derivative_2.float_0) + derivative_3.float_0);
		DOut.Tr = 1f / 6f * (derivative_0.Tr + 2f * (derivative_1.Tr + derivative_2.Tr) + derivative_3.Tr);
	}

	public void ComputeRK4Derivative(Block.Derivative[] derivative_0, Block.Derivative[] derivative_1, Block.Derivative[] derivative_2, Block.Derivative[] derivative_3, Block.Derivative[] DOut)
	{
		int i = 0;
		for (int wheelCount = m_wheelCount; i < wheelCount; i++)
		{
			ComputeRK4Derivative(ref derivative_0[i], ref derivative_1[i], ref derivative_2[i], ref derivative_3[i], ref DOut[i]);
		}
		int j = m_wheelCount;
		for (int num = derivative_0.Length; j < num; j++)
		{
			ComputeRK4DerivativeT(ref derivative_0[j], ref derivative_1[j], ref derivative_2[j], ref derivative_3[j], ref DOut[j]);
		}
	}

	public void IntegrateRK4(Block.State[] state_0, Block.State[] Stemp, Block.Derivative[] derivative_0, Block.Derivative[] Da, Block.Derivative[] Db, Block.Derivative[] Dc, Block.Derivative[] Dd, float t, float dt)
	{
		time = t;
		deltaTime = dt;
		int i = 0;
		for (int num = m_blocks.Length; i < num; i++)
		{
			m_blocks[i].PreStep();
			m_blocks[i].GetState(ref state_0[i]);
		}
		ComputeDerivative(state_0, Da);
		EvaluateRK4(state_0, Stemp, Da, t, dt * 0.5f, Db);
		EvaluateRK4(state_0, Stemp, Db, t, dt * 0.5f, Dc);
		EvaluateRK4(state_0, Stemp, Dc, t, dt, Dd);
		time = t;
		deltaTime = dt;
		ComputeRK4Derivative(Da, Db, Dc, Dd, derivative_0);
		EulerStep(state_0, derivative_0, state_0);
		time = t + dt;
		int j = 0;
		for (int num2 = m_blocks.Length; j < num2; j++)
		{
			m_blocks[j].SetState(state_0[j]);
		}
	}

	public void ComputeDerivative(Block.State[] state_0, Block.Derivative[] Out)
	{
		int i = 0;
		for (int num = m_blocks.Length; i < num; i++)
		{
			m_blocks[i].SetSubstepState(state_0[i]);
			m_blocks[i].ComputeStateUpstream();
		}
		for (int num2 = m_blocks.Length - 1; num2 >= 0; num2--)
		{
			m_blocks[num2].EvaluateTorqueDownstream();
			m_blocks[num2].GetSubstepDerivative(ref Out[num2]);
		}
	}

	public Block[] GetBlockArray()
	{
		return m_blocks;
	}
}
