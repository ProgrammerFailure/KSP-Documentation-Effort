using System;
using System.Runtime.CompilerServices;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Effects/Segmented Speed Gauge", 21)]
public class VPSegmentedSpeedGauge : VehicleBehaviour
{
	[Serializable]
	public class SpeedMark
	{
		public float speedKph;

		public float angle;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpeedMark()
		{
			throw null;
		}
	}

	public Transform speedGauge;

	public SpeedMark[] speedMarks;

	private InterpolatedFloat m_speedMs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPSegmentedSpeedGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVehicle()
	{
		throw null;
	}
}
