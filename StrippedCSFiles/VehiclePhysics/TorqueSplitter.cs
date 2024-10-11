using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class TorqueSplitter : Block
{
	[Serializable]
	public class Settings
	{
		public float preload;

		[Range(0f, 1f)]
		public float stiffness;

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
	public TorqueSplitter()
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
}
