using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class EssentialsChart : PerformanceChart
{
	public DataLogger.Channel m_rpm;

	public DataLogger.Channel m_gear;

	public DataLogger.Channel m_speed;

	public DataLogger.Channel m_throttle;

	public DataLogger.Channel m_brake;

	public DataLogger.Channel m_clutch;

	public DataLogger.Channel m_steering;

	public override string Title()
	{
		return "Essentials";
	}

	public override void Initialize()
	{
		base.dataLogger.topLimit = 12f;
		base.dataLogger.bottomLimit = -0.5f;
	}

	public override void ResetView()
	{
		base.dataLogger.rect = new Rect(0f, -0.5f, 30f, 12.5f);
	}

	public override void SetupChannels()
	{
		m_throttle = base.dataLogger.NewChannel("Throttle", GColor.green);
		m_throttle.captionPositionY = 2.6f;
		m_throttle.valueFormat = "0 %";
		m_brake = base.dataLogger.NewChannel("Brake", GColor.Alpha(GColor.red, 0.6f));
		m_brake.captionPositionY = 1.8f;
		m_brake.alphaBlend = true;
		m_brake.valueFormat = "0 %";
		m_clutch = base.dataLogger.NewChannel("Clutch", GColor.Alpha(GColor.blue, 0.6f));
		m_clutch.alphaBlend = true;
		m_clutch.valueFormat = "0 %";
		m_steering = base.dataLogger.NewChannel("Steering", GColor.Alpha(GColor.teal, 0.8f), 2f);
		m_steering.scale = -0.9f;
		m_steering.valueFormat = "0 %";
		m_rpm = base.dataLogger.NewChannel("RPM", GColor.red * 0.9f, 8f);
		m_rpm.scale = 1f / base.reference.maxRpm * 3f;
		m_rpm.valueFormat = "0.0 rpm";
		m_gear = base.dataLogger.NewChannel("Gear", GColor.indigo, 7f);
		m_gear.showSegmentBegin = true;
		m_gear.scale = 1f / (float)base.reference.numGears * 0.9f;
		m_gear.valueFormat = "0";
		m_speed = base.dataLogger.NewChannel("Speed", GColor.cyan, 3f);
		m_speed.scale = 1f / (base.reference.maxSpeed * 3.6f) * 4f;
		m_speed.valueFormat = "0.0 km/h";
	}

	public override void RecordData()
	{
		int[] array = base.vehicle.data.Get(1);
		float value = (float)array[1] / 1000f;
		bool flag = array[14] != 0;
		int num = array[12];
		float num2 = (float)array[0] / 1000f;
		float value2 = (float)array[21] / 10000f;
		int[] array2 = base.vehicle.data.Get(0);
		float value3 = (float)array2[1] / 10000f;
		float value4 = (float)array2[2] / 10000f;
		float value5 = (float)array2[4] / 10000f;
		if (!flag)
		{
			m_gear.Write(num);
		}
		m_rpm.Write(value);
		m_speed.Write(num2 * 3.6f);
		m_steering.Write(value2);
		m_throttle.Write(value3);
		m_brake.Write(value4);
		m_clutch.Write(value5);
	}
}
