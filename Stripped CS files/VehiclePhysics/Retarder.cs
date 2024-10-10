using System;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class Retarder : Block
{
	public enum Curve
	{
		Simple,
		Parametric
	}

	[Serializable]
	public class Settings
	{
		[Range(0f, 10f)]
		public int levels = 5;

		public Curve curve;

		public float maxTorque = 3500f;

		public float minRpm = 100f;

		public float maxRpm = 2500f;

		[Range(0.001f, 0.999f)]
		public float curveBias = 0.7f;

		public float baseRpm = 100f;

		public float peakRpm = 1000f;

		public float peakTorque = 3500f;

		public float limitRpm = 2500f;

		public float limitTorque = 2000f;
	}

	public int retarderInput;

	public Settings settings = new Settings();

	public Connection m_input;

	public Connection m_output;

	public float m_rpm;

	public float m_retarderTorque;

	public BiasedRatio m_torqueRatioBias = new BiasedRatio();

	public float sensorRetarderTorque => m_retarderTorque;

	public override void Initialize()
	{
		SetInputs(1);
		SetOutputs(1);
	}

	public override bool CheckConnections()
	{
		m_input = base.inputs[0];
		m_output = base.outputs[0];
		return m_output != null;
	}

	public override void PreStep()
	{
		retarderInput = Mathf.Clamp(retarderInput, 0, settings.levels);
	}

	public override void ComputeStateUpstream()
	{
		m_rpm = m_output.float_0 / m_output.float_1 * Block.WToRpm;
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
		else
		{
			m_output.outTd = 0f;
		}
		if (retarderInput > 0)
		{
			float num = (float)retarderInput / (float)settings.levels;
			m_retarderTorque = num * EvaluateRetarderTorque(m_rpm);
			m_output.outTd += m_retarderTorque * Mathf.Sign(0f - m_rpm);
		}
		else
		{
			m_retarderTorque = 0f;
		}
	}

	public float EvaluateRetarderTorque(float rpm)
	{
		float result = 0f;
		rpm = Mathf.Abs(rpm);
		switch (settings.curve)
		{
		case Curve.Parametric:
			result = ((!(rpm <= settings.baseRpm)) ? ((!(rpm < settings.peakRpm)) ? ((!(rpm < settings.limitRpm)) ? settings.limitTorque : MathUtility.CubicLerp(settings.peakRpm, settings.peakTorque, settings.limitRpm, settings.limitTorque, rpm)) : MathUtility.CubicLerp(settings.baseRpm, 0f, settings.peakRpm, settings.peakTorque, rpm)) : 0f);
			break;
		case Curve.Simple:
		{
			float x = Mathf.InverseLerp(settings.minRpm, settings.maxRpm, rpm);
			result = m_torqueRatioBias.BiasedLerp(x, settings.curveBias) * settings.maxTorque;
			break;
		}
		}
		return result;
	}

	public float GetMaxOperationalRpms()
	{
		float result = 0f;
		switch (settings.curve)
		{
		case Curve.Parametric:
			result = settings.limitRpm;
			break;
		case Curve.Simple:
			result = settings.maxRpm;
			break;
		}
		return result;
	}

	public float GetPeakRpms()
	{
		float result = 0f;
		switch (settings.curve)
		{
		case Curve.Parametric:
			result = settings.peakRpm;
			break;
		case Curve.Simple:
			result = settings.maxRpm;
			break;
		}
		return result;
	}
}
