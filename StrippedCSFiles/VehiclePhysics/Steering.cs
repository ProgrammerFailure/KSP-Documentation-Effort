using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class Steering
{
	public enum SteeringMode
	{
		Disabled,
		Steerable,
		Ratio,
		Reference
	}

	[Serializable]
	public class Settings
	{
		[Range(0f, 90f)]
		public float maxSteerAngle;

		[Range(-15f, 15f)]
		public float toeAngle;

		public bool ackerman;

		public Transform ackermanReference;

		public Transform ratioReference;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	private class WheelData
	{
		public VehicleBase.WheelState wheelState;

		public Vector3 position;

		public SteeringMode mode;

		public float ratio;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public WheelData()
		{
			throw null;
		}
	}

	public float steerInput;

	public Settings settings;

	private List<WheelData> m_wheels;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Steering()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddWheel(VehicleBase.WheelState wheelState, Vector3 localPosition, SteeringMode steeringMode, float steerRatio = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddWheel(VehicleBase.WheelState wheelState, Vector3 localPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RecalculateRelativeSteerRatios()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DoUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSteeringAngle(WheelData wd, float steerAngle)
	{
		throw null;
	}
}
