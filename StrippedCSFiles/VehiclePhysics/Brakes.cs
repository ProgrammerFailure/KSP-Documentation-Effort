using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class Brakes
{
	public enum BrakeCircuit
	{
		Neutral,
		Front,
		Rear,
		NoBrakes
	}

	public enum LateralPosition
	{
		Undefined,
		Left,
		Right
	}

	public enum AbsMode
	{
		Simple,
		MultiPosition,
		Continuous
	}

	public enum AbsTrigger
	{
		PeakSlipOffset,
		CustomSlip
	}

	public enum AbsOverride
	{
		None,
		ForceEnabled,
		ForceDisabled
	}

	[Serializable]
	public class Settings
	{
		public float maxBrakeTorque;

		[Range(0f, 1f)]
		public float brakeBias;

		public float handbrakeTorque;

		[Range(0f, 1f)]
		public float handbrakeAxle;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	[Serializable]
	public class AbsSettings
	{
		public bool enabled;

		public AbsMode mode;

		public AbsTrigger trigger;

		public float minSlipOffset;

		public float maxSlipOffset;

		public float minSlip;

		public float maxSlip;

		[Range(0f, 1f)]
		public float minPressureRatio;

		[Range(2f, 8f)]
		public int valvePositions;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AbsSettings()
		{
			throw null;
		}
	}

	private class WheelData
	{
		public float positionRatio;

		public Wheel wheel;

		public VehicleBase.WheelState wheelState;

		public LateralPosition lateralPosition;

		public float externalBrakeRatio;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public WheelData()
		{
			throw null;
		}
	}

	public float brakeInput;

	public float handbrakeInput;

	public Settings settings;

	public AbsSettings absSettings;

	private List<WheelData> m_wheelData;

	private float m_absActivationTime;

	public bool sensorAbsEngaged
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public AbsOverride absOverride
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
	public Brakes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddWheel(VehicleBase.WheelState wheelState, Wheel wheel, float relPosition = 0f, LateralPosition lateralPosition = LateralPosition.Undefined)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddWheel(VehicleBase.WheelState wheelState, Wheel wheel, BrakeCircuit circuit, LateralPosition lateralPosition = LateralPosition.Undefined)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddBrakeRatio(float ratio, BrakeCircuit circuit, LateralPosition lateralPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DoUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetValvePressureRatio(WheelData wd)
	{
		throw null;
	}
}
