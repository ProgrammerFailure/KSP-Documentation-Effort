using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Dynamic Suspension", 22)]
public class VPDynamicSuspension : VehicleBehaviour
{
	private class SuspensionData
	{
		public VehicleBase.WheelState wheelState;

		public VPWheelCollider wheelCol;

		public float ratio;

		public float springRate;

		public float targetSpringRate;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SuspensionData()
		{
			throw null;
		}
	}

	public int[] axles;

	[Range(0.01f, 2f)]
	[Space(5f)]
	public float suspensionDistance;

	[Range(0f, 1f)]
	public float targetCompression;

	[Range(0.1f, 2f)]
	public float changeRate;

	public bool ignoreEngineState;

	[Space(5f)]
	public float minSpringRate;

	public float maxSpringRate;

	[Range(0.5f, 120f)]
	[Space(5f)]
	public float fastUpdateInterval;

	[Range(0.5f, 120f)]
	public float slowUpdateInterval;

	[Range(0.1f, 1f)]
	[Space(5f)]
	public float inhibitWheelSleepFactor;

	public bool debugLabels;

	private float m_sumForce;

	private int m_numValues;

	private float m_lastAdjustTime;

	private List<SuspensionData> m_suspension;

	[NonSerialized]
	public bool disableSuspensionUpdates;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPDynamicSuspension()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetUpdateOrder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicleSuspension()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AdjustNow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddWheel(int wheelIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustSuspension()
	{
		throw null;
	}
}
