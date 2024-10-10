using System;
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
	}

	public Transform speedGauge;

	public SpeedMark[] speedMarks;

	public InterpolatedFloat m_speedMs = new InterpolatedFloat();

	public override void FixedUpdateVehicle()
	{
		m_speedMs.Set((float)base.vehicle.data.Get(1, 0) / 1000f);
	}

	public override void UpdateVehicle()
	{
		if (speedGauge != null && speedMarks.Length > 1)
		{
			float num = m_speedMs.GetInterpolated() * 3.6f;
			int i;
			for (i = 1; i < speedMarks.Length && !(num < speedMarks[i].speedKph); i++)
			{
			}
			if (i >= speedMarks.Length)
			{
				i = speedMarks.Length - 1;
			}
			float t = Mathf.InverseLerp(speedMarks[i - 1].speedKph, speedMarks[i].speedKph, num);
			float z = Mathf.Lerp(speedMarks[i - 1].angle, speedMarks[i].angle, t);
			Vector3 localEulerAngles = speedGauge.localEulerAngles;
			localEulerAngles.z = z;
			speedGauge.localEulerAngles = localEulerAngles;
		}
	}
}
