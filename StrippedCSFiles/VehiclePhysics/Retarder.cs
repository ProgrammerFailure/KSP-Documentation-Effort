using System;
using System.Runtime.CompilerServices;
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
		public int levels;

		public Curve curve;

		public float maxTorque;

		public float minRpm;

		public float maxRpm;

		[Range(0.001f, 0.999f)]
		public float curveBias;

		public float baseRpm;

		public float peakRpm;

		public float peakTorque;

		public float limitRpm;

		public float limitTorque;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	public int retarderInput;

	public Settings settings;

	private Connection m_input;

	private Connection m_output;

	private float m_rpm;

	private float m_retarderTorque;

	private BiasedRatio m_torqueRatioBias;

	public float sensorRetarderTorque
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Retarder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CheckConnections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PreStep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ComputeStateUpstream()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void EvaluateTorqueDownstream()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float EvaluateRetarderTorque(float rpm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetMaxOperationalRpms()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetPeakRpms()
	{
		throw null;
	}
}
