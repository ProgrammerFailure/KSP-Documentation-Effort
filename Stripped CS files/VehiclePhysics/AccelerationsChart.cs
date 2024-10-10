using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class AccelerationsChart : PerformanceChart
{
	public DataLogger.Channel m_speed;

	public DataLogger.Channel m_throttle;

	public DataLogger.Channel m_brake;

	public DataLogger.Channel m_clutch;

	public DataLogger.Channel m_steering;

	public DataLogger.Channel m_longitudinalG;

	public DataLogger.Channel m_lateralG;

	public override string Title()
	{
		return "Accelerations";
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
		m_speed = base.dataLogger.NewChannel("Speed", GColor.lightGreen, 3f);
		m_speed.scale = 1f / (base.reference.maxSpeed * 3.6f) * 7f;
		m_speed.valueFormat = "0.0 km/h";
		m_longitudinalG = base.dataLogger.NewChannel("Longitudinal G", GColor.blue, 6f);
		m_longitudinalG.scale = 1f / base.reference.maxAccelerationG * 3.5f;
		m_longitudinalG.valueFormat = "0.00 G";
		m_longitudinalG.captionPositionY = 1.2f;
		m_lateralG = base.dataLogger.NewChannel("Lateral G", GColor.red, 6f);
		m_lateralG.scale = 1f / base.reference.maxAccelerationG * 3.5f;
		m_lateralG.valueFormat = "0.00 G";
		m_lateralG.captionPositionY = -0.3f;
	}

	public override void RecordData()
	{
		int[] array = base.vehicle.data.Get(1);
		float num = (float)array[0] / 1000f;
		float value = (float)array[21] / 10000f;
		int[] array2 = base.vehicle.data.Get(0);
		float value2 = (float)array2[1] / 10000f;
		float value3 = (float)array2[2] / 10000f;
		float value4 = (float)array2[4] / 10000f;
		m_speed.Write(num * 3.6f);
		m_steering.Write(value);
		m_throttle.Write(value2);
		m_brake.Write(value3);
		m_clutch.Write(value4);
		m_longitudinalG.Write(base.vehicle.localAcceleration.z / base.vehicle.gravity.Magnitude);
		m_lateralG.Write(base.vehicle.localAcceleration.x / base.vehicle.gravity.Magnitude);
	}
}
