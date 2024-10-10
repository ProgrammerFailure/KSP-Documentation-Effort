using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class SuspensionTravelChart : PerformanceChart
{
	public DataLogger.Channel[] m_wheels;

	public DataLogger.Channel m_speed;

	public override string Title()
	{
		return "Suspension Travel";
	}

	public override void Initialize()
	{
		base.dataLogger.topLimit = 10f;
		base.dataLogger.bottomLimit = -0.5f;
	}

	public override void ResetView()
	{
		base.dataLogger.rect = new Rect(0f, -0.5f, 30f, 10.5f);
	}

	public override void SetupChannels()
	{
		m_speed = base.dataLogger.NewChannel("Speed", GColor.lightGreen, 3f);
		m_speed.scale = 1f / (base.reference.maxSpeed * 3.6f) * 7f;
		m_speed.valueFormat = "0.0 km/h";
		int num = Mathf.Min(base.vehicle.wheelCount, PerformanceChart.wheelChartColors.Length);
		m_wheels = new DataLogger.Channel[num];
		int i = 0;
		for (int num2 = m_wheels.Length; i < num2; i++)
		{
			VehicleBase.WheelState wheelState = base.vehicle.wheelState[i];
			DataLogger.Channel channel = base.dataLogger.NewChannel(wheelState.wheelCol.name, GColor.Alpha(PerformanceChart.wheelChartColors[i], 0.7f));
			channel.alphaBlend = true;
			channel.scale = 1f / (base.reference.maxSuspensionDistance * 1000f) * 10f;
			channel.valueFormat = "0 mm";
			channel.captionPositionY = 1f + 0.8f * (float)(m_wheels.Length - i - 1);
			m_wheels[i] = channel;
		}
	}

	public override void RecordData()
	{
		int[] array = base.vehicle.data.Get(1);
		m_speed.Write((float)array[0] / 1000f * 3.6f);
		int i = 0;
		for (int num = m_wheels.Length; i < num; i++)
		{
			VehicleBase.WheelState wheelState = base.vehicle.wheelState[i];
			if (wheelState.grounded)
			{
				m_wheels[i].Write((wheelState.wheelCol.suspensionDistance - wheelState.contactDepth) * 1000f);
			}
		}
	}
}
