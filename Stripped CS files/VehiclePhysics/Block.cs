using System;
using UnityEngine;

namespace VehiclePhysics;

public class Block
{
	public class Connection
	{
		public float float_0;

		public float float_1;

		public float Tr;

		public float outTd;

		public Block input;

		public int inputSlot;

		public Block output;

		public int outputSlot;
	}

	public struct State
	{
		public float float_0;

		public Vector2 vector2_0;

		public float Lr;
	}

	public struct Derivative
	{
		public float float_0;

		public Vector2 vector2_0;

		public float Tr;
	}

	public Connection[] m_inputs;

	public Connection[] m_outputs;

	public int m_connectedInputs;

	public int m_connectedOutputs;

	public static float RpmToW = (float)Math.PI / 30f;

	public static float WToRpm = 30f / (float)Math.PI;

	public Connection[] inputs => m_inputs;

	public Connection[] outputs => m_outputs;

	public bool hasInputs => inputs.Length != 0;

	public bool hasOutputs => outputs.Length != 0;

	public int connectedInputs => m_connectedInputs;

	public int connectedOutputs => m_connectedOutputs;

	public Block()
	{
		Initialize();
	}

	public virtual void Initialize()
	{
		SetInputs(0);
		SetOutputs(0);
	}

	public virtual bool CheckConnections()
	{
		return true;
	}

	public virtual void PreStep()
	{
	}

	public virtual void GetState(ref State state_0)
	{
	}

	public virtual void SetSubstepState(State state_0)
	{
	}

	public virtual void ComputeStateUpstream()
	{
	}

	public virtual void EvaluateTorqueDownstream()
	{
	}

	public virtual void GetSubstepDerivative(ref Derivative derivative_0)
	{
	}

	public virtual void SetState(State state_0)
	{
	}

	public void SetInputs(int count)
	{
		count = Mathf.Max(count, 0);
		m_inputs = new Connection[count];
	}

	public void SetOutputs(int count)
	{
		count = Mathf.Max(count, 0);
		m_outputs = new Connection[count];
	}

	public static bool Connect(Block inputUnit, int inputSlot, Block outputUnit, int outputSlot)
	{
		if (inputUnit.hasInputs && inputSlot < inputUnit.inputs.Length && outputUnit.hasOutputs && outputSlot < outputUnit.outputs.Length)
		{
			outputUnit.DisconnectOutput(outputSlot);
			inputUnit.DisconnectInput(inputSlot);
			Connection connection = new Connection();
			connection.input = inputUnit;
			connection.inputSlot = inputSlot;
			connection.output = outputUnit;
			connection.outputSlot = outputSlot;
			inputUnit.inputs[inputSlot] = connection;
			outputUnit.outputs[outputSlot] = connection;
			inputUnit.CountConnections();
			outputUnit.CountConnections();
			return true;
		}
		return false;
	}

	public static bool Connect(Block inputUnit, Block outputUnit)
	{
		return Connect(inputUnit, 0, outputUnit, 0);
	}

	public static bool Connect(Block inputUnit, Block outputUnit, int outputSlot)
	{
		return Connect(inputUnit, 0, outputUnit, outputSlot);
	}

	public static bool Connect(params Block[] blocks)
	{
		if (blocks.Length < 2)
		{
			return true;
		}
		int num = 0;
		while (true)
		{
			if (num < blocks.Length - 1)
			{
				if (!Connect(blocks[num], blocks[num + 1]))
				{
					break;
				}
				num++;
				continue;
			}
			return true;
		}
		return false;
	}

	public bool DisconnectInput(int inputSlot)
	{
		if (hasInputs && inputSlot < inputs.Length)
		{
			Connection connection = inputs[inputSlot];
			if (connection != null)
			{
				inputs[inputSlot] = null;
				CountConnections();
				connection.output.DisconnectOutput(connection.outputSlot);
			}
			return true;
		}
		return false;
	}

	public bool DisconnectOutput(int outputSlot)
	{
		if (hasOutputs && outputSlot < outputs.Length)
		{
			Connection connection = outputs[outputSlot];
			if (connection != null)
			{
				outputs[outputSlot] = null;
				CountConnections();
				connection.input.DisconnectInput(connection.inputSlot);
			}
			return true;
		}
		return false;
	}

	public void CountConnections()
	{
		m_connectedInputs = 0;
		Connection[] array = inputs;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				m_connectedInputs++;
			}
		}
		m_connectedOutputs = 0;
		array = outputs;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				m_connectedOutputs++;
			}
		}
	}
}
