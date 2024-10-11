using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Effects/Tire Effects", 2)]
public class VPTireEffects : VehicleBehaviour
{
	public class TireFxData
	{
		public VPGroundMarksRenderer lastRenderer;

		public int lastMarksIndex;

		public float marksDelta;

		public VPGroundParticleEmitter lastEmitter;

		public float lastParticleTime;

		public float slipTime;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TireFxData()
		{
			throw null;
		}
	}

	public float tireWidth;

	public float minSlip;

	public float maxSlip;

	[Range(0f, 2f)]
	[Header("Tire marks")]
	public float intensity;

	public float updateInterval;

	[Header("Smoke")]
	public float minIntensityTime;

	public float maxIntensityTime;

	public float limitIntensityTime;

	private TireFxData[] m_tireData;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPTireEffects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnReposition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTireMarks(VehicleBase.WheelState wheelState, TireFxData fxData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTireParticles(VehicleBase.WheelState wheelState, TireFxData fxData)
	{
		throw null;
	}
}
