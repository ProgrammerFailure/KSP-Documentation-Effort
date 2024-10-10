using System.Collections.Generic;
using UnityEngine;

namespace VehiclePhysics;

public class EnergyProvider
{
	public class Output
	{
		public float demanded;

		public float maximum;

		public float delivered;
	}

	public class OutputSlot
	{
		public int priority;

		public Output output;
	}

	public float maxEnergy = 100000f;

	public float antiStallRamp = 0.01f;

	public Engine m_engine;

	public float m_energyLimit;

	public float m_demandedEnergy;

	public float m_deliveredEnergy;

	public float m_engineEnergy;

	public float m_engineRpm;

	public float m_frictionTorque;

	public List<OutputSlot> m_outputs = new List<OutputSlot>();

	public float sensorTorque => m_frictionTorque;

	public float sensorRpm => m_engineRpm;

	public float sensorEnergyLimit => m_energyLimit;

	public float sensorEngineEnergy => m_engineEnergy;

	public float sensorDemandedEnergy => m_demandedEnergy;

	public float sensorDeliveredEnergy => m_deliveredEnergy;

	public EnergyProvider(Engine engine)
	{
		m_engine = engine;
	}

	public void DoUpdate()
	{
		m_engineRpm = m_engine.sensorRpm;
		m_engineEnergy = Mathf.Max(0f, m_engine.CalculateTorque(m_engineRpm) * m_engineRpm * Block.RpmToW);
		m_energyLimit = ((maxEnergy > m_engineEnergy) ? Mathf.Lerp(m_engineEnergy, maxEnergy, (m_engineRpm - m_engine.settings.idleRpm) * antiStallRamp) : maxEnergy);
		float num = m_energyLimit;
		m_deliveredEnergy = 0f;
		m_demandedEnergy = 0f;
		foreach (OutputSlot output in m_outputs)
		{
			float num2 = Mathf.Clamp(output.output.demanded, 0f, output.output.maximum);
			output.output.delivered = Mathf.Clamp(num2, 0f, num);
			num -= output.output.delivered;
			m_deliveredEnergy += output.output.delivered;
			m_demandedEnergy += num2;
		}
		m_frictionTorque = m_deliveredEnergy / Mathf.Max(1f, m_engineRpm * Block.RpmToW);
		m_engine.AddFrictionTorque(m_frictionTorque);
	}

	public void RegisterOutput(int priority, Output output)
	{
		OutputSlot outputSlot = m_outputs.Find((OutputSlot x) => x.output == output);
		if (outputSlot == null)
		{
			outputSlot = new OutputSlot();
			m_outputs.Add(outputSlot);
		}
		outputSlot.output = output;
		outputSlot.priority = priority;
		if (m_outputs.Find((OutputSlot x) => x.priority == priority && x.output != output) != null)
		{
			Debug.LogWarning("EnergyProvider: another output is already registered with priority " + priority + ". Results will be undefined.");
		}
		m_outputs.Sort((OutputSlot x, OutputSlot y) => x.priority - y.priority);
	}

	public void RemoveOutput(Output output)
	{
		OutputSlot item = m_outputs.Find((OutputSlot x) => x.output == output);
		m_outputs.Remove(item);
	}
}
