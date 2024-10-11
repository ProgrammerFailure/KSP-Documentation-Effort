using System;
using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class AntiSpin
{
	[Serializable]
	public class Settings
	{
		public bool enabled;

		public float maxSpeed;

		public float minRotationDiffRpm;

		public float maxRotationDiffRpm;

		public float maxBrakeTorque;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	public enum Override
	{
		None,
		ForceEnabled,
		ForceDisabled
	}

	public Settings settings;

	public float stateVehicleSpeed;

	public float stateAngularVelocityL;

	public float stateAngularVelocityR;

	public bool sensorEngaged
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float sensorBrakeTorqueL
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float sensorBrakeTorqueR
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public Override asrOverride
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AntiSpin()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DoUpdate()
	{
		throw null;
	}
}
