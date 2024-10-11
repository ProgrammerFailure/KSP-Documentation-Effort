using System;
using System.Runtime.CompilerServices;
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
		public Type type;

		[Range(1f, 12f)]
		public float gearRatio;

		public float preload;

		[Range(0f, 1f)]
		public float powerStiffness;

		[Range(0f, 1f)]
		public float coastStiffness;

		public float clutchPreload;

		[Range(0f, 1f)]
		public float clutchPackFriction;

		[Range(10f, 90f)]
		public float powerAngle;

		[Range(10f, 90f)]
		public float coastAngle;

		public float torquePreload;

		[Range(1f, 10f)]
		public float powerRatio;

		[Range(1f, 10f)]
		public float coastRatio;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	public Settings settings;

	public float damping;

	private Connection m_input;

	private Connection m_output1;

	private Connection m_output2;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Differential()
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
	private float ComputeLockingTorque(float dt)
	{
		throw null;
	}
}
